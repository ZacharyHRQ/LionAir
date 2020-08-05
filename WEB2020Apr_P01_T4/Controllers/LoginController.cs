using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB2020Apr_P01_T4.Models;
using WEB2020Apr_P01_T4.DAL;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB2020Apr_P01_T4.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Login credentails
        [HttpPost]
        public ActionResult StaffLogin(IFormCollection formData)
        {
            //Email address converted to lowercase
            string loginID = formData["username"].ToString().ToLower();
            string password = formData["password"].ToString();
            int id = 0;

            CustomerDAL customerDAL = new CustomerDAL();
            FlightPersonnelDAL flightPersonnelDAL = new FlightPersonnelDAL();

            if (flightPersonnelDAL.VaildAdmin(email: loginID, password: password, staffID: out id))
            {
                //Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetInt32("id", id);

                //Store the user role "Staff" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", "Admin");

                //Redirect use to the Staff Main 
                return RedirectToAction("Index", "Home");
            }
            else if (customerDAL.VaildCustomer(email: loginID, password: password, customerID: out id))
            {
                //Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetInt32("id", id);
                //Store Password in session with the key "password"
                HttpContext.Session.SetString("password", password);
                //Store the user role "Customer" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", "Customer");
                //Store login datetime in session as a string
                HttpContext.Session.SetString("Datetime", @DateTime.Now.ToString());
                //Redirect use to the Staff Main 
                return RedirectToAction("CustomerMain");
            }
            else if (flightPersonnelDAL.VaildStaff(email: loginID, password: password, staffID: out id))
            {
                //Store Login ID in session with the key "LoginID"
                HttpContext.Session.SetInt32("id", id);

                //Store the user role "Staff" as a string in session with the key "Role"
                HttpContext.Session.SetString("Role", "Staff");

                //Redirect use to the Staff Main 
                return RedirectToAction("StaffMain");
            }
            else
            {
                //Store an error message in TempData for display at the index view
                TempData["Message"] = "Invalid Login Credentials!";

                //Redirect user back to the index view through an action
                return RedirectToAction("Index");
            }
        }

        public ActionResult StaffMain()
        {
            //Stop accessing the action if not logged in
            //or account not in the  "Staff" Role
            if ((HttpContext.Session.GetString("Role") != null) || (HttpContext.Session.GetString("Role") == "Staff"))
            {
                int id = (int)HttpContext.Session.GetInt32("id");
                List<FlightPersonnel> flightPersonnels = new FlightPersonnelDAL().GetAllFlightPersonal(id);
                return View(flightPersonnels);
            }
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            //Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            //Call the Index action of home Controller
            return RedirectToAction("Index");
        }
        public ActionResult CustomerMain()
        {
            //Stop accessing the action if not logged in
            //or account not in the  "Staff" Role
            if ((HttpContext.Session.GetString("Role") != null) || (HttpContext.Session.GetString("Role") == "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //About Us Destination
        [HttpPost]
        public ActionResult Destination(IFormCollection formData)
        {
            string from = formData["from"].ToString();
            string to = formData["to"].ToString();
            int scheduleID = 0;

            CustomerDAL customerDAL = new CustomerDAL();

            if (customerDAL.VaildDestination(DepartureCountry: from, ArrivalCountry: to, scheduleID: out scheduleID))
            {
                //Redirect use to the BookAirTicket
                return RedirectToAction("BookAirTicket", "Customer");
            }
            else
            {
                //Redirect user back to the index view through an action
                return RedirectToAction("ViewFlightSchedule", "Customer");
            }
        }
    }
}
