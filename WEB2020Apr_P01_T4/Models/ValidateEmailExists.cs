using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2020Apr_P01_T4.DAL;

namespace WEB2020Apr_P01_T4.Models
{
    public class ValidateEmailExists : ValidationAttribute
    {
        private CustomerDAL customerContext = new CustomerDAL();

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string email = Convert.ToString(value);
            // Casting the validattion context to the "Customer" model class
            Register register = (Register)validationContext.ObjectInstance;
            // Get the Customer Id from the customer instance
            int customerId = register.CustomerID;

            if (customerContext.IsEmailExist(email, customerId))
                // validation failed
                return new ValidationResult
                    ("Email address already exists!");
            else
                // validation passed
                return ValidationResult.Success;
        }
    }
}
