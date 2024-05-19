using first_exam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace first_exam.Controllers
{
    [Authorize]
    public class SecureController : Controller
{
        private readonly ApiClient _apiClient;

        public SecureController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult Features()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (!string.IsNullOrEmpty(token))
            {
                _apiClient.SetAuthorizationHeader(token);
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
}
}
