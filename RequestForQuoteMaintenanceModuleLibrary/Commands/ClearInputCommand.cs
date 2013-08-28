using RequestForQuoteInterfacesLibrary.Commands;
using RequestForQuoteInterfacesLibrary.ViewModelInterfaces;

namespace RequestForQuoteMaintenanceModuleLibrary.Commands
{
    public class ClearInputCommand : AbstractRequestForQuoteCommand
    {
        public ClearInputCommand(IClearInputViewModel viewModel)
            : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanClearInput();
        }

        public override void Execute(object parameter)
        {
            viewModel.ClearInput();
        }

        private readonly IClearInputViewModel viewModel;
    }
}
