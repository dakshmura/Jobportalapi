using Jobportalapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jobportalapi.DataAccessLayer
{
    public class JobPortalDB
    {
        SqlConnection DBCon;
        public JobPortalDB()
        {
            var configuration = GetConfiguration();
            DBCon = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("MyConnection").Value);
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public Boolean UserRegistration(UserObjects FirststepObjects)
        {
            try
            {
                Boolean insertRecord = false;
                DBCon.Open();
                string StrQuery = "insert into UserDetails(UserId,FirstName,LastName,Password,UserName,EmailId)values(@UserId,@FirstName,@LastName,@Password,@UserName,@EmailId)";
                SqlCommand cmd = new SqlCommand(StrQuery, DBCon);
                cmd.Parameters.AddWithValue("@UserId", Guid.NewGuid().ToString().Replace("-", string.Empty));
                cmd.Parameters.AddWithValue("@FirstName", FirststepObjects.FirstName);
                cmd.Parameters.AddWithValue("@LastName", FirststepObjects.LastName);
                cmd.Parameters.AddWithValue("@Password", FirststepObjects.Password);
                cmd.Parameters.AddWithValue("@UserName", FirststepObjects.UserName);
                cmd.Parameters.AddWithValue("@EmailId", FirststepObjects.EmailId);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    insertRecord = true;
                }
                DBCon.Close();
                return insertRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable UserLogin(UserObjects userObjects)
        {
            try
            {
                DataTable getvendorid = new DataTable();
                DBCon.Open();
                string StrQuery = "select * from UserDetails where UserName=@UserName and Password=@Password";
                SqlCommand cmd = new SqlCommand(StrQuery, DBCon);
                cmd.Parameters.AddWithValue("@UserName", userObjects.UserName);
                cmd.Parameters.AddWithValue("@Password", userObjects.Password);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(getvendorid);
                DBCon.Close();
                return getvendorid;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CheckEmail(UserObjects userObjects)
        {


            try
            {

                string result = "";
                DBCon.Open();
                string StrQuery = "select EmailId from UserDetails where EmailId=@EmailId";
                SqlCommand cmd = new SqlCommand(StrQuery, DBCon);
                cmd.Parameters.AddWithValue("@EmailId", userObjects.EmailId);
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    result = "true";
                }
                else
                {
                    result = "false";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<jobobjects> ListJobInfo()
        {
            try
            {
                DBCon.Open();
                List<jobobjects> lstvendor = new List<jobobjects>();
                string strQuery = "select * from JobDetails";
                DataTable dtGetVendors = new DataTable();
                SqlCommand cmd = new SqlCommand(strQuery, DBCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtGetVendors);
                DBCon.Close();
                foreach (DataRow dr in dtGetVendors.Rows)
                {
                    lstvendor.Add(
                        new jobobjects
                        {
                            JobId = Convert.ToString(dr["JobId"]),
                            Title = Convert.ToString(dr["Title"]),
                            Description = Convert.ToString(dr["Description"]),
                            Salary = Convert.ToString(dr["Salary"]),
                            Location = Convert.ToString(dr["Location"]),
                        });
                }
                return lstvendor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
