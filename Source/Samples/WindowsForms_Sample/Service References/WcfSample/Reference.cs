﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsForms_Sample.WcfSample {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfSample.ISampleWcfService")]
    public interface ISampleWcfService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleWcfService/GetMessage", ReplyAction="http://tempuri.org/ISampleWcfService/GetMessageResponse")]
        string GetMessage(string key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleWcfService/GetMessage", ReplyAction="http://tempuri.org/ISampleWcfService/GetMessageResponse")]
        System.Threading.Tasks.Task<string> GetMessageAsync(string key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleWcfService/MakeError", ReplyAction="http://tempuri.org/ISampleWcfService/MakeErrorResponse")]
        void MakeError();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleWcfService/MakeError", ReplyAction="http://tempuri.org/ISampleWcfService/MakeErrorResponse")]
        System.Threading.Tasks.Task MakeErrorAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISampleWcfServiceChannel : WindowsForms_Sample.WcfSample.ISampleWcfService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SampleWcfServiceClient : System.ServiceModel.ClientBase<WindowsForms_Sample.WcfSample.ISampleWcfService>, WindowsForms_Sample.WcfSample.ISampleWcfService {
        
        public SampleWcfServiceClient() {
        }
        
        public SampleWcfServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SampleWcfServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleWcfServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleWcfServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetMessage(string key) {
            return base.Channel.GetMessage(key);
        }
        
        public System.Threading.Tasks.Task<string> GetMessageAsync(string key) {
            return base.Channel.GetMessageAsync(key);
        }
        
        public void MakeError() {
            base.Channel.MakeError();
        }
        
        public System.Threading.Tasks.Task MakeErrorAsync() {
            return base.Channel.MakeErrorAsync();
        }
    }
}
