using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using BrowserPoker.GameObjects;

namespace BrowserPoker
{
    public class Startup
    {
        static Dictionary<string, Table> IDTableMap = new Dictionary<string, Table>();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.Map("/map1", HandleMapTest1);
            app.Map("/map2", HandleMapTest2);
            app.Map("/bodytest", HandleRequestBodyTest);
            app.Map("/onRequestID", HandleOnRequestID);

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
            });
        }

        static void HandleOnRequestID(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                // TODO check if there is no other way to get or create a session id
                var id = Guid.NewGuid();
                var table = new Table(id);
                IDTableMap.Add(id.ToString(), table);
                await context.Response.WriteAsync(id.ToString());
            });
        }

        static void HandleRequestBodyTest(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var result = new StreamReader(context.Request.Body).ReadToEnd();
                await context.Response.WriteAsync("body processed: " + result);
            });
        }

        static void HandleMapTest1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }

        static void HandleMapTest2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 2");
            });
        }
    }
}
