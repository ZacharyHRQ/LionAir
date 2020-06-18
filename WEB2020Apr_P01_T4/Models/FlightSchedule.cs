using System;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.DAL
{
    public class FlightSchedule
    {
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
        public DateTime? DepartureDateTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date Time")]
        public DateTime? ArrivalDateTime { get; set; }

        [Display(Name = "Economy Class Price")]
        [DataType(DataType.Currency)]
        [Required]
        public float EconomyClassPrice { get; set; }

        [Display(Name = "Business Class Price")]
        [DataType(DataType.Currency)]
        [Required]
        public float BusinessClassPrice { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        [Required]
        public String Status { get; set; }
    }
}
