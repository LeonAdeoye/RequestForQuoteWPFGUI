using System;
using System.Globalization;
using System.Windows.Controls;

namespace RequestForQuoteInterfacesLibrary.ValidationRules
{
    /// <summary>
    /// Validation rule class which validates that a value is a number greater than zero.
    /// </summary>
    public class GreaterThanZeroNumberValidationRule : ValidationRule
    {
        /// <summary>
        /// Validates that the value parameter is a number greater than zero. 
        /// </summary>.
        /// <param name="value"> the value to be validated.</param>
        /// <param name="cultureInfo"> not used.</param>
        /// <returns> true if the value is a number greater than zero; false otherwise.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Value is not a number greater than zero."); 

            decimal currentValue;
            if (!Decimal.TryParse(value.ToString(), out currentValue) || currentValue <= 0)
                return new ValidationResult(false, "Value is not a number greater than zero.");

            return new ValidationResult(true, null); 
        }
    }
}
