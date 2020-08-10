using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.ViewModel
{
    public class RouteViewModel
    {
        /// <summary>
        /// The the route list for all the route
        /// that i want to display
        /// </summary>
        public List<Route> RouteList { get; set; }

        /// <summary>
        /// For the creation of new Flight Schedule in the
        /// the route view
        /// </summary>
        public FlightSchedule FlightSchedule { get; set; }

        /// <summary>
        /// Used for the drop down list for the serach bar
        /// </summary>
        public List<String> SearchOption { get; set; }

        /// <summary>
        /// Get the total ticket booked size
        /// </summary>
        public int TicketSize { get; set; }

        /// <summary>
        /// This a object to give the route form 
        /// </summary>
        public Route CreateRoute { get; set; }

        /// <summary>
        /// This is the condition to inform the
        /// if the schdule form need to be showed
        /// </summary>
        public bool ShowAddPop { get; set; }

        /// <summary>
        /// The is to show if the
        /// route form need to be showed
        /// </summary>
        public bool ShowRoutePop { get; set; }
    }
}
