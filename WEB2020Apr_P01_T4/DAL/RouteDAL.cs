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
                        FlightDuration = sqlDataReader.IsDBNull(FlightDuration) ? 0 : sqlDataReader.GetInt32(FlightDuration),



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

        public bool InsertData(Route route)
        {

            if (CheckExistingRoute(route)){
                return false;
            }

            try
            {
                SqlCommand cm;
                if (route.FlightDuration != null)
                {
                    // writing sql query  
                     cm = new SqlCommand(String.Format("INSERT INTO FlightRoute " +
                        "(DepartureCity, DepartureCountry, ArrivalCity, ArrivalCountry, FlightDuration)values('{0}', '{1}', '{2}', '{3}', {4})"
                        , route.DepartureCity,
                        route.DepartureCountry,
                        route.ArrivalCity,
                        route.ArrivalCountry,
                        route.FlightDuration
                        ), con);
                }
                else
                {
                    // writing sql query  
                    cm = new SqlCommand(String.Format("INSERT INTO FlightRoute " +
                        "(DepartureCity, DepartureCountry, ArrivalCity, ArrivalCountry)values('{0}', '{1}', '{2}', '{3}')"
                        , route.DepartureCity,
                        route.DepartureCountry,
                        route.ArrivalCity,
                        route.ArrivalCountry
                        ), con);
                }
     


                // Opening Connection  
                con.Open();
                // Executing the SQL query  
                cm.ExecuteNonQuery();

                return true;
                
            }
            catch (Exception e)
            {
                return false;
            }
            // Closing the connection  
            finally
            {
                con.Close();
            }
        }



        private bool CheckExistingRoute(Route route)
        {
            try
            {

                // writing sql query  
                SqlCommand cm = new SqlCommand(String.Format("SELECT * FROM FlightRoute WHERE " +
                    "DepartureCity='{0}' AND DepartureCountry='{1}' AND ArrivalCity='{2}' AND ArrivalCountry='{3}' AND FlightDuration = {4} ;",
                    route.DepartureCity,
                    route.DepartureCountry,
                    route.ArrivalCity,
                    route.ArrivalCountry,
                    route.FlightDuration
                    ), con);

                //Open the connection
                con.Open();

                //Excuting the query
                SqlDataReader sqlDataReader = cm.ExecuteReader();


                return sqlDataReader.Read();


            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
