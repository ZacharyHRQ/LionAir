using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.ViewModel
{
    public class ScheduleRouteViewModel
    {
        public ScheduleViewModel ScheduleViewModel { get; set; }
        public List<FlightSchedule> FlightScheduleList { get; set; }
        public Route CreateRoute { get; set; }
        


    }
}
