using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;
using Microsoft.VisualBasic;

namespace WEB2020Apr_P01_T4.Models
{
    public class Aircraftschedule
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int ScheduleID { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureCountry { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalCountry { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DepartureDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ArrivalDateTime { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.00}")]
        public decimal EconomyClassPrice { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.00}")]
        public decimal BusinessClassPrice { get; set; }

        public string Status { get; set; }

        [Required]
        [Display(Name = "Passenger Name")]
        public string PassengerName { get; set; }

        [Required]
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }

        [Required]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Seat Class")]
        public string SeatClass { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,##0.00}")]
        public decimal AmtPayable { get; set; }

        [Display(Name = "Special requirement")]
        public string Remarks { get; set; }

        public DateTime DateTimeCreated { get; set; }
        public Aircraftschedule()
        {
            this.DateTimeCreated = DateTime.UtcNow;
        }
        public string FlightNumber { get; set; }
        public int FlightDuration { get; set; }

        public string From { get; set; }
        public string To { get; set; }
    }
}
