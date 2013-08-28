using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteFunctionsModuleLibrary.Commands
{
    public class ClearCriteriaCommand : AbstractRequestForQuoteCommand
    {
        public ClearCriteriaCommand(RequestForQuoteFunctionsViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanClearCriteria();
        }

        public override void Execute(object parameter)
        {
            viewModel.ClearCriteria();
        }
        
        private readonly RequestForQuoteFunctionsViewModel viewModel;
    }
}
