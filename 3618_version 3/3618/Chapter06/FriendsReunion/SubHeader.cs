using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FriendsReunion
{
	public class SubHeader : WebControl
	{
		public SubHeader()
		{
			// Initialize default values
			this.Width = new Unit(100, UnitType.Percentage);
			this.CssClass = "SubHeader";
		}

		// Property to allow the user to define the URL for the
		// registration page
		public string RegisterUrl
		{
			get { return _register; }
			set { _register = value; }
		} string _register = string.Empty;

		// This method is called when the control is being built
		protected override void CreateChildControls()
		{
			Label lbl;
			HyperLink reg = new HyperLink();
			if (_register == string.Empty)
			{
				reg.NavigateUrl = "~/Secure/NewUser.aspx";
			} 
			else
			{
				reg.NavigateUrl = _register;
			}

			if (Context.User.Identity.IsAuthenticated)
				reg.Text = "Edit my profile";
			else 
				reg.Text = "Register";

			this.Controls.AddAt(0, reg);

			// Add a couple of blank spaces and a separator character
			this.Controls.Add(new LiteralControl("&nbsp;-&nbsp;"));

			// Add a label with the current data
			lbl = new Label();
			lbl.Text = DateTime.Now.ToLongDateString();
			this.Controls.Add(lbl);
		}
	}
}
