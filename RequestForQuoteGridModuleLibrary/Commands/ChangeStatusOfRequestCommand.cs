using System;
using RequestForQuoteInterfacesLibrary.Commands;
using RequestForQuoteInterfacesLibrary.Enums;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class ChangeStatusOfRequestCommand : AbstractRequestForQuoteCommand
    {
        public ChangeStatusOfRequestCommand(RequestForQuoteGridViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !viewModel.IsSelectRequestNull();
        }

        public override void Execute(object parameter)
        {
            viewModel.ChangeStatusOfRequest((StatusEnum)Enum.Parse(typeof(StatusEnum), parameter.ToString()));
        }

        private readonly RequestForQuoteGridViewModel viewModel;
    }
}
