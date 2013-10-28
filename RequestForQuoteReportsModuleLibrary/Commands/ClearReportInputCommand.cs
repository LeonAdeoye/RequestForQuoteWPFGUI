using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteReportsModuleLibrary.Commands
{
    public class ClearReportInputCommand : AbstractRequestForQuoteCommand
    {
        public ClearReportInputCommand(RequestForQuoteReportsViewModel viewModel): base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanClearReportInput();
        }

        public override void Execute(object parameter)
        {
            viewModel.ClearReportInput();
        }

        private readonly RequestForQuoteReportsViewModel viewModel;
    }
}
