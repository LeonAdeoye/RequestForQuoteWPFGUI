namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IConfigurationManager
    {
        bool IsStandAlone();
        bool? IsTrue(string configKey);
        int? GetIntValue(string configKey);
        decimal? GetDecimalValue(string configKey);
        void Add(string configKey, string value);
        string this[string configKey] { get; set; }
    }
}
