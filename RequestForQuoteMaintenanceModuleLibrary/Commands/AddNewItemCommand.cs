using RequestForQuoteInterfacesLibrary.Commands;
using RequestForQuoteInterfacesLibrary.ViewModelInterfaces;

namespace RequestForQuoteMaintenanceModuleLibrary.Commands
{
    public class AddNewItemCommand : AbstractRequestForQuoteCommand
    {
        public AddNewItemCommand(IAddNewItemViewModel viewModel)
            : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanAddNewItem();
        }

        public override void Execute(object parameter)
        {
            viewModel.AddNewItem();
        }

        private readonly IAddNewItemViewModel viewModel;
    }
}
