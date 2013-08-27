using RequestForQuoteInterfacesLibrary.Commands;
using RequestForQuoteInterfacesLibrary.Constants;

namespace RequestForQuoteFunctionsModuleLibrary.Commands
{
    public class SearchRequestsCommand : AbstractRequestForQuoteCommand
    {
        public SearchRequestsCommand(RequestForQuoteFunctionsViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanSearchRequests(parameter != null && parameter.ToString() == RequestForQuoteConstants.EXISTING_CRITERIA);
        }

        public override void Execute(object parameter)
        {
            viewModel.SearchRequests(parameter != null && parameter.ToString() == RequestForQuoteConstants.EXISTING_CRITERIA);
        }
        
        private readonly RequestForQuoteFunctionsViewModel viewModel;
    }
}
