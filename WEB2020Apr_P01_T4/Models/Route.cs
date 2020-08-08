using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Route
    {
        private double? _FlightDuration;

        [Required]
        public int RouteID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Departure City")]
        [RegularExpression("[a-zA-Z]", ErrorMessage = "Only letters are allowed")]
        public String DepartureCity { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Departure Country")]
        public String DepartureCountry { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Arrival City")]
        public String ArrivalCity { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Arrival Country")]
        public String ArrivalCountry { get; set; }

        [Display(Name = "Flight Duration")]
        [Range(0.0, 36.0, ErrorMessage = "Please enter a flight time from 0 to 36")]
        public double? FlightDuration {
            get { return _FlightDuration; }
            set {
                if (value != null)
                {
                    _FlightDuration = (double)Math.Round((decimal)value, 0);
                }
                else
                {
                    _FlightDuration = value;
                }
            }
        }

        public static List<String> GetTableList()
        {
            return new List<String>() { "Route ID", "Departure City", "Departure Country", "Arrival City", "Arrival Country", "Flight Duration" };
        }

    }
}
