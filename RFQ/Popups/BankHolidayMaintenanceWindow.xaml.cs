using System.Windows;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ.Popups
{
    /// <summary>
    /// Interaction logic for BankHolidayMaintenanceWindow.xaml
    /// </summary>
    public partial class BankHolidayMaintenanceWindow : Window, IWindowPopup
    {
        public BankHolidayMaintenanceWindow()
        {
            InitializeComponent();
        }

        public void ShowWindow(object viewModelArg)
        {
            DataContext = viewModelArg;
            Show();
        }

        public bool? ShowModalWindow()
        {
            return ShowDialog();
        }

        public void CloseWindow()
        {
            Close();
        }

        public void ActivateWindow()
        {
            Activate();
        }


        public void ShowWindow()
        {
            Show();
        }

        public bool? ShowModalWindow(object viewModelArg)
        {
            DataContext = viewModelArg;
            return ShowDialog();
        }

        public void HideWindow()
        {
            Hide();
        }

        public bool IsApplicationRemainingOpen { get; set; }
    }
}
