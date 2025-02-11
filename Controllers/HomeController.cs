using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("")]
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductRepository productRepository, ILogger<HomeController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var products = _productRepository.FetchAll();
            var paginatedProducts = PaginatedList<Product>.Create(products, page, size);
            return View("Index", paginatedProducts);
        }
    }
}
