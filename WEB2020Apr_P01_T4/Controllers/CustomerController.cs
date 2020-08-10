using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

                TempData["Message"] = "Password have been successfully changed!";

                return View(changePassword);
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
        // GET: AircraftSchedule
        public ActionResult AboutUs_ViewFlightSchedule()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            string from = HttpContext.Session.GetString("from");
            string to = HttpContext.Session.GetString("to");
            List<Aircraftschedule> aircraftscheduleList = CustomerContext.AboutUSGetAllAircraftSchedule(from, to);
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
                //Add customer record to database
                int customerid = (int)HttpContext.Session.GetInt32("id");
                CustomerContext.Add(booking);
                TempData["Newbooking"] = "New Booking have been successfully created!";
                return RedirectToAction("ViewAirTicket");
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
  
        private List<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries.Add(new SelectListItem
            {
                Value = "Afghanistan",
                Text = "Afghanistan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Algeria",
                Text = "Algeria"
            });
            countries.Add(new SelectListItem
            {
                Value = "Angola",
                Text = "Angola"
            });
            countries.Add(new SelectListItem
            {
                Value = "Argentina",
                Text = "Argentina"
            });
            countries.Add(new SelectListItem
            {
                Value = "Australia",
                Text = "Australia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Austria",
                Text = "Austria"
            });
            countries.Add(new SelectListItem
            {
                Value = "Azerbaijan",
                Text = "Azerbaijan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Bangladesh",
                Text = "Bangladesh"
            });
            countries.Add(new SelectListItem
            {
                Value = "Belarus",
                Text = "Belarus"
            });
            countries.Add(new SelectListItem
            {
                Value = "Belgium",
                Text = "Belgium"
            });
            countries.Add(new SelectListItem
            {
                Value = "Benin",
                Text = "Benin"
            });
            countries.Add(new SelectListItem
            {
                Value = "Boliva",
                Text = "Boliva"
            });
            countries.Add(new SelectListItem
            {
                Value = "Brazil",
                Text = "Brazil"
            });
            countries.Add(new SelectListItem
            {
                Value = "Brunei",
                Text = "Brunei"
            });
            countries.Add(new SelectListItem
            {
                Value = "Bulgaria",
                Text = "Bulgaria"
            });
            countries.Add(new SelectListItem
            {
                Value = "Burkina Faso",
                Text = "Burkina Faso"
            });
            countries.Add(new SelectListItem
            {
                Value = "Burundi",
                Text = "Burundi"
            });
            countries.Add(new SelectListItem
            {
                Value = "Cambodia",
                Text = "Cambodia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Cameroon",
                Text = "Cameroon"
            });
            countries.Add(new SelectListItem
            {
                Value = "Canada",
                Text = "Canada"
            });
            countries.Add(new SelectListItem
            {
                Value = "Chad",
                Text = "Chad"
            });
            countries.Add(new SelectListItem
            {
                Value = "Chille",
                Text = "Chille"
            });
            countries.Add(new SelectListItem
            {
                Value = "China",
                Text = "China"
            });
            countries.Add(new SelectListItem
            {
                Value = "Colombia",
                Text = "Colombia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Congo",
                Text = "Congo"
            });
            countries.Add(new SelectListItem
            {
                Value = "Costa Rica",
                Text = "Costa Rica"
            });
            countries.Add(new SelectListItem
            {
                Value = "Cuba",
                Text = "Cuba"
            });
            countries.Add(new SelectListItem
            {
                Value = "Czech Republic (Czechia)",
                Text = "Czech Republic (Czechia)"
            });
            countries.Add(new SelectListItem
            {
                Value = "Cote d'Ivoire",
                Text = "Cote d'Ivoire"
            });
            countries.Add(new SelectListItem
            {
                Value = "Denmark",
                Text = "Denmark"
            });
            countries.Add(new SelectListItem
            {
                Value = "Dominican Republic",
                Text = "Dominican Republic"
            });
            countries.Add(new SelectListItem
            {
                Value = "DR Congo",
                Text = "DR Congo"
            });
            countries.Add(new SelectListItem
            {
                Value = "Escudor",
                Text = "Escudor"
            });
            countries.Add(new SelectListItem
            {
                Value = "Egypt",
                Text = "Egypt"
            });
            countries.Add(new SelectListItem
            {
                Value = "EI Salvador",
                Text = "EI Salvador"
            });
            countries.Add(new SelectListItem
            {
                Value = "Ethiopia",
                Text = "Ethiopia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Finland",
                Text = "Finland"
            });
            countries.Add(new SelectListItem
            {
                Value = "France",
                Text = "France"
            });
            countries.Add(new SelectListItem
            {
                Value = "Germany",
                Text = "Germany"
            });
            countries.Add(new SelectListItem
            {
                Value = "Ghana",
                Text = "Ghana"
            });
            countries.Add(new SelectListItem
            {
                Value = "Greece",
                Text = "Greece"
            });
            countries.Add(new SelectListItem
            {
                Value = "Guatemala",
                Text = "Guatemala"
            });
            countries.Add(new SelectListItem
            {
                Value = "Guinea",
                Text = "Guinea"
            });
            countries.Add(new SelectListItem
            {
                Value = "Haiti",
                Text = "Haiti"
            });
            countries.Add(new SelectListItem
            {
                Value = "Honduras",
                Text = "Honduras"
            });
            countries.Add(new SelectListItem
            {
                Value = "Hungary",
                Text = "Hungary"
            });
            countries.Add(new SelectListItem
            {
                Value = "India",
                Text = "India"
            });
            countries.Add(new SelectListItem
            {
                Value = "Indonesia",
                Text = "Indonesia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Iran",
                Text = "Iran"
            });
            countries.Add(new SelectListItem
            {
                Value = "Iraq",
                Text = "Iraq"
            });
            countries.Add(new SelectListItem
            {
                Value = "Ireland",
                Text = "Ireland"
            });
            countries.Add(new SelectListItem
            {
                Value = "Israel",
                Text = "Israel"
            });
            countries.Add(new SelectListItem
            {
                Value = "Italy",
                Text = "Italy"
            });
            countries.Add(new SelectListItem
            {
                Value = "Japan",
                Text = "Japan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Jordan",
                Text = "Jordan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Kazakhstan",
                Text = "Kazakhstan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Kenya",
                Text = "Kenya"
            });
            countries.Add(new SelectListItem
            {
                Value = "Kyrgyzstan",
                Text = "Kyrgyzstan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Laos",
                Text = "Laos"
            });
            countries.Add(new SelectListItem
            {
                Value = "Lebanon",
                Text = "Lebanon"
            });
            countries.Add(new SelectListItem
            {
                Value = "Liberia",
                Text = "Liberia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Libya",
                Text = "Libya"
            });
            countries.Add(new SelectListItem
            {
                Value = "Madagascar",
                Text = "Madagascar"
            });
            countries.Add(new SelectListItem
            {
                Value = "Malawi",
                Text = "Malawi"
            });
            countries.Add(new SelectListItem
            {
                Value = "Malaysia",
                Text = "Malaysia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Mali",
                Text = "Mali"
            });
            countries.Add(new SelectListItem
            {
                Value = "Mexico",
                Text = "Mexico"
            });
            countries.Add(new SelectListItem
            {
                Value = "Morocco",
                Text = "Morocco"
            });
            countries.Add(new SelectListItem
            {
                Value = "Mozambique",
                Text = "Mozambique"
            });
            countries.Add(new SelectListItem
            {
                Value = "Myanmar",
                Text = "Myanmar"
            });
            countries.Add(new SelectListItem
            {
                Value = "Nepal",
                Text = "Nepal"
            });
            countries.Add(new SelectListItem
            {
                Value = "Netherlands",
                Text = "Netherlands"
            });
            countries.Add(new SelectListItem
            {
                Value = "New Zealand",
                Text = "New Zealand"
            });
            countries.Add(new SelectListItem
            {
                Value = "Nicaragua",
                Text = "Nicaragua"
            });
            countries.Add(new SelectListItem
            {
                Value = "Niger",
                Text = "Niger"
            });
            countries.Add(new SelectListItem
            {
                Value = "Nigeria",
                Text = "Nigeria"
            });
            countries.Add(new SelectListItem
            {
                Value = "North Korea",
                Text = "North Korea"
            });
            countries.Add(new SelectListItem
            {
                Value = "Norway",
                Text = "Norway"
            });
            countries.Add(new SelectListItem
            {
                Value = "Oman",
                Text = "Oman"
            });
            countries.Add(new SelectListItem
            {
                Value = "Pakistan",
                Text = "Pakistan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Papua New Guinea",
                Text = "Papua New Guinea"
            });
            countries.Add(new SelectListItem
            {
                Value = "Paraguay",
                Text = "Paraguay"
            });
            countries.Add(new SelectListItem
            {
                Value = "Peru",
                Text = "Peru"
            });
            countries.Add(new SelectListItem
            {
                Value = "Philippines",
                Text = "Philoppines"
            });
            countries.Add(new SelectListItem
            {
                Value = "Poland",
                Text = "Poland"
            });
            countries.Add(new SelectListItem
            {
                Value = "Portugal",
                Text = "Portugal"
            });
            countries.Add(new SelectListItem
            {
                Value = "Romania",
                Text = "Romania"
            });
            countries.Add(new SelectListItem
            {
                Value = "Russia",
                Text = "Russia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Rwanda",
                Text = "Rwanda"
            });
            countries.Add(new SelectListItem
            {
                Value = "Saudia Arabia",
                Text = "Saudia Arabia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Senegal",
                Text = "Senegal"
            });
            countries.Add(new SelectListItem
            {
                Value = "Serbia",
                Text = "Serbia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Sierra Leone",
                Text = "Sierra Leone"
            });
            countries.Add(new SelectListItem
            {
                Value = "Singapore",
                Text = "Singapore"
            });
            countries.Add(new SelectListItem
            {
                Value = "Slovakia",
                Text = "Slovakia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Somalia",
                Text = "Somalia"
            });
            countries.Add(new SelectListItem
            {
                Value = "South Africa",
                Text = "South Africa"
            });
            countries.Add(new SelectListItem
            {
                Value = "South Korea",
                Text = "South Korea"
            });
            countries.Add(new SelectListItem
            {
                Value = "South Sudan",
                Text = "South Sudan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Spain",
                Text = "Spain"
            });
            countries.Add(new SelectListItem
            {
                Value = "Sri Lanka",
                Text = "Sri Lanka"
            });
            countries.Add(new SelectListItem
            {
                Value = "State of Palestine",
                Text = "State of Palestine"
            });
            countries.Add(new SelectListItem
            {
                Value = "Sudan",
                Text = "Sudan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Sweden",
                Text = "Sweden"
            });
            countries.Add(new SelectListItem
            {
                Value = "Switzerland",
                Text = "Switzerland"
            });
            countries.Add(new SelectListItem
            {
                Value = "Syria",
                Text = "Syria"
            });
            countries.Add(new SelectListItem
            {
                Value = "Tajikistan",
                Text = "Tajikistan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Tanzania",
                Text = "Tanzania"
            });
            countries.Add(new SelectListItem
            {
                Value = "Thailand",
                Text = "Thailand"
            });
            countries.Add(new SelectListItem
            {
                Value = "Togo",
                Text = "Togo"
            });
            countries.Add(new SelectListItem
            {
                Value = "Tunisia",
                Text = "Tunisia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Turkey",
                Text = "Turkey"
            });
            countries.Add(new SelectListItem
            {
                Value = "Turkmenistan",
                Text = "Turkmenistan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Uganda",
                Text = "Uganda"
            });
            countries.Add(new SelectListItem
            {
                Value = "Ukraine",
                Text = "Ukraine"
            });
            countries.Add(new SelectListItem
            {
                Value = "United Arab Emirates",
                Text = "United Arab Emirates"
            });
            countries.Add(new SelectListItem
            {
                Value = "United Kingdom",
                Text = "United Kingdom"
            });
            countries.Add(new SelectListItem
            {
                Value = "United States",
                Text = "United States"
            });
            countries.Add(new SelectListItem
            {
                Value = "Uzbekistan",
                Text = "Uzbekistan"
            });
            countries.Add(new SelectListItem
            {
                Value = "Venezuela",
                Text = "Venezuela"
            });
            countries.Add(new SelectListItem
            {
                Value = "Vietnam",
                Text = "Vietnam"
            });
            countries.Add(new SelectListItem
            {
                Value = "Yemen",
                Text = "Yemen"
            });
            countries.Add(new SelectListItem
            {
                Value = "Zambia",
                Text = "Zambia"
            });
            countries.Add(new SelectListItem
            {
                Value = "Zimbabiwe",
                Text = "Zimbabiwe"
            });
            return countries;
        }
    }
}
