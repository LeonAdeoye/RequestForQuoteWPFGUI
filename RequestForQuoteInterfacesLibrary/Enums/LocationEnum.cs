using System.ComponentModel;

namespace RequestForQuoteInterfacesLibrary.Enums
{
    public enum LocationEnum
    {
        [DescriptionAttribute("London")]
        LONDON,
        [DescriptionAttribute("Tokyo")]
        TOKYO,
        [DescriptionAttribute("Singapore")]
        SINGAPORE,
        [DescriptionAttribute("New York")]
        NEW_YORK,
        [DescriptionAttribute("Hong Kong")]
        HONG_KONG,
        [DescriptionAttribute("Shanghai")]
        SHANGHAI,
        [DescriptionAttribute("Sydney")]
        SYDNEY
    }
}