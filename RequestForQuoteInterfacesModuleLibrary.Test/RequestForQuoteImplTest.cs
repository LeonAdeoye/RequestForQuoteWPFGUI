﻿using System;
using System.Collections.Generic;
using FluentAssertions;
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

        [Test]
        public void DoesRequestMatchFilter_NullCriteria_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new RequestForQuoteImpl().DoesRequestMatchFilter(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because criteria parameter cannot be empty.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidTradeStartDateCriteria_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() {TradeDate = new DateTime(2012, 12, 23)}.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "INVALID_DATE-24Dec2012"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid start date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidTradeEndDate_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "22Dec2014-INVALID_DATE"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid end date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidTradeEndDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-INVALID_DATE"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid end date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidTradeStartDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.TRADE_DATE_CRITERION, "INVALID_DATE-"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid start date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_ClientCriteriaMatch_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() {Client = testClient}.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.CLIENT_CRITERION, "1"}
                })
                                                           .Should()
                                                           .BeTrue("because the client identifiers match");
        }

        [Test]
        public void DoesRequestMatchFilter_ClientCriteriaMisMatch_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { Client = testClient }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.CLIENT_CRITERION, "2"}
                })
                                                           .Should()
                                                           .BeFalse("because the client identifiers do not match");
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidCriteriaKey_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { Client = testClient }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {"INVALID_CRITERIA", "2"}
                })
                                                           .Should()
                                                           .BeFalse("because the criteria key is inavlid");
        }

        [Test]
        public void DoesRequestMatchFilter_BookCriteriaMatch_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { BookCode = "testBook" }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.BOOK_CRITERION, "testBook"}
                })
                                                           .Should()
                                                           .BeTrue("because the book codes match");
        }

        [Test]
        public void DoesRequestMatchFilter_BookCriteriaMisMatch_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { BookCode = "testBook1" }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.BOOK_CRITERION, "testBook2"}
                })
                                                           .Should()
                                                           .BeFalse("because the book codes do not match");
        }

        [Test]
        public void DoesRequestMatchFilter_StatusCriteriaMatch_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { Status = StatusEnum.TRADEDAWAY }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.STATUS_CRITERION, StatusEnum.TRADEDAWAY.ToString()}
                })
                                                           .Should()
                                                           .BeTrue("because the status match");
        }

        [Test]
        public void DoesRequestMatchFilter_StatusCriteriaMisMatch_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { Status = StatusEnum.FILLED }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.STATUS_CRITERION, StatusEnum.TRADEDAWAY.ToString()}
                })
                                                           .Should()
                                                           .BeFalse("because the status do not match");
        }

        [Test]
        public void DoesRequestMatchFilter_SameTradeDateCriteria_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = DateTime.Today }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, DateTime.Today.ToShortDateString()}
                })
                                                           .Should()
                                                           .BeTrue("because the trade dates match");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaOutOfRange_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012,12,23)}.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2015"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's trade date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaOutOfRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2015-"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's trade date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithinRange_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2011"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithinRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2011-"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithinEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-23Dec2016"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaOutsideEndDateRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-22Dec2012"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's trade end date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaSameEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-23Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade end date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaSameStartDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2012-"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade start date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothStartAndEndDate_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "22Dec2012-24Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade date is inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesAndSameStartDate_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "23Dec2012-24Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesAndSameEndDate_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "22Dec2012-23Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesAndStartDateLaterEndDate_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "24Dec2012-22Dec2012"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's trade date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_TradeDateCriteriaWithBothDatesMissing_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { TradeDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-"}
                })
                                                           .Should()
                                                           .BeFalse("because both the start and end dates are missing");
        }
        
        [Test]
        public void DoesRequestMatchFilter_SameExpiryDateCriteria_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = DateTime.Today }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, DateTime.Today.ToShortDateString()}
                })
                                                           .Should()
                                                           .BeTrue("because the expiry dates match");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaOutOfRange_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012,12,23)}.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2015"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's expiry date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaOutOfRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2015-"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's expiry date is not within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithinRange_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2011"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithinRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2011-"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithinEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-23Dec2016"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry date is within range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaOutsideEndDateRangeWithHyphen_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-22Dec2012"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's expiry end date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaSameEndDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-23Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry end date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaSameStartDateRangeWithHyphen_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2012-"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry start date is just inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothStartAndEndDate_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "22Dec2012-24Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry date is inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesAndSameStartDate_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "23Dec2012-24Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesAndSameEndDate_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "22Dec2012-23Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's expiry date is still inside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesAndStartDateLaterEndDate_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "24Dec2012-22Dec2012"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's expiry date is outside range");
        }

        [Test]
        public void DoesRequestMatchFilter_ExpiryDateCriteriaWithBothDatesMissing_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-"}
                })
                                                           .Should()
                                                           .BeFalse("because both the expiry start and end dates are missing");
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidExpiryStartDateCriteria_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() {ExpiryDate = new DateTime(2012, 12, 23)}.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "INVALID_DATE-24Dec2012"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid start date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidExpiryEndDate_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "22Dec2014-INVALID_DATE"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid end date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidExpiryEndDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "-INVALID_DATE"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid end date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_InvalidExpiryStartDateWithHyphen_ArgumentExceptionThrown()
        {
            // Act
            Action act =
                () =>
                new RequestForQuoteImpl() { ExpiryDate = new DateTime(2012, 12, 23) }.DoesRequestMatchFilter(new Dictionary
                                                                                                              <string,
                                                                                                              string>()
                    {
                        {RequestForQuoteConstants.EXPIRY_DATE_CRITERION, "INVALID_DATE-"}
                    });
            // Assert
            act.ShouldThrow<ArgumentException>("because invalid start date criteria.").WithMessage("criteria", ComparisonMode.Substring);
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithNullLegs_ReturnsFalse()
        {
            // Assert
            new RequestForQuoteImpl() { Legs = null }.DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's legs collection is null");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithOneLegWithMatchingRIC_ReturnsTrue()
        {
            // Assert
            new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>(){ new OptionDetailImpl() {RIC = "test.HK"}} }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's leg has a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithOneLegWithoutMatchingRIC_ReturnFalse()
        {
            // Assert
            new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.HK" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.T"}
                })
                                                           .Should()
                                                           .BeFalse("because the RFQ's leg does not have a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithMultipleLegsWithAllMatchingRIC_ReturnTrue()
        {
            // Assert
            new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.HK" }, new OptionDetailImpl() { RIC = "test.HK" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                })
                                                           .Should()
                                                           .BeTrue("because all the RFQ's legs have a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithMultipleLegsWithoutMatchingRIC_ReturnFalse()
        {
            // Assert
            new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.T" }, new OptionDetailImpl() { RIC = "test.T" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                })
                                                           .Should()
                                                           .BeFalse("because all the RFQ's leg do not have a matching RIC");
        }

        [Test]
        public void DoesRequestMatchFilter_UnderlyingCriteriaWithMultipleLegsWithOneMatchingRIC_ReturnTrue()
        {
            // Assert
            new RequestForQuoteImpl() { Legs = new List<OptionDetailImpl>() { new OptionDetailImpl() { RIC = "test.HK" }, new OptionDetailImpl() { RIC = "test.T" } } }
                .DoesRequestMatchFilter(new Dictionary<string, string>()
                {
                    {RequestForQuoteConstants.UNDERLYIER_CRITERION, "test.HK"}
                })
                                                           .Should()
                                                           .BeTrue("because one of RFQ's multiple legs has a matching RIC");
        }
    }
}
