using CoolJob.Models;
using CoolJob.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CoolJob.Controllers
{
    public class AccountController : Controller
    {
        private readonly CoolJobContext _context;

        public AccountController(CoolJobContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Пользователь с таким email уже существует");
                    return View(model);
                }

                var user = new User
                {
                    Email = model.Email,
                    Password = HashPassword(model.Password),
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
    
    
}