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
	/// Summary description for NewUser.
	/// </summary>
	public class NewUser : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox txtLogin;
		protected System.Web.UI.WebControls.TextBox txtPwd;
		protected System.Web.UI.WebControls.TextBox txtFName;
		protected System.Web.UI.WebControls.TextBox txtLName;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtPhone;
		protected System.Web.UI.WebControls.TextBox txtMobile;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtBirth;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqLogin;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqPwd;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqFName;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqLName;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqPhone;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqEmail;
		protected System.Web.UI.WebControls.CompareValidator compBirth;
		protected System.Web.UI.WebControls.RegularExpressionValidator regPhone;
		protected System.Web.UI.WebControls.RegularExpressionValidator regEmail;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.ValidationSummary valErrors;
		protected System.Web.UI.WebControls.Button btnAccept;
	
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
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnAccept_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
				lblMessage.Text = "Validation succeeded!";
			else
				lblMessage.Text = "Fix the following errors and retry:";		
		}
	}
}
