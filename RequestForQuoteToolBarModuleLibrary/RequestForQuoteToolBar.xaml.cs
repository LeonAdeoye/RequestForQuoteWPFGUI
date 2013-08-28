using System.Windows.Controls;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

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

            DataContext = new RequestForQuoteToolBarViewModel(container.Resolve<IOptionRequestParser>(), 
                container.Resolve<IClientManager>(),
                container.Resolve<IBookManager>());
        }
    }
}
