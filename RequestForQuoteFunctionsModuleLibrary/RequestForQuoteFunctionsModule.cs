using Microsoft.Practices.Prism.Modularity;
using RequestForQuoteInterfacesLibrary.Constants;
using log4net;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace RequestForQuoteFunctionsModuleLibrary
{
    class RequestForQuoteFunctionsModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestForQuoteFunctionsModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteFunctionsModule constructed successfully.");

        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(RegionNames.FUNCTIONS_USER_CONTROL_REGION, typeof(RequestForQuoteFunctions));
            regionManager.RegisterViewWithRegion(RegionNames.SAVE_SEARCH_USER_CONTROL_REGION, typeof(SaveSearchUserControl));
            regionManager.RegisterViewWithRegion(RegionNames.TREE_BROWSER_USER_CONTROL_REGION, typeof (TreeBrowserUserControl));

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteFunctionsModule initialized successfully.");
        }

        public bool CanSearchForRequests()
        {
            return true;
        }

        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
    }
}
