using System.Collections.Generic;
using System.Collections.ObjectModel;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IClientManager
    {
        List<IClient> Clients { get; set; }
        void AddClient(string name, string tier, bool isValid);
        bool SaveToDatabase(string name, string tier);
        bool UpdateTier(int identifier, string tier);
        bool UpdateValidity(int identifier, bool isValid);
        void Initialize();
        IClient GetClientWithMatchingIdentifier(int clientId);
    }
}