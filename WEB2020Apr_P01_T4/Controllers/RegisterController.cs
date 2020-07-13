using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.Controllers
{
    public class RegisterController : Controller
    {
        private CustomerDAL CustomerContext = new CustomerDAL();
        // GET: Register/Index
        public ActionResult Index()
        {
            Register register = new Register();
            register.Password = "p@55Cust";
            return View(register);
        }

        // POST: Register/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Register register)
        {
            if (ModelState.IsValid)
            {
                //Add customer record to database
                register.CustomerID = CustomerContext.Add(register);
                //Redirect user to Login/Index view
                return RedirectToAction("Index", "Login");
            }
            else
            {
                //Input validation fails, return to the register view to display error message
                return View(register);
            }
        }
    }
}