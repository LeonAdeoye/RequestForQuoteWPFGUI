using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteReportsModuleLibrary.Commands
{
    /// <summary>
    /// Command class used to execute the compilation of a report.
    /// </summary>
    public sealed class CompileReportCommand : AbstractRequestForQuoteCommand
    {
        public CompileReportCommand(RequestForQuoteReportsViewModel viewModel)
            : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanCompileResport();
        }

        public override void Execute(object parameter)
        {
            viewModel.CompileReport();
        }

        private readonly RequestForQuoteReportsViewModel viewModel;
    }
}
