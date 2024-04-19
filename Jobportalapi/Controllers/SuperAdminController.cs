using Jobportalapi.BusinessLayer;
using Jobportalapi.DataAccessLayer;
using Jobportalapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jobportalapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        static IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        string AuthApiKey = configuration.GetSection("AuthApiKey").GetSection("AuthApiKey").Value;
        JobDetailDB jobdetailsDBobj = new JobDetailDB();
        [HttpPost]
        [ActionName("AddJobDetails")]
        public IActionResult AddJobDetails(jobobjects jobobj)
        {
            string APIAccessKey = EncryptDecryptBL.Decrypt(jobobj.Encryptedkey);

            try
            {
                if (APIAccessKey == AuthApiKey)
                {

                    Boolean signup = jobdetailsDBobj.AddJobDetails(jobobj);


                    return Ok("Inserted Successfully");
                }
                else
                {
                    return Ok("AccessDenied");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
    }
}
