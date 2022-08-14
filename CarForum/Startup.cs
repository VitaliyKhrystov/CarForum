using CarForum.Domain;
using CarForum.Domain.Entities;
using CarForum.Domain.Repositories.Abstract;
using CarForum.Domain.Repositories.EntityFrameWork;
using CarForum.Models;
using CarForum.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
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

            services.AddIdentity<User, IdentityRole>( options => 
                                                    {
                                                        options.Password.RequiredLength = 10;
                                                    })
                                                    .AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options =>
                        {
                            var policy = new AuthorizationPolicyBuilder()
                                        .RequireAuthenticatedUser()
                                        .Build();
                            options.Filters.Add(new AuthorizeFilter(policy));
                        }).AddXmlSerializerFormatters();


            services.ConfigureApplicationCookie(options =>
                           options.AccessDeniedPath = new PathString("/Account/AccessDenied")
                        );


            services.AddTransient<ITopicFieldRepository, EFTopicField>();
            services.AddTransient<IResponseRepository, EFResponse>();
            services.AddTransient<DataManager>();
            services.AddTransient<TopicResponseModel>();
            services.AddTransient<TopicField>();
            services.AddTransient<Response>();
            services.AddTransient<IdentityRole>();
            services.AddTransient<User>();
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

            app.UseAuthentication();
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
