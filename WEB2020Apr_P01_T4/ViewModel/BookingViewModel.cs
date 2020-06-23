using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class BookingViewModel
    {
        public FlightSchedule FlightSchedule { get; set; }
        public List<String> SearchOption { get; set; }
        public IEnumerable<Booking> BookingList { get; set; }
        public Route CreateRoute { get; set; }
    }
}
