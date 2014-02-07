using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewUserEventPayload 
    {
        public IUser NewUser { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("UserId = ");
            builder.Append(NewUser.UserId);
            builder.Append(", First name = ");
            builder.Append(NewUser.FirstName);
            builder.Append(", Last name = ");
            builder.Append(NewUser.LastName);
            builder.Append(", Email address = ");
            builder.Append(NewUser.EmailAddress);
            builder.Append(", Location name = ");
            builder.Append(NewUser.LocationName);
            builder.Append(", Group id = ");
            builder.Append(NewUser.GroupId);
            builder.Append(", IsValid = ");
            builder.Append(NewUser.IsValid ? "TRUE" : "FALSE");
            return builder.ToString();
        }
    }
}
