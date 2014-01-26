using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class DeleteRequestCommand : AbstractRequestForQuoteCommand
    {
        public DeleteRequestCommand(RequestForQuoteGridViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !viewModel.IsSelectedRequestNull();
        }

        public override void Execute(object parameter)
        {
            viewModel.DeleteRequest();
        }

        private readonly RequestForQuoteGridViewModel viewModel;
    }
}
