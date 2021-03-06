﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RequestForQuoteServicesModuleLibrary.BookMaintenanceService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://book.rfq.ws.leon.com/", ConfigurationName="BookMaintenanceService.BookController")]
    public interface BookController {
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAllResponse getAll(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult BegingetAll(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll request, System.AsyncCallback callback, object asyncState);
        
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAllResponse EndgetAll(System.IAsyncResult result);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.saveResponse save(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult Beginsave(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save request, System.AsyncCallback callback, object asyncState);
        
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.saveResponse Endsave(System.IAsyncResult result);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidityResponse updateValidity(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult BeginupdateValidity(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity request, System.AsyncCallback callback, object asyncState);
        
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidityResponse EndupdateValidity(System.IAsyncResult result);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.deleteResponse delete(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="", ReplyAction="*")]
        System.IAsyncResult Begindelete(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete request, System.AsyncCallback callback, object asyncState);
        
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.deleteResponse Enddelete(System.IAsyncResult result);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://book.rfq.ws.leon.com/")]
    public partial class bookDetailImpl : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string bookCodeField;
        
        private string entityField;
        
        private bool isValidField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string bookCode {
            get {
                return this.bookCodeField;
            }
            set {
                this.bookCodeField = value;
                this.RaisePropertyChanged("bookCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string entity {
            get {
                return this.entityField;
            }
            set {
                this.entityField = value;
                this.RaisePropertyChanged("entity");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public bool isValid {
            get {
                return this.isValidField;
            }
            set {
                this.isValidField = value;
                this.RaisePropertyChanged("isValid");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAll", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class getAll {
        
        public getAll() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllResponse", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class getAllResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bookDetailImpl[] @return;
        
        public getAllResponse() {
        }
        
        public getAllResponse(bookDetailImpl[] @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="save", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class save {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bookCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entity;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string savedByUser;
        
        public save() {
        }
        
        public save(string bookCode, string entity, string savedByUser) {
            this.bookCode = bookCode;
            this.entity = entity;
            this.savedByUser = savedByUser;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="saveResponse", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class saveResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool @return;
        
        public saveResponse() {
        }
        
        public saveResponse(bool @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateValidity", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class updateValidity {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bookCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isValid;
        
        public updateValidity() {
        }
        
        public updateValidity(string bookCode, bool isValid) {
            this.bookCode = bookCode;
            this.isValid = isValid;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateValidityResponse", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class updateValidityResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool @return;
        
        public updateValidityResponse() {
        }
        
        public updateValidityResponse(bool @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="delete", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class delete {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bookCode;
        
        public delete() {
        }
        
        public delete(string bookCode) {
            this.bookCode = bookCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="deleteResponse", WrapperNamespace="http://book.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class deleteResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://book.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool @return;
        
        public deleteResponse() {
        }
        
        public deleteResponse(bool @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface BookControllerChannel : RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class getAllCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public getAllCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bookDetailImpl[] Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bookDetailImpl[])(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class saveCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public saveCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class updateValidityCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public updateValidityCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class deleteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public deleteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BookControllerClient : System.ServiceModel.ClientBase<RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController>, RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController {
        
        private BeginOperationDelegate onBegingetAllDelegate;
        
        private EndOperationDelegate onEndgetAllDelegate;
        
        private System.Threading.SendOrPostCallback ongetAllCompletedDelegate;
        
        private BeginOperationDelegate onBeginsaveDelegate;
        
        private EndOperationDelegate onEndsaveDelegate;
        
        private System.Threading.SendOrPostCallback onsaveCompletedDelegate;
        
        private BeginOperationDelegate onBeginupdateValidityDelegate;
        
        private EndOperationDelegate onEndupdateValidityDelegate;
        
        private System.Threading.SendOrPostCallback onupdateValidityCompletedDelegate;
        
        private BeginOperationDelegate onBegindeleteDelegate;
        
        private EndOperationDelegate onEnddeleteDelegate;
        
        private System.Threading.SendOrPostCallback ondeleteCompletedDelegate;
        
        public BookControllerClient() {
        }
        
        public BookControllerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BookControllerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BookControllerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BookControllerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<getAllCompletedEventArgs> getAllCompleted;
        
        public event System.EventHandler<saveCompletedEventArgs> saveCompleted;
        
        public event System.EventHandler<updateValidityCompletedEventArgs> updateValidityCompleted;
        
        public event System.EventHandler<deleteCompletedEventArgs> deleteCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAllResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.getAll(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll request) {
            return base.Channel.getAll(request);
        }
        
        public bookDetailImpl[] getAll() {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll();
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAllResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).getAll(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.BegingetAll(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BegingetAll(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BegingetAll(System.AsyncCallback callback, object asyncState) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAll();
            return ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).BegingetAll(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAllResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.EndgetAll(System.IAsyncResult result) {
            return base.Channel.EndgetAll(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public bookDetailImpl[] EndgetAll(System.IAsyncResult result) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.getAllResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).EndgetAll(result);
            return retVal.@return;
        }
        
        private System.IAsyncResult OnBegingetAll(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BegingetAll(callback, asyncState);
        }
        
        private object[] OnEndgetAll(System.IAsyncResult result) {
            bookDetailImpl[] retVal = this.EndgetAll(result);
            return new object[] {
                    retVal};
        }
        
        private void OngetAllCompleted(object state) {
            if ((this.getAllCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.getAllCompleted(this, new getAllCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void getAllAsync() {
            this.getAllAsync(null);
        }
        
        public void getAllAsync(object userState) {
            if ((this.onBegingetAllDelegate == null)) {
                this.onBegingetAllDelegate = new BeginOperationDelegate(this.OnBegingetAll);
            }
            if ((this.onEndgetAllDelegate == null)) {
                this.onEndgetAllDelegate = new EndOperationDelegate(this.OnEndgetAll);
            }
            if ((this.ongetAllCompletedDelegate == null)) {
                this.ongetAllCompletedDelegate = new System.Threading.SendOrPostCallback(this.OngetAllCompleted);
            }
            base.InvokeAsync(this.onBegingetAllDelegate, null, this.onEndgetAllDelegate, this.ongetAllCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.saveResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.save(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save request) {
            return base.Channel.save(request);
        }
        
        public bool save(string bookCode, string entity, string savedByUser) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save();
            inValue.bookCode = bookCode;
            inValue.entity = entity;
            inValue.savedByUser = savedByUser;
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.saveResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).save(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.Beginsave(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.Beginsave(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult Beginsave(string bookCode, string entity, string savedByUser, System.AsyncCallback callback, object asyncState) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.save();
            inValue.bookCode = bookCode;
            inValue.entity = entity;
            inValue.savedByUser = savedByUser;
            return ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).Beginsave(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.saveResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.Endsave(System.IAsyncResult result) {
            return base.Channel.Endsave(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public bool Endsave(System.IAsyncResult result) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.saveResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).Endsave(result);
            return retVal.@return;
        }
        
        private System.IAsyncResult OnBeginsave(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string bookCode = ((string)(inValues[0]));
            string entity = ((string)(inValues[1]));
            string savedByUser = ((string)(inValues[2]));
            return this.Beginsave(bookCode, entity, savedByUser, callback, asyncState);
        }
        
        private object[] OnEndsave(System.IAsyncResult result) {
            bool retVal = this.Endsave(result);
            return new object[] {
                    retVal};
        }
        
        private void OnsaveCompleted(object state) {
            if ((this.saveCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.saveCompleted(this, new saveCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void saveAsync(string bookCode, string entity, string savedByUser) {
            this.saveAsync(bookCode, entity, savedByUser, null);
        }
        
        public void saveAsync(string bookCode, string entity, string savedByUser, object userState) {
            if ((this.onBeginsaveDelegate == null)) {
                this.onBeginsaveDelegate = new BeginOperationDelegate(this.OnBeginsave);
            }
            if ((this.onEndsaveDelegate == null)) {
                this.onEndsaveDelegate = new EndOperationDelegate(this.OnEndsave);
            }
            if ((this.onsaveCompletedDelegate == null)) {
                this.onsaveCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnsaveCompleted);
            }
            base.InvokeAsync(this.onBeginsaveDelegate, new object[] {
                        bookCode,
                        entity,
                        savedByUser}, this.onEndsaveDelegate, this.onsaveCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidityResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.updateValidity(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity request) {
            return base.Channel.updateValidity(request);
        }
        
        public bool updateValidity(string bookCode, bool isValid) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity();
            inValue.bookCode = bookCode;
            inValue.isValid = isValid;
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidityResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).updateValidity(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.BeginupdateValidity(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginupdateValidity(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginupdateValidity(string bookCode, bool isValid, System.AsyncCallback callback, object asyncState) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidity();
            inValue.bookCode = bookCode;
            inValue.isValid = isValid;
            return ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).BeginupdateValidity(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidityResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.EndupdateValidity(System.IAsyncResult result) {
            return base.Channel.EndupdateValidity(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public bool EndupdateValidity(System.IAsyncResult result) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.updateValidityResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).EndupdateValidity(result);
            return retVal.@return;
        }
        
        private System.IAsyncResult OnBeginupdateValidity(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string bookCode = ((string)(inValues[0]));
            bool isValid = ((bool)(inValues[1]));
            return this.BeginupdateValidity(bookCode, isValid, callback, asyncState);
        }
        
        private object[] OnEndupdateValidity(System.IAsyncResult result) {
            bool retVal = this.EndupdateValidity(result);
            return new object[] {
                    retVal};
        }
        
        private void OnupdateValidityCompleted(object state) {
            if ((this.updateValidityCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.updateValidityCompleted(this, new updateValidityCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void updateValidityAsync(string bookCode, bool isValid) {
            this.updateValidityAsync(bookCode, isValid, null);
        }
        
        public void updateValidityAsync(string bookCode, bool isValid, object userState) {
            if ((this.onBeginupdateValidityDelegate == null)) {
                this.onBeginupdateValidityDelegate = new BeginOperationDelegate(this.OnBeginupdateValidity);
            }
            if ((this.onEndupdateValidityDelegate == null)) {
                this.onEndupdateValidityDelegate = new EndOperationDelegate(this.OnEndupdateValidity);
            }
            if ((this.onupdateValidityCompletedDelegate == null)) {
                this.onupdateValidityCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnupdateValidityCompleted);
            }
            base.InvokeAsync(this.onBeginupdateValidityDelegate, new object[] {
                        bookCode,
                        isValid}, this.onEndupdateValidityDelegate, this.onupdateValidityCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.deleteResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.delete(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete request) {
            return base.Channel.delete(request);
        }
        
        public bool delete(string bookCode) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete();
            inValue.bookCode = bookCode;
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.deleteResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).delete(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.Begindelete(RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.Begindelete(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult Begindelete(string bookCode, System.AsyncCallback callback, object asyncState) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete inValue = new RequestForQuoteServicesModuleLibrary.BookMaintenanceService.delete();
            inValue.bookCode = bookCode;
            return ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).Begindelete(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.BookMaintenanceService.deleteResponse RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController.Enddelete(System.IAsyncResult result) {
            return base.Channel.Enddelete(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public bool Enddelete(System.IAsyncResult result) {
            RequestForQuoteServicesModuleLibrary.BookMaintenanceService.deleteResponse retVal = ((RequestForQuoteServicesModuleLibrary.BookMaintenanceService.BookController)(this)).Enddelete(result);
            return retVal.@return;
        }
        
        private System.IAsyncResult OnBegindelete(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string bookCode = ((string)(inValues[0]));
            return this.Begindelete(bookCode, callback, asyncState);
        }
        
        private object[] OnEnddelete(System.IAsyncResult result) {
            bool retVal = this.Enddelete(result);
            return new object[] {
                    retVal};
        }
        
        private void OndeleteCompleted(object state) {
            if ((this.deleteCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.deleteCompleted(this, new deleteCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void deleteAsync(string bookCode) {
            this.deleteAsync(bookCode, null);
        }
        
        public void deleteAsync(string bookCode, object userState) {
            if ((this.onBegindeleteDelegate == null)) {
                this.onBegindeleteDelegate = new BeginOperationDelegate(this.OnBegindelete);
            }
            if ((this.onEnddeleteDelegate == null)) {
                this.onEnddeleteDelegate = new EndOperationDelegate(this.OnEnddelete);
            }
            if ((this.ondeleteCompletedDelegate == null)) {
                this.ondeleteCompletedDelegate = new System.Threading.SendOrPostCallback(this.OndeleteCompleted);
            }
            base.InvokeAsync(this.onBegindeleteDelegate, new object[] {
                        bookCode}, this.onEnddeleteDelegate, this.ondeleteCompletedDelegate, userState);
        }
    }
}
