using System;
using System.Windows;
using System.Windows.Controls;
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
            InitializeComponent();

            viewModel = container.Resolve<RequestForQuoteGridViewModel>();            
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
