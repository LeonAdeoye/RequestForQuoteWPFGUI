using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteReportsModuleLibrary.Commands
{
    public class SaveReportInputCommand : AbstractRequestForQuoteCommand
    {
        public SaveReportInputCommand(RequestForQuoteReportsViewModel viewModel)
            : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanSaveReportInput();
        }

        public override void Execute(object parameter)
        {
            viewModel.SaveReportInput();
        }

        private readonly RequestForQuoteReportsViewModel viewModel;
    }
}
