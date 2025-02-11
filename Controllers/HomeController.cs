using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("")]
    [Route("home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
