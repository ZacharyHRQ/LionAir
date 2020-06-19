using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace WEB2020Apr_P01_T4.DAL
{
    public class BaseDAL
    {
        public IConfiguration Configuration { get; }
        public SqlConnection con = null;

        public BaseDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            //Creating the connection
            con = new SqlConnection(Configuration.GetConnectionString("LionAirConnectionString"));
        }


    }
}
