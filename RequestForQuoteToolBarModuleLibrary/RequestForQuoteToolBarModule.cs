using Microsoft.Practices.Prism.Modularity;
using RequestForQuoteInterfacesLibrary.Constants;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

namespace RequestForQuoteToolBarModuleLibrary
{
    class RequestForQuoteToolBarModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestForQuoteToolBarModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteToolBarModule constructed successfully.");

        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(RegionNames.TOOLBAR_USER_CONTROL_REGION, typeof(RequestForQuoteToolBar));

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteToolBarModule initialized successfully.");
        }

        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

    }
}
