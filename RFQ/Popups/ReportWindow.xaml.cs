﻿using System.Windows;
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
