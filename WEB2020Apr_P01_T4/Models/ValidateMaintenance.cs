using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2020Apr_P01_T4.DAL;
namespace WEB2020Apr_P01_T4.Models
{
    public class ValidateMaintenance : ValidationAttribute
    {
        private AircraftDAL aircraftContext = new AircraftDAL();

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            Aircraft newaircraft = (Aircraft)validationContext.ObjectInstance; //model from update view 
            Aircraft oldaircraft = aircraftContext.FindAircraft(newaircraft.AircraftID); // model from database
            String newstatus = newaircraft.Status;
            String oldstatus = oldaircraft.Status;

            if (newstatus == oldstatus)
            {
                return new ValidationResult("Same status was selected , no update in status was made");
            }
            else if (newstatus == "Operational")
            {
                return ValidationResult.Success;
            }
            else if (aircraftContext.CheckMaintenance(newaircraft.AircraftID))
            {
                return new ValidationResult("This aircraft has flight schedules");
            }
            else return ValidationResult.Success;
        }

        public ValidateMaintenance()
        {
        }
    }
}
