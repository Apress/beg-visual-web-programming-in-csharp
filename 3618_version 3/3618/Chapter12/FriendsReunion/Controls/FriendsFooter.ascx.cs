namespace FriendsReunion.Controls
{
	using System;
	using System.ComponentModel;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for FriendsFooter.
	/// </summary>
	public class FriendsFooter : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label lblCounter;
		protected System.Web.UI.WebControls.Image imgShow;
		protected System.Web.UI.WebControls.DropDownList cbBackColor;
		protected System.Web.UI.WebControls.Panel pnlFooterGlobal;

		private void Page_Load(object sender, System.EventArgs e)
		{
			lblCounter.Text = Application["counter"].ToString();

			// Script to show/hide the options and change the image.
			string script = @"
				var table=document.getElementById('tbPrefs');
				if (table.style.display=='block') 
				{
					this.src='%down%';
					table.style.display='none';
				}
				else
				{
					this.src='%up%';
					table.style.display='block';
				}";

			// Resolve images relative to the current context.
			script = script.Replace("%down%", 
				ResolveUrl("../Images/down.gif"));
			script = script.Replace("%up%", 
				ResolveUrl("../Images/up.gif"));

			imgShow.Attributes.Add("onclick", script);

			imgShow.Style.Add("cursor", "pointer");
			if (!Page.IsPostBack)
			{
				// Empty item to clear color preference
				cbBackColor.Items.Add(String.Empty);
				ColorConverter cv = new ColorConverter();

				// Retrieve current color preference to preselect the item
				Color selected = Color.Empty;
				if (Request.Cookies["backcolor"] != null && 
					Request.Cookies["backcolor"].Value != null &&
					Request.Cookies["backcolor"].Value != String.Empty)
				{
					selected = (Color)
						cv.ConvertFromString(Request.Cookies["backcolor"].Value);
				}

				// Get all standard colors.
				ICollection col = cv.GetStandardValues();
				foreach (Color c in col)
				{
					// Convert each color to its HTML equivalent.
					ListItem li = new ListItem(c.Name,
						ColorTranslator.ToHtml(c));
					if (c.Equals(selected)) li.Selected = true;
					cbBackColor.Items.Add(li);
				}
			}
		}

		private void cbBackColor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Response.Cookies.Add(new HttpCookie("backcolor", 
				((DropDownList)sender).SelectedItem.Value));		
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
			this.cbBackColor.SelectedIndexChanged += new System.EventHandler(this.cbBackColor_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
