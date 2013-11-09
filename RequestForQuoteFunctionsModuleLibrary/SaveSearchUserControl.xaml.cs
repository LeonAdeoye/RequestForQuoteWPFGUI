using System;
using System.Windows;
using System.Windows.Controls;

namespace RequestForQuoteFunctionsModuleLibrary
{
    /// <summary>
    /// Interaction logic for SaveSearchUserControl.xaml
    /// </summary>
    public partial class SaveSearchUserControl : UserControl
    {
        public SaveSearchUserControl()
        {
            this.Resources = Application.LoadComponent(new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;
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
