using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    //L'attributo avrebbe effetto su tutte le action del controller
    //[ResponseCache(CacheProfileName = "Home")]
    public class HomeController : Controller
    {
        // [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [ResponseCache(CacheProfileName = "Home")]
        public IActionResult Index()
        {
            // return Content("Sono la Index della Home");
            return View();
        }
    }
}