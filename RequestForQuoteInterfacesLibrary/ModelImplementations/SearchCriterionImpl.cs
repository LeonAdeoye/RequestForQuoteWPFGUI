using System.ComponentModel;
using System.Runtime.Serialization;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    public sealed class SearchCriterionImpl : ISearchCriterion, INotifyPropertyChanged
    {
        [DataMember] private bool isPrivate;
        [DataMember] private string key;
        [DataMember] private bool isFilter;
        [DataMember] private string owner;
        [DataMember] private string controlName;
        [DataMember] private string controlValue;

        public bool IsPrivate
        {
            get
            {
                return isPrivate;
            }
            set
            {
                if (isPrivate == value)
                    return;
                isPrivate = value;
                NotifyPropertyChanged("IsPrivate");
            }
        }

        public string DescriptionKey
        {
            get
            {
                return key;
            }
            set
            {
                if (key == value)
                    return;
                key = value;
                NotifyPropertyChanged("DescriptionKey");
            }
        }

        public bool IsFilter
        {
            get
            {
                return isFilter;
            }
            set
            {
                if (isFilter == value)
                    return;
                isFilter = value;
                NotifyPropertyChanged("IsFilter");
            }
        }

        public string Owner
        {
            get
            {
                return owner;
            }
            set
            {
                if (owner == value)
                    return;
                owner = value;
                NotifyPropertyChanged("Owner");
            }
        }

        public string ControlName
        {
            get
            {
                return controlName;
            }
            set
            {
                if (controlName == value)
                    return;
                controlName = value;
                NotifyPropertyChanged("ControlName");
            }
        }

        public string ControlValue
        {
            get
            {
                return controlValue;
            }
            set
            {
                if (controlValue == value)
                    return;
                controlValue = value;
                NotifyPropertyChanged("ControlValue");
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
