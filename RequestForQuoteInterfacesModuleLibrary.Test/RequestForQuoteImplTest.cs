using System;
using System.Collections.Generic;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelImplementations;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesModuleLibrary.Test
{
    public class RequestForQuoteImplTest
    {
        private readonly IClient testClient = new ClientImpl()
        {
            Identifier = 1,
            IsValid = true,
            Name = "test client",
            Tier = TierEnum.Top.ToString()
        };

        #region DoesRequestMatchFilter

        [Test]
        [ExpectedException("System.ArgumentNullException")]
        public void DoesRequestMatchFilter_NullCriteria_ArgumentNullExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl().DoesRequestMatchFilter(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidTradeStartDateCriteria_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() {TradeDate = new DateTime(2012, 12, 23)}.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "INVALID_DATE-24Dec2012"}
                    });

        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidTradeEndDate_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "22Dec2014-INVALID_DATE"}
                    });
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidTradeEndDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-INVALID_DATE"}
                    });
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidTradeStartDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "INVALID_DATE-"}
                    });
        }

        [Test]
        public void DoesRequestMatchFilter_ClientCriteriaMatch_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() {Client = testClient}.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.CLIENT_CRITERION, "1"}
                }),"because the client identifiers match");
        }

        [Test]
        public void DoesRequestMatchFilter_ClientCriteriaMisMatch_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { Client = testClient }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.CLIENT_CRITERION, "2"}
                }),"because the client identifiers do not match");
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidCriteriaKey_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { Client = testClient }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {"INVALID_CRITERIA", "2"}
                }),"because the criteria key is inavlid");
        }

        [Test]
        public void DoesRequestMatchFilter_BookCriteriaMatch_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { BookCode = "testBook" }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.BOOK_CRITERION, "testBook"}
                }),"because the book codes match");
        }

        [Test]
        public void DoesRequestMatchFilter_BookCriteriaMisMatch_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { BookCode = "testBook1" }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.BOOK_CRITERION, "testBook2"}
                }),"because the book codes do not match");
        }

        [Test]
        public void DoesRequestMatchFilter_StatusCriteriaMatch_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { Status = StatusEnum.TRADEDAWAY }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.STATUS_CRITERION, StatusEnum.TRADEDAWAY.ToString()}
                }),"because the status match");
        }

        [Test]
        public void DoesRequestMatchFilter_StatusCriteriaMisMatch_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { Status = StatusEnum.FILLED }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.STATUS_CRITERION, StatusEnum.TRADEDAWAY.ToString()}
                }),"because the status do not match");
        }

        [Test]
        public void DoesRequestMatchFilter_SameTradeDateCriteria_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = DateTime.Today }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, DateTime.Today.ToShortDateString()}
                }),"because the trade dates match");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaOutOfRange_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { TradeDate = new DateTime(2012,12,23)}.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2015"}
                }),"because the RFQ's trade date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaOutOfRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2015-"}
                }),"because the RFQ's trade date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithinRange_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2011"}
                }),"because the RFQ's trade date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithinRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2011-"}
                }),"because the RFQ's trade date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithinEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-23Dec2016"}
                }),"because the RFQ's trade date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaOutsideEndDateRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-22Dec2012"}
                }),"because the RFQ's trade end date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaSameEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-23Dec2012"}
                }),"because the RFQ's trade end date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaSameStartDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2012-"}
                }),"because the RFQ's trade start date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothStartAndEndDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "22Dec2012-24Dec2012"}
                }),"because the RFQ's trade date is inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesAndSameStartDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2012-24Dec2012"}
                }),"because the RFQ's trade date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesAndSameEndDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "22Dec2012-23Dec2012"}
                }),"because the RFQ's trade date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesAndStartDateLaterEndDate_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "24Dec2012-22Dec2012"}
                }),"because the RFQ's trade date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesMissing_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-"}
                }),"because both the start and end dates are missing");
        }
        
        [Test]
        public void DoesRequestMatchFilter_SameExpiryDateCriteria_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = DateTime.Today }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, DateTime.Today.ToShortDateString()}
                }),"because the expiry dates match");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaOutOfRange_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012,12,23)}.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2015"}
                }),"because the RFQ's expiry date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaOutOfRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2015-"}
                }),"because the RFQ's expiry date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithinRange_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2011"}
                }),"because the RFQ's expiry date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithinRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2011-"}
                }),"because the RFQ's expiry date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithinEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-23Dec2016"}
                }),"because the RFQ's expiry date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaOutsideEndDateRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-22Dec2012"}
                }),"because the RFQ's expiry end date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaSameEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-23Dec2012"}
                }),"because the RFQ's expiry end date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaSameStartDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2012-"}
                }),"because the RFQ's expiry start date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothStartAndEndDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "22Dec2012-24Dec2012"}
                }),"because the RFQ's expiry date is inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesAndSameStartDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2012-24Dec2012"}
                }),"because the RFQ's expiry date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesAndSameEndDate_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "22Dec2012-23Dec2012"}
                }),"because the RFQ's expiry date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesAndStartDateLaterEndDate_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "24Dec2012-22Dec2012"}
                }),"because the RFQ's expiry date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesMissing_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-"}
                }),"because both the expiry start and end dates are missing");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidExpiryStartDateCriteria_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() {ExpiryDate = new DateTime(2012, 12, 23)}.DoesRequestMatchFilter(new Dictionary
                                                                                                            <string,
                                                                                                            string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "INVALID_DATE-24Dec2012"}
                });
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidExpiryEndDate_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                            <string,
                                                                                                            string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "22Dec2014-INVALID_DATE"}
                });
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidExpiryEndDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                            <string,
                                                                                                            string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-INVALID_DATE"}
                });
        }

        [Test]
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "criteria")]
        public void DoesRequestMatchFilter_InvalidExpiryStartDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            var test = new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                            <string,
                                                                                                            string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "INVALID_DATE-"}
                });
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithNullLegs_ReturnsFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { Legs = null }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                }),"because the RFQ's legs collection is null");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithOneLegWithMatchingRIC_ReturnsTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>(){ new OptionDetailImpl() {RIC = "test.HK"}} }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                }),"because the RFQ's leg has a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithOneLegWithoutMatchingRIC_ReturnFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.HK" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.T"}
                }),"because the RFQ's leg does not have a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithMultipleLegsWithAllMatchingRIC_ReturnTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.HK" }, new OptionDetailImpl() { RIC = "test.HK" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                }),"because all the RFQ's legs have a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithMultipleLegsWithoutMatchingRIC_ReturnFalse()
        {
            // Assert
            Assert.IsFalse(new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.T" }, new OptionDetailImpl() { RIC = "test.T" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                }),"because all the RFQ's leg do not have a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithMultipleLegsWithOneMatchingRIC_ReturnTrue()
        {
            // Assert
            Assert.IsTrue(new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.HK" }, new OptionDetailImpl() { RIC = "test.T" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                }),"because one of RFQ's multiple legs has a matching RIC");
        }
        #endregion

    }
}
