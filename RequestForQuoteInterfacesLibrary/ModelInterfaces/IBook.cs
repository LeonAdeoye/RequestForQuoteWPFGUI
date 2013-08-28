namespace RequestForQuoteInterfacesLibrary.ModelInterfaces
{
    public interface IBook
    {
        string BookCode { get; set; }
        bool IsValid { get; set; }
        string Entity { get; set; }
    }
}
