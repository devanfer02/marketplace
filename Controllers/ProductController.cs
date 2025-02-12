using Marketplace.Infra.Exceptions;
using Marketplace.Packages.Uploader;
using Marketplace.Repositories;
using Marketplace.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("products")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        private readonly IImageUploader _imageUploader;

        public ProductController(
            IProductRepository productRepository, 
            ILogger<ProductController> logger,
            IImageUploader imageUploader
        )
        {
            _productRepository = productRepository;
            _logger = logger;
            _imageUploader = imageUploader;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("create")]
        [Authorize]
        public IActionResult Create()
        {
            return View(new ProductRequestViewModel());
        }

        [Route("show/{id}")]
        public async Task<IActionResult> Show()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }
            try
            {
                var imageUrl = await _imageUploader.Upload(model.Image);
                var product = model.ToProduct(imageUrl);

                await _productRepository.CreateProduct(product);

                TempData["SuccessMessage"] = "Successfully created new product";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to create new product";
                _logger.LogError("Product.Craete|ERR: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/edit/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var product = await _productRepository.FetchById(id);

                if (product == null)
                {
                    ViewBag.ErrorMessage = "Product not found";
                    return View();
                }

                return View(ProductRequestViewModel.FromModel(product));
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productRepository.DeleteProduct(id);

                return Json(new { success = true, message = "Successfully deleted product" });
            }
            catch (Exception ex)
            {
                var errorMessage = "Failed to delete product";
                if (ex is AppException appEx)
                {
                    errorMessage = appEx.Message;
                }
                _logger.LogError($"Product.Delete|ERR: {ex.Message}");
                return Json(new { success = false, message = errorMessage });
            }
        }
    }
}
