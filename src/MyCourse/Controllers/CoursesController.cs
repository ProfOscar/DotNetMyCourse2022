using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.Services.Application;
using MyCourse.Models.ViewModels;

namespace MyCourse.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            CourseService courseService = new CourseService();
            List<CourseViewModel> courses = courseService.GetCourses();
            ViewBag.Title = "Catalogo dei corsi";
            return View(courses);
        }

        public IActionResult Detail(int id)
        {
            CourseService courseService = new CourseService();
            CourseDetailViewModel viewModel = courseService.GetCourse(id);
            ViewBag.Title = viewModel.Title;
            return View(viewModel
            );
        }
    }
}