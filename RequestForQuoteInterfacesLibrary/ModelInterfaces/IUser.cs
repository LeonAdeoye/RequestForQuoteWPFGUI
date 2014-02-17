using RequestForQuoteInterfacesLibrary.Enums;

namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IUser
    {
        string UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string EmailAddress { get; set; }
        int GroupId { get; set; }
        LocationEnum LocationName { get; set; }
        bool IsValid { get; set; }
    }
}
