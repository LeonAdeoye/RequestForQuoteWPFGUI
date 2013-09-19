﻿using System;
using System.ComponentModel;
using System.Windows;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RFQ
{
    /// <summary>
    /// Interaction logic for RequestForQuoteDetailsWindow.xaml
    /// </summary>
    public partial class RequestForQuoteDetailsWindow : Window, IRequestForQuoteDetailsPopupWindow
    {
        public RequestForQuoteDetailsWindow()
        {
            InitializeComponent();
            IsApplicationRemainingOpen = true;
            Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            cancelEventArgs.Cancel = IsApplicationRemainingOpen;
            Hide();
        }

        public void ShowWindow(object viewModelArg)
        {
            DataContext = viewModelArg;
            Show();
        }

        public void ShowWindow()
        {
            Show();
            Activate();
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
