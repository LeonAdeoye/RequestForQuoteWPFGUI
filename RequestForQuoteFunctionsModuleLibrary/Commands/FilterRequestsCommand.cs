using RequestForQuoteInterfacesLibrary.Commands;
using RequestForQuoteInterfacesLibrary.Constants;

namespace RequestForQuoteFunctionsModuleLibrary.Commands
{
    public class FilterRequestsCommand : AbstractRequestForQuoteCommand
    {
        public FilterRequestsCommand(RequestForQuoteFunctionsViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanFilterRequests();
        }

        public override void Execute(object parameter)
        {
            viewModel.FilterRequests(parameter != null && parameter.ToString() == RequestForQuoteConstants.EXISTING_CRITERIA);
        }
        
        private readonly RequestForQuoteFunctionsViewModel viewModel;
    }
}
