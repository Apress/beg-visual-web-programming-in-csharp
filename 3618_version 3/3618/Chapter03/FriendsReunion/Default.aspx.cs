using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for _Default.
	/// </summary>
	public class _Default : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.PlaceHolder phNav;
		protected FriendsReunion.SubHeader ccSubHeader;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			Table tb = new Table();
			TableRow row;
			TableCell cell;
			Image img;
			HyperLink lnk;

			if (Context.User.Identity.IsAuthenticated)
			{
				// Create a new blank table row
				row = new TableRow();

				// Set up the News image
				img = new Image();
				img.ImageUrl = "Images/winbook.gif";
				img.ImageAlign = ImageAlign.Middle;
				img.Width = new Unit(24, UnitType.Pixel);
				img.Height = new Unit(24, UnitType.Pixel);

				// Create a cell and add the image
				cell = new TableCell();
				cell.Controls.Add(img);

				// Add the new cell to the row
				row.Cells.Add(cell);

				// Set up the News link
				lnk = new HyperLink();
				lnk.Text = "News";
				lnk.NavigateUrl = "News.aspx";

				// Create the cell and add the link
				cell = new TableCell();
				cell.Controls.Add(lnk);

				// Add the new cell to the row
				row.Cells.Add(cell);

				// Add the row to the table
				tb.Rows.Add(row);
			}
			else
			{
				// Code for unauthenticated users here...
			}
 

			// Finally, add the table to the placeholder
			phNav.Controls.Add(tb);
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
