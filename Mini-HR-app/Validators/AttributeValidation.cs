using System;
using System.ComponentModel.DataAnnotations;

namespace Mini_HR_app.Models
{
    public class DateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime todayDate = Convert.ToDateTime(value);
            return todayDate <= DateTime.Now;
        }
    }   
}