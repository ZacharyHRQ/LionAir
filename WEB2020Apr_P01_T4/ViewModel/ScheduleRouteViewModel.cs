using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.ViewModel
{
    public class ScheduleRouteViewModel
    {
        public ScheduleViewModel ScheduleViewModel { get; set; }
        public Route CreateRoute { get; set; }
        public int TicketSize { get; set; }
        public List<String> SearchOption { get; set; }
        public bool isFlightSchedule { get; set; }


    }
}
