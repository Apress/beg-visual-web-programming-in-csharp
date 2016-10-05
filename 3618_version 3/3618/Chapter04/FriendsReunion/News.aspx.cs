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

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for News.
	/// </summary>
	public class News : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList cbDay;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected FriendsReunion.SubHeader SubHeader1;
		protected System.Web.UI.WebControls.Calendar calDates;
	
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
			this.calDates.SelectionChanged += new System.EventHandler(this.calDates_SelectionChanged);
			this.cbDay.SelectedIndexChanged += new System.EventHandler(this.cbDay_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void cbDay_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			calDates.SelectedDate =
				DateTime.Now.AddDays(Convert.ToDouble(cbDay.SelectedItem.Value));
			calDates.VisibleDate = calDates.SelectedDate;
			lblMessage.Text =
				"Current Date: " + calDates.SelectedDate.ToLongDateString();		
		}

		private void calDates_SelectionChanged(object sender, System.EventArgs e)
		{
			lblMessage.Text =
				"Current Date: " + calDates.SelectedDate.ToLongDateString();		
		}
	}
}
