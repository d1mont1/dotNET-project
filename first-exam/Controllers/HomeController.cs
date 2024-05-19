using first_exam.Data;
using first_exam.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace first_exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IHttpContextAccessor _context;
        private IConfiguration _config;
        private IOptions<ApiEndpoint> _settings;
        private PeachyContext _db;
        private readonly HttpClient _httpClient;
        private readonly ApiClient _apiClient;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer,
             IConfiguration config,
             IOptions<ApiEndpoint> settings, IHttpContextAccessor context, PeachyContext db, HttpClient httpClient, ApiClient apiClient)
        {
            _logger = logger;
            _localizer = localizer;
            _config = config;
            _settings = settings;
            _context = context;
            _db = db;
            _httpClient = httpClient;
            _apiClient = apiClient;
        }

        [TypeFilter(typeof(CustomExceptionFilter), Order = 2)]
        [TimeElapsed]
        public IActionResult Index(string culture = "")
        {
            GetCulture(culture);
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await _apiClient.LoginAsync(model.Email, model.Password);

                    if (token != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Email),
                            new Claim("AuthToken", token)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        HttpContext.Session.SetString("AuthToken", token);
                        Response.Cookies.Append("AuthToken", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure=true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.UtcNow.AddHours(1)
                        });
                        ViewBag.AuthToken = token;
                    }
                        return View(model);

                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, "Login failed: " + ex.Message);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _apiClient.RegisterAsync(model.Email, model.Password);
                    return RedirectToAction("Home");
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, "Registration failed: " + ex.Message);
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AuthToken");
            return RedirectToAction("Login");
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

        public async Task<IActionResult> Integrations()
        {
            //List<Integration> data = _db.Integrations.ToList();

            List<Integration> data = new List<Integration>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient
                    .GetAsync("https://localhost:44383/api/Home/GetAllIntegrations"))
                {
                    var content = await response
                        .Content.ReadAsStringAsync();

                    data = JsonConvert
                        .DeserializeObject<List<Integration>>(content);
                }
            }

            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
