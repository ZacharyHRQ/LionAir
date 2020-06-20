using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.DAL
{
    public class BookingDAL : BaseDAL
    {

        private static readonly int BookingID = 0;
        private static readonly int CustomerID = 1;
        private static readonly int ScheduleID = 2;
        private static readonly int PassengerName = 3;
        private static readonly int PassportNumber = 4;
        private static readonly int Nationality = 5;
        private static readonly int SeatClass = 6;
        private static readonly int AmtPayable = 7;
        private static readonly int Remarks = 8;
        private static readonly int DateTimeCreated = 9;


        public BookingDAL(){}


        public List<Booking> GetAllBooking()
        {
            List<Booking> bookingList = new List<Booking>();
            try
            {

                // writing sql query  
                SqlCommand cm = new SqlCommand("SELECT * FROM Booking", con);

                //Open the connection
                con.Open();

                //Excuting the query
                SqlDataReader sqlDataReader = cm.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    bookingList.Add(new Booking
                    {
                        BookingID = sqlDataReader.GetInt32(BookingID),
                        CustomerID = sqlDataReader.GetInt32(CustomerID),
                        ScheduleID = sqlDataReader.GetInt32(ScheduleID),
                        PassengerName = sqlDataReader.GetString(PassengerName),
                        PassportNumber = sqlDataReader.GetString(PassportNumber),
                        Nationality = sqlDataReader.GetString(Nationality),
                        SeatClass = sqlDataReader.GetString(SeatClass),
                        AmtPayable =  sqlDataReader.GetDecimal(AmtPayable),
                        Remarks = sqlDataReader.IsDBNull(Remarks) ?  "Null" : sqlDataReader.GetString(Remarks),
                        DateTimeCreated = sqlDataReader.GetDateTime(DateTimeCreated),
                    });
                }


                return bookingList;


            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
