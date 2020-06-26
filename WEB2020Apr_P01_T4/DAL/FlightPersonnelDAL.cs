using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.DAL
{
    public class FlightPersonnelDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //Constructor     
        public FlightPersonnelDAL()
        {
            //Read ConnectionString from appsettings.json file       
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("LionAirConnectionString");

            //Instantiate a SqlConnection object with the     
            //Connection String read.     
            conn = new SqlConnection(strConn);
        }

        public List<FlightPersonnel> GetAllStaff()
        {
            //Create a SqlCommand object from connection object      
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement          
            cmd.CommandText = @"SELECT * FROM Staff";
            //Open a database connection         
            conn.Open();
            //Execute the SELECT SQL through a DataReader       
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list     
            List<FlightPersonnel> staffList = new List<FlightPersonnel>();
            while (reader.Read())
            {
                staffList.Add(
                    new FlightPersonnel
                    {
                        StaffID = reader.GetInt32(0),
                        StaffName = reader.GetString(1),
                        Gender = reader.GetString(2)[0],
                        DateEmployed = reader.GetDateTime(3),
                        //Gender = !reader.IsDBNull(2) ? reader.GetChar(2) : (char?) null,             
                        //DateEmployed = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                        Vocation = reader.GetString(4),
                        //Vocation = !reader.IsDBNull(4) ? reader.GetString(4) : (string?)null,                  
                        EmailAddr = reader.GetString(5),
                        Status = reader.GetString(7),
                    }
                    );
            }
            //Close DataReader      
            reader.Close();
            //Close the database connection      
            conn.Close();

            return staffList;
        }

        public bool VaildStaff(String email, String password, out int staffID)
        {

            staffID = 0;
            try
            {

                // writing sql query  
                SqlCommand cm = new SqlCommand(String.Format("SELECT StaffID FROM Staff WHERE UPPER(EmailAddr) = UPPER('{0}') AND Password = '{1}'",
                    email.ToUpper(),
                    password
                    ), conn);


                // Opening Connection  
                conn.Open();
                // Executing the SQL query  
                SqlDataReader sqlDataReader = cm.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    staffID = sqlDataReader.GetInt32(0);
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception e)
            {
                return false;
            }
            // Closing the connection  
            finally
            {
                conn.Close();
            }
        }
    }
}
