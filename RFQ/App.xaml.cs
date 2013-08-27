using System.Windows;

namespace RFQ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // The bootstrapper will create the Shell so no need to set StartupUri in XAML
            RequestForQuoteBootstrapper bootstrapper = new RequestForQuoteBootstrapper();
            bootstrapper.Run();
        }
    }
}
