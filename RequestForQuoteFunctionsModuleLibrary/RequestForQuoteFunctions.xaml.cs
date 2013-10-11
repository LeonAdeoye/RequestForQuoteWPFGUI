using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;

namespace RequestForQuoteFunctionsModuleLibrary
{
    /// <summary> 
    /// Interaction logic for RequestForQuoteFunctions.xaml
    /// </summary>
    public partial class RequestForQuoteFunctions : UserControl
    {
        public RequestForQuoteFunctions(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.Resources = Application.LoadComponent(
            new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;

            InitializeComponent();

            DataContext = container.Resolve<RequestForQuoteFunctionsViewModel>();
        }
    }
}
