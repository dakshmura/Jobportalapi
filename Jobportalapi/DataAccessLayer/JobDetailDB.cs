using Jobportalapi.Models;
using System.Data.SqlClient;

namespace Jobportalapi.DataAccessLayer
{
    public class JobDetailDB
    {
        SqlConnection DBCon;
        public JobDetailDB()
        {
            var configuration = GetConfiguration();
            DBCon = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("MyConnection").Value);
        }
        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public Boolean AddJobDetails(jobobjects jobobj)
        {
            try
            {
                Boolean insertRecord = false;
                DBCon.Open();
                string StrQuery = "insert into JobDetails(JobId,Title,Description,Location,Salary)values(@JobId,@Title,@Description,@Location,@Salary)";
                SqlCommand cmd = new SqlCommand(StrQuery, DBCon);
                cmd.Parameters.AddWithValue("@JobId", Guid.NewGuid().ToString().Replace("-", string.Empty));
                cmd.Parameters.AddWithValue("@Title", jobobj.Title);
                cmd.Parameters.AddWithValue("@Description", jobobj.Description);
                cmd.Parameters.AddWithValue("@Location", jobobj.Location);
                cmd.Parameters.AddWithValue("@Salary", jobobj.Salary);
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
    }
}
