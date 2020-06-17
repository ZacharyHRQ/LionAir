using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.DAL
{
    public class AircraftDAL
    {
        private IConfiguration Configuration { get; }

        private SqlConnection conn;
        //Constructor
        public AircraftDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "LionAirConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Aircraft> GetAllAircraft()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Aircraft ORDER BY AircraftID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Aircraft> aircraftList = new List<Aircraft>();
            while (reader.Read())
            {
                aircraftList.Add(
                new Aircraft
                {
                    AircraftID = reader.GetInt32(0), //0: 1st column
                    AircraftModel = reader.GetString(1), //1: 2nd column
                    NumEconomySeat = reader.GetInt32(2), //2: 3rd column
                    NumBusinessSeat = reader.GetInt32(3), //3: 4th column
                    DateLastMaintenance = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    Status = reader.GetString(5), 
                    
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return aircraftList;
        }
    }
}
