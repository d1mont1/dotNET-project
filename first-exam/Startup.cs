using first_exam.Controllers;
using first_exam.Data;
using first_exam.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nest;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace first_exam
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
            services.Configure<ApiEndpoint>(Configuration.GetSection("ApiEndpoint"));

            services.AddControllersWithViews();
            services.AddHttpClient<ApiClient>();
            services.AddDistributedMemoryCache();

            services.AddDbContext<PeachyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddRazorPages();

            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Home/Login";
                    option.ExpireTimeSpan = TimeSpan.FromHours(1);
                    option.Cookie.Name = "AuthToken";
                }
                );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "PeachySession";
            });

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<PeachyContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Login";
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            services.AddHttpClient<HomeController>();

            services.AddControllers(options =>
            {
                options.Filters.Add<CustomExceptionFilter>();

            });

            services.AddMvc(options =>
            {
                options.Filters.Add<AddCustomHeaderResourceFilter>();
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog();
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var culturies = new[]
                {
                    new CultureInfo("ru-RU"),
                    new CultureInfo("kk-KZ"),
                    new CultureInfo("en-US"),
                };
                options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                options.SupportedCultures = culturies;
                options.SupportedUICultures = culturies;
            });

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Seq("http://localhost:5341/")
                .WriteTo.File("Logs/logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddSingleton<Serilog.ILogger>(Log.Logger);

            services.AddTransient<IRepository, Repository>();

            services.AddHttpContextAccessor();

            services.AddDistributedMemoryCache();

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.Map("/hc", appMap =>
            {
                appMap.Run(async context =>
                {
                    await context.Response.WriteAsync("Hello from middleware");
                });

            });

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseSerilogRequestLogging();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseJwtCookieMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
