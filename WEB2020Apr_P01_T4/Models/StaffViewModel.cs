using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class StaffViewModel
    {
        [Display(Name = "Staff ID")]
        public int StaffID { get; set; }

        [Display(Name = "Staff Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 character!")]
        public string StaffName { get; set; }

        [Display(Name = "Gender")]
        public char Gender { get; set; }

        [Display(Name = "Date Employed")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM-dd-yyyy}")]
        public DateTime DateEmployed { get; set; }

        [Display(Name = "Vocation")]
        public string Vocation { get; set; }

        [Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 character!")]
        // Custom Validation Attribute for checking email address exists
        public string EmailAddr { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "ScheduleID")]
        public int ScheduleID { get; set; }

        [Display(Name = "RouteID")]
        public int RouteID { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
