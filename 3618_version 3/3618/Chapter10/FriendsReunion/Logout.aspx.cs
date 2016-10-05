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

namespace FriendsReunion.Secure
{
	/// <summary>
	/// Summary description for Logout.
	/// </summary>
	public class Logout : FriendsBase
	{
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Button btnLogout;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.HeaderMessage = "Leave the Application";
			base.HeaderIconImageUrl = "~/Images/back.gif";
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
			this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnLogout_Click(object sender, System.EventArgs e)
		{
			// Remove the authentication ticket
			System.Web.Security.FormsAuthentication.SignOut();

			// Redirect the user to the root application path
			Response.Redirect(Request.ApplicationPath);		
		}
	}
}
