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
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.WindowInterfaces;

namespace RequestForQuoteFunctionsModuleLibrary
{
    /// <summary> 
    /// Interaction logic for RequestForQuoteFunctions.xaml
    /// </summary>
    public partial class RequestForQuoteFunctions : UserControl
    {
        public RequestForQuoteFunctions(IUnityContainer container)
        {
            this.Resources = Application.LoadComponent(
            new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;

            InitializeComponent();

            DataContext = new RequestForQuoteFunctionsViewModel(container.Resolve<IClientManager>(), container.Resolve<IUnderlyingManager>(),
                container.Resolve<IBookManager>(), container.Resolve<ISearchManager>());
        }
    }
}
