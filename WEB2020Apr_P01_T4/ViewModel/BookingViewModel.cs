using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class BookingViewModel
    {
        /// <summary>
        /// The flight object that has been selected 
        /// </summary>
        public FlightSchedule FlightSchedule { get; set; }

        /// <summary>
        /// The search options for booking 
        /// </summary>
        public List<String> SearchOption { get; set; }

        /// <summary>
        /// The booking that will be listed in
        /// that view
        /// </summary>
        public IEnumerable<Booking> BookingList { get; set; }
        
    }
}
