using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.DAL
{
    public class FlightScheduleDAL:BaseDAL
    {
        private static int ScheduleID = 0;
        private static int FlightNumber = 1;
        private static int RouteID = 2;
        private static int AircraftID = 3;
        private static int DepartureDateTime = 4;
        private static int ArrivalDateTime = 5;
        private static int EconomyClassPrice = 6;
        private static int BusinessClassPrice = 7;
        private static int Status = 8;


        public FlightScheduleDAL(){}

        public List<FlightSchedule> getAllFlightSchedule()
        {
            List<FlightSchedule> flightScheduleList = new List<FlightSchedule>();
            try
            {

                // writing sql query  
                SqlCommand cm = new SqlCommand("SELECT * FROM FlightSchedule", con);

                //Open the connection
                con.Open();

                //Excuting the query
                SqlDataReader sqlDataReader = cm.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    flightScheduleList.Add(new FlightSchedule
                    {

                        ScheduleID = sqlDataReader.GetInt32(ScheduleID),
                        FlightNumber = sqlDataReader.GetString(FlightNumber),
                        RouteID = sqlDataReader.GetInt32(RouteID),
                        AircraftID = sqlDataReader.GetInt32(AircraftID),
                        DepartureDateTime = sqlDataReader.GetDateTime(DepartureDateTime),
                        ArrivalDateTime = sqlDataReader.GetDateTime(ArrivalDateTime),
                        EconomyClassPrice = sqlDataReader.GetDecimal(EconomyClassPrice),
                        BusinessClassPrice = sqlDataReader.GetDecimal(BusinessClassPrice),
                        Status = sqlDataReader.GetString(Status),


                    });
                }


                return flightScheduleList;


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
