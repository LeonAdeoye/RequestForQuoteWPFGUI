using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using RequestForQuoteInterfacesLibrary.Enums;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    [DebuggerDisplay("Bank holiday on date {HolidayDate} at location {Location}.")]
    public sealed class BankHolidayImpl : IBankHoliday, INotifyPropertyChanged
    {
        [DataMember] private string location;
        [DataMember] private string holidayDate;

        public LocationEnum Location
        {
            get
            {
                return (LocationEnum) Enum.Parse(typeof(LocationEnum), location);
            }
            set
            {
                location = value.ToString();
                NotifyPropertyChanged("Location");
            }
        }

        public DateTime HolidayDate
        {
            get
            {
                return Convert.ToDateTime(holidayDate);
            }
            set
            {
                holidayDate = value.ToShortDateString();
                NotifyPropertyChanged("HolidayDate");
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
