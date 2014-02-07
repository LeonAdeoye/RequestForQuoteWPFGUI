namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IGroup
    {
        int GroupId { get; set; }
        string GroupName { get; set; }
        bool IsValid { get; set; }
    }
}
