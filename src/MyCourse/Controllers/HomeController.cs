using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // return Content("Sono la Index della Home");
            return View();
        }
    }
}