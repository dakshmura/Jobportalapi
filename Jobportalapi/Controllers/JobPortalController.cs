using Jobportalapi.BusinessLayer;
using Jobportalapi.DataAccessLayer;
using Jobportalapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Jobportalapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobPortalController : ControllerBase
    {
        static IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        string AuthApiKey = configuration.GetSection("AuthApiKey").GetSection("AuthApiKey").Value;
        JobPortalDB jobportaldbobj = new JobPortalDB();
        [HttpPost]
        [ActionName("Registeration")]
        public IActionResult Register(UserObjects userObj)
        {
            string APIAccessKey = EncryptDecryptBL.Decrypt(userObj.Encryptedkey);

            try
            {
                if (APIAccessKey == AuthApiKey)
                {

                    Boolean signup = jobportaldbobj.UserRegistration(userObj);
                   

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
        [HttpPost]
        [ActionName("EmailIdValidate")]
        public ActionResult Emailcheck(UserObjects userObj)
        {
            string APIAccessKey = EncryptDecryptBL.Decrypt(userObj.Encryptedkey);

            try
            {
                if (APIAccessKey == AuthApiKey)
                {
                    string result = jobportaldbobj.CheckEmail(userObj);
                    if (result == "true")
                    {
                        result = "true";

                    }
                    else if (result == "false")
                    {
                        result = "false";
                    }
                    return Ok(result);

                }
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [ActionName("UserLogin")]
        public ActionResult UserLogin(UserObjects userObj)
        {

            string[] DecryptSigninObj = EncryptDecryptBL.Decrypt(userObj.EncryptObjSignin).ToString().Split("~");
            string AccessKey = DecryptSigninObj[0];
            userObj.UserName = DecryptSigninObj[1];
            userObj.Password = DecryptSigninObj[2];
            try
            {
                if (AccessKey == AuthApiKey)
                {
                    DataTable dtlogin = jobportaldbobj.UserLogin(userObj);
                    if (dtlogin.Rows.Count > 0)
                    {
                        userObj.UserId = dtlogin.Rows[0]["UserId"].ToString();

                        return Ok(userObj.UserId);
                    }
                    else
                    {
                        return Ok("UserName or Password Wrong");
                    }

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
        [HttpGet]
        [ActionName("ListJobInfo")]
        public IActionResult ListJobInfo()
        {

            return Ok(jobportaldbobj.ListJobInfo());


        }


    }
}
