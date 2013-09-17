using System.ComponentModel;
using System.Globalization;
using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public sealed class ClientImpl : IClient, INotifyPropertyChanged
    {
        private bool isValid;
        public int Identifier { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
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

        public override string ToString()
        {
            var builder = new StringBuilder("Identifier = ");
            builder.Append(Identifier.ToString(CultureInfo.InvariantCulture));
            builder.Append(", Name = ");
            builder.Append(Name);
            builder.Append(", Tier = ");
            builder.Append(Tier);
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
