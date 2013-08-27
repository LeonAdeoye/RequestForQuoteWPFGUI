using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteToolBarModuleLibrary.Commands
{
    public class GetTodaysRequestsCommand : AbstractRequestForQuoteCommand
    {
        public GetTodaysRequestsCommand(RequestForQuoteToolBarViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanGetTodaysRequests();
        }

        public override void Execute(object parameter)
        {
            viewModel.GetTodaysRequests();
        }

        private readonly RequestForQuoteToolBarViewModel viewModel;
    }
}
