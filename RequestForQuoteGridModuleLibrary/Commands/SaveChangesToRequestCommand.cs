using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class SaveChangesToRequestCommand : AbstractRequestForQuoteCommand
    {
        public SaveChangesToRequestCommand(RequestForQuoteDetailsViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            viewModel.Save(parameter.ToString());
        }

        private readonly RequestForQuoteDetailsViewModel viewModel;
    }
}
