using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IUnderlyier
    {
        string RIC { get; set; }
        string BBG { get; set; }
        string Description { get; set; }
        bool IsValid { get; set; }
    }
}
