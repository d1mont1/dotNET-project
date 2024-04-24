using first_exam.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Serilog;

namespace first_exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;
        private readonly IHttpContextAccessor _context;
        private readonly IStringLocalizer<HomeController> _local;

        public HomeController(ILogger<HomeController> logger, IRepository repo, IHttpContextAccessor context,
            IStringLocalizer<HomeController> local)
        {
            _logger = logger;
            _repo = repo;
            _context = context;
            _local = local;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("testInfo");
            _logger.LogError("testInfo");
            return View();
        }

        public IActionResult Login()
        {
            _logger.LogInformation("testInfo");
            _logger.LogError("testInfo");
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

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
    }
}
