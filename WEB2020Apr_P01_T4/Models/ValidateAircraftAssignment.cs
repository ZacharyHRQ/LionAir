using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2020Apr_P01_T4.DAL;
using WEB2020Apr_P01_T4.ViewModel;
namespace WEB2020Apr_P01_T4.Models
{
    public class ValidateAircraftAssignment : ValidationAttribute
    {
        private AircraftDAL aircraftContext = new AircraftDAL();

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            AircraftAssignViewModel aircraft = (AircraftAssignViewModel)validationContext.ObjectInstance;

            if (aircraft.status == "Under Maintenance")
            {
                return new ValidationResult("Aircraft selected is under maintenance ");
            }
            else if (aircraftContext.CheckFlight(aircraft.AircraftID, Convert.ToInt32(aircraft.flightSchedule))) //checks for conflicts in timings when assignment flight schedules
            {
                return new ValidationResult("This aircraft a flight schedule that conflicts with selected flight schedule");
            }
            return ValidationResult.Success;
        }
        public ValidateAircraftAssignment()
        {
        }
    }
}
