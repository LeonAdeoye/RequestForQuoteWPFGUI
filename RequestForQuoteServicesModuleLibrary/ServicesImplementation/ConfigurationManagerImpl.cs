using System;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{

    public class ConfigurationManagerImpl : IConfigurationManager
    {
        private bool isStandAlone;

        public void Initialize()
        {
            isStandAlone =  (Environment.GetCommandLineArgs().Length > 1 
                && Environment.GetCommandLineArgs()[1] == RequestForQuoteConstants.STANDALONE_MODE_WITHOUT_WEB_SERVICE);
        }

        public bool IsStandAlone()
        {
            return isStandAlone;
        }
    }
}
