using Microsoft.Practices.Prism.Modularity;
using RequestForQuoteInterfacesLibrary.Constants;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

namespace RequestForQuoteGridModuleLibrary
{
    class RequestForQuoteGridModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestForQuoteGridModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteGridModule constructed successfully.");
        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(RegionNames.REQUESTS_GRID_USER_CONTROL_REGION, typeof(RequestForQuoteGrid));
            regionManager.RegisterViewWithRegion(RegionNames.REQUEST_DETAIL_USER_CONTROL_REGION, typeof(RequestForQuoteDetails));
            regionManager.RegisterViewWithRegion(RegionNames.REQUEST_LEG_DETAIL_USER_CONTROL_REGION, typeof(RequestForQuoteLegDetails));

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteGridModule initialized successfully.");
        }

        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
    }
}
