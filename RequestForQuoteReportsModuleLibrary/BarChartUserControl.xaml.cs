using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteReportsModuleLibrary
{
    /// <summary>
    /// Interaction logic for BarChartUserControl.xaml
    /// </summary>
    public partial class BarChartUserControl : UserControl
    {
        public BarChartUserControl(IUnityContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<RequestForQuoteReportsViewModel>();
        }
    }
}
