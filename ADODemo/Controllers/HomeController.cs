using ADODemo.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADODemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        public IConfiguration _configuration { get; }
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Inventory> Index()
        {
            List<Inventory> list = new List<Inventory>();
            // connection
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            //SqlConnection connection = new SqlConnection(connectionString);


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;
            SqlConnection connection = new SqlConnection(builder.ConnectionString);

            //command
            string sql = "Select * From dbo.Inventory";
            SqlCommand command = new SqlCommand(sql, connection);

            // execute command
            connection.Open();
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                // Loop over the results
                while (dataReader.Read())
                {
                    list.Add(new Inventory
                    {
                        Id = Convert.ToInt32(dataReader["Id"]),
                        Name = Convert.ToString(dataReader["Name"]),
                        Price = Convert.ToDecimal(dataReader["Price"]),
                        Quantity = Convert.ToInt32(dataReader["Quantity"]),
                        AddedOn = Convert.ToDateTime(dataReader["AddedOn"])
                    }
                    );
                }
            }
            connection.Close();


            return list;
        }
    }
}
