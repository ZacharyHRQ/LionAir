using System;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Aircraft
    {
  

        public int AircraftID { get; set; }

        [Required]
        public string AircraftModel { get; set; }
       
        public int? NumEconomySeat { get; set; }

        public int? NumBusinessSeat { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateLastMaintenance { get; set; }

        [ValidateMaintenance]
        public string Status { get; set; }

        public Aircraft()
        {
        }
    }
}
