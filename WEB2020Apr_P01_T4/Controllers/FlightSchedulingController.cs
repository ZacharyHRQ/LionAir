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


        RouteDAL routeDAL = new RouteDAL();
        FlightScheduleDAL flightScheduleDAL = new FlightScheduleDAL();
        BookingDAL bookingDAL = new BookingDAL();

        private  IActionResult CheckAdmin(IActionResult view)
        {
            //Change later (HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff")
            if (false)
            {

                return RedirectToAction("Index", "Home");

            }

            return view;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {

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

            return CheckAdmin(View(routeViewModel));
            
        }

        [Route("FlightScheduling/Schedule")]
        public IActionResult Schedule()
        {

            ScheduleViewModel scheduleViewModel = new ScheduleViewModel
            {
                FlightSchedule = new FlightSchedule(),
                CreateRoute = new Route(),
                SearchOption = FlightSchedule.GetTableList(),
                FlightScheduleList = flightScheduleDAL.GetAllFlightSchedule(),
                ShowEditPop = false,
                TicketSize = bookingDAL.GetAllBooking().Count(),

            };
            return CheckAdmin(View("Schedule", scheduleViewModel));
        }


        public IActionResult FlightBooking(int id)
        {



            
            BookingViewModel bookingViewModel = new BookingViewModel
            {
                BookingList = bookingDAL.GetAllBooking().Where(booking => booking.ScheduleID == id),
                CreateRoute = new Route(),
                SearchOption = Booking.GetTableList(),
                FlightSchedule = flightScheduleDAL.GetFlightSchedule(id)

            };

            return CheckAdmin(View(bookingViewModel));
        }


        
        public IActionResult EditSchedule(int id)
        {
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel
            {
                CreateRoute = new Route(),
                SearchOption = FlightSchedule.GetTableList(),
                FlightScheduleList = flightScheduleDAL.GetAllFlightSchedule(),
                ShowEditPop = true,
                TicketSize = bookingDAL.GetAllBooking().Count(),
                

            };

            scheduleViewModel.FlightSchedule = scheduleViewModel.FlightScheduleList.First(s => s.ScheduleID == id);

            return CheckAdmin(View("Schedule", scheduleViewModel));
        }

        public IActionResult AddSchedule(int id)
        {



            RouteViewModel routeViewModel = new RouteViewModel
            {
                RouteList = routeDAL.getAllRoutes(),
                SearchOption = Route.GetTableList(),
                TicketSize = bookingDAL.GetAllBooking().Count(),
                FlightSchedule = new FlightSchedule(),
                CreateRoute = new Route(),
                ShowAddPop = true
            };

            routeViewModel.FlightSchedule.RouteID = id;

            return CheckAdmin(View("Index", routeViewModel));
         
        }




        [HttpPost]
        public IActionResult SaveRoute(Route route)
        {

            if (ModelState.IsValid)
            {

                //Insert the data
                routeDAL.InsertData(route);

                return RedirectToAction("Index");
            }
            else
            {
                RouteViewModel routeViewModel = new RouteViewModel
                {
                    RouteList = routeDAL.getAllRoutes(),
                    SearchOption = Route.GetTableList(),
                    TicketSize = bookingDAL.GetAllBooking().Count(),
                    FlightSchedule = new FlightSchedule(),
                    CreateRoute = new Route(),
                    ShowAddPop = false,
                    ShowRoutePop = true
                };



                return View("Index", routeViewModel);

            }


        }



        [HttpPost]
        [Route("FlightScheduling/SaveSchedule/{id}/{isEdit}")]
        public IActionResult SaveSchedule(FlightSchedule flightSchedule, int id, bool isEdit)
        {
            int RouteID = id;


            if (ModelState.IsValid)
            {

                Route route = routeDAL.getAllRoutes().First(r => r.RouteID == id);

                flightSchedule.RouteID = id;
                flightSchedule.ArrivalDateTime = ((DateTime)flightSchedule.DepartureDateTime).AddHours((double)route.FlightDuration);


                //Insert the data
                flightScheduleDAL.InsertData(flightSchedule);
                return RedirectToAction("Index");
            }
            else
            {
           

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

                return View("Index", routeViewModel);
                

    
            }


        }

        [HttpPost]
        public IActionResult UpdateSchedule(FlightSchedule flightSchedule)
        {

            //Insert the data
            flightScheduleDAL.Update(flightSchedule);

            return RedirectToAction("Schedule");
        }

    }
}
