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
            string strConn = Configuration.GetConnectionString("MyConnection");

            //Instantiate a SqlConnection object with Connection String read.
            conn = new SqlConnection(strConn);
        }
        public List<Register> GetAllCustomer()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Customer ORDER BY CustomerID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Register> customerList = new List<Register>();
            while (reader.Read())
            {
                customerList.Add(
                    new Register
                    {
                        CustomerID = reader.GetInt32(0),
                        CustomerName = reader.GetString(1),
                        Nationality = reader.GetString(2),
                        BirthDate = reader.GetDateTime(3),
                        TelNo = reader.GetInt32(4),
                        EmailAddr = reader.GetString(5),
                        Password = reader.GetString(6),
                    });
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return customerList;
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
            cmd.Parameters.AddWithValue("@customername", register.CustomerName);
            cmd.Parameters.AddWithValue("@nationality", register.Nationality);
            cmd.Parameters.AddWithValue("@birthdate", register.BirthDate);
            cmd.Parameters.AddWithValue("@telno", register.TelNo);
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
    }
}
