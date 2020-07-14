using System;
using System.ComponentModel.DataAnnotations;
namespace WEB2020Apr_P01_T4.Models
{
    public class ADayBeforeDepature : ValidationAttribute
    {
        public ADayBeforeDepature()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                DateTime datetime = (DateTime)((FlightSchedule)validationContext.ObjectInstance).DepartureDateTime;

                if (DateTime.Now.AddHours(24) <= datetime)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Departure date must be at least one day after today");
            }
            catch(InvalidOperationException i)
            {
                return ValidationResult.Success;
            }            
                
          


        }
    }
}
