using System;
using System.Linq;

namespace RequestForQuoteInterfacesLibrary.Utilities
{
    public static class UtilityMethods
    {
        /// <summary>
        /// Verifies whether a date is within a date range as specified in the criteria value.
        /// </summary>
        /// <param name="dateToCheck"> the date to check if in range.</param>
        /// <param name="criterionValue"> the string value representing the start date and end date or either one of them. 
        /// The format of the date must be DDMMMYYYY</param>
        /// <returns> true if the date is within range.</returns>
        /// <exception cref="ArgumentException">thrown if the criterionValue parameter is NULL or empty to cannot be converted into one or more dates.</exception>       
        public static bool IsWithinDateRange(DateTime dateToCheck, string criterionValue)
        {
            if (String.IsNullOrEmpty(criterionValue))
                throw new ArgumentException("criterionValue");

            try
            {
                var dates = criterionValue.Split('-').ToArray();
                dates = dates.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                DateTime startDate, endDate;
                if (dates.Count() > 1)
                {
                    startDate = Convert.ToDateTime(dates[0]);
                    endDate = Convert.ToDateTime(dates[1]);
                    return dateToCheck >= startDate && dateToCheck <= endDate;
                }
                if (dates.Count() == 1 && criterionValue[0] != '-')
                {
                    startDate = Convert.ToDateTime(dates[0]);
                    return dateToCheck >= startDate;
                }
                if (dates.Count() == 1 && criterionValue[0] == '-')
                {
                    endDate = Convert.ToDateTime(dates[0]);
                    return dateToCheck <= endDate;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("criterionValue");
            }
            return false;
        }
    }
}
