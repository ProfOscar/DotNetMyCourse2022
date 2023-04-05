using Microsoft.AspNetCore.Mvc;
using MyCourse.Models.ViewModels;

namespace MyCourse.Customizations.ViewComponents
{
    // [ViewComponent(Name = "Pagination")]
    public class PaginationBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CourseListViewModel model)
        {
            // num pag corrente
            // num ris totali
            // num ris per pagina
            return View(model);
        }
    }
}