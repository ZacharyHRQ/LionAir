using System;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Aircraft
    {
  

        public int AircraftID { get; set; }

        [Required]
        public String AircraftModel { get; set; }
        [Required]
        public int NumEconomySeat { get; set; }
        [Required]
        public int NumBusinessSeat { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateLastMaintenance { get; set; }

        [ValidateMaintenance]
        public String Status { get; set; }

        public Aircraft()
        {
        }
    }
}
