﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RequestForQuoteServicesModuleLibrary.ClientMaintenanceService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://client.rfq.ws.leon.com/", ConfigurationName="ClientMaintenanceService.ClientController")]
    public interface ClientController {
        
        // CODEGEN: Parameter 'name' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.saveResponse save(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.save request);
        
        // CODEGEN: Parameter 'identifier' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateValidityResponse updateValidity(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateValidity request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.getAllResponse getAll(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.getAll request);
        
        // CODEGEN: Parameter 'identifier' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.deleteResponse delete(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.delete request);
        
        // CODEGEN: Parameter 'identifier' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateTierResponse updateTier(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateTier request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="save", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class save {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int tier;
        
        public save() {
        }
        
        public save(string name, int tier) {
            this.name = name;
            this.tier = tier;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="saveResponse", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class saveResponse {
        
        public saveResponse() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateValidity", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class updateValidity {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int identifier;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isValid;
        
        public updateValidity() {
        }
        
        public updateValidity(int identifier, bool isValid) {
            this.identifier = identifier;
            this.isValid = isValid;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateValidityResponse", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class updateValidityResponse {
        
        public updateValidityResponse() {
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1009")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://client.rfq.ws.leon.com/")]
    public partial class clientDetail : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int identifierField;
        
        private bool isValidField;
        
        private string nameField;
        
        private int tierField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int identifier {
            get {
                return this.identifierField;
            }
            set {
                this.identifierField = value;
                this.RaisePropertyChanged("identifier");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public bool isValid {
            get {
                return this.isValidField;
            }
            set {
                this.isValidField = value;
                this.RaisePropertyChanged("isValid");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
                this.RaisePropertyChanged("name");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public int tier {
            get {
                return this.tierField;
            }
            set {
                this.tierField = value;
                this.RaisePropertyChanged("tier");
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAll", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class getAll {
        
        public getAll() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllResponse", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class getAllResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public clientDetail[] @return;
        
        public getAllResponse() {
        }
        
        public getAllResponse(clientDetail[] @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="delete", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class delete {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int identifier;
        
        public delete() {
        }
        
        public delete(int identifier) {
            this.identifier = identifier;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="deleteResponse", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class deleteResponse {
        
        public deleteResponse() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateTier", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class updateTier {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int identifier;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://client.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int tier;
        
        public updateTier() {
        }
        
        public updateTier(int identifier, int tier) {
            this.identifier = identifier;
            this.tier = tier;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="updateTierResponse", WrapperNamespace="http://client.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class updateTierResponse {
        
        public updateTierResponse() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ClientControllerChannel : RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ClientControllerClient : System.ServiceModel.ClientBase<RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController>, RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController {
        
        public ClientControllerClient() {
        }
        
        public ClientControllerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ClientControllerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ClientControllerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ClientControllerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.saveResponse RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController.save(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.save request) {
            return base.Channel.save(request);
        }
        
        public void save(string name, int tier) {
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.save inValue = new RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.save();
            inValue.name = name;
            inValue.tier = tier;
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.saveResponse retVal = ((RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController)(this)).save(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateValidityResponse RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController.updateValidity(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateValidity request) {
            return base.Channel.updateValidity(request);
        }
        
        public void updateValidity(int identifier, bool isValid) {
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateValidity inValue = new RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateValidity();
            inValue.identifier = identifier;
            inValue.isValid = isValid;
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateValidityResponse retVal = ((RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController)(this)).updateValidity(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.getAllResponse RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController.getAll(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.getAll request) {
            return base.Channel.getAll(request);
        }
        
        public clientDetail[] getAll() {
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.getAll inValue = new RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.getAll();
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.getAllResponse retVal = ((RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController)(this)).getAll(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.deleteResponse RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController.delete(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.delete request) {
            return base.Channel.delete(request);
        }
        
        public void delete(int identifier) {
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.delete inValue = new RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.delete();
            inValue.identifier = identifier;
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.deleteResponse retVal = ((RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController)(this)).delete(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateTierResponse RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController.updateTier(RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateTier request) {
            return base.Channel.updateTier(request);
        }
        
        public void updateTier(int identifier, int tier) {
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateTier inValue = new RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateTier();
            inValue.identifier = identifier;
            inValue.tier = tier;
            RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.updateTierResponse retVal = ((RequestForQuoteServicesModuleLibrary.ClientMaintenanceService.ClientController)(this)).updateTier(inValue);
        }
    }
}
