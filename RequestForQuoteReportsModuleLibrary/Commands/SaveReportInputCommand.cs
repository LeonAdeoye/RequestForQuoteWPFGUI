using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteReportsModuleLibrary.Commands
{
    /// <summary>
    /// Command class used to execute the saving of generated reports.
    /// </summary>
    public sealed class SaveReportInputCommand : AbstractRequestForQuoteCommand
    {
        public SaveReportInputCommand(RequestForQuoteReportsViewModel viewModel) : base()
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
