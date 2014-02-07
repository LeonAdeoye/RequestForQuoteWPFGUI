using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public class GroupImpl : IGroup
    {
        private int groupId;
        private string groupName;
        private bool isValid;

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }
    }
}
