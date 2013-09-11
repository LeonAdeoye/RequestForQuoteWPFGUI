using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using RequestForQuoteInterfacesLibrary.Constants;
using RequestForQuoteInterfacesLibrary.EventPayloads;
using RequestForQuoteInterfacesLibrary.Events;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;
using RequestForQuoteInterfacesLibrary.ServiceInterfaces;
using RequestForQuoteInterfacesLibrary.ViewModelInterfaces;
using RequestForQuoteMaintenanceModuleLibrary.Commands;
using log4net;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    public class BookMaintenanceViewModel : DependencyObject, IUpdateValidityViewModel, IClearInputViewModel, IAddNewItemViewModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
        private readonly IBookManager bookManager = ServiceLocator.Current.GetInstance<IBookManager>();

        public ObservableCollection<IBook> Books { get; set; }
        public ICommand AddNewItemCommand { get; set; }
        public ICommand ClearInputCommand { get; set; }
        public ICommand UpdateValidityCommand { get; set; }

        public BookMaintenanceViewModel()
        {
            AddNewItemCommand = new AddNewItemCommand(this);
            ClearInputCommand = new ClearInputCommand(this);
            UpdateValidityCommand = new UpdateValidityCommand(this);

            InitializeCollections();
            InitializeEventSubscriptions();
        }

        private void InitializeCollections()
        {
            Books = new ObservableCollection<IBook>(bookManager.Books);
        }

        private void InitializeEventSubscriptions()
        {
            eventAggregator.GetEvent<NewBookEvent>().Subscribe(HandleNewBookEvent, ThreadOption.UIThread, RequestForQuoteConstants.MAINTAIN_STRONG_REFERENCE);
        }

        public void HandleNewBookEvent(NewBookEventPayload eventPayLoad)
        {
            if (log.IsDebugEnabled)
                log.Debug("Received new book event from BookManager: " + eventPayLoad);

            Books.Add(eventPayLoad.NewBook);
        }

        public string NewBookCode
        {
            get { return (string)GetValue(NewBookCodeProperty); }
            set { SetValue(NewBookCodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewBookCode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewBookCodeProperty =
            DependencyProperty.Register("NewBookCode", typeof(string), typeof(BookMaintenanceViewModel), new UIPropertyMetadata(""));

        public string NewEntityCode
        {
            get { return (string)GetValue(NewEntityCodeProperty); }
            set { SetValue(NewEntityCodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewEntityCode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewEntityCodeProperty =
            DependencyProperty.Register("NewEntityCode", typeof(string), typeof(BookMaintenanceViewModel), new UIPropertyMetadata(""));
      
        public void ClearInput()
        {
            NewBookCode = "";
            NewEntityCode = "";
        }

        public bool CanClearInput()
        {
            return !string.IsNullOrEmpty(NewBookCode) || !string.IsNullOrEmpty(NewEntityCode);
        }

        public void AddNewItem()
        {
            if (!bookManager.Books.Exists((book) => book.BookCode == NewBookCode))
            {
                bookManager.AddBook(NewBookCode, NewEntityCode, RequestForQuoteConstants.VALID, RequestForQuoteConstants.SAVE_TO_DATABASE);
                ClearInput();
            }
            else
                MessageBox.Show("Cannot add the book code: " + NewBookCode + " because it already exists!",
                                "Error adding book...", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool CanAddNewItem()
        {
            return !string.IsNullOrEmpty(NewBookCode);    
        }

        public bool CanUpdateValidity(bool isRequestToMakeValid)
        {
            if (SelectedBook == null)
                return false;
            return (SelectedBook.IsValid != isRequestToMakeValid);
        }

        public void UpdateValidity()
        {
            if (!bookManager.UpdateValidity(SelectedBook.BookCode, !SelectedBook.IsValid))
                MessageBox.Show("Failed to update validity of the book with the book code " + SelectedBook.BookCode, "Book Management Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                SelectedBook.IsValid = !SelectedBook.IsValid;
        }

        public IBook SelectedBook
        {
            get { return (IBook)GetValue(SelectedBookProperty); }
            set { SetValue(SelectedBookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBook.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBookProperty =
            DependencyProperty.Register("SelectedBook", typeof(IBook), typeof(BookMaintenanceViewModel), new UIPropertyMetadata(null));      
    }
}
