using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
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
            IBookMaintenancePopupWindow bookMaintenanceWindow = ServiceLocator.Current.GetInstance<IBookMaintenancePopupWindow>();
            bookMaintenanceWindow.ShowModalWindow();
        }

        private void LaunchClientMaintenancePopupWindow()
        {
            IClientMaintenancePopupWindow clientMaintenanceWindow = ServiceLocator.Current.GetInstance<IClientMaintenancePopupWindow>();
            clientMaintenanceWindow.ShowModalWindow();
        }

        private void LaunchBankHolidayMaintenancePopupWindow()
        {
            IBankHolidayMaintenancePopupWindow bankHolidayMaintenanceWindow = ServiceLocator.Current.GetInstance<IBankHolidayMaintenancePopupWindow>();
            bankHolidayMaintenanceWindow.ShowModalWindow();
        }

        private void LaunchUnderlyierMaintenancePopupWindow()
        {
            IUnderlyingMaintenancePopupWindow underlyingMaintenanceWindow = ServiceLocator.Current.GetInstance<IUnderlyingMaintenancePopupWindow>();
            underlyingMaintenanceWindow.ShowModalWindow();
        }

        public void ShutdownServerCommunication()
        {
            IServerCommunicator serverCommunicator = ServiceLocator.Current.GetInstance<IServerCommunicator>();
            serverCommunicator.DisconnectFromServer();
        }
    }
}
