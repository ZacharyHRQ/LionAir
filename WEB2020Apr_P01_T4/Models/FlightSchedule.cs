using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class FlightSchedule
    {

        private List<String> StatusOption = new List<String>() {"Full", "Delayed", "Cancelled" };

        [Required]
        public int ScheduleID { get; set; }

        [Display(Name = "Flight Number")]
        [StringLength(20)]
        [Required]
        public String FlightNumber { get; set; }

        [Required]
        public int RouteID { get; set; }

        public int AircraftID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Departure Date Time")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [ADayBeforeDepature]
        public DateTime? DepartureDateTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date Time")]
        public DateTime? ArrivalDateTime { get; set; }

        [Display(Name = "Economy Class Price")]
        [DataType(DataType.Currency)]
        [Required]
        public Decimal EconomyClassPrice { get; set; }

        [Display(Name = "Business Class Price")]
        [DataType(DataType.Currency)]
        [Required]
        public Decimal BusinessClassPrice { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        [Required]
        public String Status { get; set; }


        public List<String> GetStatusOption()
        {
            return StatusOption;
        }

        public static List<String> GetTableList()
        {
            return new List<String>() { "Flight Number",
                "Route ID",
                "Aircraft ID",
                "Departure Date and Time",
                "Arrival Date and Time",
                "Economy Price",
                "Business Price",
                "Status"
            };
        }


    }
}
					