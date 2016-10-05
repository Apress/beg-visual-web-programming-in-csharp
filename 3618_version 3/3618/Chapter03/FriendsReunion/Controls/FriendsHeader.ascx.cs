namespace FriendsReunion.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for FriendsHeader.
	/// </summary>
	public class FriendsHeader : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Panel pnlHeaderGlobal;
		protected System.Web.UI.WebControls.Image imgFriends;
		protected System.Web.UI.WebControls.Panel pnlHeaderLocal;
		protected System.Web.UI.WebControls.Image imgIcon;
		protected System.Web.UI.WebControls.Label lblWelcome;

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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		// Accessor method for the Message property
		public string Message
		{
			get { return _message; }
			set { _message = value; }
		} string _message = String.Empty;

		// Accessor method for the IconImageUrl property
		public string IconImageUrl
		{
			get { return _imageurl; }
			set {_imageurl = value; }
		} string _imageurl = String.Empty;

		// Populate the controls with the property values
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (Message != String.Empty)
				this.lblWelcome.Text = Message;
			if (IconImageUrl != String.Empty)
				this.imgIcon.ImageUrl = IconImageUrl;
			base.Render(writer);
		}
	}
}
