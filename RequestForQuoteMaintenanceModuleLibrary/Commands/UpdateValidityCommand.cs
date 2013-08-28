using System;
using RequestForQuoteInterfacesLibrary.Commands;
using RequestForQuoteInterfacesLibrary.ViewModelInterfaces;

namespace RequestForQuoteMaintenanceModuleLibrary.Commands
{
    public class UpdateValidityCommand : AbstractRequestForQuoteCommand
    {
        public UpdateValidityCommand(IUpdateValidityViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanUpdateValidity(Convert.ToBoolean(parameter));
        }

        public override void Execute(object parameter)
        {
            viewModel.UpdateValidity();
        }

        private readonly IUpdateValidityViewModel viewModel;
    }
}
