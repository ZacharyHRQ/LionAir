using System;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Route
    {
        [Required]
        public int RouteID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Departure City")]
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

        [Range(1,24)]
        [Display(Name = "Flight Duration")]
        public int FlightDuration { get; set; }

    }
}
