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
	/// Summary description for Login.
	/// </summary>
	public class Login : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlInputText txtLogin;
		protected System.Web.UI.HtmlControls.HtmlInputText txtPwd;
		protected System.Web.UI.HtmlControls.HtmlInputButton btnLogin;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblMessage;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
			this.lblMessage.InnerText =
				"Welcome " + this.txtLogin.Value;
		}
	}
}
