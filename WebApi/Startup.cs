using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApi
{
    public class Startup
    {
        private SwaggerOptions _swaggerOptions { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(_swaggerOptions);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            //});

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(_swaggerOptions.Version,
                    new OpenApiInfo
                    {
                        Title = _swaggerOptions.Title,
                        Version = _swaggerOptions.Version
                    });

                // Controller XML Documentation
                var xmlFileController = "WebApi.xml";
                var xmlPathController = Path.Combine(AppContext.BaseDirectory, xmlFileController);
                options.IncludeXmlComments(xmlPathController, includeControllerXmlComments: true);

                options.OperationFilter<VersionHeaderFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            //}

            app.UseSwagger(options =>
            {
                options.RouteTemplate = _swaggerOptions.JsonRoute;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_swaggerOptions.Endpoint, _swaggerOptions.Title);
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
