using System;
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

        // GET: FliightPersonnel
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
            return View();
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
            return View();
        }

        private List<SelectListItem> GetVocation()
        { 
            List<SelectListItem> vocation = new List<SelectListItem>(); 
            vocation.Add(new SelectListItem { Value = "Administrator,", Text = "Administrator" }); 
            vocation.Add(new SelectListItem { Value = "Pilot", Text = "Pilot" }); 
            vocation.Add(new SelectListItem { Value = "Flight Attendance", Text = "Flight Attendance" });
            return vocation;
        }


        // POST: Staff/Create     
        [HttpPost]    
        [ValidateAntiForgeryToken]   
        public ActionResult Create(FlightPersonnel flightPersonnel)   
        {       
            //Get country list for drop-down list       
            //in case of the need to return to Create.cshtml view        
            ViewData["VocationList"] = GetVocation();       
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FlightPersonnel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FlightPersonnel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FlightPersonnel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}