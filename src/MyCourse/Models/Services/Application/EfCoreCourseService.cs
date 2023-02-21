using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCourse.Models.Entities;
using MyCourse.Models.Exceptions;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class EfCoreCourseService : ICourseService
    {
        private readonly ILogger<EfCoreCourseService> logger;
        private readonly MyCourseDbContext dbContext;
        private readonly IOptionsMonitor<CoursesOptions> coursesOptions;

        public EfCoreCourseService(ILogger<EfCoreCourseService> logger, MyCourseDbContext dbContext, IOptionsMonitor<CoursesOptions> coursesOptions)
        {
            this.coursesOptions = coursesOptions;
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<CourseDetailViewModel> GetCourseAsync(int id)
        {
            IQueryable<CourseDetailViewModel> queryLinq = dbContext.Courses
                .AsNoTracking() //Miglioramento prestazionale disabilitando il "change tracking" (ottimo in situazioni read-only)
                .Include(course => course.Lessons)
                .Where(course => course.Id == id)
                .Select(course => CourseDetailViewModel.FromEntity(course)); //Usando metodi statici come FromEntity, la query potrebbe essere inefficiente. Mantenere il mapping nella lambda oppure usare un extension method personalizzato

            //CourseDetailViewModel viewModel = await queryLinq.SingleAsync();
            //.FirstOrDefaultAsync(); //Restituisce null se l'elenco è vuoto e non solleva mai un'eccezione
            //.SingleOrDefaultAsync(); //Tollera il fatto che l'elenco sia vuoto e in quel caso restituisce null, oppure se l'elenco contiene più di 1 elemento, solleva un'eccezione
            //.FirstAsync(); //Restituisce il primo elemento, ma se l'elenco è vuoto solleva un'eccezione

            CourseDetailViewModel viewModel = await queryLinq.FirstOrDefaultAsync();
            if (viewModel == null)
            {
                logger.LogWarning("Course {id} not found", id);
                throw new CourseNotFoundException(id);
            }

            return viewModel;
        }

        public async Task<List<CourseViewModel>> GetCoursesAsync(string search, int page, string orderby, bool ascending)
        {
            search = search ?? "";
            page = Math.Max(1, page);
            int limit = coursesOptions.CurrentValue.PerPage;
            int offset = (page - 1) * limit;

            var orderOptions = coursesOptions.CurrentValue.Order;
            if (!orderOptions.Allow.Contains(orderby))
            {
                orderby = orderOptions.By;
                ascending = orderOptions.Ascending;
            }
            IQueryable<Course> baseQuery = dbContext.Courses;
            switch (orderby)
            {
                case "Title":
                    if (ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Title);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Title);
                    }
                    break;
                case "Rating":
                    if (ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.Rating);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.Rating);
                    }
                    break;
                case "CurrentPrice":
                    if (ascending)
                    {
                        baseQuery = baseQuery.OrderBy(course => course.CurrentPrice.Amount);
                    }
                    else
                    {
                        baseQuery = baseQuery.OrderByDescending(course => course.CurrentPrice.Amount);
                    }
                    break;
            }

            IQueryable<CourseViewModel> queryLinq = baseQuery
                .AsNoTracking() //Miglioramento prestazionale disabilitando il "change tracking" (ottimo in situazioni read-only)
                .Where(course => course.Title.ToLower().Contains(search.ToLower()))
                // .Where(course => EF.Functions.Like(course.Title, $"%{search}%")) // funziona ed è case-insensitive usando il LIKE
                // .Where(course => course.Title.Contains(search)) // funziona, ma è case-sensitive, pertanto scomoda
                .Skip(offset)
                .Take(limit)
                .Select(course => CourseViewModel.FromEntity(course)); //Usando metodi statici come FromEntity, la query potrebbe essere inefficiente. Mantenere il mapping nella lambda oppure usare un extension method personalizzato

            List<CourseViewModel> courses = await queryLinq.ToListAsync(); //La query al database viene inviata qui, quando manifestiamo l'intenzione di voler leggere i risultati

            return courses;
        }
    }
}