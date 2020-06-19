using System;
using System.ComponentModel.DataAnnotations;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class AircraftScheduleViewModel
    {
        public int AircraftID { get; set; }
        
        public String AircraftModel { get; set; }

        public int NumEconomySeat { get; set; }

        public int NumBusinessSeat { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateLastMaintenance { get; set; }

        public String FlightSchedule { get; set; }

        public String Status { get; set; }

        public AircraftScheduleViewModel()
        {
        }
    }
}
