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

namespace WPFStyley_Metallic4.CustomControls
{
    /// <summary>
    /// Interaction logic for MainTitle.xaml
    /// </summary>
    public partial class MainTitle : UserControl
    {
        public static readonly DependencyProperty Title = DependencyProperty.Register("WpfsTitle", typeof(String), typeof(MainTitle));

        public String WpfsTitle
        {
            get
            {
                return (String)GetValue(Title);
            }
            set
            {
                SetValue(Title, value);
            }
        }
        public MainTitle()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainTitle_Loaded);
        }

        void MainTitle_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(WpfsTitle))
            {
                WpfsTitle = string.Empty;
            }

            txtMain.Text = WpfsTitle + " ";
        }
    }
}
