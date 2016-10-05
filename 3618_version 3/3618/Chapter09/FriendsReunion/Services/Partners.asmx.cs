using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace FriendsReunion.Services
{
	/// <summary>
	/// Summary description for Partners.
	/// </summary>
	[WebService(Namespace="http://www.apress.com/services/friendsreunion")]
	public class Partners : System.Web.Services.WebService
	{
		public Partners()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		private System.Data.SqlClient.SqlConnection cnFriends;
		private System.Data.SqlClient.SqlCommand cmAttendeesCount;
		private System.Data.SqlClient.SqlCommand cmContacts;

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
			this.cnFriends = new System.Data.SqlClient.SqlConnection();
			this.cmAttendeesCount = new System.Data.SqlClient.SqlCommand();
			this.cmContacts = new System.Data.SqlClient.SqlCommand();
			// 
			// cnFriends
			// 
			this.cnFriends.ConnectionString = ((string)(configurationAppSettings.GetValue("cnFriends.ConnectionString", typeof(string))));
			// 
			// cmAttendeesCount
			// 
			this.cmAttendeesCount.CommandText = @"
IF EXISTS(SELECT PlaceID FROM Place WHERE PlaceID = @PlaceID)
SELECT COUNT(*) AS Attendees, @PlaceID 
FROM  (SELECT UserID
               FROM   TimeLapse
               WHERE PlaceID = @PlaceID
               GROUP BY UserID) Users
ELSE
SELECT -1";
			this.cmAttendeesCount.CommandTimeout = 29;
			this.cmAttendeesCount.Connection = this.cnFriends;
			this.cmAttendeesCount.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PlaceID", System.Data.SqlDbType.Variant));
			// 
			// cmContacts
			// 
			this.cmContacts.CommandText = @"SELECT ContactUser.FirstName, ContactUser.LastName, ContactUser.Email, ContactUser.Notes, ContactUser.IsApproved FROM [User] INNER JOIN (SELECT [User].FirstName, [User].LastName, [User].Email, Contact.Notes, Contact.IsApproved, Contact.DestinationID FROM Contact INNER JOIN [User] ON [User].UserID = Contact.RequestID) ContactUser ON [User].UserID = ContactUser.DestinationID WHERE ([User].Login = @Login) AND ([User].Password = @Password)";
			this.cmContacts.Connection = this.cnFriends;
			this.cmContacts.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Login", System.Data.SqlDbType.VarChar, 15, "Login"));
			this.cmContacts.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Password", System.Data.SqlDbType.VarChar, 15, "Password"));

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		[WebMethod]
		public int GetAttendees(string placeId)
		{
			cnFriends.Open();
			try
			{
				cmAttendeesCount.Parameters["@PlaceID"].Value = placeId;
				int count = (int) cmAttendeesCount.ExecuteScalar();
				if (count == -1)
					throw new SoapException("Invalid Place identifier!",
						SoapException.ClientFaultCode, Context.Request.Url.AbsoluteUri);

				return count;
			}
			finally 
			{
				cnFriends.Close();
			}
		}

		[WebMethod(CacheDuration=600)]
		public XmlDocument GetContactRequests(string login, string password)
		{
			cnFriends.Open();
			try
			{
				cmContacts.Parameters["@Login"].Value = login;
				cmContacts.Parameters["@Password"].Value = password;

				DataSet contacts = new DataSet("Contacts");
				SqlDataAdapter ad = new SqlDataAdapter(cmContacts);
				ad.Fill(contacts, "Contact");
				return new XmlDataDocument(contacts);
			}
			finally 
			{
				cnFriends.Close();
			}
		}

		[WebMethod(CacheDuration=600)]
		[return:System.Xml.Serialization.XmlRoot("Contacts")]
		public Contact[] GetContactRequestsCustom(string login, string password)
		{
			cnFriends.Open();
			try
			{
				cmContacts.Parameters["@Login"].Value = login;
				cmContacts.Parameters["@Password"].Value = password;

				SqlDataReader reader = cmContacts.ExecuteReader();

				ArrayList contacts = new ArrayList();
				while (reader.Read())
				{
					Contact ct = new Contact();
					ct.FirstName = (string)reader["FirstName"];
					ct.LastName = (string)reader["LastName"];
					ct.Email = (string)reader["Email"];
					ct.Notes = (string)reader["Notes"];
					ct.IsApproved = (bool) reader["IsApproved"];
					contacts.Add(ct);
				}
				
				return (Contact[]) 
					contacts.ToArray(typeof(Contact));
			}
			finally 
			{
				cnFriends.Close();
			}
		}

		public class Contact
		{
			[System.Xml.Serialization.XmlAttribute()]
			public string FirstName;
			[System.Xml.Serialization.XmlAttribute()]
			public string LastName;
			[System.Xml.Serialization.XmlAttribute()]
			public string Email;
			[System.Xml.Serialization.XmlAttribute()]
			public string Notes;
			[System.Xml.Serialization.XmlAttribute()]
			public bool IsApproved;
		}
		
		public class Contacts : CollectionBase
		{
			public int Add(Contact contact)
			{
				return base.InnerList.Add(contact);
			}

			public Contact this[int index]
			{
				get { return (Contact) base.InnerList[index]; }
				set { base.InnerList[index] = value; }
			}
		}
	}
}
