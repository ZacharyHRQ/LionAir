using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.ViewModel
{
    public class ScheduleRouteViewModel
    {
        public List<FlightSchedule> FlightScheduleList { get; set; }
        public List<Route> RouteList { get; set; }
        public Route CreateRoute { get; set; }
        public FlightSchedule CreateSchedule { get; set; }


    }
}
