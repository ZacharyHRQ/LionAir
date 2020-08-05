using System;
using WEB2020Apr_P01_T4.Models;
namespace WEB2020Apr_P01_T4.ViewModel
{
    public class AircraftUpdateViewModel
    {
        public int AircraftID { get; set; }

        public string AircraftModel { get; set; }

        public DateTime DateLastMaintenance { get; set; }

        public string Status { get; set; }
        
        public AircraftUpdateViewModel()
        {
        }
    }
}
