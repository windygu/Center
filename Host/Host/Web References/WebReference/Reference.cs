﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.261
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 原始程式碼已由 Microsoft.VSDesigner 自動產生，版本 4.0.30319.261。
// 
#pragma warning disable 1591

namespace Host.WebReference {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://tempuri.org/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private System.Threading.SendOrPostCallback printReportOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetMovingContructEventOperationCompleted;
        
        private System.Threading.SendOrPostCallback CloseMovingConstructEventOperationCompleted;
        
        private System.Threading.SendOrPostCallback SendSMSOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::Host.Properties.Settings.Default.Host_WebReference_Service;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        public event printReportCompletedEventHandler printReportCompleted;
        
        /// <remarks/>
        public event SetMovingContructEventCompletedEventHandler SetMovingContructEventCompleted;
        
        /// <remarks/>
        public event CloseMovingConstructEventCompletedEventHandler CloseMovingConstructEventCompleted;
        
        /// <remarks/>
        public event SendSMSCompletedEventHandler SendSMSCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/printReport", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void printReport(int rptId) {
            this.Invoke("printReport", new object[] {
                        rptId});
        }
        
        /// <remarks/>
        public void printReportAsync(int rptId) {
            this.printReportAsync(rptId, null);
        }
        
        /// <remarks/>
        public void printReportAsync(int rptId, object userState) {
            if ((this.printReportOperationCompleted == null)) {
                this.printReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnprintReportOperationCompleted);
            }
            this.InvokeAsync("printReport", new object[] {
                        rptId}, this.printReportOperationCompleted, userState);
        }
        
        private void OnprintReportOperationCompleted(object arg) {
            if ((this.printReportCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.printReportCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SetMovingContructEvent", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void SetMovingContructEvent(int id, string notifier, string timeStamp, string lineID, string directionID, int startMileage, int endMileage, int blockTypeId, string blocklane, string description) {
            this.Invoke("SetMovingContructEvent", new object[] {
                        id,
                        notifier,
                        timeStamp,
                        lineID,
                        directionID,
                        startMileage,
                        endMileage,
                        blockTypeId,
                        blocklane,
                        description});
        }
        
        /// <remarks/>
        public void SetMovingContructEventAsync(int id, string notifier, string timeStamp, string lineID, string directionID, int startMileage, int endMileage, int blockTypeId, string blocklane, string description) {
            this.SetMovingContructEventAsync(id, notifier, timeStamp, lineID, directionID, startMileage, endMileage, blockTypeId, blocklane, description, null);
        }
        
        /// <remarks/>
        public void SetMovingContructEventAsync(int id, string notifier, string timeStamp, string lineID, string directionID, int startMileage, int endMileage, int blockTypeId, string blocklane, string description, object userState) {
            if ((this.SetMovingContructEventOperationCompleted == null)) {
                this.SetMovingContructEventOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetMovingContructEventOperationCompleted);
            }
            this.InvokeAsync("SetMovingContructEvent", new object[] {
                        id,
                        notifier,
                        timeStamp,
                        lineID,
                        directionID,
                        startMileage,
                        endMileage,
                        blockTypeId,
                        blocklane,
                        description}, this.SetMovingContructEventOperationCompleted, userState);
        }
        
        private void OnSetMovingContructEventOperationCompleted(object arg) {
            if ((this.SetMovingContructEventCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetMovingContructEventCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CloseMovingConstructEvent", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void CloseMovingConstructEvent(int id) {
            this.Invoke("CloseMovingConstructEvent", new object[] {
                        id});
        }
        
        /// <remarks/>
        public void CloseMovingConstructEventAsync(int id) {
            this.CloseMovingConstructEventAsync(id, null);
        }
        
        /// <remarks/>
        public void CloseMovingConstructEventAsync(int id, object userState) {
            if ((this.CloseMovingConstructEventOperationCompleted == null)) {
                this.CloseMovingConstructEventOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCloseMovingConstructEventOperationCompleted);
            }
            this.InvokeAsync("CloseMovingConstructEvent", new object[] {
                        id}, this.CloseMovingConstructEventOperationCompleted, userState);
        }
        
        private void OnCloseMovingConstructEventOperationCompleted(object arg) {
            if ((this.CloseMovingConstructEventCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CloseMovingConstructEventCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/SendSMS", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int SendSMS(string phoneNo, string body) {
            object[] results = this.Invoke("SendSMS", new object[] {
                        phoneNo,
                        body});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void SendSMSAsync(string phoneNo, string body) {
            this.SendSMSAsync(phoneNo, body, null);
        }
        
        /// <remarks/>
        public void SendSMSAsync(string phoneNo, string body, object userState) {
            if ((this.SendSMSOperationCompleted == null)) {
                this.SendSMSOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendSMSOperationCompleted);
            }
            this.InvokeAsync("SendSMS", new object[] {
                        phoneNo,
                        body}, this.SendSMSOperationCompleted, userState);
        }
        
        private void OnSendSMSOperationCompleted(object arg) {
            if ((this.SendSMSCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SendSMSCompleted(this, new SendSMSCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void printReportCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SetMovingContructEventCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void CloseMovingConstructEventCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SendSMSCompletedEventHandler(object sender, SendSMSCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendSMSCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SendSMSCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591