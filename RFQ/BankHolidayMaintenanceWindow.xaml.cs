﻿using System;
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

namespace RFQ
{
    /// <summary>
    /// Interaction logic for BankHolidayMaintenanceWindow.xaml
    /// </summary>
    public partial class BankHolidayMaintenanceWindow : Window, IBankHolidayMaintenancePopupWindow
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
    }
}
