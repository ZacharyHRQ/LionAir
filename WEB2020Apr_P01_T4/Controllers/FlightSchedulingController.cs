using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.ViewModel;
using WEB2020Apr_P01_T4.Models;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class FlightSchedulingController : Controller
    {


        RouteDAL routeDAL = new RouteDAL();
        FlightScheduleDAL flightScheduleDAL = new FlightScheduleDAL();
        BookingDAL bookingDAL = new BookingDAL();




        // GET: /<controller>/
        public IActionResult Index()
        {
            ScheduleRouteViewModel scheduleRouteViewModel = new ScheduleRouteViewModel
            {
                FlightScheduleList = flightScheduleDAL.GetAllFlightSchedule(),
                ScheduleViewModel = new ScheduleViewModel{
                    RouteList = routeDAL.getAllRoutes(),
                    CreateSchedule = new FlightSchedule()
                },
                CreateRoute = new Route(),
                
                
            };
            return View(scheduleRouteViewModel);
        }


        public IActionResult FlightBooking(int id)
        {


            //Get Booking
            var bookingList = bookingDAL.GetAllBooking().Where(booking => booking.ScheduleID == id);


            return View(bookingList);
        }

        

        [HttpPost]
        public IActionResult SaveRoute(ScheduleRouteViewModel scheduleRouteViewModel)
        {
         
            //Insert the data
            routeDAL.insertData(scheduleRouteViewModel.CreateRoute);
         
            return RedirectToAction("Index");
        }



        [HttpPost]
        public IActionResult SaveSchedule(ScheduleViewModel scheduleViewModel)
        {

            scheduleViewModel.RouteList = routeDAL.getAllRoutes();
            scheduleViewModel.Route = (Route)scheduleViewModel.RouteList.Where(r => r.RouteID == scheduleViewModel.CreateSchedule.RouteID).First();
            var r = scheduleViewModel.RouteList.Where(r => r.RouteID == scheduleViewModel.CreateSchedule.RouteID);


            //Calculate
            scheduleViewModel.CalculateArrival();
            

            //Insert the data
            flightScheduleDAL.InsertData(scheduleViewModel.CreateSchedule);

            return RedirectToAction("Index");
        }

    }
}
