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
         
            
            Aircraft aircraft = (Aircraft)validationContext.ObjectInstance;
            String status = aircraft.Status;
            int aircraftid = aircraft.AircraftID;
            //if (status == "Under Maintance")
            //{
            //    return ValidationResult.Success;
            //}
            if (aircraftContext.CheckMaintenance(aircraftid))
            {
                return new ValidationResult("Aircraft has flight schedules");
            }  
            else return ValidationResult.Success;
        }

        public ValidateMaintenance()
        {
        }
    }
}
