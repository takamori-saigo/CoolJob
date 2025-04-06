using Microsoft.AspNetCore.Mvc;

namespace CoolJob.Controllers;

public class HomeController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}