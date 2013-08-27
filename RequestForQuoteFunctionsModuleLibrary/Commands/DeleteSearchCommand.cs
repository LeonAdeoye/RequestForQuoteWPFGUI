using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteFunctionsModuleLibrary.Commands
{
    public class DeleteSearchCommand : AbstractRequestForQuoteCommand
    {
        public DeleteSearchCommand(RequestForQuoteFunctionsViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanDeleteSearch();
        }

        public override void Execute(object parameter)
        {
            viewModel.DeleteSearch();
        }

        private readonly RequestForQuoteFunctionsViewModel viewModel;
    }
}
