using System;
using System.Collections.Generic;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{

    public class ConfigurationManagerImpl : IConfigurationManager
    {
        private bool isStandAlone;
        private readonly IDictionary<string, string> configs = new Dictionary<string, string>();

        public void Initialize()
        {
            isStandAlone =  (Environment.GetCommandLineArgs().Length > 1 
                && Environment.GetCommandLineArgs()[1] == RequestForQuoteConstants.STANDALONE_MODE_WITHOUT_WEB_SERVICE);
        }

        public bool IsStandAlone()
        {
            return isStandAlone;
        }

        public bool? IsTrue(string configKey)
        {
            if(string.IsNullOrEmpty(configKey))
                throw new ArgumentException("configKey");

            if (!configs.ContainsKey(configKey))
                return null;
            
            bool result;

            if (!Boolean.TryParse(configs[configKey], out result))
                return null;

            return result;
        }

        public int? GetIntValue(string configKey)
        {
            if (string.IsNullOrEmpty(configKey))
                throw new ArgumentException("configKey");

            if (!configs.ContainsKey(configKey))
                return null;

            int result;

            if (!Int32.TryParse(configs[configKey], out result))
                return null;

            return result;
        }

        public decimal? GetDecimalValue(string configKey)
        {
            if (string.IsNullOrEmpty(configKey))
                throw new ArgumentException("configKey");

            if (!configs.ContainsKey(configKey))
                return null;

            decimal result;

            if (!Decimal.TryParse(configs[configKey], out result))
                return null;

            return result;
        }

        public void Add(string configKey, string value)
        {
            if (string.IsNullOrEmpty(configKey))
                throw new ArgumentException("configKey");

            this[configKey] = value;
        }

        public string this[string configKey]
        {
            get
            {
                if (string.IsNullOrEmpty(configKey))
                    throw new ArgumentException("configKey");

                if (!configs.ContainsKey(configKey))
                    return String.Empty;

                return configs[configKey];
            }
            set
            {
                if (string.IsNullOrEmpty(configKey))
                    throw new ArgumentException("configKey");

                configs[configKey] = value;
            }
        }
    }
}
