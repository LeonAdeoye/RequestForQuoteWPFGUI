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
    /// Interaction logic for SunkenBackground.xaml
    /// </summary>
    public partial class SunkenBackground : UserControl
    {
        public static readonly DependencyProperty RadiusX = DependencyProperty.Register("WpfsRadiusX", typeof(double), typeof(SunkenBackground));
        public static readonly DependencyProperty RadiusY = DependencyProperty.Register("WpfsRadiusY", typeof(double), typeof(SunkenBackground));

        public double WpfsRadiusX
        {
            get
            {
                return (double)GetValue(RadiusX);
            }
            set
            {
                SetValue(RadiusX, value);
            }
        }
        public double WpfsRadiusY
        {
            get
            {
                return (double)GetValue(RadiusY);
            }
            set
            {
                SetValue(RadiusY, value);
            }
        }
        public SunkenBackground()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SunkenBackground_Loaded);
        }

        void SunkenBackground_Loaded(object sender, RoutedEventArgs e)
        {
            BackgroundRect.RadiusX = WpfsRadiusX;
            BackgroundRect.RadiusY = WpfsRadiusY;
        }

    }
}
