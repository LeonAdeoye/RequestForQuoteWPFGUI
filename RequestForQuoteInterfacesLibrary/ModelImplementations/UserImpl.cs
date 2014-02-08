using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    [DebuggerDisplay("User with user id {UserId}, first name {FirstName}, last name {LastName}, " +
                     "email address {EmailAddress}, location name {LocationName}, group id {GroupId}, and validity of {IsValid}.")]
    public class UserImpl : IUser, INotifyPropertyChanged
    {
        [DataMember] private string userId;
        [DataMember] private string firstName;
        [DataMember] private string lastName;
        [DataMember] private string emailAddress;
        [DataMember] private int groupId;
        [DataMember] private string locationName;
        [DataMember] private bool isValid;

        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                NotifyPropertyChanged("UserId");
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
                NotifyPropertyChanged("LastName");
            }
        }

        public string EmailAddress
        {
            get
            {
                return emailAddress;
            }
            set
            {
                emailAddress = value;
                NotifyPropertyChanged("EmailAddress");
            }
        }

        public int GroupId
        {
            get
            {
                return groupId;
            }
            set
            {
                groupId = value;
                NotifyPropertyChanged("GroupId");
            }
        }

        public string LocationName
        {
            get
            {
                return locationName;
            }
            set
            {
                locationName = value;
                NotifyPropertyChanged("LocationName");
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
