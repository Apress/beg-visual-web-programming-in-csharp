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

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for Components.
	/// </summary>
	public class Components : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Data.SqlClient.SqlDataAdapter sqlDataAdapter1;
		protected DataSet dsUser;
		protected System.Data.SqlClient.SqlCommand sqlCommand1;
		protected System.Data.SqlClient.SqlCommand sqlSelectCommand1;
		protected System.Data.SqlClient.SqlConnection cnFriends;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			sqlDataAdapter1.Fill(dsUser);
			DataBind();
			// Put user code to initialize the page here
		}

		protected string GetValue()
		{
			//return String.Format(dsUser.Tables[0].Rows[0]["DateOfBirth"].ToString(), "{0:MMMM dd, yyyy}");
			return ((DateTime)dsUser.Tables[0].Rows[0]["DateOfBirth"]).ToLongDateString();
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
			this.sqlDataAdapter1 = new System.Data.SqlClient.SqlDataAdapter();
			this.dsUser = new System.Data.DataSet();
			this.sqlCommand1 = new System.Data.SqlClient.SqlCommand();
			this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
			((System.ComponentModel.ISupportInitialize)(this.dsUser)).BeginInit();
			// 
			// cnFriends
			// 
			this.cnFriends.ConnectionString = ((string)(configurationAppSettings.GetValue("cnFriends.ConnectionString", typeof(string))));
			// 
			// sqlDataAdapter1
			// 
			this.sqlDataAdapter1.SelectCommand = this.sqlSelectCommand1;
			this.sqlDataAdapter1.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																									  new System.Data.Common.DataTableMapping("Table", "User", new System.Data.Common.DataColumnMapping[] {
																																																			  new System.Data.Common.DataColumnMapping("FirstName", "FirstName"),
																																																			  new System.Data.Common.DataColumnMapping("LastName", "LastName"),
																																																			  new System.Data.Common.DataColumnMapping("Notes", "Notes"),
																																																			  new System.Data.Common.DataColumnMapping("UserID", "UserID")})});
			// 
			// dsUser
			// 
			this.dsUser.DataSetName = "DataSet1";
			this.dsUser.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// sqlCommand1
			// 
			this.sqlCommand1.CommandText = "SELECT COUNT(*) FROM Contact WHERE IsApproved = 0 AND DestinationID = @ID";
			this.sqlCommand1.Connection = this.cnFriends;
			this.sqlCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 36, "DestinationID"));
			// 
			// sqlSelectCommand1
			// 
			this.sqlSelectCommand1.CommandText = "SELECT [User].FirstName, [User].LastName, Contact.Notes, [User].UserID FROM [User" +
				"] INNER JOIN Contact ON [User].UserID = Contact.RequestID WHERE (Contact.Destina" +
				"tionID = @ID) AND (Contact.IsApproved = 0)";
			this.sqlSelectCommand1.Connection = this.cnFriends;
			this.sqlSelectCommand1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 36, "DestinationID"));
			this.Load += new System.EventHandler(this.Page_Load);
			((System.ComponentModel.ISupportInitialize)(this.dsUser)).EndInit();

		}
		#endregion
	}
}
