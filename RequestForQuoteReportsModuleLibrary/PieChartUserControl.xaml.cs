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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;

namespace RequestForQuoteReportsModuleLibrary
{
    /// <summary>
    /// Interaction logic for PieChartUserControl.xaml
    /// </summary>
    public partial class PieChartUserControl : UserControl
    {
        public PieChartUserControl(IUnityContainer container)
        {
            InitializeComponent();
        }
    }
}
