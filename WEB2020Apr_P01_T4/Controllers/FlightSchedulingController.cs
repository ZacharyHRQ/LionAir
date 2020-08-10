using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.ViewModel;
using WEB2020Apr_P01_T4.Models;
using Microsoft.AspNetCore.Http;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class FlightSchedulingController : Controller
    {

        //Member variabvles 
        private RouteDAL routeDAL = new RouteDAL();  // Data Access layer for route
        private FlightScheduleDAL flightScheduleDAL = new FlightScheduleDAL(); // Data Access layer for Schedule 
        private BookingDAL bookingDAL = new BookingDAL(); // Data access layer for Booking 

        /**
         * 
         * This is a method used to check
         * if the person accessing this is 
         * a admin if it is, return the view if 
         * not return back to the index page 
         * 
         */
        ///<param name="view">View thats needs to be displayed</param>
        ///<returns>The view that needs to be displayed</returns>
        private  IActionResult CheckAdmin(IActionResult view)
        {

            // Check if the HTTP session with role is null and not adminn
            // true: Return to the index
            // false: Return the view passed in
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Admin"))
            {
                //Return to the index home page
                return RedirectToAction("Index", "Home");

            }
            else
            {
                //Return the view passed into this functions
                return view;
            }
        }

        // GET: /<controller>/
        /// <summary>
        ///
        /// This is to return the index page
        /// of the flightSchdule controller
        /// whcih is the route table view based
        /// on the view model
        /// 
        /// </summary>
        /// <returns>The index view (Route page)</returns>
        public IActionResult Index()
        {

            //Creates the view model for the index page
            RouteViewModel routeViewModel = new RouteViewModel
            {
                RouteList = routeDAL.getAllRoutes(),
                SearchOption = Route.GetTableList(),
                TicketSize = bookingDAL.GetAllBooking().Count(),
                FlightSchedule = new FlightSchedule(),
                CreateRoute = new Route(),
                ShowAddPop = false,
                ShowRoutePop = false
            };

            //Checks the if it is the admin
            //Returns the index page
            return CheckAdmin(View(routeViewModel));
            
        }


        /// <summary>
        ///
        /// This is return the Schedule.cshtml page
        /// to see the schdule view. This also helps
        /// with the filtering based on the routeid (@param id).
        /// 
        /// </summary>
        /// 
        /// <param name="id">The is the route id that will be used
        /// to filter the schdeule if ii is not null</param>
        /// 
        /// <returns>Return the Schedule.cshtml view</returns>
        public IActionResult Schedule(int? id)
        {
            //Create the view model needed to be passed in
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel
            {
                SearchOption = FlightSchedule.GetTableList(),
                TicketSize = bookingDAL.GetAllBooking().Count(),

            };

            //To check if the id is null
            //true: get filtered list and set the routeID
            //false: Get all the schedules 
            if (id != null)
            {
                //Get the value of the id
                int routeId = id.Value;

                //Get the filtered list based on the route id
                scheduleViewModel.FlightScheduleList = flightScheduleDAL.GetAllFlightSchedule(routeId);
                scheduleViewModel.RouteID = routeId;// Set the routeid to be visible by the user

                //To info the view this is a filtered list
                ViewBag.Flitered = true;


            }
            else
            {
                //Get all the fligh schedueles 
                scheduleViewModel.FlightScheduleList = flightScheduleDAL.GetAllFlightSchedule();

                //To info the view this is not a filtered list
                ViewBag.Flitered = false;

            }


            //This is to show the Economy seats and bussiness seats to be compared
            List<String> ecoOccupied = new List<string>();
            List<String> bOccupied = new List<string>();

            //if the list is not null
            if (scheduleViewModel.FlightScheduleList != null)
            {
                //Look through the list for each flightSchedule
                foreach (FlightSchedule flightSchedule in scheduleViewModel.FlightScheduleList)
                {
                    //if the aircraft is null means no point calculating the value
                    //true: Create a string to be displayed by getting the bookings for that schedule
                    //false: Just show a message that is "Currently No Aircarft ID"
                    if (flightSchedule.AircraftID != 0)
                    {
                        //Get the seats for the aircraft id
                        List<int> values = flightScheduleDAL.GetSeats(flightSchedule.AircraftID);
                        //create string to compare it with the booking 
                        ecoOccupied.Add(flightScheduleDAL.CountEconomySeat(flightSchedule.ScheduleID).ToString() + "/" + values[0]);
                        bOccupied.Add(flightScheduleDAL.CountBusinessSeat(flightSchedule.ScheduleID).ToString() + "/" + values[1]);
                    }
                    else
                    {
                        //Just add in a message 
                        ecoOccupied.Add("Currently No Aircarft ID");
                        bOccupied.Add("Currently No Aircarft ID");
                    }

                }

                //Put this is a view bag
                ViewBag.ecoOccupied = ecoOccupied;
                ViewBag.bOccupied = bOccupied;
            }


            //Return the Schedule.cshtml view after checkinf if it is admin
            return CheckAdmin(View("Schedule", scheduleViewModel));
        }


        ///
        /// <summary>
        /// This shows the booking
        /// for the schedule selected 
        /// </summary>
        /// 
        /// <param name="id">The schedule ID</param>
        /// <returns>Returns the FlightBooking.cshtml</returns>
        public IActionResult FlightBooking(int id)
        {
            //Creates the view model
            BookingViewModel bookingViewModel = new BookingViewModel
            {
                BookingList = bookingDAL.GetAllBooking().Where(booking => booking.ScheduleID == id),
                SearchOption = Booking.GetTableList(),
                FlightSchedule = flightScheduleDAL.GetFlightSchedule(id)

            };

            //Returns the FlightBooking.cshtml after checking if it is admin
            return CheckAdmin(View(bookingViewModel));
        }




        /// <summary>
        /// The is to return a __ScheduleForm
        /// </summary>
        /// <param name="id">The schedule ID</param>
        /// <returns>Returns the partial view to be loaded into a div</returns>
        public ActionResult EditSchedule(int id)
        {
            //Gets the FlightSchedule based on the id
            FlightSchedule fs = flightScheduleDAL.GetFlightSchedule(id);

            //Returns the partial view
            return PartialView("_ScheduleForm", fs);
        }

        /// <summary>
        ///
        /// This to edit the route so the
        /// route will pop up
        /// 
        /// </summary>
        /// <param name="id">< The route id/param>
        /// <returns>Return to the index but with edit view pop up</returns>
        public IActionResult EditRoute(int id)
        {

            RouteViewModel routeViewModel = new RouteViewModel
            {
                RouteList = routeDAL.getAllRoutes(),
                SearchOption = Route.GetTableList(),
                TicketSize = bookingDAL.GetAllBooking().Count(),
                FlightSchedule = new FlightSchedule(), 
                CreateRoute = routeDAL.getAllRoutes().First(r => r.RouteID == id), //Get the route need to be edited
                ShowRoutePop = true //TO pop the route form
            };

            //Set the route id in the flightSehedule object
            routeViewModel.FlightSchedule.RouteID = id;

            //Return to the index but with edit view pop up
            return CheckAdmin(View("Index", routeViewModel));

        }

        /// <summary>
        /// 
        /// This is to add the
        /// schedule from the index page
        /// 
        /// </summary>
        /// <param name="id">The route id</param>
        /// <returns>Returns the index but with add schedule pop up</returns>
        public IActionResult AddSchedule(int id)
        {


            //Create the model
            RouteViewModel routeViewModel = new RouteViewModel
            {
                RouteList = routeDAL.getAllRoutes(),
                SearchOption = Route.GetTableList(),
                TicketSize = bookingDAL.GetAllBooking().Count(),
                FlightSchedule = new FlightSchedule(),
                CreateRoute = new Route(),
                ShowAddPop = true //To make the add schedule pop up
            };

            //Set the route id in the flightSehedule object
            routeViewModel.FlightSchedule.RouteID = id;

            //Returns the index but with add schedule pop up
            return CheckAdmin(View("Index", routeViewModel));
         
        }



        /// <summary>
        /// 
        /// The method used in http post
        /// to save data into sql
        /// 
        /// </summary>
        /// <param name="route">The route object from the form</param>
        /// <returns>A view depending of the condition</returns>
        [HttpPost]
        public IActionResult SaveRoute(Route route)
        {
            TempData["errorMessage"] = null;
            bool isValid = true;
            //To check if the form is valide
            //true: do some more validation
            //false: set the isvalid to false
            if (ModelState.IsValid)
            {

                // To check if the if arrival country and departure country is the same
                // if they are the check if the city is the same
                //true: Add tempData
                //fasle: Check if the route exist
                if ((route.ArrivalCountry == route.DepartureCountry) && (route.ArrivalCity == route.DepartureCity))
                {
                    isValid = false;
                    //Error message
                    TempData["errorMessage"] = "The departure city and arrival city";
                }
                else
                {

                    //Insert the data
                    isValid = routeDAL.InsertData(route);

                    //If adding the data is invaild
                    //true: Add errpr message
                    if (!isValid)
                    {
                        //Add error message
                        TempData["errorMessage"] = "The route already exist";
                    }
                }

            }
            else
            {
                //Set the isvalid to false
                isValid = false;
            }


            //if true return back to the index
            if (isValid)
            {

                return RedirectToAction("Index");

            }
            else //recreate the model show the error message
            {
                RouteViewModel routeViewModel = new RouteViewModel
                {
                    RouteList = routeDAL.getAllRoutes(),
                    SearchOption = Route.GetTableList(),
                    TicketSize = bookingDAL.GetAllBooking().Count(),
                    FlightSchedule = new FlightSchedule(),
                    CreateRoute = new Route(),
                    ShowAddPop = false,
                    ShowRoutePop = true // Show the route form
                };



                return View("Index", routeViewModel);
            }


        }

        /// <summary>
        ///
        /// This used as the HTTP Post to update the
        /// route. 
        /// 
        /// </summary>
        /// <param name="route">The route object from the form</param>
        /// <returns>Return to the index page</returns>
        [HttpPost]
        public IActionResult UpdateRoute(Route route)
        {

            //To check if the form is valide
            //true: do some more validation
            //false: Redirect to the index page with the model
            if (ModelState.IsValid)
            {
                //Check if the FlightDuration not null and ntot 0
                if ((route.FlightDuration != null) && route.FlightDuration != 0) {
                    //Update the route data
                    routeDAL.UpdateData(route.RouteID, (double)route.FlightDuration);
                }
                //Redirect to index action 
                return RedirectToAction("Index");

            }
            else
            {
                //The model
                RouteViewModel routeViewModel = new RouteViewModel
                {
                    RouteList = routeDAL.getAllRoutes(),
                    SearchOption = Route.GetTableList(),
                    TicketSize = bookingDAL.GetAllBooking().Count(),
                    FlightSchedule = new FlightSchedule(),
                    CreateRoute = route,
                    ShowAddPop = false,
                    ShowRoutePop = true
                };


                //Redirect to the index view with view model
                return View("Index", routeViewModel);
            }


        }


        /// <summary>
        ///
        /// This is used has HTTP post
        /// to save data
        /// 
        /// </summary>
        /// <param name="flightSchedule">The flightSchedule oject from the form</param>
        /// <param name="id">The route id</param>
        /// <returns>Returns a view</returns>
        [HttpPost]
        [Route("FlightScheduling/SaveSchedule/{id}")]
        public IActionResult SaveSchedule(FlightSchedule flightSchedule, int id)
        {

            int RouteID = id;

            DateTime? ArrivalDateTime = null;

            //Check if the modeel is vaild
            //true: calcualte arrival date 
            //false: to return a view
            if (ModelState.IsValid)
            {

                //Get the route with the route id
                Route route = routeDAL.getAllRoutes().First(r => r.RouteID == id);

                //If departure date time not null
                if (flightSchedule.DepartureDateTime != null)
                {
                    //Add hours top arrival date
                    ArrivalDateTime = ((DateTime)flightSchedule.DepartureDateTime).AddHours((double)route.FlightDuration);
                }
               

                flightSchedule.RouteID = id;
                flightSchedule.ArrivalDateTime = ArrivalDateTime;


                //Insert the data
                flightScheduleDAL.InsertData(flightSchedule);
                return RedirectToAction("Index");
            }
            else
            {
           
                //Craete a model
                RouteViewModel routeViewModel = new RouteViewModel
                {
                    RouteList = routeDAL.getAllRoutes(),
                    SearchOption = Route.GetTableList(),
                    TicketSize = bookingDAL.GetAllBooking().Count(),
                    FlightSchedule = new FlightSchedule(),
                    CreateRoute = new Route(),
                    ShowAddPop = true
                };

                //Set the route ID again
                routeViewModel.FlightSchedule.RouteID = RouteID;

                //Return back the index
                return View("Index", routeViewModel);
                

    
            }


        }

        /// <summary>
        /// To update the schedule, the used as a
        /// http post 
        /// </summary>
        /// <param name="flightSchedule">The flightSchedule oject from the form</param>
        /// <returns>return a action to redirect to schedule action</returns>
        [HttpPost]
        public IActionResult UpdateSchedule(FlightSchedule flightSchedule)
        {

            //Update data 
            flightScheduleDAL.Update(flightSchedule);

            return RedirectToAction("Schedule");
        }


       /// <summary>
       ///
       /// This to delete schedule
       ///
       /// Not implemented in the view 
       /// </summary>
       /// <param name="id">The schedule id</param>
       /// <param name="routeID">The route id</param>
       /// <returns>To redirect to action</returns>
        [Route("FlightScheduling/DeleteSchedule/{id}/{routeID}")]
        public IActionResult DeleteSchedule(int id, int routeID)
        {

            //Delete the schdule
            flightScheduleDAL.Delete(id);

            //To check if the route id is 0
            if (routeID == 0)
            {
                return RedirectToAction("Schedule");
            }
            else
            {
                return RedirectToAction("Schedule", new { id = routeID});
            }

        }
    }
}
