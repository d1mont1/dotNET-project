using first_exam.Data;
using first_exam.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<HomeController> localizer,
             IConfiguration config,
             IOptions<ApiEndpoint> settings, IHttpContextAccessor context, PeachyContext db)
        {
            _logger = logger;
            _localizer = localizer;
            _config = config;
            _settings = settings;
            _context = context;
            _db = db;
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

        public async Task<IActionResult> Login(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var formData = new
                {
                    email = model.Email,
                    password = model.Password
                };

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://localhost:44383/login", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                        // Сохраняем токен в куки с префиксом Bearer
                        Response.Cookies.Append("Authorization", $"Bearer {tokenResponse.AccessToken}", new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
                        });

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Logging in failed. Please try again.");
                    }
                }
            }

            return View(model);
        }


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Отправить запрос на бэкэнд для регистрации пользователя, используя данные из модели
                // model.Email и model.Password
                // Например:
                var formData = new
                {
                    email = model.Email,
                    password = model.Password
                };

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("https://localhost:44383/register", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        // Обработать успешный ответ от сервера, например, перенаправление на другую страницу
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Обработать ошибку, например, показать сообщение пользователю
                        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    }
                }
            }
            // Если модель невалидна, вернуть представление с ошибками
            return View(model);
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
