using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace RequestForQuoteInterfacesLibrary.Enums
{
    [MarkupExtensionReturnType(typeof(EnumerationMember[]))]
    public class EnumerationExtension : MarkupExtension
    {
        private Type typeOfEnum;

        // constructor takes argument x:Type which must be an enum and cannot be null
        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            if (enumType.IsEnum == false)
                throw new ArgumentException("Type must be an Enum.");

            typeOfEnum = enumType;
        }

        // Optional property get/set to be used instead of constructor setting
        public Type TypeOfEnum
        {
            get { return typeOfEnum; }
            private set
            {
                if (typeOfEnum == value)
                    return;

                // If nullable type is used to set this property, get the underlying type.
                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                typeOfEnum = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(typeOfEnum);

            return (from object enumValue in enumValues select new EnumerationMember
              {
                  Value = enumValue,
                  Description = GetDescription(enumValue)
              }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = typeOfEnum
              .GetField(enumValue.ToString())
              .GetCustomAttributes(typeof(DescriptionAttribute), false)
              .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute != null ? descriptionAttribute.Description : enumValue.ToString();
        }

        public class EnumerationMember
        {
            public string Description { get; set; }
            public object Value { get; set; }
        }
    }
}
