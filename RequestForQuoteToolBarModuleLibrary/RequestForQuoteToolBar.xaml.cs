using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteToolBarModuleLibrary
{
    /// <summary>
    /// Interaction logic for RequestForQuoteToolBar.xaml
    /// </summary>
    public partial class RequestForQuoteToolBar : UserControl
    {
        public RequestForQuoteToolBar(IUnityContainer container)
        {
            InitializeComponent();
            DataContext = container.Resolve<RequestForQuoteToolBarViewModel>();
        }
    }
}
