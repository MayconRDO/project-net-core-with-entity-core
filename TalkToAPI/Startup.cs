using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TalkToAPI.DataBase;
using TalkToAPI.Helpers;
using TalkToAPI.Helpers.Swagger;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Repositories;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI
{
    /// <summary>
    /// Classe de configuração
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuração
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configuração inicial
        /// </summary>
        /// <param name="services"></param>
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

            services.Configure<ApiBehaviorOptions>(op =>
            {
                op.SuppressModelStateInvalidFilter = true;
            });

            // Politica de acesso
            services.AddCors(conf =>
            {
                conf.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:44306/", "http://localhost:50558/")
                          .WithMethods("GET")
                          .WithHeaders("Accept", "Authorization");
                });
            });

            services.AddMvc(conf =>
            {
                // Suporta formato XML
                conf.ReturnHttpNotAcceptable = true;
                conf.InputFormatters.Add(new XmlSerializerInputFormatter(conf));
                conf.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                conf.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TalkToContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<TalkToContext>(conf =>
            {
                conf.UseSqlite("Data Source=Database\\TalkTo.db");
            });

            #region Versionamento

            services.AddApiVersioning(conf =>
            {
                conf.ReportApiVersions = true;

                //conf.ApiVersionReader = new HeaderApiVersionReader("api-version");
                conf.AssumeDefaultVersionWhenUnspecified = true;
                conf.DefaultApiVersion = new ApiVersion(1, 0);
            });

            #endregion

            #region Swagger
            services.AddSwaggerGen(conf =>
            {
                // Geração de Token
                // campo bearer token
                conf.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    In = "header",
                    Type = "apiKey",
                    Description = "Insira o Json Web Token (JWT) para se autenticar",
                    Name = "Authorization"
                });

                var security = new Dictionary<string, IEnumerable<string>>()
                {
                    {"Bearer", new string[] {} }
                };
                conf.AddSecurityRequirement(security);

                // Versionamento futuro
                conf.SwaggerDoc("v1", new Info()
                {
                    Title = "API - Fale comigo",
                    Version = "v1"
                });

                // Configuração para exibir comentários
                var projectPath = PlatformServices.Default.Application.ApplicationBasePath;
                var projectName = $"{PlatformServices.Default.Application.ApplicationName}.xml";
                var xmlFilePathComment = Path.Combine(projectPath, projectName);

                conf.IncludeXmlComments(xmlFilePathComment);
            });
            #endregion

            #region JwtToken

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key-api-jwt-talk-to"))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                                            .RequireAuthenticatedUser()
                                            .Build());
            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configuração inicial
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseCors();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(conf =>
            {
                conf.SwaggerEndpoint("/swagger/v1/swagger.json", "API - Fale comigo");
                conf.RoutePrefix = "swagger";
            });
        }
    }
}
