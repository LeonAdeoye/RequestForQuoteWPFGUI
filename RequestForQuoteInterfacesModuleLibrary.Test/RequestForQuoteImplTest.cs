using System;
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
                    {RequestForQuoteConstants.TRADE_DATE_CRITERION, "-23Dec2012"}
                })
                                                           .Should()
                                                           .BeTrue("because the RFQ's trade end date is outside range");
        }
    }
}
