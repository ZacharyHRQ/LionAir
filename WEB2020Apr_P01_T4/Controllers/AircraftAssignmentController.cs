using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
       
        // GET: /<controller>/Display
        public IActionResult DisplayAircraft(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Login");
            }

            AircraftScheduleViewModel aircraftScheduleView = new AircraftScheduleViewModel();
            
            aircraftScheduleView.aircraftList = aircraftContext.GetAllAircraft();

            aircraftScheduleView.maintainceList = aircraftContext.GetMaintenanceAircraft();

            if(id != null)
            {
                ViewData["selectedAircraft"] = id.Value.ToString();
                aircraftScheduleView.scheduleList = aircraftContext.GetSchedules(id.Value);
            }
            else
            {
                ViewData["selectedAircraft"] = "";
            }
            
            return View(aircraftScheduleView);
        }

        public IActionResult MaintainAircraft()
        {
            AircraftScheduleViewModel aircraftScheduleView = new AircraftScheduleViewModel();

            aircraftScheduleView.aircraftList = aircraftContext.GetMaintenanceAircraft();
            ViewData["selectedAircraft"] = "";
            return View("DisplayAircraft" , aircraftScheduleView);
        }

       
        public IActionResult CreateAircraft()
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewData["statusList"] = GetStatus();
            ViewData["ModelList"] = GetModel();
            return View();
        }

        [HttpPost]
        public IActionResult CreateAircraft(Aircraft aircraft)
        {
            ViewData["ModelList"] = GetModel();
            ViewData["statusList"] = GetStatus();
            if (ModelState.IsValid)
            {
                aircraft.AircraftID = aircraftContext.Add(aircraft);

                return RedirectToAction("DisplayAircraft");
            }
            else
            {
                return View(aircraft);
            }
        
        }
        
        // GET: /<controller>/Assign
        public IActionResult AssignAircraft(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Login");
            }
            if (id != null)
            {

                ViewData["flightList"] = GetFlights();
                Aircraft aircraft = aircraftContext.FindAircraft(id.Value);
                AircraftAssignViewModel aircraftAssignViewModel = new AircraftAssignViewModel
                {
                    AircraftID = aircraft.AircraftID,
                    AircraftModel = aircraft.AircraftModel,
                    NumBusinessSeat = aircraft.NumBusinessSeat,
                    NumEconomySeat = aircraft.NumEconomySeat,
                    status = aircraft.Status
                };
                return View(aircraftAssignViewModel);
            }
            else
            {
                return RedirectToAction("DisplayAircraft");
            }
        }

        [HttpPost]
        public IActionResult AssignAircraft(AircraftAssignViewModel aircraft)
        {
            ViewData["flightList"] = GetFlights();
            if (ModelState.IsValid)
            {
                aircraftContext.Assign(aircraft.AircraftID, Convert.ToInt32(aircraft.flightSchedule));
                return RedirectToAction("DisplayAircraft");
            }
            else
            {
                return View(aircraft);
            }
            
        }

        // GET: /<controller>/Update
        public IActionResult UpdateAircraft(int? id)
        {
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Login");
            }
            ViewData["statusList"] = GetStatus();
            if (id != null)
            {
                Aircraft aircraft = aircraftContext.FindAircraft(id.Value);
                ViewData["status"] = aircraft.Status;
                return View(aircraft);
            }
            else
            {
                return RedirectToAction("DisplayAircraft");
            }
        }

        [HttpPost]
        public IActionResult UpdateAircraft(Aircraft aircraft)
        {
            ViewData["statusList"] = GetStatus();
            if (ModelState.IsValid)
            {
                aircraftContext.Update(aircraft);
                return RedirectToAction("DisplayAircraft");
            }
            else
            {
                return View(aircraft);
            }
            

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

            models.Insert(0, new SelectListItem
            {
                Value = "--Select--",
                Text = "--Select--"
   
            });
            return models;
        }

        
        private List<SelectListItem> GetFlights()
        {
            List<SelectListItem> flights = new List<SelectListItem>();
            List<FlightSchedule> flightSchedules = aircraftContext.GetAvailableFlights();
            foreach (FlightSchedule schedule in flightSchedules)
            {
                flights.Add(new SelectListItem
                {
                    Value = schedule.ScheduleID.ToString(),
                    Text = schedule.FlightNumber
                });
            }
            
            return flights;
        }


        private List<SelectListItem> GetStatus()
        {
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem
            {
                Value = "Operational",
                Text = "Operational"
            });
            status.Add(new SelectListItem
            {
                Value = "Under Maintenance",
                Text = "Under Maintenance"
            });
            return status;

        }

        public bool CheckAssignmentValid(AircraftAssignViewModel aircraft)
        {
  
            if (aircraft.status == "Under Maintenance")
            {
                return false;
            }
            else if (aircraftContext.CheckFlight(aircraft.AircraftID , Convert.ToInt32(aircraft.flightSchedule)))
            {
                return false; 
            }
            return true;
        }
        
    }
}
