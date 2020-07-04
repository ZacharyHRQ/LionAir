using System;
using System.ComponentModel.DataAnnotations;
using WEB2020Apr_P01_T4.Models;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class AircraftAssignViewModel
    {
        public int AircraftID { get; set; }

        public string AircraftModel { get; set; }

        public int NumEconomySeat { get; set; }

        public int NumBusinessSeat { get; set; }

        public string status { get; set; }

        [ValidateAircraftAssignment]
        public string flightSchedule { get; set; }

        public AircraftAssignViewModel()
        {
        }
    }
}
