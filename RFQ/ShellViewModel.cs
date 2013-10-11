using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ
{
    public class ShellViewModel
    {
        public ICommand BookMaintenanceCommand { get; set; }
        public ICommand ClientMaintenanceCommand { get; set; }
        public ICommand BankHolidayMaintenanceCommand { get; set; }
        public ICommand UnderlyierMaintenanceCommand { get; set; }

        public ShellViewModel()
        {
            BookMaintenanceCommand = new DelegateCommand(LaunchBookMaintenancePopupWindow);
            ClientMaintenanceCommand = new DelegateCommand(LaunchClientMaintenancePopupWindow);
            BankHolidayMaintenanceCommand = new DelegateCommand(LaunchBankHolidayMaintenancePopupWindow);
            UnderlyierMaintenanceCommand = new DelegateCommand(LaunchUnderlyierMaintenancePopupWindow);
        }

        private void LaunchBookMaintenancePopupWindow()
        {
            IWindowPopup bookMaintenanceWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.BOOK_MAINTENANCE_WINDOW_POPUP);
            bookMaintenanceWindow.ShowModalWindow();
        }

        private void LaunchClientMaintenancePopupWindow()
        {
            IWindowPopup clientMaintenanceWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.CLIENT_MAINTENANCE_WINDOW_POPUP);
            clientMaintenanceWindow.ShowModalWindow();
        }

        private void LaunchBankHolidayMaintenancePopupWindow()
        {
            IWindowPopup bankHolidayMaintenanceWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.BANK_HOLIDAY_MAINTENANCE_WINDOW_POPUP);
            bankHolidayMaintenanceWindow.ShowModalWindow();
        }

        private void LaunchUnderlyierMaintenancePopupWindow()
        {
            IWindowPopup underlyingMaintenanceWindow = ServiceLocator.Current.GetInstance<IWindowPopup>(WindowPopupNames.UNDERLYING_MAINTENANCE_WINDOW_POPUP);
            underlyingMaintenanceWindow.ShowModalWindow();
        }

        public void ShutdownServerCommunication()
        {
            IServerCommunicator serverCommunicator = ServiceLocator.Current.GetInstance<IServerCommunicator>();
            serverCommunicator.DisconnectFromServer();
        }
    }
}
