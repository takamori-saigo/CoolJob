using CoolJob.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CoolJob.Controllers
{
    [Authorize] 
    public class ProfileController : Controller
    {
        private readonly CoolJobContext _context;
        private ILogger _logger;

        public ProfileController(CoolJobContext context, ILogger<ProfileController> logger)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users
                .Include(u => u.Applications)
                .ThenInclude(a => a.Job)
                .FirstOrDefault(u => u.Id == int.Parse(userId));

            return View(user);
        }


        public IActionResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Find(int.Parse(userId));
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {

                var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);

                if (user == null)
                {
                    return NotFound();
                }
                
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Faculty = model.Faculty;
                user.Skills = model.Skills;

                _context.SaveChanges();
                
                await HttpContext.SignOutAsync();
                await SignInUserAsync(user, true);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при редактировании профиля");
                ModelState.AddModelError("", "Произошла ошибка при сохранении");
                return View(model);
            }
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
    }
}