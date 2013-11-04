using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteReportsModuleLibrary
{
    /// <summary>
    /// Interaction logic for ReportsUserControl.xaml
    /// </summary>
    public partial class ReportsUserControl : UserControl
    {
        private readonly RequestForQuoteReportsViewModel viewModel;

        public ReportsUserControl(IUnityContainer container)
        {
            InitializeComponent();
            viewModel = container.Resolve<RequestForQuoteReportsViewModel>();
            DataContext = viewModel;
        }

        private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
