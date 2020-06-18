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

        public int Add(Aircraft aircraft)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Aircraft (MakeModel, NumEconomySeat, NumBusinessSeat, DateLastMaintenance,Status)
            OUTPUT INSERTED.AircraftID
            VALUES(@model, @econSeat, @businessSeat, @DOM,
            @status)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@model", aircraft.AircraftModel);
            cmd.Parameters.AddWithValue("@econSeat", aircraft.NumEconomySeat);
            cmd.Parameters.AddWithValue("@businessSeat", aircraft.NumBusinessSeat);
            cmd.Parameters.AddWithValue("@DOM", DBNull.Value);
            cmd.Parameters.AddWithValue("@status", "Operational");
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            aircraft.AircraftID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return aircraft.AircraftID;
        }

       
    } 
}
