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
            if (e.AddedItems.Count > 0)
            {
                var tab = e.AddedItems[0] as TabItem;
                if (tab != null)             
                    viewModel.SelectedReportTab = tab.Header as string;               
                e.Handled = true;
            }
        }
    }
}
