using first_exam.Api.Data;
using first_exam.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace first_exam.Api.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private PeachyContext _db;

        public HomeController(PeachyContext db)
        {
            _db = db;
        }

        [HttpGet("GetAllIntegrations")]
        public IEnumerable<Integration> GetAllIntegrations()
        {
            var integrations = _db.Integrations;

            return integrations;
        }
        
    }
}
