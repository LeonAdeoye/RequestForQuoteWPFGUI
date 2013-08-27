namespace RequestForQuoteInterfacesLibrary.WindowInterfaces
{
    public interface IWindowPopup
    {
        void ShowWindow(object viewModelArg);
        void ShowWindow();
        bool? ShowModalWindow();
        bool? ShowModalWindow(object viewModelArg);
        void CloseWindow();
        void ActivateWindow();
    }
}
