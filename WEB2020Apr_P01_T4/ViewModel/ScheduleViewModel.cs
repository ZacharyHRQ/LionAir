using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.ViewModel
{
    public class ScheduleViewModel
    {
        /// <summary>
        /// Used for the drop down list for the serach bar
        /// </summary>
        public List<String> SearchOption { get; set; }

        /// <summary>
        /// The list of the flighSchedule needed to be displayed
        /// </summary>
        public List<FlightSchedule> FlightScheduleList { get; set; }

        /// <summary>
        /// Get the total ticket booked size
        /// </summary>
        public int TicketSize { get; set; }

        /// <summary>
        /// The route id that will be showed in
        /// the dashboard when it is filtered
        /// </summary>
        public int RouteID { get; set; }

    }
}
