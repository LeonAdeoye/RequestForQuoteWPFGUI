using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.EventPayloads
{
    public class NewGroupEventPayload 
    {
        public IGroup NewGroup { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("Group Id = ");
            builder.Append(NewGroup.GroupId);
            builder.Append(", Group name = ");
            builder.Append(NewGroup.GroupName);
            builder.Append(", Is Valid = ");
            builder.Append(NewGroup.IsValid ? "TRUE" : "FALSE");
            return builder.ToString();
        }
    }
}
