using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class ScheduleViewModel
    {
        public Route Route { get; set; }
        public List<FlightSchedule> FlightScheduleList{ get; set; }
        public FlightSchedule CreateSchedule { get; set; }

    }
}
