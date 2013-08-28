using System.Windows.Input;

namespace RequestForQuoteInterfacesLibrary.ViewModelInterfaces
{
    public interface IClearInputViewModel
    {
        ICommand ClearInputCommand { get; set; }
        bool CanClearInput();
        void ClearInput();
    }
}
