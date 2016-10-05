using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion.Admin
{
	/// <summary>
	/// Summary description for Users.
	/// </summary>
	public class Users : FriendsBase
	{
		protected System.Data.SqlClient.SqlDataAdapter adUsers;
		protected System.Data.SqlClient.SqlConnection cnFriends;
		protected System.Data.SqlClient.SqlCommand cmUsers;
		protected FriendsReunion.Admin.UserData dsData;
		protected System.Web.UI.WebControls.DataGrid grdUsers;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.HeaderIconImageUrl = "~/Images/padlock.gif";
			base.HeaderMessage = "Administer Users";

			if (!IsPostBack)
			{
				this.adUsers.Fill(this.dsData);
				this.grdUsers.DataBind();
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
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
			this.adUsers = new System.Data.SqlClient.SqlDataAdapter();
			this.cmUsers = new System.Data.SqlClient.SqlCommand();
			this.cnFriends = new System.Data.SqlClient.SqlConnection();
			this.dsData = new FriendsReunion.Admin.UserData();
			((System.ComponentModel.ISupportInitialize)(this.dsData)).BeginInit();
			// 
			// adUsers
			// 
			this.adUsers.SelectCommand = this.cmUsers;
			this.adUsers.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							  new System.Data.Common.DataTableMapping("Table", "User", new System.Data.Common.DataColumnMapping[] {
																																																	  new System.Data.Common.DataColumnMapping("UserID", "UserID"),
																																																	  new System.Data.Common.DataColumnMapping("Login", "Login"),
																																																	  new System.Data.Common.DataColumnMapping("Password", "Password"),
																																																	  new System.Data.Common.DataColumnMapping("FirstName", "FirstName"),
																																																	  new System.Data.Common.DataColumnMapping("LastName", "LastName"),
																																																	  new System.Data.Common.DataColumnMapping("DateOfBirth", "DateOfBirth"),
																																																	  new System.Data.Common.DataColumnMapping("PhoneNumber", "PhoneNumber"),
																																																	  new System.Data.Common.DataColumnMapping("CellNumber", "CellNumber"),
																																																	  new System.Data.Common.DataColumnMapping("Address", "Address"),
																																																	  new System.Data.Common.DataColumnMapping("Email", "Email"),
																																																	  new System.Data.Common.DataColumnMapping("IsAdministrator", "IsAdministrator")})});
			// 
			// cmUsers
			// 
			this.cmUsers.CommandText = "SELECT UserID, Login, Password, FirstName, LastName, DateOfBirth, PhoneNumber, Ce" +
				"llNumber, Address, Email, IsAdministrator FROM [User]";
			this.cmUsers.Connection = this.cnFriends;
			// 
			// cnFriends
			// 
			this.cnFriends.ConnectionString = ((string)(configurationAppSettings.GetValue("cnFriends.ConnectionString", typeof(string))));
			// 
			// dsData
			// 
			this.dsData.DataSetName = "UserData";
			this.dsData.Locale = new System.Globalization.CultureInfo("en-US");
			this.Load += new System.EventHandler(this.Page_Load);
			((System.ComponentModel.ISupportInitialize)(this.dsData)).EndInit();

		}
		#endregion
	}
}
