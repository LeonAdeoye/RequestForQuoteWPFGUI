using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    /// <summary>
    /// Interaction logic for UserMaintenanceUserControl.xaml
    /// </summary>
    public partial class UserMaintenanceUserControl : UserControl
    {
        public UserMaintenanceUserControl(IUnityContainer container)
        {
            this.Resources = Application.LoadComponent(
                new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as
                             ResourceDictionary;
            InitializeComponent();
            DataContext = container.Resolve<UserMaintenanceViewModel>();
        }
    }
}
