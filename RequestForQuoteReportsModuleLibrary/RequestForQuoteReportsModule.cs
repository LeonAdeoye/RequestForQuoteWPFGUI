using Microsoft.Practices.Prism.Modularity;
using RequestForQuoteInterfacesLibrary.Constants;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Regions;

namespace RequestForQuoteReportsModuleLibrary
{
    class RequestForQuoteReportsModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestForQuoteReportsModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteReportsModule constructed successfully.");

        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(RegionNames.GENERATED_REPORT_USER_CONTROL_REGION, typeof(GeneratedReportUserControl));

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteReportsModule initialized successfully.");
        }

        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

    }
}
