using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DotNetCoreAppLifetimeEvents
{
    public class Startup
    {
        string path;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

            path = $"{env.WebRootPath}/AppLog.txt";

            lifetime.ApplicationStarted.Register(OnAppStarted);
            lifetime.ApplicationStopping.Register(OnAppStopping);
            lifetime.ApplicationStopped.Register(OnAppStopped);
        }

        public void OnAppStarted() 
        {
            string contents = $"App started at {DateTime.Now}\n";
            File.AppendAllText(path, contents);
        }

        public void OnAppStopping()
        {
            string contents = $"App stopping at {DateTime.Now}\n";
            File.AppendAllText(path, contents);
        }

        public void OnAppStopped()
        {
            string contents = $"App stopped at {DateTime.Now}\n";
            File.AppendAllText(path, contents);
        }
    }
}
