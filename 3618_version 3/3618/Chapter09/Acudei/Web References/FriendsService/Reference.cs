﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 1.1.4322.573.
// 
namespace Acudei.FriendsService {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PartnersSoap", Namespace="http://www.apress.com/services/friendsreunion")]
    public class Partners : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        /// <remarks/>
        public Partners() {
            this.Url = "http://localhost/FriendsReunion/Services/Partners.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.apress.com/services/friendsreunion/GetAttendees", RequestNamespace="http://www.apress.com/services/friendsreunion", ResponseNamespace="http://www.apress.com/services/friendsreunion", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int GetAttendees(string placeId) {
            object[] results = this.Invoke("GetAttendees", new object[] {
                        placeId});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetAttendees(string placeId, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetAttendees", new object[] {
                        placeId}, callback, asyncState);
        }
        
        /// <remarks/>
        public int EndGetAttendees(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.apress.com/services/friendsreunion/GetContactRequests", RequestNamespace="http://www.apress.com/services/friendsreunion", ResponseNamespace="http://www.apress.com/services/friendsreunion", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlNode GetContactRequests(string login, string password) {
            object[] results = this.Invoke("GetContactRequests", new object[] {
                        login,
                        password});
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetContactRequests(string login, string password, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetContactRequests", new object[] {
                        login,
                        password}, callback, asyncState);
        }
        
        /// <remarks/>
        public System.Xml.XmlNode EndGetContactRequests(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Xml.XmlNode)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.apress.com/services/friendsreunion/GetContactRequestsCustom", RequestNamespace="http://www.apress.com/services/friendsreunion", ResponseNamespace="http://www.apress.com/services/friendsreunion", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Contact[] GetContactRequestsCustom(string login, string password) {
            object[] results = this.Invoke("GetContactRequestsCustom", new object[] {
                        login,
                        password});
            return ((Contact[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetContactRequestsCustom(string login, string password, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetContactRequestsCustom", new object[] {
                        login,
                        password}, callback, asyncState);
        }
        
        /// <remarks/>
        public Contact[] EndGetContactRequestsCustom(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Contact[])(results[0]));
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.apress.com/services/friendsreunion")]
    public class Contact {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FirstName 
		{
			get { return _FirstName; }
			set { _FirstName = value; }
		} private string _FirstName;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string LastName 
		{
			get { return _LastName; }
			set { _LastName = value; }
		} private string _LastName;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Email 
		{
			get { return _Email; }
			set { _Email = value; }
		} private string _Email;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Notes 
		{
			get { return _Notes; }
			set { _Notes = value; }
		} private string _Notes;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsApproved 
		{
			get { return _IsApproved; }
			set { _IsApproved = value; }
		} private bool _IsApproved;
    }
}
