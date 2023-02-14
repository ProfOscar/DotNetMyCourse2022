using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;

namespace MyCourse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // services.AddTransient<ICourseService, CourseService>();
            // services.AddTransient<ICourseService, AdoNetCourseService>();
            services.AddTransient<ICourseService, EfCoreCourseService>();
            services.AddTransient<IDatabaseAccessor, SqliteDatabaseAccessor>();

            // services.AddScoped<MyCourseDbContext>();
            // services.AddDbContext<MyCourseDbContext>();
            services.AddDbContextPool<MyCourseDbContext>(optionsBuilder =>
            {
                // string connStr = Config.GetSection("ConnectionStrings").GetValue<string>("Default");
                string connStr = Configuration.GetConnectionString("Default"); // sintassi più comoda
                optionsBuilder.UseSqlite(connStr);
            });

            //Options
            services.Configure<ConnectionStringsOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<CoursesOptions>(Configuration.GetSection("Courses"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsProduction())
            {
                app.UseHttpsRedirection();
            }

            // if (env.IsDevelopment())
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();

                lifetime.ApplicationStarted.Register(() =>
                {
                    string filePath = Path.Combine(env.ContentRootPath, "bin/reload.txt");
                    File.WriteAllText(filePath, DateTime.Now.ToString());
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            // app.Run(async (context) =>
            // {
            //     string nome = context.Request.Query["nome"];
            //     await context.Response.WriteAsync($"Hello {nome.ToUpper()}!");
            // });

            // app.UseMvcWithDefaultRoute();
            app.UseMvc(routeBuilder =>
            {
                // /courses/detail/5
                routeBuilder.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
