using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    [DebuggerDisplay("Book with book code {BookCode}, entity {Entity}, and validity of {IsValid}.")]
    public sealed class BookImpl : IBook, INotifyPropertyChanged
    {
        [DataMember] private bool isValid;
        [DataMember] private string bookCode;
        [DataMember] private string entity;

        public string BookCode
        {
            get
            {
                return bookCode;
            }
            set
            {
                bookCode = value;
                NotifyPropertyChanged("BookCode");
            }
        }

        public string Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
                NotifyPropertyChanged("Entity");
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
