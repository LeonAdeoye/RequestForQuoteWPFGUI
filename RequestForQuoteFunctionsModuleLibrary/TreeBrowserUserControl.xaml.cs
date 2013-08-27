using System;
using System.Windows;
using System.Windows.Controls;

namespace RequestForQuoteFunctionsModuleLibrary
{
    /// <summary>
    /// Interaction logic for TreeBrowserUserControl.xaml
    /// </summary>
    public partial class TreeBrowserUserControl : UserControl
    {
        public TreeBrowserUserControl()
        {
            this.Resources = Application.LoadComponent(
            new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;

            InitializeComponent();
        }
    }
}
