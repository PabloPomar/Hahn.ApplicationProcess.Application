using Hahn.ApplicationProcess.December2020.Data.Models;
using Hahn.ApplicationProcess.December2020.Web.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Reflection;

namespace Hahn.ApplicationProcess.December2020.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(options => { options.AllowAnyOrigin(); options.AllowAnyMethod(); options.WithHeaders(HeaderNames.ContentType); }) ;
            });
            services.AddControllers();
            services.AddHttpClient("appClient", c => {
                c.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
            }) ;
            services.AddDbContext<ApplicantDBContextClass>(options => options.UseInMemoryDatabase(databaseName: "Applicants"));
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
            services.AddLocalization(options => options.ResourcesPath = "LocalizationResources");
            //services.AddLocalization(options => options.ResourcesPath = "your-translations-folder");
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Hahn.ApplicationProcess.December2020.Web", 
                    Version = "v1",
                    Description = "A project to applicate to the company",
                    Contact = new OpenApiContact
                    {
                        Name = "Pablo Pomar",
                        Email = "PabloPomar94@gmail.com",
                        Url = new Uri("https://github.com/PabloPomar")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                var baseDir = AppContext.BaseDirectory.Replace('\\', '/');
                char[] temp = baseDir.ToCharArray();
                for (int index = 0; index < temp.Length; index++)
                    switch (temp[index])
                    {
                        case '\\':
                            temp[index] = '/';
                            break;                          
                    }
                string output = new string(temp);
                output = output + "Hahn.ApplicatonProcess.December2020.Domain.xml";
                var xmlPath2 = Path.Combine(AppContext.BaseDirectory, "Hahn.ApplicationProcess.December2020.Domain.xml");
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(output);
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn.ApplicationProcess.December2020.Web v1"));
            }

            var supportedCultures = new[] { "en-US", "de", "es"};
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
