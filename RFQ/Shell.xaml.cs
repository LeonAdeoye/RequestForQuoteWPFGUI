using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;
using log4net;

namespace RFQ
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ShellViewModel shellViewModel;

        public Shell(IModuleManager moduleManager)
        {
            this.Resources = Application.LoadComponent(new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;

            InitializeComponent();
            this.moduleManager = moduleManager;
            shellViewModel = new ShellViewModel();
            DataContext = shellViewModel;
        }

        private void Shell_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.moduleManager.LoadModuleCompleted += ModuleManagerOnLoadModuleCompleted;
        }

        private void ModuleManagerOnLoadModuleCompleted(object sender, LoadModuleCompletedEventArgs args)
        {
            if(args.Error != null)
                log.Error(args.ModuleInfo.ModuleName + " module failed to load.", args.Error);
            
            if(log.IsDebugEnabled)
                log.Debug(args.ModuleInfo.ModuleName + " module loaded successfully.");
        }

        private void Shell_OnClosing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close RequestForQuote?", "Closing down...",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var window in Application.Current.Windows.OfType<IWindowPopup>())
                    {
                        window.IsApplicationRemainingOpen = false;
                        window.CloseWindow();
                    }
                        
                    shellViewModel.ShutdownServerCommunication();
                }
                catch (Exception)
                {
                    log.Error("Failed to close down properly. Exiting anyway.");
                }                
            }
            else
                e.Cancel = true;
        }

        private readonly IModuleManager moduleManager;
    }
}
