using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.Utilities;

namespace RequestForQuoteInterfacesModuleLibrary.Test
{
    public class UtilityMethodsTest
    {
        [Test]
        public void IsWithinDateRange_NullCriterionValue_ArgumentExceptionThrown()
        {
            // Act
            Action act = () => UtilityMethods.IsWithinDateRange(DateTime.Today, null);
            // Assert
            act.ShouldThrow<ArgumentException>("because criterionValue parameter cannot be null.").WithMessage("criterionValue", ComparisonMode.Substring);
        }


        [Test]
        public void IsWithinDateRange_EmptyCriterionValue_ArgumentExceptionThrown()
        {
            // Act
            Action act = () => UtilityMethods.IsWithinDateRange(DateTime.Today, String.Empty);
            // Assert
            act.ShouldThrow<ArgumentException>("because criterionValue parameter cannot be empty.").WithMessage("criterionValue", ComparisonMode.Substring);
        }

        [Test]
        public void IsWithinDateRange_ValidStartDateAndEndDateInRange_ReturnsTrue()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-24Dec2012").Should().BeTrue("because the 23 Dec 2012 is within range");
        }

        [Test]
        public void IsWithinDateRange_OutOfRangeStartDateAndOutOfRangeEndDate_ReturnsTrue()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2013-24Dec2013").Should().BeFalse("because the 23 Dec 2012 is out of range");
        }


        [Test]
        public void IsWithinDateRange_InRangeStartDateAndOutOfRangeEndDate_ReturnsFalse()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-24Dec2011").Should().BeFalse("because the 23 Dec 2012 is out of range of the end date");
        }

        [Test]
        public void IsWithinDateRange_OutOfRangeStartDateAndInRangeEndDate_ReturnsFalse()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2014-24Dec2012").Should().BeFalse("because the 23 Dec 2012 is out of range of the start date");
        }

        [Test]
        public void IsWithinDateRange_InvalidStartDate_ThrowsArgumentException()
        {
            // Act
            Action act = () => UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "INAVLID_DATE-24Dec2012");
            // Assert
            act.ShouldThrow<ArgumentException>("because the start date is INVALID").WithMessage("criterionValue", ComparisonMode.Substring);
        }

        [Test]
        public void IsWithinDateRange_InvalidEndDate_ThrowsArgumentException()
        {
            // Act
            Action act = () => UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-INAVLID_DATE");
            // Assert
            act.ShouldThrow<ArgumentException>("because the end date is INVALID").WithMessage("criterionValue", ComparisonMode.Substring);
        }

        [Test]
        public void IsWithinDateRange_AfterStartDateWithNoEndDate_ReturnsTrue()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-").Should().BeTrue("because the 23 Dec 2012 is after the start date");
        }

        [Test]
        public void IsWithinDateRange_BeforeStartDateWithNoEndDate_ReturnFalse()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2013-").Should().BeFalse("because the 23 Dec 2012 is before the start date");
        }

        [Test]
        public void IsWithinDateRange_InvalidStartDateAndNoEndDate_ThrowsArgumentException()
        {
            // Act
            Action act = () => UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "INVALID_DATE-");
            // Assert
            act.ShouldThrow<ArgumentException>("because the start date is INVALID").WithMessage("criterionValue", ComparisonMode.Substring);
        }

        [Test]
        public void IsWithinDateRange_AfterStartDateWithNoHypenAndNoEndDate_ReturnsTrue()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012").Should().BeTrue("because the 23 Dec 2012 is after the start date");
        }

        [Test]
        public void IsWithinDateRange_BeforeStartDateWithNoHyphenAndNoEndDate_ReturnFalse()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2013").Should().BeFalse("because the 23 Dec 2012 is before the start date");
        }

        [Test]
        public void IsWithinDateRange_InvalidStartDatewithNoHyphenAndNoEndDate_ThrowsArgumentException()
        {
            // Act
            Action act = () => UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "INVALID_DATE");
            // Assert
            act.ShouldThrow<ArgumentException>("because the start date is INVALID").WithMessage("criterionValue", ComparisonMode.Substring);
        }

        [Test]
        public void IsWithinDateRange_BeforeEndDateWithNostartDate_ReturnsTrue()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-24Dec2012").Should().BeTrue("because the 23 Dec 2012 is before the end date");
        }

        [Test]
        public void IsWithinDateRange_AfterEndDateWithNoStartDate_ReturnFalse()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-22Dec2012").Should().BeFalse("because the 23 Dec 2012 is after the end date");
        }

        [Test]
        public void IsWithinDateRange_InvalidEndDateAndNoStartDate_ThrowsArgumentExcpetion()
        {
            // Act
            Action act = () => UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-INVALID_DATE");
            // Assert
            act.ShouldThrow<ArgumentException>("because the end date is INVALID").WithMessage("criterionValue", ComparisonMode.Substring);
        }

        [Test]
        public void IsWithinDateRange_NoDates_ReturnFalse()
        {
            // Assert
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-").Should().BeFalse("because the start and end date are absent");
        }
    }
}
