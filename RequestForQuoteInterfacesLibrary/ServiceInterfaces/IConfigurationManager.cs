namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IConfigurationManager
    {
        string CurrentUser { get; set; }
        bool IsStandAlone { get; set; }
        bool? IsTrue(string configKey);
        int? GetIntValue(string configKey);
        decimal? GetDecimalValue(string configKey);
        void Add(string configKey, string value);
        string this[string configKey] { get; set; }
        void Initialize();
    }
}
