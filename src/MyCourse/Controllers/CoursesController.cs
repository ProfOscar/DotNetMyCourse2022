using Microsoft.AspNetCore.Mvc;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            // return Content("Sono Index di Courses");
            return View();
        }

        public IActionResult Detail(string id)
        {
            // return Content($"Sono Detail di Courses, ho ricevuto l'id {id}");
            return View();
        }
    }
}