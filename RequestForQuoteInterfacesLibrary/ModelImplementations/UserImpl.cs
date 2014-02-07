using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public class UserImpl : IUser
    {
        private string userId;
        private string firstName;
        private string lastName;
        private string emailAddress;
        private int groupId;
        private string locationName;
        private bool isValid;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; }
        }

        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; }
        }
    }
}
