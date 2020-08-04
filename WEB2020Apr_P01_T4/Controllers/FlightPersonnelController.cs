﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.Controllers
{
    public class FlightPersonnelController : Controller
    {
        private FlightPersonnelDAL staffContext = new FlightPersonnelDAL();
        private FlightCrewDAL crewContext = new FlightCrewDAL();
        private FlightScheduleDAL scheduleContext = new FlightScheduleDAL();
        
        // GET: FlightPersonnel
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in      
            // or account not in the "Staff" role         
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<FlightPersonnel> staffList = staffContext.GetAllStaff();
            return View(staffList);
        }

        // GET: FlightPersonnel/Details/5
        public ActionResult Details(int id)
        {
            // Stop accessing the action if not logged in   
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))    
            {     
                return RedirectToAction("Index", "Home");   
            }  
            FlightPersonnel flightPersonnel = staffContext.GetDetails(id);   
            List <StaffViewModel> staffVM = MapToStaffVM(flightPersonnel);    
            return View(staffVM);
        }

        public List <StaffViewModel> MapToStaffVM(FlightPersonnel flightPersonnel)
        {
            string flightno = "";
            int routeid = 0;
            int aircraftid = 0;
            string status = "";

            List<StaffViewModel> staffvmList = new List<StaffViewModel>();
            List<FlightSchedule> fslist = scheduleContext.GetAllFlightSchedule();
            List<FlightCrew> fcList = crewContext.GetFlightCrew(flightPersonnel.StaffID);

            if (flightPersonnel.StaffID != null)
            {
                foreach (FlightCrew fc in fcList)
                {
                    foreach (FlightSchedule fs in fslist)
                    {
                        if (fc.ScheduleID == fs.ScheduleID)
                        {
                            flightno = fs.FlightNumber;
                            routeid = fs.RouteID;
                            aircraftid = fs.AircraftID;
                            status = fs.Status;

                            staffvmList.Add(new StaffViewModel
                            {
                                StaffID = flightPersonnel.StaffID,
                                StaffName = flightPersonnel.StaffName,
                                ScheduleID = fc.ScheduleID,
                                Role = fc.Role,
                                FlightNumber = flightno,
                                AircraftID = aircraftid,
                                RouteID = routeid,
                                Status = status,
                            });

                            break;
                        }
                    }
                }
                   
        
            }

            return staffvmList;
        }

        private List<SelectListItem> GetVocation()
        { 
            List<SelectListItem> vocation = new List<SelectListItem>();
            vocation.Add(new SelectListItem { Value = "null", Text = "--Please Select--" });
            vocation.Add(new SelectListItem { Value = "Administrator", Text = "Administrator" }); 
            vocation.Add(new SelectListItem { Value = "Pilot", Text = "Pilot" }); 
            vocation.Add(new SelectListItem { Value = "Flight Attendance", Text = "Flight Attendance" });
            return vocation;
        }

        private List<SelectListItem> GetStatus()
        {
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem { Value = "Active", Text = "Active" });
            status.Add(new SelectListItem { Value = "Inactive", Text = "Inactive" });
            return status;
        }

        private List<SelectListItem> GetSchedule()
        {
            List<FlightSchedule> fslist = scheduleContext.GetAllFlightSchedule();
            List<SelectListItem> schedule = new List<SelectListItem>();

            foreach (FlightSchedule fs in fslist)
            {
                schedule.Add(new SelectListItem { Value = fs.ScheduleID.ToString(), Text = fs.ScheduleID.ToString() });
            }
            return schedule;
        }

        private List<SelectListItem> GetGender()
        {
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem { Value = "null", Text = "--Please Select--" });
            gender.Add(new SelectListItem { Value = "M", Text = "Male" });
            gender.Add(new SelectListItem { Value = "F", Text = "Female" });
            return gender;
        }

        // GET: FlightPersonnel/Create
        public ActionResult Create()
        {
            // Stop accessing the action if not logged in       
            // or account not in the "Staff" role         
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["VocationList"] = GetVocation();
            ViewData["GenderList"] = GetGender();
            return View();
        }

        // POST: Staff/Create     
        [HttpPost]    
        [ValidateAntiForgeryToken]   
        public ActionResult Create(FlightPersonnel flightPersonnel)   
        {       
            //Get country list for drop-down list       
            //in case of the need to return to Create.cshtml view        
            ViewData["VocationList"] = GetVocation();
            ViewData["GenderList"] = GetGender();
            if (ModelState.IsValid)        
            {                 
                //Add staff record to database     
                flightPersonnel.StaffID = staffContext.Add(flightPersonnel);   
                //Redirect user to Staff/Index view    
                return RedirectToAction("Index");      
            }         
            else     
            {           
                //Input validation fails, return to the Create view    
                //to display error message          
                return View(flightPersonnel);     
            }    
        }

        // GET: FlightPersonnel/Edit/5
        public ActionResult Edit(int? id)
        {
            // Stop accessing the action if not logged in         
            // or account not in the "Staff" role      
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Query string parameter not provided          
                //Return to listing page, not allowed to edit     
                return RedirectToAction("Index");
            }
            FlightPersonnel flightPersonnel = staffContext.GetDetails(id.Value);
            if (flightPersonnel == null)
            {
                //Return to listing page, not allowed to edit      
                return RedirectToAction("Index");
            }
            ViewData["StatusList"] = GetStatus();
            return View(flightPersonnel);
        }


        // POST: FlightPersonnel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FlightPersonnel flightPersonnel)
        {
            FlightSchedule fs = scheduleContext.GetFlightSchedule(flightPersonnel.StaffID);
            DateTime currentDate = DateTime.Today;
            bool check = false;
            if (fs != null)
            {
                if(fs.DepartureDateTime > currentDate)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }

            //Get status list for drop-down list       
            //in case of the need to return to Edit.cshtml view        
            
            if (ModelState.IsValid)
            {
                if (check == true)
                {
                    //Update staff record to database    
                    staffContext.Update(flightPersonnel);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["ScheduleList"] = GetStatus();
                    return View();
                }

            }
            else
            {
                //Input validation fails, return to the view   
                //to display error message     
                return View(flightPersonnel);
            }

        }

        // GET: FlightPersonnel/Assign/5
        public ActionResult Assign(int? id)
        {
            // Stop accessing the action if not logged in         
            // or account not in the "Staff" role      
            if ((HttpContext.Session.GetString("Role") == null) || (HttpContext.Session.GetString("Role") != "Staff"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                //Query string parameter not provided          
                //Return to listing page, not allowed to edit     
                return RedirectToAction("Index");
            }
            FlightPersonnel flightPersonnel = staffContext.GetDetails(id.Value);
            if (flightPersonnel == null)
            {
                //Return to listing page, not allowed to edit      
                return RedirectToAction("Index");
            }
            ViewData["ScheduleList"] = GetSchedule();
            return View(flightPersonnel);
        }


        // POST: FlightPersonnel/Assign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(FlightPersonnel flightPersonnel)
        {
            //Get status list for drop-down list       
            //in case of the need to return to Edit.cshtml view        
           
            if (ModelState.IsValid)
            {
                //Update staff record to database    
                staffContext.Update(flightPersonnel);
                return RedirectToAction("Index");

            }
            else
            {
                ViewData["ScheduleList"] = GetSchedule();
                //Input validation fails, return to the view   
                //to display error message     
                return View(flightPersonnel);
            }

        }

        //GET:

    }
}
