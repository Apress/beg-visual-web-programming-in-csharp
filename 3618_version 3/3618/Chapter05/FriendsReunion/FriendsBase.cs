using System;
using System.IO;
using System.Web.UI;
using FriendsReunion.Controls;

namespace FriendsReunion
{
	public class FriendsBase : System.Web.UI.Page
	{
		protected string HeaderMessage = String.Empty;
		protected string HeaderIconImageUrl = String.Empty;

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			// Get a reference to the form control
			Control form = Page.Controls[1];

			// Create and place the page header
			FriendsHeader header;
			header = (FriendsHeader) this.LoadControl("~/Controls/FriendsHeader.ascx");

			header.Message = HeaderMessage;
			header.IconImageUrl = HeaderIconImageUrl;
			form.Controls.AddAt(0, header);

			// Add the SubHeader custom control
			form.Controls.AddAt(1, new SubHeader());

			// Add space separating the main content.
			form.Controls.AddAt(2, new LiteralControl("<p/>"));
			form.Controls.AddAt(form.Controls.Count, new LiteralControl("<p/>"));

			// Finally, add the page footer
			FriendsFooter footer;
			footer = (FriendsFooter) this.LoadControl("~/Controls/FriendsFooter.ascx");
			form.Controls.AddAt(form.Controls.Count, footer);

			// Render as usual
			base.Render(writer);
		}
	}
}