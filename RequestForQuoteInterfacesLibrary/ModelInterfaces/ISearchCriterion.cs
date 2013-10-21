namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface ISearchCriterion
    {
        string DescriptionKey { get; set; }
        bool IsPrivate { get; set; }
        bool IsFilter { get; set; }
        string Owner { get; set; }
        string ControlName { get; set; }
        string ControlValue { get; set; }
    }
}
