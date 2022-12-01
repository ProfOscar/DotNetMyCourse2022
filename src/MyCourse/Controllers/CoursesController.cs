using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courseService;

        public CoursesController(ICourseService courseService)
        {
            this.courseService = courseService;
            
        }

        public IActionResult Index()
        {
            List<CourseViewModel> courses = courseService.GetCourses();
            ViewBag.Title = "Catalogo dei corsi";
            return View(courses);
        }

        public IActionResult Detail(int id)
        {
            CourseDetailViewModel viewModel = courseService.GetCourse(id);
            ViewBag.Title = viewModel.Title;
            return View(viewModel
            );
        }
    }
}