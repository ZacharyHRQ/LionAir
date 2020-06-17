using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.Models;
using WEB2020Apr_P01_T4.DAL;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class AircraftAssignmentController : Controller
    {

        private AircraftDAL aircraftContext = new AircraftDAL();

        // GET: /<controller>/Display
        public IActionResult DisplayAircraft()
        {
            List<Aircraft> aircraftList = aircraftContext.GetAllAircraft();
            return View(aircraftList);
        }

        // GET: /<controller>/Display
        public IActionResult AssignAircraft()
        {
            return View();
        }

        // GET: /<controller>/Display
        public IActionResult CreateAircraft()
        {
            return View();
        }

        // GET: /<controller>/Display
        public IActionResult UpdateAircraft()
        {
            return View();
        }




        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


    }
}
