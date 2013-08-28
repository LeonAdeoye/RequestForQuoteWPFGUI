using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class InvalidateRequestCommand : AbstractRequestForQuoteCommand
    {
        public InvalidateRequestCommand(RequestForQuoteGridViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanInvalidateRequest();
        }

        public override void Execute(object parameter)
        {
            viewModel.InvalidateRequest();
        }

        private readonly RequestForQuoteGridViewModel viewModel;
    }
}
