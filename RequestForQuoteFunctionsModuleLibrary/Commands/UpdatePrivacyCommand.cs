using System;
using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteFunctionsModuleLibrary.Commands
{
    public class UpdatePrivacyCommand : AbstractRequestForQuoteCommand
    {
        public UpdatePrivacyCommand(RequestForQuoteFunctionsViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanUpdatePrivacy(Convert.ToBoolean(parameter));
        }

        public override void Execute(object parameter)
        {
            viewModel.UpdatePrivacy();
        }

        private readonly RequestForQuoteFunctionsViewModel viewModel;
    }
}
