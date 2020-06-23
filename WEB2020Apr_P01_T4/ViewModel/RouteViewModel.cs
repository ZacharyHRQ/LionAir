using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.ViewModel
{
    public class RouteViewModel
    {
        public List<Route> RouteList { get; set; }
        public FlightSchedule FlightSchedule { get; set; }
        public List<String> SearchOption { get; set; }
        public int TicketSize { get; set; }
        public Route CreateRoute { get; set; }
        public bool ShowAddPop { get; set; }
        public bool ShowRoutePop { get; set; }
    }
}
