using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    /// <summary>
    /// Interaction logic for BookMaintenanceUserControl.xaml
    /// </summary>
    public partial class BookMaintenanceUserControl : UserControl
    {
        public BookMaintenanceUserControl(IUnityContainer container)
        {
            this.Resources = Application.LoadComponent(new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            InitializeComponent();
            DataContext = container.Resolve<BookMaintenanceViewModel>();
        }
    }
}
