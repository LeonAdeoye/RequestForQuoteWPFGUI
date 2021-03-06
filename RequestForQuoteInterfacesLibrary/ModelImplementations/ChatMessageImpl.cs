﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RequestForQuoteInterfacesLibrary.ModelImplementations
{
    [DataContract]
    [DebuggerDisplay("Chat message with content {Content} sent by {Owner} for RFQ ID {RequestForQuoteId} with sequence ID {SequenceId} at time {TimeStamp}")]
    public class ChatMessageImpl : INotifyPropertyChanged
    {
        [DataMember] private string content;
        [DataMember] private string owner;
        [DataMember] private string timeStamp;
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
            this.timeStamp = timeStamp.ToLongTimeString();
        }

        public String Owner
        {
            get
            {
                return owner;
            }
            set 
            { 
                owner = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Owner"));
            }
        }

        public String Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Content"));
            }
        }

        public int RequestForQuoteId
        {
            get
            {
                return requestForQuoteId;
            }
            set
            {
                requestForQuoteId = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("RequestForQuoteId"));
            }
        }

        public int SequenceId
        {
            get
            {
                return sequenceId;
            }
            set
            {
                sequenceId = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SequenceId"));
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return Convert.ToDateTime(timeStamp);
            }
            set
            {
                timeStamp = value.ToShortDateString();
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
