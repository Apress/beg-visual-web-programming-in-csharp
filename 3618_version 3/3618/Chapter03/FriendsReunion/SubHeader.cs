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

			// If the user is authenticated, we will render their name
			if (Context.User.Identity.IsAuthenticated)
			{
				lbl = new Label();
				lbl.Text = Context.User.Identity.Name;

				// Add the newly created label to our collection of child controls
				this.Controls.Add(lbl);
			}
			else
			{
				// Otherwise, we will render a link to the registration page
				HyperLink reg = new HyperLink();
				reg.Text = "Register";

				// If a URL isn't provided, use a default URL to the
				// registration page
				if (_register == string.Empty)
				{
					reg.NavigateUrl = Context.Request.ApplicationPath +
						Path.AltDirectorySeparatorChar + "Secure" +
						Path.AltDirectorySeparatorChar + "NewUser.aspx";
				}
				else
				{
					reg.NavigateUrl = _register;
				}

				// Add the newly created link to our collection of child controls
				this.Controls.Add(reg);
			}

			// Add a couple of blank spaces and a separator character
			this.Controls.Add(new LiteralControl("&nbsp;-&nbsp;"));

			// Add a label with the current data
			lbl = new Label();
			lbl.Text = DateTime.Now.ToLongDateString();
			this.Controls.Add(lbl);
		}
	}
}
