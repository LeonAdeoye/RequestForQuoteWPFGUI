using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IUserManager
    {
        List<IUser> Users { get; set; }
        void AddUser(string userId, string firstName, string lastName, string emailAddress, string locationName, int groupId, bool isValid);
        bool SaveToDatabase(string userId, string firstName, string lastName, string emailAddress, string locationName, int groupId);
        bool UpdateValidity(string userId, bool isValid);
        void Initialize();
    }
}
