using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for RequestContact.
	/// </summary>
	public class RequestContact : FriendsBase
	{
		protected System.Web.UI.WebControls.TextBox txtMessage;
		protected System.Web.UI.WebControls.Button btnSend;
		protected System.Web.UI.WebControls.ListBox lstUsers;
		protected System.Data.SqlClient.SqlConnection cnFriends;
		protected System.Data.SqlClient.SqlCommand cmInsert;
		protected System.Web.UI.WebControls.Label lblSuccess;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.HeaderMessage = "Contact your buddies!";
			base.HeaderIconImageUrl = "~/Images/contact.gif";

			// Initialize the list of users only once
			if (!Page.IsPostBack)
			{
				// Retrieve selection from transient state
				StringCollection sel =
					Context.Items["selected"] as StringCollection;
				// If no selection was made, go back to search...
				if (sel == null || sel.Count == 0) Server.Transfer("Search.aspx");

				StringBuilder sql = new StringBuilder(
					@"SELECT 
						FirstName + ', ' + LastName AS FullName, 
						UserID 
					FROM [User]");

				// Build the WHERE clause based on the list received
				sql.Append(" WHERE ");
				foreach (string id in sel)
				{
					sql.Append("UserID = '").Append(id).Append("' OR ");
				}
				// Remove trailing OR
				sql.Remove(sql.Length - 3, 3);
				sql.Append("ORDER BY FirstName, LastName");

				SqlCommand cmd = new SqlCommand(sql.ToString(), cnFriends);
				cnFriends.Open();
				using (SqlDataReader r = cmd.ExecuteReader(CommandBehavior.CloseConnection))
				{
					// Add the items with the corresponding ID
					while (r.Read())
					{
						lstUsers.Items.Add(new ListItem(
							r[0].ToString(),
							r[1].ToString()));
					}	
				}
			}
		}

		private void btnSend_Click(object sender, System.EventArgs e)
		{
			// These parameters are fixed for each selected user.
			cmInsert.Parameters["@RequestID"].Value = 
				Page.User.Identity.Name;
			cmInsert.Parameters["@IsApproved"].Value = false;
			cmInsert.Parameters["@Message"].Value = 
				txtMessage.Text;

			try
			{
				cnFriends.Open();

				foreach (ListItem it in lstUsers.Items)
				{
					cmInsert.Parameters["@DestinationID"].Value =
						it.Value;
					cmInsert.ExecuteNonQuery();
				}

				lblSuccess.Text = "Message successfully sent!";
			}		
			finally
			{
				// Always close the connection.
				cnFriends.Close();
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
			this.cnFriends = new System.Data.SqlClient.SqlConnection();
			this.cmInsert = new System.Data.SqlClient.SqlCommand();
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// cnFriends
			// 
			this.cnFriends.ConnectionString = ((string)(configurationAppSettings.GetValue("cnFriends.ConnectionString", typeof(string))));
			// 
			// cmInsert
			// 
			this.cmInsert.CommandText = "INSERT INTO Contact (RequestID, IsApproved, Notes, DestinationID) VALUES (@Reques" +
				"tID, @IsApproved, @Message, @DestinationID)";
			this.cmInsert.Connection = this.cnFriends;
			this.cmInsert.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RequestID", System.Data.SqlDbType.VarChar, 36, "RequestID"));
			this.cmInsert.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IsApproved", System.Data.SqlDbType.Bit, 1, "IsApproved"));
			this.cmInsert.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Message", System.Data.SqlDbType.VarChar, 300, "Notes"));
			this.cmInsert.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DestinationID", System.Data.SqlDbType.VarChar, 36, "DestinationID"));
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
