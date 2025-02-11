using Microsoft.AspNetCore.Mvc;
using Marketplace.ViewModel;
using Marketplace.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Marketplace.Infra.Exceptions;
using System.Security.Claims;
using Marketplace.Packages.Auth;

namespace Marketplace.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly AuthManager _authManager;
        public AuthController(
            ILogger<AuthController> logger, 
            IUserRepository userRepository, 
            AuthManager authManager
        )
        {
            _logger = logger;
            _userRepository = userRepository;
            _authManager = authManager;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AttemptLogin(LoginViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", request);
            }

            try
            {
                var user = await _userRepository.GetUserByEmail(request.Email);

                if (user == null || !user.VerifyPassword(request.Password))
                {
                    ViewBag.ErrorMessage = "Invalid username or password";
                    return View("Login");
                }

                await _authManager.SignInAsync(user);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError("Auth.Login|ERR: " + ex.Message);
                ViewBag.ErrorMessage = "Something error happened";
                return View("Login");
            }
        }

        [HttpPost]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", request);
            }

            try
            {
                var user = request.ToUser().HashPassword();
                await _userRepository.CreateUser(user);

                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                _logger.LogError("Auth.Register|ERR: " + ex.Message);
                ViewBag.ErrorMessage = "Something error happened";
                if (ex is AppException)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
                
                return View("Register");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _authManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
