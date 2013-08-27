using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class CloneRequestCommand : AbstractRequestForQuoteCommand
    {
        public CloneRequestCommand(RequestForQuoteGridViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !viewModel.IsSelectRequestNull();
        }

        public override void Execute(object parameter)
        {
            viewModel.CloneRequest();
        }

        private readonly RequestForQuoteGridViewModel viewModel;
    }
}
