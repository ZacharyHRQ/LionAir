using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.Models;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class AircraftAssignmentController : Controller
    {

        private AircraftDAL aircraftContext = new AircraftDAL();
        FlightScheduleDAL flightScheduleDAL = new FlightScheduleDAL();

        // GET: /<controller>/Display
        public IActionResult DisplayAircraft()
        {
            List<AircraftScheduleViewModel> viewModels = MapToAircraftVM();
           
            return View(viewModels);
        }

        // GET: /<controller>/Create
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
        
        // GET: /<controller>/Update
        public IActionResult UpdateAircraft(int aircrafID)
        {
            return View();
        }

        // GET: /<controller>/Assign
        public IActionResult AssignAircraft(int aircrafID)
        {
            return View();
        }

        // Aircraft Models 
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

        // Transfer data from aircraft and flight schedule to AircraftScheduleViewModel
        public List<AircraftScheduleViewModel> MapToAircraftVM()
        {
            List<AircraftScheduleViewModel> aircraftScheduleViewModels = new List<AircraftScheduleViewModel>();
            List<Aircraft> aircraftList = aircraftContext.GetAllAircraft();
            List<FlightSchedule> flightSchedules = flightScheduleDAL.getAllFlightSchedule();
            foreach (Aircraft aircraft in aircraftList)
            {
                // finds the flight schedules that matches aircraft ID
                FlightSchedule flight = flightSchedules.Find(delegate (FlightSchedule flight)
                {
                    return flight.AircraftID == aircraft.AircraftID;
                });

                string scheduleID = "";
                if (flight != null)
                {
                     scheduleID = flight.FlightNumber;
                }
                
                aircraftScheduleViewModels.Add(new AircraftScheduleViewModel
                {
                    AircraftID = aircraft.AircraftID,
                    AircraftModel = aircraft.AircraftModel,
                    NumBusinessSeat = aircraft.NumBusinessSeat,
                    NumEconomySeat = aircraft.NumEconomySeat,
                    DateLastMaintenance = aircraft.DateLastMaintenance,
                    FlightSchedule = scheduleID,
                    Status = aircraft.Status
                });
            }
           
            return aircraftScheduleViewModels;
        }

        //check dates if < today 

    }
}
