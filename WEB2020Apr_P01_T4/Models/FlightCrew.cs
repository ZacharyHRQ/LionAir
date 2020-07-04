using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class FlightCrew
    {
        [Display(Name = "ScheduleID")]
        public int ScheduleID { get; set; }

        [Display(Name = "StaffID")]
        public int StaffID { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
