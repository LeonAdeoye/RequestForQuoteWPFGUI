using System;
using System.Globalization;
using System.Windows.Controls;

namespace RequestForQuoteInterfacesLibrary.ValidationRules
{
    /// <summary>
    /// Validation rule class which validates a value is a number greater than or equal to zero.
    /// </summary>
    public class ZeroMinimumNumberValidationRule : ValidationRule
    {
        /// <summary>
        /// Validates that the value parameter is a number greater than or equal to zero. 
        /// </summary>.
        /// <param name="value"> the value to be validated.</param>
        /// <param name="cultureInfo"> not used.</param>
        /// <returns> true if the value is a number greater than or equal to zero; false otherwise.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || String.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(true, null);

            decimal currentValue;
            if (!decimal.TryParse(value.ToString(), out currentValue) || currentValue < 0)
                return new ValidationResult(false, "Value is not a number greater than or equal to zero.");

            return new ValidationResult(true, null); 
        }
    }
}
