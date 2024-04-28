using first_exam.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace first_exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IViewLocalizer _localizer;
        private IConfiguration _config;
        private IOptions<ApiEndpoint> _settings;

        public HomeController(ILogger<HomeController> logger, IViewLocalizer localizer,
             IConfiguration config,
             IOptions<ApiEndpoint> settings)
        {
            _logger = logger;
            _localizer = localizer;
            _config = config;
            _settings = settings;
        }

        [TypeFilter(typeof(CustomExceptionFilter), Order = 2)]
        [TimeElapsed]
        public IActionResult Index(string culture = "")
        {
            var data0 = _settings.Value.Url;
            var data =
                _config.GetSection("Middleware")
                .GetSection("EnableContentMiddleware")
                .Value;

            var data2 =
            _config.GetSection("Middleware")
            .GetValue<bool>("EnableContentMiddleware");


            var data3 = _config
                .GetSection("Middleware:EnableContentMiddleware")
                .Value;

            _logger.LogInformation("testInfo");
            _logger.LogError("testInfo");

            return View();
        }
        public string GetCulture(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                CultureInfo.CurrentCulture = new CultureInfo(code);

                CultureInfo.CurrentUICulture = new CultureInfo(code);
            }
            return "";
        }

        public IActionResult Login()
        {
            _logger.LogInformation("Test Login Info");
            _logger.LogError("Test Login Info");

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public IActionResult Features()
        {
            return View();
        }

        public IActionResult Pricing()
        {
            return View();
        }

        public IActionResult Integrations()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
