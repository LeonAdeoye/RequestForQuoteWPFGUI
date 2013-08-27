using System.Windows.Input;

namespace RequestForQuoteInterfacesLibrary.ViewModelInterfaces
{
    public interface IAddNewItemViewModel
    {
        ICommand AddNewItemCommand { get; set; }
        bool CanAddNewItem();
        void AddNewItem();
    }
}
