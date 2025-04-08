using CoolJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoolJob.Controllers;

public class AccountController: Controller
{
    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Registration(User model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
        
            //логика сохранения пользователя в БД
        
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка регистрации для логина {model.FirstName}: {ex.Message}");
            ModelState.AddModelError("", "Произошла ошибка при регистрации");
            return View(model);
        }
    }
}