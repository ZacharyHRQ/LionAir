using System;
namespace WEB2020Apr_P01_T4.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public int  CustomerID { get; set; }
        public int ScheduleID { get; set; }
        public String PassengerName { get; set; }
        public String PassportNumber { get; set; }
        public String Nationality { get; set; }
        public String SeatClass { get; set; }
        public Decimal AmtPayable { get; set; }
        public String Remarks { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
