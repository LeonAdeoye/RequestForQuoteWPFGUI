using System;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.Utilities;

namespace RequestForQuoteInterfacesModuleLibrary.Test
{
    public class UtilityMethodsTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criterionValue")]
        public void IsWithinDateRange_NullCriterionValue_ArgumentExceptionThrown()
        {
            // Act
            UtilityMethods.IsWithinDateRange(DateTime.Today, null);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criterionValue")]
        public void IsWithinDateRange_EmptyCriterionValue_ArgumentExceptionThrown()
        {
            // Act
            UtilityMethods.IsWithinDateRange(DateTime.Today, String.Empty);
        }

        [Test]
        public void IsWithinDateRange_ValidStartDateAndEndDateInRange_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-24Dec2012"),"because the 23 Dec 2012 is within range");
        }

        [Test]
        public void IsWithinDateRange_OutOfRangeStartDateAndOutOfRangeEndDate_ReturnsTrue()
        {
            // Assert
            Assert.IsFalse(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2013-24Dec2013"), "because the 23 Dec 2012 is out of range");
        }


        [Test]
        public void IsWithinDateRange_InRangeStartDateAndOutOfRangeEndDate_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-24Dec2011"), "because the 23 Dec 2012 is out of range of the end date");
        }

        [Test]
        public void IsWithinDateRange_OutOfRangeStartDateAndInRangeEndDate_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2014-24Dec2012"), "because the 23 Dec 2012 is out of range of the start date");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criterionValue")]
        public void IsWithinDateRange_InvalidStartDate_ThrowsArgumentException()
        {
            // Act
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "INAVLID_DATE-24Dec2012");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criterionValue")]
        public void IsWithinDateRange_InvalidEndDate_ThrowsArgumentException()
        {
            // Act
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-INAVLID_DATE");
        }

        [Test]
        public void IsWithinDateRange_AfterStartDateWithNoEndDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012-"),"because the 23 Dec 2012 is after the start date");
        }

        [Test]
        public void IsWithinDateRange_BeforeStartDateWithNoEndDate_ReturnFalse()
        {
            // Assert
            Assert.IsFalse(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2013-"), "because the 23 Dec 2012 is before the start date");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criterionValue")]
        public void IsWithinDateRange_InvalidStartDateAndNoEndDate_ThrowsArgumentException()
        {
            // Act
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "INVALID_DATE-");;
        }

        [Test]
        public void IsWithinDateRange_AfterStartDateWithNoHypenAndNoEndDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2012"), "because the 23 Dec 2012 is after the start date");
        }

        [Test]
        public void IsWithinDateRange_BeforeStartDateWithNoHyphenAndNoEndDate_ReturnFalse()
        {
            // Assert
            Assert.IsFalse(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "22Dec2013"), "because the 23 Dec 2012 is before the start date");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criterionValue")]
        public void IsWithinDateRange_InvalidStartDatewithNoHyphenAndNoEndDate_ThrowsArgumentException()
        {
            // Act
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "INVALID_DATE");
        }

        [Test]
        public void IsWithinDateRange_BeforeEndDateWithNostartDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-24Dec2012"), "because the 23 Dec 2012 is before the end date");
        }

        [Test]
        public void IsWithinDateRange_AfterEndDateWithNoStartDate_ReturnFalse()
        {
            // Assert
            Assert.IsFalse(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-22Dec2012"), "because the 23 Dec 2012 is after the end date");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criterionValue")]
        public void IsWithinDateRange_InvalidEndDateAndNoStartDate_ThrowsArgumentExcpetion()
        {
            // Act
            UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-INVALID_DATE");
        }

        [Test]
        public void IsWithinDateRange_NoDates_ReturnFalse()
        {
            // Assert
            Assert.IsFalse(UtilityMethods.IsWithinDateRange(new DateTime(2012, 12, 23), "-"), "because the start and end date are absent");
        }
    }
}
