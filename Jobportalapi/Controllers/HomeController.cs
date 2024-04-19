using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jobportalapi.Controllers
{
    [Route("Jobportalapi/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        [ActionName("Get")]
        public IActionResult Get()
        {
            return Content("Job portal API Is Running....");
        }
    }
}
