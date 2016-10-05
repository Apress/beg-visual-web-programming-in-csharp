using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Security;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion.Secure
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	public class Login : FriendsBase
	{
		protected System.Web.UI.HtmlControls.HtmlInputText txtLogin;
		protected System.Web.UI.HtmlControls.HtmlInputText txtPwd;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnLogin;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.CheckBox chkPersist;
		protected System.Web.UI.WebControls.Panel pnlError;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.HeaderIconImageUrl = "~/Images/securekeys.gif";
			base.HeaderMessage = "Login Page";
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
			this.btnLogin.ServerClick += new System.EventHandler(this.btnLogin_ServerClick);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnLogin_ServerClick(object sender, System.EventArgs e)
		{
			SqlConnection con = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlCommand cmd = new SqlCommand( 
				@"SELECT UserID FROM [User] 
				WHERE Login=@Login and Password=@Pwd", 
				con );

			// Add parameters for the values provided.
			cmd.Parameters.Add( "@Login", txtLogin.Value );
			cmd.Parameters.Add( "@Pwd", txtPwd.Value );
			con.Open();

			string id = null;

			try
			{
				// Retrieve the UserID
				id = (string) cmd.ExecuteScalar();
			}
			finally 
			{
				con.Close();
			}

			if (id != null)
			{
				// Set the user as authenticated and send him to the
				// page originally requested.
				FormsAuthentication.RedirectFromLoginPage(id, chkPersist.Checked);
			}
			else
			{
				this.pnlError.Visible = true;
				this.lblError.Text = "Invalid user name or password!";
			}
		}
	}
}
