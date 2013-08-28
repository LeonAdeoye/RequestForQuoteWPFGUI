using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteToolBarModuleLibrary.Commands
{
    public class AddRequestCommand : AbstractRequestForQuoteCommand
    {
        public AddRequestCommand(RequestForQuoteToolBarViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanAddNewRequest();
        }

        public override void Execute(object parameter)
        {
            viewModel.AddRequest();
        }

        private readonly RequestForQuoteToolBarViewModel viewModel;
    }
}
