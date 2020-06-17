using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            return View();
        }

        // POST: FlightPersonnel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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