using RequestForQuoteInterfacesLibrary.Commands;

namespace RequestForQuoteGridModuleLibrary.Commands
{
    public class GroupByCommand : AbstractRequestForQuoteCommand
    {
        public GroupByCommand(RequestForQuoteGridViewModel viewModel) : base()
        {
            this.viewModel = viewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return !viewModel.IsSelectedRequestNull();
        }

        public override void Execute(object parameter)
        {
            viewModel.GroupRequests(parameter.ToString());
        }

        private readonly RequestForQuoteGridViewModel viewModel;
    }
}
