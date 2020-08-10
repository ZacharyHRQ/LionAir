using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using WEB2020Apr_P01_T4.ViewModel;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.Controllers
{
    public class HomeController : Controller
    {
        private CustomerDAL CustomerContext = new CustomerDAL();
        [HttpGet]
        // GET: About Us
        public IActionResult Index()
        {
            Aircraftschedule aboutus = new Aircraftschedule();
            ViewData["DestinationList"] = GetDestination();
            return View(aboutus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection index)
        {
            string from = index["from"];
            string to = index["to"];

            HttpContext.Session.SetString("from", from);
            HttpContext.Session.SetString("to", to);

            return RedirectToAction("AboutUs_ViewFlightSchedule", "Customer");
        }
        private List<SelectListItem> GetDestination()
        {
            List<SelectListItem> destination = new List<SelectListItem>();
            destination.Add(new SelectListItem
            {
                Value = "Australia",
                Text = "Australia"
            });
            destination.Add(new SelectListItem
            {
                Value = "London",
                Text = "London"
            });
            destination.Add(new SelectListItem
            {
                Value = "Malaysia",
                Text = "Malaysia"
            });
            destination.Add(new SelectListItem
            {
                Value = "Singapore",
                Text = "Singapore"
            });
            destination.Add(new SelectListItem
            {
                Value = "Sydney",
                Text = "Sydney"
            });
            destination.Add(new SelectListItem
            {
                Value = "United Kingdom",
                Text = "United Kingdom"
            });
            return destination;
        }
    }
}
