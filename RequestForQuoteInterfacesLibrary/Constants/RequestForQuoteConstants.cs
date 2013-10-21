namespace RequestForQuoteInterfacesLibrary.Constants
{
    public static class RequestForQuoteConstants
    {
        public const string CALCULATE_REQUEST = "Calculate Request";
        public const string ENTER_SALES_COMMENTARY = "Sales' comment...";
        public const string ENTER_TRADER_COMMENTARY = "Trader's comment...";
        public const string ENTER_CLIENT_FEEDBACK = "Client's feedback...";
        public const string SAVE_SEARCH_DESCRIPTION_PROMPT = "Enter a unique description key...";

        public const string CLIENT_CRITERION = "Client";
        public const string STATUS_CRITERION = "Status";
        public const string BOOK_CRITERION = "Book";
        public const string UNDERLYIER_CRITERION = "Underlyier";
        public const string TRADE_DATE_CRITERION = "TradeDate";
        public const string EXPIRY_DATE_CRITERION = "ExpiryDate";

        public const string EXISTING_CRITERIA = "EXISTING_CRITERIA";
        public const string MAKE_PUBLIC_BY_SETTING_PRIVACY_TO_FALSE = "false";
        public const string MAKE_PRIVATE_BY_SETTING_PRIVACY_TO_TRUE = "true";

        public const string INVALIDATE_BY_SETTING_ISVALID_TO_FALSE = "false";
        public const string VALIDATE_BY_SETTING_ISVALID_TO_TRUE = "true";

        public static readonly string MY_USER_NAME = System.Environment.UserName;        

        public const string SERVER_IP_ADDRESS = "127.0.0.1";
        public const int SERVER_PORT_NUMBER = 1972;
        public const int SERVER_SLEEP_INTERVAL = 1000;
        public const int JSON_MESSAGE_SIZE_PREFIX_LENGTH = 4;
        public const int JSON_MESSAGE_SIZE_MAXIMUM = 5120;

        public const bool SAVE_TO_DATABASE = true;
        public const bool DO_NOT_SAVE_TO_DATABASE = false;
        public const bool VALID = true;
        public const bool MAINTAIN_STRONG_REFERENCE = true;

        public const string STANDALONE_MODE_WITHOUT_WEB_SERVICE = "StandAlone";
    }
}
