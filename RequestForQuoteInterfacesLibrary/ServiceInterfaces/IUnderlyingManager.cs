﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IUnderlyingManager
    {
        List<IUnderlyier> Underlyiers { get; set; }
        void AddUnderlyier(string RIC, string BBG, string description, bool isValid, bool saveToDatabase);
        void RemoveUnderlyier();
    }
}