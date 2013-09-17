using System.Collections.Generic;
using System.ComponentModel;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public sealed class SearchImpl : ISearch, INotifyPropertyChanged
    {
        private bool isPrivate;
        public string DescriptionKey { get; set; }
        public bool IsFilter { get; set; }
        public string Owner { get; set; }
        public IDictionary<string, string> Criteria { get; set; }

        public bool IsPrivate
        {
            get
            {
                return isPrivate;
            }
            set
            {
                isPrivate = value;
                NotifyPropertyChanged("IsPrivate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
