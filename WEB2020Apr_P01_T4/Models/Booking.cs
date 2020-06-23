using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WEB2020Apr_P01_T4.Models
{
    public class Booking
    {
        
        public int BookingID { get; set; }
        public int  CustomerID { get; set; }
        public int ScheduleID { get; set; }

        [Display(Name = "Name")]
        public String PassengerName { get; set; }

        [Display(Name = "Passport Number")]
        public String PassportNumber { get; set; }


        public String Nationality { get; set; }

        [Display(Name = "Seat Class")]
        public String SeatClass { get; set; }

        [Display(Name = "Amount Paid")]
        public Decimal AmtPayable { get; set; }

        public String Remarks { get; set; }

        [Display(Name = "Date Time Created")]
        public DateTime DateTimeCreated { get; set; }

        public static List<String> GetTableList()
        {
            return new List<String>() { "Name", "Passport Number", "Nationality", "Seat Class", "Amount Paid", "Remarks" };
        }
    }
}
