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
	/// Summary description for ViewUser.
	/// </summary>
	public class ViewUser : FriendsBase
	{
		protected System.Web.UI.WebControls.Label lblName;
		protected System.Web.UI.WebControls.Label lblBirth;
		protected System.Web.UI.WebControls.Label lblPhone;
		protected System.Web.UI.WebControls.Label lblMobile;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.HyperLink lnkEmail;
		protected System.Web.UI.WebControls.Button btnAuthorize;
		protected System.Web.UI.WebControls.Label lblPending;
	
		protected DataSet dsUser;

		private void Page_Load(object sender, System.EventArgs e)
		{
			string userID = Request.QueryString["RequestID"];

			// Ensure we received an ID
			if (userID == null)
			{
				userID = Request.QueryString["UserID"];
				if (userID == null)
				{
					throw new ArgumentException(
						"This page expects either a RequestID or a UserID parameter.");
				}
				else 
				{
					btnAuthorize.Visible = false;
				}
			}

			// Create the connection and data adapter
			SqlConnection cnFriends = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlDataAdapter adUser = new SqlDataAdapter(
				"SELECT * FROM [User] WHERE UserID=@ID", cnFriends);
			adUser.SelectCommand.Parameters.Add("@ID", userID);

			// Initialize the dataset and fill it with data
			dsUser = new DataSet();
			adUser.Fill(dsUser, "User");

			// Finally, bind all the controls on the page
			this.DataBind();
		}		

		protected string GetPending()
		{
			// Create the connection and command to execute
			SqlConnection cnFriends = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlCommand cmd = new SqlCommand(
				@"SELECT COUNT(*) FROM Contact 
				  WHERE IsApproved=0 AND DestinationID=@ID",
				cnFriends);
			cmd.Parameters.Add("@ID", Page.User.Identity.Name);
			cnFriends.Open();

			try
			{
				return cmd.ExecuteScalar().ToString();
			} 
			finally
			{
				cnFriends.Close();
			}
		}

		private void btnAuthorize_Click(object sender, System.EventArgs e)
		{
			// Create the connection and command to execute
			SqlConnection cnFriends = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlCommand cmd = new SqlCommand(
				@"UPDATE Contact 
						SET IsApproved=1 
					WHERE 
						RequestID=@RequestID AND DestinationID=@DestinationID",
				cnFriends);
			cmd.Parameters.Add("@RequestID", Request.QueryString["RequestID"]);
			cmd.Parameters.Add("@DestinationID", Page.User.Identity.Name);

			cnFriends.Open();

			try
			{
				cmd.ExecuteNonQuery();
			}
			finally
			{
				cnFriends.Close();
			}

			// Return to the news page
			Response.Redirect("News.aspx");		
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
			this.btnAuthorize.Click += new System.EventHandler(this.btnAuthorize_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	}
}
