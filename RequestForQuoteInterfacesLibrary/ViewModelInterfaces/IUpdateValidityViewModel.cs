using System.Windows.Input;

namespace RequestForQuoteInterfacesLibrary.ViewModelInterfaces
{
    public interface IUpdateValidityViewModel
    {
        ICommand UpdateValidityCommand { get; set; }
        bool CanUpdateValidity(bool isRequestToMakeValid);
        void UpdateValidity();
    }
}
