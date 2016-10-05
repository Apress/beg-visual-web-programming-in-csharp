using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Services.Protocols;

namespace Acudei
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public class _Default : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtLogin;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.DataGrid grdContacts;
		protected System.Web.UI.WebControls.Button btnRefresh;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label txtCount;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			FriendsService.Partners friends = new Acudei.FriendsService.Partners();
			try
			{
				int count = friends.GetAttendees(
					ConfigurationSettings.AppSettings["PlaceID"]);
				this.txtCount.Text = count.ToString();
			}
			catch (SoapException se)
			{
				lblError.Text = String.Format(@"
					<h2>An error happened connecting to Friends Reunion service.</h2>
					<h3>Service location: {0}</h3>
					Error: <br/>{1}", 
					se.Actor, se.Message.Substring(45, se.Message.IndexOf("\n") - 45));
				lblError.Visible = true;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{
			FriendsService.Partners friends = new Acudei.FriendsService.Partners();

			// Using generic Dataset returned.
//			DataSet ds = friends.EndGetContactRequests(
//				this.txtLogin.Text, 
//				this.txtPassword.Text);

			// Using XmlDocument return type.
//			// We now retrieve a bare Xml representation.
//			XmlNode contacts = friends.GetContactRequests(
//				this.txtLogin.Text, 
//				this.txtPassword.Text);
//
//			DataSet ds = new DataSet();
//			// Read from the node.
//			ds.ReadXml(new XmlNodeReader(contacts));

			// Using custom classes.
			FriendsService.Contact[] contacts = friends.GetContactRequestsCustom(
				this.txtLogin.Text, 
				this.txtPassword.Text);

			this.grdContacts.DataSource = contacts;
			this.grdContacts.DataBind();
		}

		public class Contacts : CollectionBase
		{
			public Contacts(FriendsService.Contact[] contacts)
			{
				base.InnerList.AddRange(contacts);
			}

			public int Add(FriendsService.Contact contact)
			{
				return base.InnerList.Add(contact);
			}

			public FriendsService.Contact this[int index]
			{
				get { return (FriendsService.Contact) base.InnerList[index]; }
				set { base.InnerList[index] = value; }
			}

			#region IListSource Members

			public IList GetList()
			{
				return this;
			}

			public bool ContainsListCollection
			{
				get
				{
					return true;
				}
			}

			#endregion
		}
	}
}
