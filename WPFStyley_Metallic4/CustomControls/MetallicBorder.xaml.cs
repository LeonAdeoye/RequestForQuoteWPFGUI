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
    /// Interaction logic for WPFStyley_MetallicBorder.xaml
    /// </summary>
    public partial class MetallicBorder : UserControl
    {
        public static readonly DependencyProperty CornerRadius = DependencyProperty.Register("WpfsCornerRadius", typeof(CornerRadius), typeof(MetallicBorder));

        public CornerRadius WpfsCornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadius);
            }
            set
            {
                SetValue(CornerRadius, value);
            }
        }

        public MetallicBorder()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(WPFStyley_MetallicBorder_Loaded);
        }

        void WPFStyley_MetallicBorder_Loaded(object sender, RoutedEventArgs e)
        {            
            double TopLeft = 0;
            double TopRight = 0;
            double BottomRight = 0;
            double BottomLeft = 0;
            
            if(WpfsCornerRadius.TopLeft > 2)
                TopLeft = WpfsCornerRadius.TopLeft - 2;
            if(WpfsCornerRadius.TopRight > 2)
                TopRight = WpfsCornerRadius.TopRight - 2;
            if(WpfsCornerRadius.BottomRight > 2)
                BottomRight = WpfsCornerRadius.BottomRight - 2;
            if(WpfsCornerRadius.BottomLeft > 2)
                BottomLeft = WpfsCornerRadius.BottomLeft - 2;

            CornerRadius CrTemp = new CornerRadius(TopLeft, TopRight, BottomRight, BottomLeft);

            this.WpfsBorder1.CornerRadius = this.WpfsCornerRadius;
            this.WpfsBorder2.CornerRadius = this.WpfsCornerRadius;
            this.WpfsBorder3.CornerRadius = CrTemp;
            this.WpfsBorder4.CornerRadius = CrTemp;
        }
    }
}
