using Marketplace.Packages.Auth;
using Marketplace.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly AuthManager _authManager;

        
        public UserController(
            IUserRepository userRepository, 
            ILogger<UserController> logger,
            AuthManager authManager 
        )
        {
            _userRepository = userRepository;
            _logger = logger;
            _authManager = authManager;
        }

        [Route("profile")]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userInMemory = _authManager.GetUser()!;
                var user = await _userRepository.GetUserByID(userInMemory.Id);

                return View(user);
            }
            catch (Exception e)
            {
                _logger.LogError("User.Profile|ERR: " + e.Message);
                TempData["ErrorMessage"] = "Something went wrong";
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("edit")]

        public async Task<IActionResult> Edit()
        {
            try
            {
                var userInMemory = _authManager.GetUser()!;
                var user = await _userRepository.GetUserByEmail(userInMemory.Email);

                return View(user);
            }
            catch (Exception e)
            {
                _logger.LogError("User.Profile|ERR: " + e.Message);
                TempData["ErrorMessage"] = "Something went wrong";
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser()
        {
            return View();
        }
    }
}
