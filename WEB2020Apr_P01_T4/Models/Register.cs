using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Register
    {
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Nationality")]
        public string Nationality { get; set; }

        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Telephone number")]
        public int? TelNo { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Display(Name = "Email Address")]
        // Custom Validation Attribute for checking email address exists
        [ValidateEmailExists]
        public string EmailAddr { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
