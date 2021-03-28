using CRMit.Items.Docs;
using CRMit.Items.Extensions;
using CRMit.Items.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace CRMit.Items
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CRMit.Items",
                    Version = "v1",
                    Description = "Items resource in CRMit API.",
                    Contact = new OpenApiContact
                    {
                        Name = "Daniil Ryzhkov",
                        Email = "drop.sovet@gmail.com",
                        Url = new Uri("https://github.com/dropik/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "The MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

#if DEBUG
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "CRMit.Items.xml");
                c.IncludeXmlComments(xmlPath);
#endif

                c.OperationFilter<OperationFilter>();
            });

            services.AddDbContext<ItemsDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ItemsDb"));
            });

            services.AddStartupTask<DatabaseMigrator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRMit.Items v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
