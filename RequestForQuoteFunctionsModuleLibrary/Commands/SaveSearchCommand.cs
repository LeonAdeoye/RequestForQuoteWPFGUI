using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteFunctionsModuleLibrary.Commands
{
    public class SaveSearchCommand : AbstractRequestForQuoteCommand
    {
        public SaveSearchCommand(RequestForQuoteFunctionsViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanSaveSearch();
        }

        public override void Execute(object parameter)
        {
            viewModel.LaunchSaveSearchWindow();
        }

        private readonly RequestForQuoteFunctionsViewModel viewModel;
    }
}
