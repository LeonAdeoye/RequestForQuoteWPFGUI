namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IGroup
    {
        string GroupId { get; set; }
        string GroupName { get; set; }
        bool IsValid { get; set; }
    }
}
