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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RequestForQuoteFunctionsModuleLibrary
{
    /// <summary>
    /// Interaction logic for SaveSearchUserControl.xaml
    /// </summary>
    public partial class SaveSearchUserControl : UserControl
    {
        public SaveSearchUserControl()
        {
            InitializeComponent();
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            RequestForQuoteFunctionsViewModel vm = DataContext as RequestForQuoteFunctionsViewModel;
            if (vm != null)
                vm.SaveSearch();
        }
    }
}
