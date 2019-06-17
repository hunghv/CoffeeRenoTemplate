using AutoMapper;
using CoffeeRenoTemplate.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Services.Mappings;
using Swashbuckle.AspNetCore.Swagger;

namespace CoffeeRenoTemplate.Configuration
{
    public class ConfigureService
    {
        public static void InitServices(IServiceCollection services, IConfiguration configuration)
        {
            InitMySQL(services, configuration);
            InitAutoMapper(services);
            InitSwagger(services);
        }

        private static void IninitAuth(IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddFacebook(options =>
                {
                    options.AppId = "400769287316836";
                    options.AppSecret = "896d529984a688a55b472eba65a967d2";
                })
                //.AddTwitter(options =>
                //{
                //    options.ConsumerKey = "";
                //    options.ConsumerSecret = "";
                //})
                .AddGoogle(options =>
                {
                    options.ClientId = "996326863156-enla0cmr75m9i74p5ubstj2tqklpmh7a.apps.googleusercontent.com";
                    options.ClientSecret = "Yuj_3Lq5KjJhgQ6tZ2UmuU52";
                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                    options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
                    options.SaveTokens = true;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                });
        }

        // ReSharper disable once InconsistentNaming
        private static void InitMySQL(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(
                    configuration.GetConnectionString("DefaultConnection")));

            IninitAuth(services);

            services.AddAuthentication();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddDbContext<CoffeeRenoContext>(options =>
            //    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            //        mySqlOptionsAction => mySqlOptionsAction.ServerVersion(new Version(), ServerType.MySql)
            //    ));
        }

        private static void InitSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Test API",
                    Description = "ASP.NET Core Web API"
                });
            });
        }

        public static void InitConfig(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfiguration configuration)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseDefaultFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            loggerFactory.AddLog4Net(configuration.GetValue<string>("Log4NetConfigFile:Name"));
        }

        private static void InitAutoMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
