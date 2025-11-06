using CoolJob.Models;
using CoolJob.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CoolJob.Controllers
{
    public class AuthController : Controller
    {
        private readonly CoolJobContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(CoolJobContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Console.WriteLine($"Попытка входа: {model.Email}");
    
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null) {
                Console.WriteLine("Пользователь не найден");
                return View(model);
            }

            Console.WriteLine($"Найден пользователь: {user.Email}");
            Console.WriteLine($"Введенный пароль (хэш): {HashPassword(model.Password)}");
            Console.WriteLine($"Пароль в БД: {user.Password}");

            if (!VerifyPassword(model.Password, user.Password)) {
                Console.WriteLine("Пароль не совпадает");
                return View(model);
            }

            await SignInUserAsync(user, model.RememberMe);
            Console.WriteLine("Аутентификация прошла успешно");
    
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out");
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        
        public async Task SignInUserAsync(User user, bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("FullName", $"{user.FirstName} {user.LastName}"),
                new Claim("Faculty", user.Faculty ?? string.Empty)
            };

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe ? DateTime.UtcNow.AddDays(30) : null,
                AllowRefresh = true
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hash = HashPassword(inputPassword);
            return hash == storedHash;
        }
        
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            
            return RedirectToAction("Index", "Home");
        }
    }
}