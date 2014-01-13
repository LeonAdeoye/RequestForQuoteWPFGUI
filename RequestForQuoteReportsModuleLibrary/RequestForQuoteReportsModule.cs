using Microsoft.Practices.Prism.Events;
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

        public RequestForQuoteReportsModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteReportsModule constructed successfully.");
        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(RegionNames.REPORTS_USER_CONTROL_REGION, typeof (ReportsUserControl))
                         .RegisterViewWithRegion(RegionNames.BAR_CHART_USER_CONTROL_REGION, typeof (BarChartUserControl))
                         .RegisterViewWithRegion(RegionNames.PIE_CHART_USER_CONTROL_REGION, typeof (PieChartUserControl))
                         .RegisterViewWithRegion(RegionNames.AREA_SERIES_USER_CONTROL_REGION, typeof (AreaSeriesUserControl))
                         .RegisterViewWithRegion(RegionNames.GREEK_SERIES_USER_CONTROL_REGION, typeof(GreekSeriesUserControl))
                         .RegisterViewWithRegion(RegionNames.LINE_GRAPH_USER_CONTROL_REGION, typeof (LineGraphUserControl));

            if (log.IsDebugEnabled)
                log.Debug("RequestForQuoteReportsModule initialized successfully.");
        }

        private readonly IRegionManager regionManager;
    }
}
