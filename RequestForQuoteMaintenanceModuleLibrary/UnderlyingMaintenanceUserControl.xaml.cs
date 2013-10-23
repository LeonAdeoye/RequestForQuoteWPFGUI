using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    /// <summary>
    /// Interaction logic for UnderlyingMaintenanceUserControl.xaml
    /// </summary>
    public partial class UnderlyingMaintenanceUserControl : UserControl
    {
        public UnderlyingMaintenanceUserControl(IUnityContainer container)
        {
            this.Resources = Application.LoadComponent(new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            InitializeComponent();
            DataContext = container.Resolve<UnderlyingMaintenanceViewModel>();
        }
    }
}
