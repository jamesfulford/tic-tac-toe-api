using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TicTacToe
{
    /// <summary>
    /// Defines the configuration of the Web API applicaiton
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="hostingEnvironment">The hosting environment.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the hosting environment.
        /// </summary>
        /// <value>
        /// The hosting environment.
        /// </value>
        public IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // SWAGGER: Register the swagger generator for a single document V1
            services.AddSwaggerGen(ConfigureSwaggerUI);
        }

        /// <summary>
        /// SWAGGER: Configures the swagger UI.
        /// </summary>
        /// <param name="swaggerGenOptions">The swagger gen options.</param>
        private void ConfigureSwaggerUI(SwaggerGenOptions swaggerGenOptions)
        {
            swaggerGenOptions.SwaggerDoc("v1", new Info { Title = "Hello World Swagger Demo", Version = "v1" });

            var filePath = Path.Combine(HostingEnvironment.ContentRootPath, "TicTacToe.config");
            swaggerGenOptions.IncludeXmlComments(filePath);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            // SWAGGER: Insert middleware to expose the generated Swagger as JSON endpoints
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    // Necessary for API management so it has the proper values for the Backend service URL otherwise
                    // you will see an error in trace similar to "Backend service URL is not defined"
                    swaggerDoc.Host = httpReq.Host.Value;
                });
            });

            // SWAGGER: swagger-ui-middleware to expose interactive documentation
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hello World Swagger Demo");
            });


        }
    }
}
