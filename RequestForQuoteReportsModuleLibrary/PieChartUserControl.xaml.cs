using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteReportsModuleLibrary
{
    /// <summary>
    /// Interaction logic for PieChartUserControl.xaml
    /// </summary>
    public partial class PieChartUserControl : UserControl
    {
        public PieChartUserControl(IUnityContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<RequestForQuoteReportsViewModel>();
        }
    }
}
