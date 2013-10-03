﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RequestForQuoteServicesModuleLibrary.OptionPricerService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://model.option.rfq.ws.leon.com/", ConfigurationName="OptionPricerService.OptionPricingController")]
    public interface OptionPricingController {
        
        // CODEGEN: Parameter 'strike' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.parameterizeResponse parameterize(RequestForQuoteServicesModuleLibrary.OptionPricerService.parameterize request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateResponse calculate(RequestForQuoteServicesModuleLibrary.OptionPricerService.calculate request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.getModelDetailsResponse getModelDetails(RequestForQuoteServicesModuleLibrary.OptionPricerService.getModelDetails request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateRangeResponse calculateRange(RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateRange request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="parameterize", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class parameterize {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double strike;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double volatility;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double underlyingPrice;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double daysToExpiry;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double interestRate;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isCall;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=6)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isEuropean;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=7)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double dayCountConvention;
        
        public parameterize() {
        }
        
        public parameterize(double strike, double volatility, double underlyingPrice, double daysToExpiry, double interestRate, bool isCall, bool isEuropean, double dayCountConvention) {
            this.strike = strike;
            this.volatility = volatility;
            this.underlyingPrice = underlyingPrice;
            this.daysToExpiry = daysToExpiry;
            this.interestRate = interestRate;
            this.isCall = isCall;
            this.isEuropean = isEuropean;
            this.dayCountConvention = dayCountConvention;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="parameterizeResponse", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class parameterizeResponse {
        
        public parameterizeResponse() {
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://model.option.rfq.ws.leon.com/")]
    public partial class optionPriceResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private double deltaField;
        
        private double gammaField;
        
        private double priceField;
        
        private double rhoField;
        
        private double thetaField;
        
        private double vegaField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public double delta {
            get {
                return this.deltaField;
            }
            set {
                this.deltaField = value;
                this.RaisePropertyChanged("delta");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public double gamma {
            get {
                return this.gammaField;
            }
            set {
                this.gammaField = value;
                this.RaisePropertyChanged("gamma");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public double price {
            get {
                return this.priceField;
            }
            set {
                this.priceField = value;
                this.RaisePropertyChanged("price");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public double rho {
            get {
                return this.rhoField;
            }
            set {
                this.rhoField = value;
                this.RaisePropertyChanged("rho");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public double theta {
            get {
                return this.thetaField;
            }
            set {
                this.thetaField = value;
                this.RaisePropertyChanged("theta");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public double vega {
            get {
                return this.vegaField;
            }
            set {
                this.vegaField = value;
                this.RaisePropertyChanged("vega");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://model.option.rfq.ws.leon.com/")]
    public partial class optionPriceResultSet : object, System.ComponentModel.INotifyPropertyChanged {
        
        private optionPriceResult[] optionPriceResultsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OptionPriceResult", IsNullable=false)]
        public optionPriceResult[] optionPriceResults {
            get {
                return this.optionPriceResultsField;
            }
            set {
                this.optionPriceResultsField = value;
                this.RaisePropertyChanged("optionPriceResults");
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculate", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class calculate {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double strike;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double volatility;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double underlyingPrice;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double daysToExpiry;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double interestRate;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isCall;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=6)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isEuropean;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=7)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double dayCountConvention;
        
        public calculate() {
        }
        
        public calculate(double strike, double volatility, double underlyingPrice, double daysToExpiry, double interestRate, bool isCall, bool isEuropean, double dayCountConvention) {
            this.strike = strike;
            this.volatility = volatility;
            this.underlyingPrice = underlyingPrice;
            this.daysToExpiry = daysToExpiry;
            this.interestRate = interestRate;
            this.isCall = isCall;
            this.isEuropean = isEuropean;
            this.dayCountConvention = dayCountConvention;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculateResponse", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class calculateResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RequestForQuoteServicesModuleLibrary.OptionPricerService.optionPriceResult @return;
        
        public calculateResponse() {
        }
        
        public calculateResponse(RequestForQuoteServicesModuleLibrary.OptionPricerService.optionPriceResult @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getModelDetails", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class getModelDetails {
        
        public getModelDetails() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getModelDetailsResponse", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class getModelDetailsResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string @return;
        
        public getModelDetailsResponse() {
        }
        
        public getModelDetailsResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculateRange", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class calculateRange {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rangeKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double startValue;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double endValue;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double increment;
        
        public calculateRange() {
        }
        
        public calculateRange(string rangeKey, double startValue, double endValue, double increment) {
            this.rangeKey = rangeKey;
            this.startValue = startValue;
            this.endValue = endValue;
            this.increment = increment;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="calculateRangeResponse", WrapperNamespace="http://model.option.rfq.ws.leon.com/", IsWrapped=true)]
    public partial class calculateRangeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://model.option.rfq.ws.leon.com/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RequestForQuoteServicesModuleLibrary.OptionPricerService.optionPriceResultSet @return;
        
        public calculateRangeResponse() {
        }
        
        public calculateRangeResponse(RequestForQuoteServicesModuleLibrary.OptionPricerService.optionPriceResultSet @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface OptionPricingControllerChannel : RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OptionPricingControllerClient : System.ServiceModel.ClientBase<RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController>, RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController {
        
        public OptionPricingControllerClient() {
        }
        
        public OptionPricingControllerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OptionPricingControllerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OptionPricingControllerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OptionPricingControllerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.parameterizeResponse RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController.parameterize(RequestForQuoteServicesModuleLibrary.OptionPricerService.parameterize request) {
            return base.Channel.parameterize(request);
        }
        
        public void parameterize(double strike, double volatility, double underlyingPrice, double daysToExpiry, double interestRate, bool isCall, bool isEuropean, double dayCountConvention) {
            RequestForQuoteServicesModuleLibrary.OptionPricerService.parameterize inValue = new RequestForQuoteServicesModuleLibrary.OptionPricerService.parameterize();
            inValue.strike = strike;
            inValue.volatility = volatility;
            inValue.underlyingPrice = underlyingPrice;
            inValue.daysToExpiry = daysToExpiry;
            inValue.interestRate = interestRate;
            inValue.isCall = isCall;
            inValue.isEuropean = isEuropean;
            inValue.dayCountConvention = dayCountConvention;
            RequestForQuoteServicesModuleLibrary.OptionPricerService.parameterizeResponse retVal = ((RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController)(this)).parameterize(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateResponse RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController.calculate(RequestForQuoteServicesModuleLibrary.OptionPricerService.calculate request) {
            return base.Channel.calculate(request);
        }
        
        public RequestForQuoteServicesModuleLibrary.OptionPricerService.optionPriceResult calculate(double strike, double volatility, double underlyingPrice, double daysToExpiry, double interestRate, bool isCall, bool isEuropean, double dayCountConvention) {
            RequestForQuoteServicesModuleLibrary.OptionPricerService.calculate inValue = new RequestForQuoteServicesModuleLibrary.OptionPricerService.calculate();
            inValue.strike = strike;
            inValue.volatility = volatility;
            inValue.underlyingPrice = underlyingPrice;
            inValue.daysToExpiry = daysToExpiry;
            inValue.interestRate = interestRate;
            inValue.isCall = isCall;
            inValue.isEuropean = isEuropean;
            inValue.dayCountConvention = dayCountConvention;
            RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateResponse retVal = ((RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController)(this)).calculate(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.getModelDetailsResponse RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController.getModelDetails(RequestForQuoteServicesModuleLibrary.OptionPricerService.getModelDetails request) {
            return base.Channel.getModelDetails(request);
        }
        
        public string getModelDetails() {
            RequestForQuoteServicesModuleLibrary.OptionPricerService.getModelDetails inValue = new RequestForQuoteServicesModuleLibrary.OptionPricerService.getModelDetails();
            RequestForQuoteServicesModuleLibrary.OptionPricerService.getModelDetailsResponse retVal = ((RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController)(this)).getModelDetails(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateRangeResponse RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController.calculateRange(RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateRange request) {
            return base.Channel.calculateRange(request);
        }
        
        public RequestForQuoteServicesModuleLibrary.OptionPricerService.optionPriceResultSet calculateRange(string rangeKey, double startValue, double endValue, double increment) {
            RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateRange inValue = new RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateRange();
            inValue.rangeKey = rangeKey;
            inValue.startValue = startValue;
            inValue.endValue = endValue;
            inValue.increment = increment;
            RequestForQuoteServicesModuleLibrary.OptionPricerService.calculateRangeResponse retVal = ((RequestForQuoteServicesModuleLibrary.OptionPricerService.OptionPricingController)(this)).calculateRange(inValue);
            return retVal.@return;
        }
    }
}
