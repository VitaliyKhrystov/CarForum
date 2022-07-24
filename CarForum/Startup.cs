using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
using CarForum.Domain.Repositories.EntityFrameWork;
using CarForum.Models;
using CarForum.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace CarForum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            Configuration.Bind("Project", new Config());
            services.AddDbContext<AppDbContext>(str => str.UseSqlServer(Config.ConnectionString));

            services.AddTransient<ITopicFieldRepository, EFTopicField>();
            services.AddTransient<IResponseRepository, EFResponse>();
            services.AddTransient<DataManager>();
            services.AddTransient<TopicResponseModel>();
            services.AddTransient<TopicField>();
            services.AddTransient<Response>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
