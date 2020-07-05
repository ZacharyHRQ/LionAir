using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerDAL CustomerContext = new CustomerDAL();
        [HttpGet]
        // GET: ChangePassword
        public IActionResult ChangePassword()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            ChangePassword changePassword = new ChangePassword();
            changePassword.DatabasePassword = HttpContext.Session.GetString("password");
            return View(changePassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                //Update password record to database

                int customerid = (int)HttpContext.Session.GetInt32("id");
                CustomerContext.Update(changePassword, customerid);

                return RedirectToAction("CustomerMain", "Login");
            }
            else
            {
                return View(changePassword);
            }
        }

        [HttpGet]
        // GET: AircraftSchedule
        public ActionResult ViewFlightSchedule()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Aircraftschedule> aircraftscheduleList = CustomerContext.GetAllAircraftSchedule();
            return View(aircraftscheduleList);
        }

        [HttpGet]
        // GET: BookAirTicket
        public IActionResult BookAirTicket(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            Aircraftschedule booking = CustomerContext.GetSchedule(id);
            Aircraftschedule bookingVM = MaptobookingVM(booking);
            ViewData["CountryList"] = GetCountries();
            return View(bookingVM);
        }

        public Aircraftschedule MaptobookingVM(Aircraftschedule booking)
        {
            Aircraftschedule customerid = new Aircraftschedule();
            customerid.CustomerID = (int)HttpContext.Session.GetInt32("id");
            Aircraftschedule bookingVM = new Aircraftschedule
            {

                BookingID = booking.BookingID,
                CustomerID = customerid.CustomerID,
                ScheduleID = booking.ScheduleID,
                PassengerName = booking.PassengerName,
                PassportNumber = booking.PassportNumber,
                Nationality = booking.Nationality,
                FlightNumber = booking.FlightNumber,
                DepartureCity = booking.DepartureCity,
                DepartureCountry = booking.DepartureCountry,
                DepartureDateTime = booking.DepartureDateTime,
                ArrivalCity = booking.ArrivalCity,
                ArrivalCountry = booking.ArrivalCountry,
                ArrivalDateTime = booking.ArrivalDateTime,
                FlightDuration = booking.FlightDuration,
                SeatClass = booking.SeatClass,
                AmtPayable = booking.AmtPayable,
                Remarks = booking.Remarks,
                DateTimeCreated = booking.DateTimeCreated,
                EconomyClassPrice = booking.EconomyClassPrice,
                BusinessClassPrice = booking.BusinessClassPrice,
                Status = booking.Status
            };
            return bookingVM;
        }

        private List<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries.Add(new SelectListItem
            {
                Value = "Singapore",
                Text = "Singapore"
            });
            countries.Add(new SelectListItem
            {
                Value = "Malaysia",
                Text = "Malaysia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Indonesia",
                Text = "Indonesia"
            });
            countries.Add(new SelectListItem
            {
                Value = "China",
                Text = "China"
            });
            countries.Add(new SelectListItem
            {
                Value = "United Kingdom",
                Text = "United Kingdom"
            });
            countries.Add(new SelectListItem
            {
                Value = "American",
                Text = "American"
            });
            return countries;
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAirTicket(Aircraftschedule booking)
        {
            //Get country list for drop-down list
            //in case of the need to return to index.cshtml view
            ViewData["CountryList"] = GetCountries();

            if (ModelState.IsValid)
            {
                if (booking.SeatClass == "Economy")
                {
                    booking.AmtPayable = booking.EconomyClassPrice;
                }
                else
                {
                    booking.AmtPayable = booking.BusinessClassPrice;
                }
                //Add customer record to database
                int customerid = (int)HttpContext.Session.GetInt32("id");
                CustomerContext.Add(booking, customerid);
                //Redirect user to Login/Index view
                return RedirectToAction("CustomerMain", "Login");
            }
            else
            {
                //Input validation fails, return to the register view to display error message
                return View(booking);
            }
        }
        [HttpGet]
        // ViewAirTicket
        public ActionResult ViewAirTicket()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            int customerid = (int)HttpContext.Session.GetInt32("id");
            List<Aircraftschedule> viewAirTicketList = CustomerContext.ViewAirTicket(customerid);
            return View(viewAirTicketList);
        }

        [HttpGet]
        // MoreDetails
        public ActionResult MoreDetails(int id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            Aircraftschedule passenger = CustomerContext.GetDetails(id);
            Aircraftschedule passengerVM = MaptopassengerVM(passenger);
            return View(passengerVM);
        }

        public Aircraftschedule MaptopassengerVM(Aircraftschedule passenger)
        {
            Aircraftschedule passengerVM = new Aircraftschedule
            {
                BookingID = passenger.BookingID,
                PassengerName = passenger.PassengerName,
                PassportNumber = passenger.PassportNumber,
                Nationality = passenger.Nationality,
                FlightNumber = passenger.FlightNumber,
                DepartureCity = passenger.DepartureCity,
                DepartureCountry = passenger.DepartureCountry,
                DepartureDateTime = passenger.DepartureDateTime,
                ArrivalCity = passenger.ArrivalCity,
                ArrivalCountry = passenger.ArrivalCountry,
                ArrivalDateTime = passenger.ArrivalDateTime,
                FlightDuration = passenger.FlightDuration,
                SeatClass = passenger.SeatClass,
                AmtPayable = passenger.AmtPayable,
                Remarks = passenger.Remarks
            };
            return passengerVM;
        }
    }
}
