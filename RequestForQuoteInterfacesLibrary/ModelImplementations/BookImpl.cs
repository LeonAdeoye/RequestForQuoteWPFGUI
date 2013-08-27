using System.ComponentModel;
using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    public class BookImpl : IBook, INotifyPropertyChanged
    {
        private bool isValid;
        public string BookCode { get; set; }
        public string Entity { get; set; }
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
            var builder = new StringBuilder("BookCode = ");
            builder.Append(BookCode);
            builder.Append(", Entity = ");
            builder.Append(Entity);
            builder.Append(", IsValid = ");
            builder.Append(IsValid);
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
