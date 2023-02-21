using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;

        public CoursesController(ICachedCourseService courseService)
        {
            this.courseService = courseService;

        }

        public async Task<IActionResult> Index(string search = null, int page = 1, string orderby = "price", bool ascending = true)
        {
            List<CourseViewModel> courses = await courseService.GetCoursesAsync(search, page, orderby, ascending);
            ViewBag.Title = "Catalogo dei corsi";
            return View(courses);
        }

        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailViewModel viewModel = await courseService.GetCourseAsync(id);
            ViewBag.Title = viewModel.Title;
            return View(viewModel);
        }
    }
}