using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class ShowRequestDetailsWindowCommand : AbstractRequestForQuoteCommand
    {
        public ShowRequestDetailsWindowCommand(RequestForQuoteGridViewModel viewModel)
            : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanShowDetailsWindow();
        }

        public override void Execute(object parameter)
        {
            viewModel.ShowDetailsWindow();
        }

        private readonly RequestForQuoteGridViewModel viewModel;
    }
}
