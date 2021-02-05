using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using MimicryAPI.DataBase;
using MimicryAPI.Helpers;
using MimicryAPI.Helpers.Swagger;
using MimicryAPI.V1.Repositories;
using MimicryAPI.V1.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Linq;

namespace MimicryAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region Auto Mapper
            var config = new MapperConfiguration(conf =>
            {
                conf.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            services.AddMvc();
            services.AddDbContext<MimicryContext>(opt =>
            {
                opt.UseSqlite("Data Source=DataBase\\Mimicry.db");
            });
            services.AddScoped<IWordRepository, WordRepository>();

            #region Api Version Swagger

            services.AddApiVersioning(conf =>
            {
                conf.ReportApiVersions = true;

                conf.ApiVersionReader = new HeaderApiVersionReader("api-version");
                conf.AssumeDefaultVersionWhenUnspecified = true;
                conf.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddSwaggerGen(conf =>
            {
                conf.ResolveConflictingActions(apiDescription => apiDescription.First());
                conf.SwaggerDoc("v2.0", new Info()
                {
                    Title = "MimicAPI - V2.0",
                    Version = "v2.0"
                });
                conf.SwaggerDoc("v1.1", new Info()
                {
                    Title = "MimicAPI - V1.1",
                    Version = "v1.1"
                });
                conf.SwaggerDoc("v1.0", new Info()
                {
                    Title = "MimicAPI - V1.0",
                    Version = "v1.0"
                });

                var projectPath = PlatformServices.Default.Application.ApplicationBasePath;
                var projectName = $"{PlatformServices.Default.Application.ApplicationName}.xml";
                var xmlFilePathComment = Path.Combine(projectPath, projectName);

                conf.IncludeXmlComments(xmlFilePathComment);

                conf.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    // would mean this action is unversioned and should be included everywhere
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                });

                conf.OperationFilter<ApiVersionOperationFilter>();
            });



            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(conf =>
            {
                conf.SwaggerEndpoint("/swagger/v2.0/swagger.json", "Mimic Api - v2.0");
                conf.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Mimic Api - v1.1");
                conf.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Mimic Api - v1.0");
                conf.RoutePrefix = string.Empty;
            });

        }
    }
}
