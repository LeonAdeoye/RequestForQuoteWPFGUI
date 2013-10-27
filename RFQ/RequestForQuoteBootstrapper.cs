using System;
using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using RFQ.Popups;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ
{
    sealed class RequestForQuoteBootstrapper : UnityBootstrapper
    {        
        protected override DependencyObject CreateShell()
        {            
            Shell shell = ServiceLocator.Current.GetInstance<Shell>();
            shell.Show();
            return shell;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override void InitializeModules()
        {
            try
            {
                base.InitializeModules();
                RegisterAllPopups();
            }
            catch (ModuleInitializeException)
            {
                MessageBox.Show("Failed to initialize modules needed by RequestForQuote application. Catastrophic failure. Shutting down now!");
                Environment.Exit(0);
            }
        }

        private void RegisterAllPopups()
        {
            Container.RegisterType<IWindowPopup, RequestForQuoteDetailsWindow>(WindowPopupNames.REQUEST_DETAIL_WINDOW_POPUP)
                .RegisterType<IWindowPopup, BookMaintenanceWindow>(WindowPopupNames.BOOK_MAINTENANCE_WINDOW_POPUP)
                .RegisterType<IWindowPopup, ClientMaintenanceWindow>(WindowPopupNames.CLIENT_MAINTENANCE_WINDOW_POPUP)
                .RegisterType<IWindowPopup, BankHolidayMaintenanceWindow>(WindowPopupNames.BANK_HOLIDAY_MAINTENANCE_WINDOW_POPUP)
                .RegisterType<IWindowPopup, UnderlyingMaintenanceWindow>(WindowPopupNames.UNDERLYING_MAINTENANCE_WINDOW_POPUP)
                .RegisterType<IWindowPopup, SaveSearchWindow>(WindowPopupNames.SAVE_SEARCH_WINDOW_POPUP)
                .RegisterType<IWindowPopup, SaveSearchWindow>(WindowPopupNames.REPORT_WINDOW_POPUP); 
        }
    }
}
