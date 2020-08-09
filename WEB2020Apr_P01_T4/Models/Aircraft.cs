using System;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Aircraft
    {
  

        public int AircraftID { get; set; }

        [Required]
        public string AircraftModel { get; set; }

        [Range(0,int.MaxValue,ErrorMessage= "Invalid value! Please enter a positive value")]
        public int? NumEconomySeat { get; set; }

        [Range(0,int.MaxValue,ErrorMessage= "Invalid value! Please enter a positive value")]
        public int? NumBusinessSeat { get; set; }

        public DateTime? DateLastMaintenance { get; set; }

        [ValidateMaintenance]
        public string Status { get; set; }

        public Aircraft()
        {
        }
    }
}
