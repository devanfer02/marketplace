using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductRepository productRepository, ILogger<HomeController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public IActionResult Index([FromQuery] ProductFilter filter)
        {
            /* 
             * Here it's doing the filter and pagination in application
             * Best approach is actually doing it in DB so the memory wont overused
             */
            var products = _productRepository.FetchAll().Where(p => p.Name.ToLower().Contains(filter.Name.ToLower())).ToList();

            var paginatedProducts = PaginatedList<Product>.Create(products, filter.Page, filter.Size);
            return View("Index", paginatedProducts);
        }
    }
}
