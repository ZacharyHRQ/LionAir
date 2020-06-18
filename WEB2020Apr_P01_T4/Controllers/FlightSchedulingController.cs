using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.DAL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class FlightSchedulingController : Controller
    {
        RouteDAL routeDAL = new RouteDAL(); 
        // GET: /<controller>/
        public IActionResult Index()
        {
            
            return View(routeDAL.getAllRoutes());
        }
    }
}
