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


        private static RouteDAL routeDAL = new RouteDAL();
        private static FlightScheduleDAL flightScheduleDAL = new FlightScheduleDAL();

       static ScheduleRouteViewModel scheduleRouteViewModel = new ScheduleRouteViewModel
        {
            FlightScheduleList = flightScheduleDAL.getAllFlightSchedule(),
            RouteList = routeDAL.getAllRoutes()
        };

        // GET: /<controller>/
        public IActionResult Index()
        {
            
            return View(scheduleRouteViewModel);
        }

        
        public IActionResult Route(Route route)
        {

            scheduleRouteViewModel.RouteList.Add(route);
            return View("Index",scheduleRouteViewModel);
        }


    }
}
