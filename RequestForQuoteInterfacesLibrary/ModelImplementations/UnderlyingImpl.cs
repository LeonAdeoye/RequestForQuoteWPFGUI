using System.Runtime.Serialization;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    public sealed class UnderlyingImpl : IUnderlying
    {
        [DataMember] private string ric;
        [DataMember] private string description;
        [DataMember] private bool isValid;

        public string RIC
        {
            get { return ric; }
            set
            {
                if (ric != value)
                    ric = value;
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                    description = value;
            }
        }
        public bool IsValid
       {
            get { return isValid; }
            set
            {
                if (isValid != value)
                    isValid = value;
            }
        }
    }
}
