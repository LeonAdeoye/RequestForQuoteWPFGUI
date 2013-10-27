using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ.Popups
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window, IWindowPopup
    {
        public ReportWindow()
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

        public void HideWindow()
        {
            Hide();
        }

        public bool IsApplicationRemainingOpen { get; set; }
    }
}
