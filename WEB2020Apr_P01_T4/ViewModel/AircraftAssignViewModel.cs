using System;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class AircraftAssignViewModel
    {
        public int AircraftID { get; set; }

        public String AircraftModel { get; set; }

        public int NumEconomySeat { get; set; }

        public int NumBusinessSeat { get; set; }

        public String FlightSchedule { get; set; }
             
        public AircraftAssignViewModel()
        {
        }
    }
}
