using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.DAL
{
    public class RouteDAL : BaseDAL
    {
  
        private static int RouteID = 0;
        private static int DepartureCity = 1;
        private static int DepartureCountry = 2;
        private static int ArrivalCity = 3;
        private static int ArrivalCountry = 4;
        private static int FlightDuration = 5;
       

        public RouteDAL(){}

        public List<Route> getAllRoutes()
        {
            List<Route> routeList = new List<Route>();
            try
            {

                // writing sql query  
                SqlCommand cm = new SqlCommand("SELECT * FROM FlightRoute", con);

                //Open the connection
                con.Open();

                //Excuting the query
                SqlDataReader sqlDataReader = cm.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    routeList.Add(new Route {

                        RouteID = sqlDataReader.GetInt32(RouteID),
                        DepartureCity = sqlDataReader.GetString(DepartureCity),
                        DepartureCountry = sqlDataReader.GetString(DepartureCountry),
                        ArrivalCity = sqlDataReader.GetString(ArrivalCity),
                        ArrivalCountry = sqlDataReader.GetString(ArrivalCountry),
                        FlightDuration = sqlDataReader.GetInt32(FlightDuration),



                    });
                }


                return routeList;


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
