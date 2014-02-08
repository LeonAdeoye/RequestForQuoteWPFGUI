using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    [DebuggerDisplay("Group with group id {GroupId}, group name {GroupName}, and validity of {IsValid}.")]
    public class GroupImpl : IGroup, INotifyPropertyChanged
    {
        [DataMember] private int groupId;
        [DataMember] private string groupName;
        [DataMember] private bool isValid;

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

        public string GroupName
        {
            get
            {
                return groupName;
            }
            set 
            { 
                groupName = value;
                NotifyPropertyChanged("GroupName");
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
