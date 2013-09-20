using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    public class ChatMessageImpl : INotifyPropertyChanged
    {
        [DataMember] private string content;
        [DataMember] private string owner;
        [DataMember] private DateTime timeStamp;
        [DataMember] private int requestForQuoteId;
        [DataMember] private int sequenceId;

        public ChatMessageImpl()
        {
        }

        public ChatMessageImpl(string owner, string content, int requestForQuoteId, int sequenceId, DateTime timeStamp)
        {
            this.content = content;
            this.owner = owner;
            this.requestForQuoteId = requestForQuoteId;
            this.sequenceId = sequenceId;
            this.timeStamp = Convert.ToDateTime(timeStamp);
        }

        public String Owner
        {
            get { return owner; }
            set 
            { 
                owner = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Owner"));
            }
        }

        public String Content
        {
            get { return content; }
            set
            {
                content = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Content"));
            }
        }

        public int RequestForQuoteId
        {
            get { return requestForQuoteId; }
            set
            {
                requestForQuoteId = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RequestForQuoteId"));
            }
        }

        public int SequenceId
        {
            get { return sequenceId; }
            set
            {
                sequenceId = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SequenceId"));
            }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set
            {
                timeStamp = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TimeStamp"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
