using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Practices.Prism.Events;
using RequestForQuoteInterfacesLibrary.Enums;
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
        /// Requests the RFQs per catgeory report data from the web services back-end and sends this data along with other parameter information
        /// to the report generation viewmodel by publishing an RequestsCountByCategoryReportEvent through the event aggregator.
        /// </summary>
        /// <param name="reportType"> the type of report - bar chart, pie chart, etc.</param>
        /// <param name="categoryType"> the category by which the RFQs will be grouped - this is passed onto the web service.</param>
        /// <param name="fromDate"> the RFQs trade date.</param>
        /// <param name="minimumCount">the minimum count of RFQs that will be excluded from the report data.</param>
        /// <exception cref="ArgumentException"> thrown if reportType parameter is null or empty.</exception>
        /// <exception cref="ArgumentException"> thrown if categoryType parameter is null or empty.</exception>
        public void CompileRequestCountByCategoryReport(string reportType, string categoryType, DateTime fromDate, int minimumCount)
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

        /// <summary>
        /// Requests the greeks per category report data from the web services back-end and sends this data along with other parameter information
        /// to the report generation viewmodel by publishing an GreeksByCategoryReportEvent through the event aggregator.
        /// </summary>
        /// <param name="reportType"> the type of report - bar chart, pie chart, etc.</param>
        /// <param name="categoryType"> the category by which the RFQs will be grouped - this is passed onto the web service.</param>
        /// <param name="greeksToBeIncluded"> the set of greeks to be included in the report</param>
        /// <param name="maturityDateFrom"> the maturity date from which the RFQ's greeks will be included.</param>
        /// <param name="maturityDateTo"> the maturity date up until which the RFQ's greeks will be included.</param>
        /// <param name="minimumGreek">the minimum greek value that will be excluded from the report data.</param>
        /// <exception cref="ArgumentException"> thrown if reportType parameter is null or empty.</exception>
        /// <exception cref="ArgumentException"> thrown if categoryType parameter is null or empty.</exception>
        /// <exception cref="ArgumentException"> thrown if maturityDateFrom or maturityDateTo parameter is null.</exception>
        /// <exception cref="ArgumentException"> thrown if greeksTobeIncluded parameter is null or empty</exception>
        public void CompileGreeksByCategoryReport(string reportType, string categoryType, ISet<string> greeksToBeIncluded, 
            DateTime maturityDateFrom, DateTime maturityDateTo, double minimumGreek)
        {
            if (String.IsNullOrEmpty(reportType))
                throw new ArgumentException("reportType");

            if (String.IsNullOrEmpty(categoryType))
                throw new ArgumentException("categoryType");

            if (maturityDateFrom == null)
                throw new ArgumentException("maturityDateFrom");

            if (maturityDateTo == null)
                throw new ArgumentException("maturityDateTo");

            if (greeksToBeIncluded == null)
                throw new ArgumentException("greeksToBeIncluded");

            if (greeksToBeIncluded.Count == 0)
                throw new ArgumentException("greeksToBeIncluded");

            try
            {
                var eventPayLoad = new GreeksByCategoryReportEventPayLoad
                {
                    ReportType = reportType,
                    Category = categoryType,
                    MaturityDateFrom = maturityDateFrom,
                    MaturityDateTo = maturityDateTo,
                    MinimumGreek = minimumGreek,
                    GreeksToBeIncluded = greeksToBeIncluded                    
                };

                if (!configManager.IsStandAlone)
                {
                    var result = reportingContollerProxy.getGreeksByCategory(categoryType, maturityDateFrom, maturityDateTo, minimumGreek);
                    if (result != null)
                    {
                        foreach (var greekTotal in result)
                        {
                            if (greeksToBeIncluded.Contains(GreeksEnum.DELTA.ToString()))
                                eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.DELTA, greekTotal.delta);
                            if (greeksToBeIncluded.Contains(GreeksEnum.GAMMA.ToString()))
                                eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.GAMMA, greekTotal.gamma);
                            if (greeksToBeIncluded.Contains(GreeksEnum.THETA.ToString()))
                                eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.VEGA, greekTotal.vega);
                            if (greeksToBeIncluded.Contains(GreeksEnum.VEGA.ToString()))
                                eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.THETA, greekTotal.theta);
                            if (greeksToBeIncluded.Contains(GreeksEnum.RHO.ToString()))
                                eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.RHO, greekTotal.rho);
                        }                        
                    }
                }

                eventAggregator.GetEvent<GreeksByCategoryReportEvent>().Publish(eventPayLoad);
            }
            catch (FaultException fe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for greeks by category: " + categoryType +
                              ": " + fe);
            }
            catch (EndpointNotFoundException epnfe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for greeks by category: " + categoryType +
                              ": " + epnfe);
            }
            catch (NullReferenceException nre)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for greeks by category: " + categoryType + ": " + nre);
            }
        }

        /// <summary>
        /// Requests the greeks per category report data from the web services back-end and sends this data along with other parameter information
        /// to the report generation viewmodel by publishing an GreeksByCategoryReportEvent through the event aggregator.
        /// </summary>
        /// <param name="reportType"> the type of report - bar chart, pie chart, etc.</param>
        /// <param name="inputType"> the input to be used for the extrapolation - this is passed onto the web service.</param>
        /// <param name="greeksToBeIncluded"> the set of greeks to be included in the report</param>
        /// <param name="maturityDateFrom"> the maturity date from which the RFQ's greeks will be included.</param>
        /// <param name="maturityDateTo"> the maturity date up until which the RFQ's greeks will be included.</param>
        /// <param name="minimumInput">the minimum input value that will be excluded from the report data.</param>
        /// <param name="maximumInput">the maximum input value that will be excluded from the report data.</param>
        /// <exception cref="ArgumentException"> thrown if reportType parameter is null or empty.</exception>
        /// <exception cref="ArgumentException"> thrown if inputType parameter is null or empty.</exception>
        /// <exception cref="ArgumentException"> thrown if maturityDateFrom or maturityDateTo parameter is null.</exception>
        /// <exception cref="ArgumentException"> thrown if greeksTobeIncluded parameter is null or empty</exception>
        public void CompileGreeksByInputReport(string reportType, string inputType, ISet<string> greeksToBeIncluded, 
            DateTime maturityDateFrom, DateTime maturityDateTo, double minimumInput, double maximumInput)
        {
            if (String.IsNullOrEmpty(reportType))
                throw new ArgumentException("reportType");

            if (String.IsNullOrEmpty(inputType))
                throw new ArgumentException("inputType");

            if (maturityDateFrom == null)
                throw new ArgumentException("maturityDateFrom");

            if (maturityDateTo == null)
                throw new ArgumentException("maturityDateTo");

            if (greeksToBeIncluded == null)
                throw new ArgumentException("greeksToBeIncluded");

            if (greeksToBeIncluded.Count == 0)
                throw new ArgumentException("greeksToBeIncluded");

            try
            {
                var eventPayLoad = new GreeksByInputReportEventPayLoad
                {
                    ReportType = reportType,
                    InputType = inputType,
                    MaturityDateFrom = maturityDateFrom,
                    MaturityDateTo = maturityDateTo,
                    MinimumInput = minimumInput,
                    MaximumInput = maximumInput,
                    GreeksToBeIncluded = greeksToBeIncluded
                };

                if (!configManager.IsStandAlone)
                {
                    var result = reportingContollerProxy.getGreeksByInput(inputType, maturityDateFrom, maturityDateTo, minimumInput, maximumInput);
                    if (result != null)
                    {
                        foreach (var output in result)
                        {
                            //if (greeksToBeIncluded.Contains(GreeksEnum.DELTA.ToString()))
                            //    eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.DELTA, greekTotal.delta);
                            //if (greeksToBeIncluded.Contains(GreeksEnum.GAMMA.ToString()))
                            //    eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.GAMMA, greekTotal.gamma);
                            //if (greeksToBeIncluded.Contains(GreeksEnum.THETA.ToString()))
                            //    eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.VEGA, greekTotal.vega);
                            //if (greeksToBeIncluded.Contains(GreeksEnum.VEGA.ToString()))
                            //    eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.THETA, greekTotal.theta);
                            //if (greeksToBeIncluded.Contains(GreeksEnum.RHO.ToString()))
                            //    eventPayLoad.AddGreek(greekTotal.categoryValue, GreeksEnum.RHO, greekTotal.rho);
                        }
                    }
                }

                eventAggregator.GetEvent<GreeksByInputReportEvent>().Publish(eventPayLoad);
            }
            catch (FaultException fe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for greeks by input type: " + inputType + ": " + fe);
            }
            catch (EndpointNotFoundException epnfe)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for greeks by input type: " + inputType + ": " + epnfe);
            }
            catch (NullReferenceException nre)
            {
                if (log.IsErrorEnabled)
                    log.Error("Exception thrown while compile report data for greeks by input type: " + inputType + ": " + nre);
            }
        }
    }
}
