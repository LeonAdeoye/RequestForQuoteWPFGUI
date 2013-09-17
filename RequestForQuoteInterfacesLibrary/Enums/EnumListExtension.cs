using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;

namespace RequestForQuoteInterfacesLibrary.Enums
{
    public class EnumListExtension : MarkupExtension
    {
        private Type theEnumType;
        private bool showDisplayString;

        public EnumListExtension()
        {
        }
 
        public EnumListExtension(Type enumType)
        {
            this.TheEnumType = enumType;
        }

        public Type TheEnumType
        {
            get { return this.theEnumType; }
            set
            {
                if (value != this.theEnumType)
                {
                    if (null != value)
                    {
                        var enumType = Nullable.GetUnderlyingType(value) ?? value;
 

                        if (enumType.IsEnum == false)
                            throw new ArgumentException("Type must be for an Enum.");
                    }
 

                    this.theEnumType = value;
                }
            }
        }
 
        public bool ShowDisplayString
        {
            get { return this.showDisplayString; }
            set { this.showDisplayString = value; }
        } 

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == this.theEnumType)
                throw new InvalidOperationException("The EnumType must be specified.");
 
            var actualEnumType = Nullable.GetUnderlyingType(this.theEnumType) ?? this.theEnumType;
            var enumValues = Enum.GetValues(actualEnumType);
 
            if (this.showDisplayString == false)
            {
                if (actualEnumType == this.theEnumType)
                    return enumValues;
 
                var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
                enumValues.CopyTo(tempArray, 1);
                return tempArray;
            }
 
            List<string> items = new List<string>();
 
            if (actualEnumType != this.theEnumType)
                items.Add(null);
 
            foreach (var item in Enum.GetValues(this.theEnumType))
            {
                var itemString = item.ToString();
                var field = this.theEnumType.GetField(itemString);
                var attribs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
 
                if (attribs != null && attribs.Length > 0)
                    itemString = ((DescriptionAttribute)attribs[0]).Description;

                items.Add(itemString);
            }

            return items.ToArray();
        }
    }
}
