﻿using RequestForQuoteInterfacesLibrary.Commands;
using RequestForQuoteInterfacesLibrary.Constants;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class CalculateRequestCommand : AbstractRequestForQuoteCommand
    {
        public CalculateRequestCommand(RequestForQuoteGridViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return viewModel.CanCalculateRequest();
        }

        public override void Execute(object parameter)
        {
            bool isFromContextMenu = parameter != null && parameter.ToString() == RequestForQuoteConstants.CALCULATE_REQUEST;
            viewModel.CalculateRequest(isFromContextMenu);
        }

        private readonly RequestForQuoteGridViewModel viewModel;
    }
}
