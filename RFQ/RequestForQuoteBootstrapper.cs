using System;
using System.Windows;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ
{
    class RequestForQuoteBootstrapper : UnityBootstrapper
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
            catch (ModuleInitializeException exception)
            {
                MessageBox.Show("Failed to initialize modules needed by RequestForQuote application. Catastrophic failure. Shutting down now!");
                Environment.Exit(0);
            }
        }

        private void RegisterAllPopups()
        {
            Container.RegisterType<IRequestForQuoteDetailsPopupWindow, RequestForQuoteDetailsWindow>();
            Container.RegisterType<IBookMaintenancePopupWindow, BookMaintenanceWindow>();
            Container.RegisterType<IClientMaintenancePopupWindow, ClientMaintenanceWindow>();
            Container.RegisterType<IBankHolidayMaintenancePopupWindow, BankHolidayMaintenanceWindow>();
            Container.RegisterType<ISaveSearchPopupWindow, SaveSearchWindow>(); 
        }
    }
}
