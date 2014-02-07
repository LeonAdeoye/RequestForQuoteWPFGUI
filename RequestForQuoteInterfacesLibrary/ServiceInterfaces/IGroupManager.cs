﻿using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IGroupManager
    {
        List<IGroup> Groups { get; set; }
        void AddUser(int groupId, string groupName, bool isValid);
        bool SaveToDatabase(int groupId, string groupName);
        bool UpdateValidity(int groupId, bool isValid);
        void Initialize();
    }
}
