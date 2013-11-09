using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    [DebuggerDisplay("Client with name {Name}, tier {Tier}, and identifier {Identifier}.")]
    public sealed class ClientImpl : IClient, INotifyPropertyChanged
    {
        [DataMember] private bool isValid;
        [DataMember] private int identifier;
        [DataMember] private string name;
        [DataMember] private string tier;
        
        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
                NotifyPropertyChanged("IsValid");
            }
        }

        public int Identifier
        {
            get
            {
                return identifier;
            }
            set
            {
                identifier = value;
                NotifyPropertyChanged("Identifier");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public string Tier
        {
            get
            {
                return tier;
            }
            set
            {
                tier = value;
                NotifyPropertyChanged("Tier");
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder("Identifier = ");
            builder.Append(identifier);
            builder.Append(", Name = ");
            builder.Append(name);
            builder.Append(", Tier = ");
            builder.Append(tier);
            return builder.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
