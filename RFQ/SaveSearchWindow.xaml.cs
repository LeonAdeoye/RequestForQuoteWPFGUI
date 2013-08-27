using System.Windows;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ
{
    /// <summary>
    /// Interaction logic for SaveSearchWindow.xaml
    /// </summary>
    public partial class SaveSearchWindow : Window, ISaveSearchPopupWindow
    {
        public SaveSearchWindow()
        {
            InitializeComponent();
        }

        public void ShowWindow(object viewModelArg)
        {
            DataContext = viewModelArg;
            Show();
        }

        public void ShowWindow()
        {
            Show();
        }

        public bool? ShowModalWindow()
        {
            return ShowDialog();
        }

        public bool? ShowModalWindow(object viewModelArg)
        {
            DataContext = viewModelArg;
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
    }
}
