using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteToolBarModuleLibrary.Commands
{
    public class ClearNewRequestCommand : AbstractRequestForQuoteCommand
    {
        public ClearNewRequestCommand(RequestForQuoteToolBarViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanClearNewRequest();
        }

        public override void Execute(object parameter)
        {
            viewModel.ClearNewRequest();
        }

        private readonly RequestForQuoteToolBarViewModel viewModel;
    }
}
