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
                ScheduleForm = new ScheduleForm
                {
                    CreateSchedule = new FlightSchedule(),
                    isFlightSchedule = false
                },
                ShowAddPop = false,
                ShowEditPop = false,
            },
            TicketSize = new BookingDAL().GetAllBooking().Count(),
            CreateRoute = new Route(),
          

        };


        // GET: /<controller>/
        [Route("/FlightScheduling/{isFlightSchedule}")]
        [Route("/FlightScheduling/")]
        public IActionResult Index(bool? isFlightSchedule)
        {

            
            if (isFlightSchedule != null)
            {
                this.scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.isFlightSchedule = (bool)isFlightSchedule;
                
            }

            bool isSchedule = this.scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.isFlightSchedule;

            if (isSchedule)
            {
                scheduleRouteViewModel.SearchOption = FlightSchedule.GetTableList();
            }
            else
            {
                scheduleRouteViewModel.SearchOption = Route.GetTableList();
            }
                

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

            scheduleRouteViewModel.SearchOption = FlightSchedule.GetTableList();
            scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.isFlightSchedule = true;

            scheduleRouteViewModel.ScheduleViewModel.ShowEditPop = true;
            scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.CreateSchedule = schdule.First(s => s.ScheduleID == id);

            return View("Index", scheduleRouteViewModel);
        }

        public IActionResult AddSchedule(int id)
        {

            scheduleRouteViewModel.SearchOption = Route.GetTableList();

            scheduleRouteViewModel.ScheduleViewModel.ShowAddPop = true;
            scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.CreateSchedule.RouteID = id;

            scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.isFlightSchedule = false;

            return View("Index", scheduleRouteViewModel);
        }


        [HttpPost]
        public IActionResult SaveSchedule(FlightSchedule flightSchedule, int id)
        {
            int RouteID = id;
            

            if (ModelState.IsValid)
            {
                scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.CreateSchedule = flightSchedule;
                scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.CreateSchedule.RouteID = RouteID;

                scheduleRouteViewModel.ScheduleViewModel.Route = scheduleRouteViewModel.ScheduleViewModel.RouteList.First(r => r.RouteID == RouteID);

                //Calculate
                scheduleRouteViewModel.ScheduleViewModel.CalculateArrival();


                //Insert the data
                flightScheduleDAL.InsertData(scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.CreateSchedule);
                return RedirectToAction("Index");
            }
            else
            {
                scheduleRouteViewModel.ScheduleViewModel.ScheduleForm.CreateSchedule.RouteID = RouteID;
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
