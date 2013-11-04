﻿using System;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteServicesModuleLibrary.ReportingService;
using log4net;

namespace RequestForQuoteServicesModuleLibrary.ServicesImplementation
{
    public sealed class ReportDataManagerImpl : IReportDataManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator;
        private readonly IConfigurationManager configManager;
        private readonly ReportingControllerClient reportingContollerProxy = new ReportingControllerClient();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configManager"> for determining whether in application is in standalone mode.</param>
        /// <param name="eventAggregator"> for publishing report data messages to listeners.</param>
        /// <exception cref="ArgumentNullException"> thrown if configManager or eventAggregator parameters are null.</exception>
        public ReportDataManagerImpl(IConfigurationManager configManager, IEventAggregator eventAggregator)
        {
            if (configManager == null)
                throw new ArgumentNullException("configManager");

            if (eventAggregator == null)
                throw new ArgumentNullException("eventAggregator");

            this.configManager = configManager;
            this.eventAggregator = eventAggregator;            
        }

        /// <summary>
        /// Requests the report data from the web services back-end and sends this data along with other parameter information
        /// to the report generation viewmodel by publishing an RequestsCountByCategoryReportEvent through the event aggregator.
        /// </summary>
        /// <param name="reportType"> the type of report - bar chart, pie chart, etc</param>
        /// <param name="categoryType"> the category by which the RFQs will be grouped - this is passed onto the web service</param>
        /// <param name="fromDate"> the RFQs trade date</param>
        /// <param name="minimumCount">the minimum count of RFQs that will be excluded from the report data</param>
        /// <exception cref="ArgumentException"> thrown if reportType parameter is null or empty</exception>
        /// <exception cref="ArgumentException"> thrown if categoryType parameter is null or empty</exception>
        public void CompileRequestCountPerCategoryReport(string reportType, string categoryType, DateTime fromDate, int minimumCount)
        {
            if (String.IsNullOrEmpty(reportType))
                throw new ArgumentException("reportType");

            if (String.IsNullOrEmpty(categoryType))
                throw new ArgumentException("categoryType");

            try
            {
                var eventPayLoad = new RequestsCountByCategoryReportEventPayLoad
                    {
                        ReportType = reportType,
                        Category = categoryType,
                        FromDate = fromDate,
                        MinimumCount = minimumCount,                       
                    };

                if (!configManager.IsStandAlone)
                {
                    var result = reportingContollerProxy.getRequestsByCategory(categoryType, fromDate, minimumCount);
                    if(result != null)
                        foreach (var categoryCount in result)
                            eventPayLoad.CountByCategory.Add(categoryCount.categoryValue, categoryCount.requestCount);
                }

                eventAggregator.GetEvent<RequestsCountByCategoryReportEvent>().Publish(eventPayLoad);
            }
            catch (FaultException fe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType +
                              ": " + fe);
            }
            catch (EndpointNotFoundException epnfe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType +
                              ": " + epnfe);
            }
            catch (NullReferenceException nre)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for requests by category: " + categoryType + ": " + nre);                
            }
        }
    }
}
