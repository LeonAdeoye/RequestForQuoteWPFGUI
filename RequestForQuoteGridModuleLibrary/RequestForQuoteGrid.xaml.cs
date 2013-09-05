using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteGridModuleLibrary
{
    /// <summary>
    /// Interaction logic for RequestForQuoteGrid.xaml
    /// </summary>
    public partial class RequestForQuoteGrid : UserControl
    {
        private readonly RequestForQuoteGridViewModel viewModel;
        private bool isManualEditCommit;

        public RequestForQuoteGrid(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.Resources = Application.LoadComponent(
            new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;

            InitializeComponent();

            viewModel = new RequestForQuoteGridViewModel(container.Resolve<IBookManager>(), container.Resolve<IClientManager>(),
                container.Resolve<IOptionRequestParser>(), container.Resolve<IOptionRequestPricer>(), 
                container.Resolve<IChatServiceManager>(), container.Resolve<IUnderlyingManager>(), eventAggregator);
            
            this.DataContext = viewModel;
        }

        
        private void HandleDataGridCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!isManualEditCommit)
            {
                isManualEditCommit = true;
                DataGrid grid = sender as DataGrid;
                if (grid != null)
                {
                    grid.CommitEdit(DataGridEditingUnit.Row, true);
                    // TODO - ONLY CALCULATE WHEN CERTAIN ROWS CHANGE.
                    const bool isFromContextMenu = false;
                    viewModel.CalculateRequest(isFromContextMenu);
                }
                isManualEditCommit = false;
            }
        }
    }
}
