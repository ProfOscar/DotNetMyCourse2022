using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyCourse.Models.Options;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public class MemoryCacheCourseService : ICachedCourseService
    {
        private readonly ICourseService courseService;
        private readonly IMemoryCache memoryCache;
        public IOptionsMonitor<CoursesOptions> coursesOptions;
        public MemoryCacheCourseService(ICourseService courseService, IMemoryCache memoryCache, IOptionsMonitor<CoursesOptions> coursesOptions)
        {
            this.coursesOptions = coursesOptions;
            this.courseService = courseService;
            this.memoryCache = memoryCache;
        }

        //TODO: ricordati di usare memoryCache.Remove($"Course{id}") quando aggiorni il corso
        public Task<CourseDetailViewModel> GetCourseAsync(int id)
        {
            return memoryCache.GetOrCreateAsync($"Course{id}", cacheEntry =>
            {
                // cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(coursesOptions.CurrentValue.CacheExpiration));
                return courseService.GetCourseAsync(id);
            });
        }

        //TODO: ricordati di usare memoryCache.Remove("Courses") quando aggiungi o elimini dei corsi
        public Task<List<CourseViewModel>> GetCoursesAsync(string search, int page)
        {
            return memoryCache.GetOrCreateAsync($"Courses{search}-{page}", cacheEntry =>
            {
                // cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(coursesOptions.CurrentValue.CacheExpiration));
                return courseService.GetCoursesAsync(search, page);
            });
        }
    }
}