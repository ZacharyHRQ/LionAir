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
            ViewData["CountryList"] = GetCountries();
            Register register = new Register();
            register.Password = "p@55Cust";
            return View(register);
        }
        private List<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries.Add(new SelectListItem{
                Value = "Singapore", Text = "Singapore"});
            countries.Add(new SelectListItem{
                Value = "Malaysia", Text = "Malaysia"});
            countries.Add(new SelectListItem{
                Value = "Indonesia", Text = "Indonesia"});
            countries.Add(new SelectListItem{
                Value = "China", Text = "China"});
            countries.Add(new SelectListItem{
                Value = "United Kingdom", Text = "United Kingdom"});
            countries.Add(new SelectListItem{
                Value = "American", Text = "American"});
            return countries;
        }
        // POST: Register/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Register register)
        {
            //Get country list for drop-down list
            //in case of the need to return to index.cshtml view
            ViewData["CountryList"] = GetCountries();

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