using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class AircraftScheduleViewModel
    {
        public List<Aircraft> aircraftList { get; set; }
        public List<FlightSchedule> scheduleList { get; set; }

        public AircraftScheduleViewModel()
        {
            aircraftList = new List<Aircraft>();
            scheduleList = new List<FlightSchedule>();
        }
    }
}
