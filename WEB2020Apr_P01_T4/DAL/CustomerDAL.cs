using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB2020Apr_P01_T4.Models;
using Microsoft.AspNetCore.Http;

namespace WEB2020Apr_P01_T4.DAL
{
    public class CustomerDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CustomerDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("LionAirConnectionString");

            //Instantiate a SqlConnection object with Connection String read.
            conn = new SqlConnection(strConn);
        }
       
        public int Add(Register register)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will return the auto-generated CustomerID after insertion
            cmd.CommandText = @"INSERT INTO Customer (CustomerName, Nationality, BirthDate, TelNo, EmailAddr, Password)
                                OUTPUT INSERTED.CustomerID 
                                VALUES(@customername, @nationality, @birthdate, @telno, @emailaddr, @password)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            if (register.BirthDate == null)
            {
                cmd.Parameters.AddWithValue("@birthdate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@birthdate", register.BirthDate);
            }
            if (register.TelNo == null)
            {
                cmd.Parameters.AddWithValue("@telno", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@telno", register.TelNo);
            }
            if (register.Nationality == null)
            {
                cmd.Parameters.AddWithValue("@nationality", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@nationality", register.Nationality);
            }
            cmd.Parameters.AddWithValue("@customername", register.CustomerName);
            cmd.Parameters.AddWithValue("@emailaddr", register.EmailAddr);
            cmd.Parameters.AddWithValue("@password", register.Password);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated CustomerID after executing the INSERT SQL statement
            register.CustomerID = (int)cmd.ExecuteScalar();

            //A Connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return register.CustomerID;
        }
        public bool IsEmailExist(string email, int customerId)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CustomerID FROM Customer
                                WHERE EmailAddr=@selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows) //Records found
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != customerId)
                        //The email address is used by another customer
                        emailFound = true;
                }
            }
            else //No record
            {
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }

        public bool VaildCustomer(String email, String password, out int customerID)
        {

            customerID = 0;
            try
            {

                // writing sql query  
                SqlCommand cm = new SqlCommand(String.Format("SELECT CustomerID FROM Customer WHERE UPPER(EmailAddr) = UPPER('{0}') AND Password = '{1}'",
                    email.ToUpper(),
                    password
                    ), conn);


                // Opening Connection  
                conn.Open();
                // Executing the SQL query  
                SqlDataReader sqlDataReader = cm.ExecuteReader();

                if (sqlDataReader.Read())
                {
                    customerID = sqlDataReader.GetInt32(0);
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

        public void Update(ChangePassword changePassword, int customerid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Customer SET Password=@newpassword
                                WHERE CustomerID =" + customerid.ToString();

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@newpassword", changePassword.NewPassword);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
        }
        public List<Aircraftschedule> GetAllAircraftSchedule()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT ScheduleID, DepartureCity, DepartureCountry, ArrivalCity, ArrivalCountry, DepartureDateTime, ArrivalDateTime, EconomyClassPrice, BusinessClassPrice, Status FROM FlightRoute
                                INNER JOIN FlightSchedule
                                ON FlightRoute.RouteID = FlightSchedule.RouteID
                                WHERE DepartureDateTime > DATEADD(DAY, 1, GETDATE())";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a AircraftSchedule list
            List<Aircraftschedule> aircraftscheduleList = new List<Aircraftschedule>();
            while (reader.Read())
            {
                aircraftscheduleList.Add(
                    new Aircraftschedule
                    {
                        ScheduleID = reader.GetInt32(0),
                        DepartureCity = reader.GetString(1),
                        DepartureCountry = reader.GetString(2),
                        ArrivalCity = reader.GetString(3),
                        ArrivalCountry = reader.GetString(4),
                        // if null value in db, assign datetime null value
                        DepartureDateTime = !reader.IsDBNull(5) ?
                                            reader.GetDateTime(5) : (DateTime?)null,
                        ArrivalDateTime = !reader.IsDBNull(6) ?
                                            reader.GetDateTime(6) : (DateTime?)null,
                        EconomyClassPrice = reader.GetDecimal(7),
                        BusinessClassPrice = reader.GetDecimal(8),
                        Status = reader.GetString(9)
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return aircraftscheduleList;
        }

        public Aircraftschedule GetSchedule(int ScheduleId)
        {
            Aircraftschedule passenger = new Aircraftschedule();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT ScheduleID, DepartureCity, DepartureCountry, ArrivalCity, ArrivalCountry, DepartureDateTime, ArrivalDateTime, EconomyClassPrice, BusinessClassPrice, Status FROM FlightRoute
                                INNER JOIN FlightSchedule
                                ON FlightRoute.RouteID = FlightSchedule.RouteID
                                WHERE DepartureDateTime > DATEADD(DAY, 1, GETDATE()) AND ScheduleID = @selectedScheduleID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter "bookingid"
            cmd.Parameters.AddWithValue("@selectedScheduleID", ScheduleId);

            //Open a database connection
            conn.Open();
            //Execute SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill passenger object with values from the data reader
                    passenger.ScheduleID = ScheduleId;
                    passenger.DepartureCity = reader.GetString(1);
                    passenger.DepartureCountry = reader.GetString(2);
                    passenger.ArrivalCity = reader.GetString(3);
                    passenger.ArrivalCountry = reader.GetString(4);
                    passenger.DepartureDateTime = reader.GetDateTime(5);
                    passenger.ArrivalDateTime = reader.GetDateTime(6);
                    passenger.EconomyClassPrice = reader.GetDecimal(7);
                    passenger.BusinessClassPrice = reader.GetDecimal(8);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return passenger;
        }
        public int Add(Aircraftschedule booking)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will return the auto-generated CustomerID after insertion
            cmd.CommandText = @"INSERT INTO Booking (CustomerID, ScheduleID, PassengerName, PassportNumber, Nationality, SeatClass, AmtPayable, Remarks, DateTimeCreated)
                                OUTPUT INSERTED.BookingID 
                                VALUES(@CustomerID, @ScheduleID, @PassengerName, @PassportNumber, @Nationality, @SeatClass, @AmtPayable, @Remarks, @DateTimeCreated)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            if (booking.Remarks == null)
            {
                cmd.Parameters.AddWithValue("@Remarks", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Remarks", booking.Remarks);
            }
            if (booking.SeatClass == "Economy")
            {
                cmd.Parameters.AddWithValue("@AmtPayable", booking.EconomyClassPrice);
            }
            else
            {
                cmd.Parameters.AddWithValue("@AmtPayable", booking.BusinessClassPrice);
            }
            cmd.Parameters.AddWithValue("@CustomerID", booking.CustomerID);
            cmd.Parameters.AddWithValue("@ScheduleID", booking.ScheduleID);
            cmd.Parameters.AddWithValue("@PassengerName", booking.PassengerName);
            cmd.Parameters.AddWithValue("@PassportNumber", booking.PassportNumber);
            cmd.Parameters.AddWithValue("@Nationality", booking.Nationality);
            cmd.Parameters.AddWithValue("@SeatClass", booking.SeatClass);
            cmd.Parameters.AddWithValue("@DateTimeCreated", booking.DateTimeCreated);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated CustomerID after executing the INSERT SQL statement
            booking.BookingID = (int)cmd.ExecuteScalar();

            //A Connection should be closed after operations.
            conn.Close();

            //Return id when no error occurs.
            return booking.BookingID;
        }
        public List<Aircraftschedule> ViewAirTicket(int customerid)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT BookingID, PassengerName, DepartureCity, DepartureCountry, DepartureDateTime, ArrivalCity, ArrivalCountry, ArrivalDateTime FROM Booking
                                INNER JOIN FlightSchedule
                                ON Booking.ScheduleID = FlightSchedule.ScheduleID
                                INNER JOIN FlightRoute
                                ON FlightSchedule.RouteID = FlightRoute.RouteID
                                WHERE CustomerID = " + customerid;
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a ViewAirTicket list
            List<Aircraftschedule> viewAirTicketList = new List<Aircraftschedule>();
            while (reader.Read())
            {
                viewAirTicketList.Add(
                    new Aircraftschedule
                    {
                        BookingID = reader.GetInt32(0),
                        PassengerName = reader.GetString(1),
                        DepartureCity = reader.GetString(2),
                        DepartureCountry = reader.GetString(3),
                        DepartureDateTime = reader.GetDateTime(4),
                        ArrivalCity = reader.GetString(5),
                        ArrivalCountry = reader.GetString(6),
                        ArrivalDateTime = reader.GetDateTime(7)
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return viewAirTicketList;
        }
        public Aircraftschedule GetDetails(int bookingId)
        {
            Aircraftschedule passenger = new Aircraftschedule();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT BookingID, PassengerName, PassportNumber, Nationality, FlightNumber, DepartureCity, DepartureCountry, DepartureDateTime, ArrivalCity, ArrivalCountry, ArrivalDateTime, FlightDuration, SeatClass, AmtPayable, Remarks FROM BOOKING
                                INNER JOIN FlightSchedule
                                ON Booking.ScheduleID = FlightSchedule.ScheduleID
                                INNER JOIN FlightRoute
                                ON FlightSchedule.RouteID = FlightRoute.RouteID
                                WHERE BookingID = @selectedBookingID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter "bookingid"
            cmd.Parameters.AddWithValue("@selectedBookingID", bookingId);

            //Open a database connection
            conn.Open();
            //Execute SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill passenger object with values from the data reader
                    passenger.BookingID = bookingId;
                    passenger.PassengerName = reader.GetString(1);
                    passenger.PassportNumber = reader.GetString(2);
                    passenger.Nationality = reader.GetString(3);
                    passenger.FlightNumber = reader.GetString(4);
                    passenger.DepartureCity = reader.GetString(5);
                    passenger.DepartureCountry = reader.GetString(6);
                    passenger.DepartureDateTime = reader.GetDateTime(7);
                    passenger.ArrivalCity = reader.GetString(8);
                    passenger.ArrivalCountry = reader.GetString(9);
                    passenger.ArrivalDateTime = reader.GetDateTime(10);
                    passenger.FlightDuration = reader.GetInt32(11);
                    passenger.SeatClass = reader.GetString(12);
                    passenger.AmtPayable = reader.GetDecimal(13);
                    passenger.Remarks = !reader.IsDBNull(14) ?
                        reader.GetString(14) : null;
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return passenger;
        }
        public List<Aircraftschedule> AboutUSGetAllAircraftSchedule(string from, string to)
        {
            //Create a SqlCommand object from connection object
      
            SqlCommand cmd = new SqlCommand(string.Format("SELECT ScheduleID, DepartureCity, DepartureCountry, ArrivalCity, ArrivalCountry, DepartureDateTime, ArrivalDateTime, EconomyClassPrice, BusinessClassPrice, Status FROM FlightRoute INNER JOIN FlightSchedule ON FlightRoute.RouteID = FlightSchedule.RouteID WHERE DepartureDateTime > DATEADD(DAY, 1, GETDATE()) AND DepartureCountry = '{0}' AND ArrivalCountry = '{1}'", from, to), conn);

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter "From" and "To"

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a AircraftSchedule list
            List<Aircraftschedule> aircraftscheduleList = new List<Aircraftschedule>();
            while (reader.Read())
            {
                aircraftscheduleList.Add(
                    new Aircraftschedule
                    {
                        ScheduleID = reader.GetInt32(0),
                        DepartureCity = reader.GetString(1),
                        DepartureCountry = reader.GetString(2),
                        ArrivalCity = reader.GetString(3),
                        ArrivalCountry = reader.GetString(4),
                        // if null value in db, assign datetime null value
                        DepartureDateTime = !reader.IsDBNull(5) ?
                                            reader.GetDateTime(5) : (DateTime?)null,
                        ArrivalDateTime = !reader.IsDBNull(6) ?
                                            reader.GetDateTime(6) : (DateTime?)null,
                        EconomyClassPrice = reader.GetDecimal(7),
                        BusinessClassPrice = reader.GetDecimal(8),
                        Status = reader.GetString(9)
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return aircraftscheduleList;
        }
    }
}
