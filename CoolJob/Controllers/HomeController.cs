using Microsoft.AspNetCore.Mvc;

namespace CoolJob.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}