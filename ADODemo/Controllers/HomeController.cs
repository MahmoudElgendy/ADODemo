using ADODemo.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
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
            DataTable dataTable = new DataTable();

            #region 1- Connection
            string connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            //SqlConnection connection = new SqlConnection(connectionString);

            //
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            builder.Encrypt = true;
            builder.TrustServerCertificate = true;

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            #endregion

            #region 2- Command
            //string sql = "Select * From dbo.Inventory";
            string sql = "ReadInventory";
            SqlCommand command = new SqlCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure; // default is text
            #endregion

            #region 3- execute Command
            #region Connected Mode using DataReader
            //connection.Open();
            //using (SqlDataReader dataReader = command.ExecuteReader())
            //{
            //    // Loop over the results
            //    while (dataReader.Read())
            //    {
            //        list.Add(new Inventory
            //        {
            //            Id = Convert.ToInt32(dataReader["Id"]),
            //            Name = Convert.ToString(dataReader["Name"]),
            //            Price = Convert.ToDecimal(dataReader["Price"]),
            //            Quantity = Convert.ToInt32(dataReader["Quantity"]),
            //            AddedOn = Convert.ToDateTime(dataReader["AddedOn"])
            //        }
            //        );
            //    }
            //} 
            //connection.Close();
            #endregion

            #region DisConnected Mode using DataAdapter
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataTable);
            list = dataTable.AsEnumerable().Select(row => new Inventory
            {
                Id = row.Field<int>("ID"),
                Name = row.Field<string>("Name"),
                Price = row.Field<decimal>("Price"),
                Quantity = row.Field<int>("Quantity"),
                AddedOn = row.Field<DateTime>("AddedOn"),
            }).ToList();
            #endregion 
            #endregion

            return list;
        }
    }
}
