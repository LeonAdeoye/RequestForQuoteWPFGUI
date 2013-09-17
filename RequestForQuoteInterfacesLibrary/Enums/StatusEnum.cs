using System.ComponentModel;

namespace RequestForQuoteInterfacesLibrary.Enums
{
    public enum StatusEnum
    {
        [DescriptionAttribute("Pending pick up")]
        PENDING,
        [DescriptionAttribute("Picked up")]
        PICKEDUP,
        [DescriptionAttribute("Filled")]
        FILLED,
        [DescriptionAttribute("Traded away")]
        TRADEDAWAY,
        [DescriptionAttribute("Invalid")]
        INVALID
    }
}