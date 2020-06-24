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
        //FlightScheduleDAL flightScheduleDAL = new FlightScheduleDAL();

        // GET: /<controller>/Display
        public IActionResult DisplayAircraft(int? id)
        {
            AircraftScheduleViewModel aircraftScheduleView = new AircraftScheduleViewModel();
            aircraftScheduleView.aircraftList = aircraftContext.GetAllAircraft();

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
        
        // GET: /<controller>/Assign
        public IActionResult AssignAircraft(int? id)
        {
            if (id != null)
            {
                ViewData["flightList"] = GetFlights();
                Aircraft aircraft = aircraftContext.FindAircraft(id.Value);
                AircraftAssignViewModel aircraftAssignViewModel = new AircraftAssignViewModel
                {
                    AircraftID = aircraft.AircraftID,
                    AircraftModel = aircraft.AircraftModel,
                    NumBusinessSeat = aircraft.NumBusinessSeat,
                    NumEconomySeat = aircraft.NumEconomySeat
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
        public IActionResult UpdateAircraft(int? id)
        {
            ViewData["statusList"] = GetStatus();
            if (id != null)
            {
                Aircraft aircraft = aircraftContext.FindAircraft(id.Value);
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
                    Value = schedule.FlightNumber,
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

        // Transfer data from aircraft and flight schedule to AircraftScheduleViewModel
        //public List<AircraftScheduleViewModel> MapToAircraftVM()
        //{
        //    List<AircraftScheduleViewModel> aircraftScheduleViewModels = new List<AircraftScheduleViewModel>();
        //    List<Aircraft> aircraftList = aircraftContext.GetAllAircraft();
        //    List<FlightSchedule> flightSchedules = flightScheduleDAL.getAllFlightSchedule();
        //    foreach (Aircraft aircraft in aircraftList)
        //    {
        //        // finds the flight schedules that matches aircraft ID
        //        FlightSchedule flight = flightSchedules.Find(delegate (FlightSchedule flight)
        //        {
        //            return flight.AircraftID == aircraft.AircraftID;
        //        });

        //        string scheduleID = "";
        //        if (flight != null)
        //        {
        //             scheduleID = flight.FlightNumber;
        //        }

        //        aircraftScheduleViewModels.Add(new AircraftScheduleViewModel
        //        {
        //            AircraftID = aircraft.AircraftID,
        //            AircraftModel = aircraft.AircraftModel,
        //            NumBusinessSeat = aircraft.NumBusinessSeat,
        //            NumEconomySeat = aircraft.NumEconomySeat,
        //            DateLastMaintenance = aircraft.DateLastMaintenance,
        //            FlightSchedule = scheduleID,
        //            Status = aircraft.Status
        //        });
        //    }

        //    return aircraftScheduleViewModels;
        //}



    }
}
