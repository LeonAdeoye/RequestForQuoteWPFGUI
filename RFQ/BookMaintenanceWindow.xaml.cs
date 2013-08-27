using System.Windows;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ
{
    /// <summary>
    /// Interaction logic for BookMaintenanceWindow.xaml
    /// </summary>
    public partial class BookMaintenanceWindow : Window, IBookMaintenancePopupWindow
    {
        public BookMaintenanceWindow()
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
    }
}
