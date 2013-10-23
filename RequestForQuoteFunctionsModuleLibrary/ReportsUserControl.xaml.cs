using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteFunctionsModuleLibrary
{
    /// <summary>
    /// Interaction logic for ReportsUserControl.xaml
    /// </summary>
    public partial class ReportsUserControl : UserControl
    {
        public ReportsUserControl(IUnityContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<RequestForQuoteReportsViewModel>();
        }
    }
}
