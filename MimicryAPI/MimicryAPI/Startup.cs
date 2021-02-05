using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimicryAPI.DataBase;
using MimicryAPI.Helpers;
using MimicryAPI.V1.Repositories;
using MimicryAPI.V1.Repositories.Interfaces;

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
            services.AddApiVersioning(conf =>
            {
                conf.ReportApiVersions = true;

                conf.ApiVersionReader = new HeaderApiVersionReader("api-version");
                conf.AssumeDefaultVersionWhenUnspecified = true;
                conf.DefaultApiVersion = new ApiVersion(1, 0);
            });

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

        }
    }
}
