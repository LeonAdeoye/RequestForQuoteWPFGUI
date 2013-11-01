using System;
using System.Globalization;
using System.Windows.Controls;

namespace RequestForQuoteInterfacesLibrary.ValidationRules
{
    /// <summary>
    /// Validation rule class which validates that a value is a number.
    /// </summary>
    public class MustBeNumberValidationRule : ValidationRule
    {
        /// <summary>
        /// Validates that the value parameter is a number. 
        /// </summary>.
        /// <param name="value"> the value to be validated.</param>
        /// <param name="cultureInfo"> not used.</param>
        /// <returns> true if the value is a number; false otherwise.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Value is not a number.");

            decimal currentValue;
            if (!Decimal.TryParse(value.ToString(), out currentValue))
                return new ValidationResult(false, "Value is not a number.");

            return new ValidationResult(true, null); 
        }
    }
}
