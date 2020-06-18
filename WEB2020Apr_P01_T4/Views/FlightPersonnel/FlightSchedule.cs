using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2020Apr_P01_T4.Views.FlightPersonnel
{
    public class FlightSchedule
    {
        [Display(Name = "RouteID")]
        public int RouteID { get; set; }
    }
}
