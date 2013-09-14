using System.Collections.Generic;
using System.Collections.ObjectModel;
using RequestForQuoteInterfacesLibrary.ModelInterfaces;

namespace RequestForQuoteInterfacesLibrary.ServiceInterfaces
{
    public interface IBookManager
    {
        List<IBook> Books { get; set; }
        bool AddBook(string bookCode, string entity, bool isValid, bool canSaveToDatabase);
        bool RemoveBook(string bookCode);
        bool UpdateValidity(string bookCode, bool isValid);
        void Initialize(bool isStandAlone);
    }
}