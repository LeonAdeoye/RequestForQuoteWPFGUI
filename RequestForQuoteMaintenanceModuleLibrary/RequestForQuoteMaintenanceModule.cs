using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using RequestForQuoteInterfacesLibrary.Constants;
using log4net;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    class RequestForQuoteMaintenanceModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RequestForQuoteMaintenanceModule(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteToolBarModule constructed successfully.");

        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(RegionNames.BOOK_MAINTENANCE_USER_CONTROL_REGION, typeof(BookMaintenanceUserControl))
                .RegisterViewWithRegion(RegionNames.CLIENT_MAINTENANCE_USER_CONTROL_REGION, typeof(ClientMaintenanceUserControl))
                .RegisterViewWithRegion(RegionNames.BANK_HOLIDAY_MAINTENANCE_USER_CONTROL_REGION, typeof(BankHolidayMaintenanceUserControl))
                .RegisterViewWithRegion(RegionNames.UNDERLYING_MAINTENANCE_USER_CONTROL_REGION, typeof(UnderlyingMaintenanceUserControl))
                .RegisterViewWithRegion(RegionNames.GROUP_MAINTENANCE_USER_CONTROL_REGION, typeof(GroupMaintenanceUserControl))
                .RegisterViewWithRegion(RegionNames.USER_MAINTENANCE_USER_CONTROL_REGION, typeof(UserMaintenanceUserControl));

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteToolBarModule initialized successfully.");
        }

        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

    }
}
