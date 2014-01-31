using System;
using FluentAssertions;
using NUnit.Framework;
using RequestForQuoteInterfacesLibrary.ModelImplementations;

namespace RequestForQuoteInterfacesModuleLibrary.Test
{
    public class RequestForQuoteImplTest
    {
        [Test]
        public void DoesRequestMatchFilter_NullCriteria_ArgumentNullExceptionThrown()
        {
            // Act
            Action act = () => new RequestForQuoteImpl().DoesRequestMatchFilter(null);
            // Assert
            act.ShouldThrow<ArgumentNullException>("because criteria parameter cannot be empty.").WithMessage("criteria", ComparisonMode.Substring);
        }
    }
}
