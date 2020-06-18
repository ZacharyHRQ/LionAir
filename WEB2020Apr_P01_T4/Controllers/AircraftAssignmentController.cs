using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.Models;
using WEB2020Apr_P01_T4.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult CreateAircraft()
        {
            ViewData["ModelList"] = GetModel();
            return View();
        }

        [HttpPost]
        public IActionResult CreateAircraft(Aircraft aircraft)
        {
            ViewData["ModelList"] = GetModel();
            if(ModelState.IsValid)
            {
                //Add staff record to database
                aircraft.AircraftID = aircraftContext.Add(aircraft);
                //Redirect user to Staff/Index view
                return RedirectToAction("DisplayAircraft");
            }
            else
            {
                //Input validation fails, return to the Create view
                //to display error message
                return View(aircraft);
            }
        
        }


        // GET: /<controller>/Display
        public IActionResult UpdateAircraft()
        {
            return View();
        }

        // GET: /<controller>/Display
        public IActionResult AssignAircraft()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        private List<SelectListItem> GetModel()
        {
            List<SelectListItem> models = new List<SelectListItem>();
            models.Add(new SelectListItem
            {
                Value = "Boeing 747",
                Text = "Boeing 747"
            });
            models.Add(new SelectListItem
            {
                Value = "Airbus A321",
                Text = "Airbus A321"
            });
            models.Add(new SelectListItem
            {
                Value = "Boeing 757",
                Text = "Boeing 757"
            });
            models.Add(new SelectListItem
            {
                Value = "Boeing 777",
                Text = "Boeing 777"
            });
            models.Add(new SelectListItem
            {
                Value = "Airbus A380",
                Text = "Airbus A380"
            });
            return models;
        }


    }
}
