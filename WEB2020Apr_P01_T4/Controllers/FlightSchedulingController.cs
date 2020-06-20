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

        ScheduleRouteViewModel scheduleRouteViewModel = new ScheduleRouteViewModel
        {


            ScheduleViewModel = new ScheduleViewModel
            {
                FlightScheduleList = new FlightScheduleDAL().GetAllFlightSchedule(),
                RouteList = new RouteDAL().getAllRoutes(),
                CreateSchedule = new FlightSchedule(),
                ShowAddPop = false,
                ShowEditPop = false

            },
            CreateRoute = new Route(),


        };


        // GET: /<controller>/
        public IActionResult Index()
        {
          
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

        
        public IActionResult EditSchedule(int id)
        {

            var schdule = flightScheduleDAL.GetAllFlightSchedule();

            scheduleRouteViewModel.ScheduleViewModel.ShowEditPop = true;
            scheduleRouteViewModel.ScheduleViewModel.CreateSchedule = schdule.First(s => s.ScheduleID == id);

            return View("Index", scheduleRouteViewModel);
        }

        public IActionResult AddSchedule(int id)
        {

            scheduleRouteViewModel.ScheduleViewModel.ShowAddPop = true;
            scheduleRouteViewModel.ScheduleViewModel.CreateSchedule.RouteID = id;

            return View("Index", scheduleRouteViewModel);
        }


        [HttpPost]
        public IActionResult SaveSchedule(FlightSchedule flightSchedule, int id)
        {
            int RouteID = id;
            

            if (ModelState.IsValid)
            {
                scheduleRouteViewModel.ScheduleViewModel.CreateSchedule = flightSchedule;
                scheduleRouteViewModel.ScheduleViewModel.CreateSchedule.RouteID = RouteID;

                scheduleRouteViewModel.ScheduleViewModel.Route = scheduleRouteViewModel.ScheduleViewModel.RouteList.First(r => r.RouteID == RouteID);

                //Calculate
                scheduleRouteViewModel.ScheduleViewModel.CalculateArrival();


                //Insert the data
                flightScheduleDAL.InsertData(scheduleRouteViewModel.ScheduleViewModel.CreateSchedule);
                return RedirectToAction("Index");
            }
            else
            {
                scheduleRouteViewModel.ScheduleViewModel.CreateSchedule.RouteID = RouteID;
                scheduleRouteViewModel.ScheduleViewModel.ShowAddPop = true;
                return View("Index", scheduleRouteViewModel);
            }

            
        }

        [HttpPost]
        public IActionResult UpdateSchedule(FlightSchedule flightSchedule)
        {

            //Insert the data
            flightScheduleDAL.Update(flightSchedule);

            return RedirectToAction("Index");
        }

    }
}
