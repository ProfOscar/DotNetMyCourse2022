using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.InputModels;
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

        public async Task<IActionResult> Index(CourseListInputModel input)
        {
            List<CourseViewModel> courses = await courseService.GetCoursesAsync(input);
            ViewBag.Title = "Catalogo dei corsi";

            CourseListViewModel viewModel = new CourseListViewModel
            {
                Courses = courses,
                Input = input
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            CourseDetailViewModel viewModel = await courseService.GetCourseAsync(id);
            ViewBag.Title = viewModel.Title;
            return View(viewModel);
        }
    }
}