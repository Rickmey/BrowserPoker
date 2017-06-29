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
using Newtonsoft.Json;

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

            app.Map("/onDefault", HandleOnDefaultRequest);
            app.Map("/onRequestID", HandleOnRequestID);

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from non-Map delegate. <p>");
            });
        }


        /// <summary>
        /// Handle default request. Reads session id and redirects the request to the corresponding game instance (table).
        /// </summary>
        /// <param name="app"></param>
        static void HandleOnDefaultRequest(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                string input;
                using (var reader = new StreamReader(context.Request.Body))
                {
                    input = reader.ReadToEnd();
                }
                var requestObject = JsonConvert.DeserializeObject<RequestObject>(input);

                GameStateModel result = null;
                if (requestObject.ID != null)
                    result = IDTableMap[requestObject.ID.ToString()]?.HandleRequest(requestObject);

                await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            });
        }

        /// <summary>
        /// Handles request for new sessions. Creates an game instance (table).
        /// TODO: handle closed sessions.
        /// </summary>
        /// <param name="app"></param>
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
    }
}
