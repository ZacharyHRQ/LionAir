using System;
using System.Collections.Generic;
using WEB2020Apr_P01_T4.Models;

namespace WEB2020Apr_P01_T4.ViewModel
{
    public class ScheduleViewModel
    {
        public Route Route{ get; set; }
        public List<Route> RouteList { get; set; }
        public List<FlightSchedule> FlightScheduleList { get; set; }
        public bool ShowAddPop { get; set; }
        public bool ShowEditPop { get; set; }
        public ScheduleForm ScheduleForm { get; set; }



        public void CalculateArrival()
        {
           ScheduleForm.CreateSchedule.ArrivalDateTime = ((DateTime)ScheduleForm.CreateSchedule.DepartureDateTime).AddHours((double)Route.FlightDuration);
        }

    }
}
