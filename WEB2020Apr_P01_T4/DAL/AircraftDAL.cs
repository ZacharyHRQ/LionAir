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


        public List<FlightSchedule> GetSchedules(int aircraftID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE (AircraftID = @aircraftid AND DepartureDateTime >= (GETDATE()))";
            cmd.Parameters.AddWithValue("@aircraftid", aircraftID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<FlightSchedule> flightSchedules = new List<FlightSchedule>();
            while (reader.Read())
            {
                flightSchedules.Add(
                new FlightSchedule
                {
                    ScheduleID = reader.GetInt32(0),
                    FlightNumber = reader.GetString(1),
                    RouteID = reader.GetInt32(2),
                    AircraftID = reader.GetInt32(3),
                    DepartureDateTime = reader.GetDateTime(4),
                    ArrivalDateTime = reader.GetDateTime(5),
                    EconomyClassPrice = reader.GetDecimal(6),
                    BusinessClassPrice = reader.GetDecimal(7),
                    Status = reader.GetString(8)

                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return flightSchedules;
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

        //find aircraft
        public Aircraft FindAircraft(int aircraftid)
        {
            Aircraft aircraft = new Aircraft();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Aircraft
 WHERE AircraftID = @selectedAircraftID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedAircraftID", aircraftid);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    aircraft.AircraftID = aircraftid;
                    aircraft.AircraftModel = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    // (char) 0 - ASCII Code 0 - null value
                    aircraft.NumEconomySeat = !reader.IsDBNull(2) ?
                    reader.GetInt32(2) : 0;
                    aircraft.NumBusinessSeat = !reader.IsDBNull(3) ?
                    reader.GetInt32(3) : 0;
                    aircraft.DateLastMaintenance = !reader.IsDBNull(4) ?
                    reader.GetDateTime(4) : (DateTime?)null;
                    aircraft.Status = !reader.IsDBNull(5) ?
                    reader.GetString(5) : null;
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return aircraft;
        }

        
        public List<FlightSchedule> GetAvailableFlights()
        {
            List<FlightSchedule> flightSchedules = new List<FlightSchedule>();
           
            SqlCommand cmd = conn.CreateCommand();
            
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE (AircraftID IS NULL AND DepartureDateTime >= GETDATE())";
            
            conn.Open();
            
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                flightSchedules.Add(
                new FlightSchedule
                {
                    ScheduleID = reader.GetInt32(0),
                    FlightNumber = reader.GetString(1),
                    RouteID = reader.GetInt32(2),
                    //AircraftID = (int)(!reader.IsDBNull(7) ? reader.GetInt32(7) : 0),
                    DepartureDateTime = reader.GetDateTime(4),
                    ArrivalDateTime = reader.GetDateTime(5),
                    EconomyClassPrice = reader.GetDecimal(6),
                    BusinessClassPrice = reader.GetDecimal(7),
                    Status = reader.GetString(8)

                }
                );
            }
            conn.Close();
            return flightSchedules;

        }

        public bool CheckFlight(int aircraftid , int scheduleid )
        {
            SqlCommand cmd = conn.CreateCommand();
            FlightSchedule flightSchedule = FindSchedule(scheduleid);
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE (AircraftID = @aircraftid  AND DepartureDateTime BETWEEN @DepartureDateTime AND @ArrivalDateTime)";
            cmd.Parameters.AddWithValue("@DepartureDateTime", flightSchedule.DepartureDateTime);
            cmd.Parameters.AddWithValue("@aircraftid", aircraftid);
            cmd.Parameters.AddWithValue("@ArrivalDateTime", flightSchedule.ArrivalDateTime);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            bool valid = reader.Read();
            conn.Close();
            return valid;

        }


        public FlightSchedule FindSchedule(int scheduleid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE ScheduleID = @scheduleid";
            cmd.Parameters.AddWithValue("@scheduleid", scheduleid);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            FlightSchedule flightSchedule = new FlightSchedule();
       
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    flightSchedule.ScheduleID = reader.GetInt32(0);
                    flightSchedule.FlightNumber = reader.GetString(1);
                    flightSchedule.RouteID = reader.GetInt32(2);
                    flightSchedule.DepartureDateTime = reader.GetDateTime(4);
                    flightSchedule.ArrivalDateTime = reader.GetDateTime(5);
                    flightSchedule.EconomyClassPrice = reader.GetDecimal(6);
                    flightSchedule.BusinessClassPrice = reader.GetDecimal(7);
                    flightSchedule.Status = reader.GetString(8);
                };
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return flightSchedule;
        }


        //assign aircraft flight
        public int Assign(int aircraftid , int scheduleid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE FlightSchedule SET AircraftID=@aircraftid WHERE ScheduleID = @scheduleid";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@aircraftid", aircraftid);
            cmd.Parameters.AddWithValue("@scheduleid", scheduleid);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;

        }


        public bool CheckMaintenance(int aircraftid)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM FlightSchedule WHERE (AircraftID = @aircraftid AND DepartureDateTime >= GETDATE()) ";
            cmd.Parameters.AddWithValue("@aircraftid", aircraftid);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            bool has = reader.Read();
            conn.Close();
            return has;

        }

        //ask 
        public int Delete(int staffId)
        {
            
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a staff record specified by a Staff ID
            SqlCommand cmdUpdate1 = conn.CreateCommand();
            SqlCommand cmdUpdate2 = conn.CreateCommand();
            SqlCommand cmdDelete1 = conn.CreateCommand();
            SqlCommand cmdDelete2 = conn.CreateCommand();

            cmdUpdate1.CommandText = @"UPDATE Staff SET SupervisorID = null WHERE StaffID = @selectStaffID";
            cmdUpdate1.Parameters.AddWithValue("@selectStaffID", staffId);


            cmdUpdate2.CommandText = @"UPDATE Branch SET MgrID = null WHERE MgrID = @selectStaffID";
            cmdUpdate2.Parameters.AddWithValue("@selectStaffID", staffId);


            cmdDelete1.CommandText = @"DELETE FROM StaffContact WHERE StaffID = @selectStaffID";
            cmdDelete1.Parameters.AddWithValue("@selectStaffID", staffId);


            cmdDelete2.CommandText = @"DELETE FROM Staff WHERE StaffID = @selectStaffID";
            cmdDelete2.Parameters.AddWithValue("@selectStaffID", staffId);


            //Execute the DELETE SQL to remove the staff record
            int rowAffected = 0;
            //Open a database connection
            conn.Open();

            rowAffected += cmdUpdate1.ExecuteNonQuery();
            rowAffected += cmdUpdate2.ExecuteNonQuery();
            rowAffected += cmdDelete1.ExecuteNonQuery();
            rowAffected += cmdDelete2.ExecuteNonQuery();

            //Close database connection
            conn.Close();
            //Return number of row of staff record updated or deleted
            return rowAffected;
        }


        //update aircraft status 
        public int Update(Aircraft aircraft)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Aircraft SET Status = @status WHERE AircraftID = @aircraftid";
            
            cmd.Parameters.AddWithValue("@aircraftid", aircraft.AircraftID);
            cmd.Parameters.AddWithValue("@status", aircraft.Status);
            
            
            conn.Open();
            
            int count = cmd.ExecuteNonQuery();
            
            conn.Close();
            return count;

        }

        public List<Aircraft> GetMaintenanceAircraft()
        {
            List<Aircraft> aircraftList = new List<Aircraft>();

            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"SELECT * FROM Aircraft WHERE DateLastMaintenance < DATEADD(DAY, -30, GETDATE());";

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
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
                    Status = reader.GetString(5)
                });
            }
            conn.Close();
            return aircraftList;

        }

    } 
}
