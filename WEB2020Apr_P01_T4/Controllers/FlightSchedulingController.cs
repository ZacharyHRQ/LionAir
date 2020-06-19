using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.ViewModel;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class FlightSchedulingController : Controller
    {


        RouteDAL routeDAL = new RouteDAL();
        FlightScheduleDAL flightScheduleDAL = new FlightScheduleDAL();


        // GET: /<controller>/
        public IActionResult Index()
        {

            ScheduleRouteViewModel scheduleRouteViewModel = new ScheduleRouteViewModel
            {
                FlightScheduleList = flightScheduleDAL.getAllFlightSchedule(),
                RouteList = routeDAL.getAllRoutes()
            };
            
            return View(scheduleRouteViewModel);
        }

        public IActionResult Route()
        {
            return View();
        }


    }
}
