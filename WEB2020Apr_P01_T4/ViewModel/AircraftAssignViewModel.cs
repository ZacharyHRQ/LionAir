using System;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class AircraftAssignViewModel
    {
        public int AircraftID { get; set; }

        public string AircraftModel { get; set; }

        public int NumEconomySeat { get; set; }

        public int NumBusinessSeat { get; set; }

        public string status { get; set; }

        public string flightSchedule { get; set; }

        public AircraftAssignViewModel()
        {
        }
    }
}
