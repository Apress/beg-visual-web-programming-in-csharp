using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for News.
	/// </summary>
	public class News : FriendsBase
	{
		protected System.Web.UI.WebControls.Panel pnlPending;
		protected System.Web.UI.WebControls.Panel pnlApproved ;
		protected System.Web.UI.WebControls.DataGrid grdApproved;
		protected System.Data.SqlClient.SqlConnection cnFriends;
		protected System.Data.SqlClient.SqlDataAdapter adApproved;
		protected System.Data.SqlClient.SqlCommand cmApproved;
		protected FriendsReunion.ContactsData dsApproved;
		protected System.Web.UI.WebControls.DataGrid grdPending;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Configure the icon and message
			base.HeaderIconImageUrl = "~/Images/winbook.gif";
			base.HeaderMessage = "News Page";

			string sql = 
				@"SELECT 
					[User].FirstName, [User].LastName, Contact.Notes, 
					[User].UserID 
				FROM 
					[User], Contact 
				WHERE 
					DestinationID=@ID AND
					IsApproved=0 AND
					[User].UserID=Contact.RequestID";

			// Create the connection and data adapter
			SqlConnection cnFriends = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlDataAdapter adUser = new SqlDataAdapter(sql, cnFriends);
			adUser.SelectCommand.Parameters.Add("@ID", 
				Page.User.Identity.Name);

			DataSet dsPending = new DataSet();

			// Fill dataset and bind to the datagrid
			adUser.Fill(dsPending, "Pending");
			grdPending.DataSource = dsPending;
			grdPending.DataBind();

			// Fill approved contacts
			adApproved.SelectCommand.Parameters["@ID"].Value =
				Page.User.Identity.Name;
			adApproved.Fill(dsApproved);
			grdApproved.DataBind();

			if (dsPending.Tables[0].Rows.Count == 0)
				pnlPending.Visible = false;
			if (dsApproved.User.Rows.Count == 0)
				pnlApproved.Visible = false;
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
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
			this.cnFriends = new System.Data.SqlClient.SqlConnection();
			this.adApproved = new System.Data.SqlClient.SqlDataAdapter();
			this.cmApproved = new System.Data.SqlClient.SqlCommand();
			this.dsApproved = new FriendsReunion.ContactsData();
			((System.ComponentModel.ISupportInitialize)(this.dsApproved)).BeginInit();
			this.grdApproved.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.grdApproved_PageIndexChanged);
			// 
			// cnFriends
			// 
			this.cnFriends.ConnectionString = ((string)(configurationAppSettings.GetValue("cnFriends.ConnectionString", typeof(string))));
			// 
			// adApproved
			// 
			this.adApproved.SelectCommand = this.cmApproved;
			this.adApproved.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																								 new System.Data.Common.DataTableMapping("Table", "User", new System.Data.Common.DataColumnMapping[] {
																																																		 new System.Data.Common.DataColumnMapping("FirstName", "FirstName"),
																																																		 new System.Data.Common.DataColumnMapping("LastName", "LastName"),
																																																		 new System.Data.Common.DataColumnMapping("PhoneNumber", "PhoneNumber"),
																																																		 new System.Data.Common.DataColumnMapping("Address", "Address"),
																																																		 new System.Data.Common.DataColumnMapping("Email", "Email"),
																																																		 new System.Data.Common.DataColumnMapping("UserID", "UserID")})});
			// 
			// cmApproved
			// 
			this.cmApproved.CommandText = "SELECT [User].FirstName, [User].LastName, [User].PhoneNumber, [User].Address, [Us" +
				"er].Email, [User].UserID FROM [User] INNER JOIN Contact ON [User].UserID = Conta" +
				"ct.RequestID WHERE (Contact.DestinationID = @ID) AND (Contact.IsApproved = 1)";
			this.cmApproved.Connection = this.cnFriends;
			this.cmApproved.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 36, "DestinationID"));
			// 
			// dsApproved
			// 
			this.dsApproved.DataSetName = "ContactsData";
			this.dsApproved.Locale = new System.Globalization.CultureInfo("en-US");
			this.Load += new System.EventHandler(this.Page_Load);
			((System.ComponentModel.ISupportInitialize)(this.dsApproved)).EndInit();

		}
		#endregion

		private void grdApproved_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			// Set the new index
			grdApproved.CurrentPageIndex = e.NewPageIndex;

			// Fill approved contacts
			adApproved.SelectCommand.Parameters["@ID"].Value =
				Page.User.Identity.Name;
			adApproved.Fill(dsApproved);
			grdApproved.DataBind();		
		}
	}
}
