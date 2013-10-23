using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteFunctionsModuleLibrary.Commands
{
    public class CompileReportCommand : AbstractRequestForQuoteCommand
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
