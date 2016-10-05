using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for Search.
	/// </summary>
	public class Search : FriendsBase
	{
		protected System.Web.UI.WebControls.Label lblLimit;
		protected System.Web.UI.WebControls.DataGrid grdResults;
		protected System.Web.UI.WebControls.Panel pnlResults;
		protected System.Web.UI.WebControls.TextBox txtLastName;
		protected System.Web.UI.WebControls.TextBox txtFirstName;
		protected System.Web.UI.WebControls.DropDownList cbPlace;
		protected System.Web.UI.WebControls.DropDownList cbType;
		protected System.Web.UI.WebControls.TextBox txtYearIn;
		protected System.Web.UI.WebControls.TextBox txtYearOut;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.WebControls.Button btnSearchResults;
		protected System.Data.SqlClient.SqlConnection cnFriends;
		protected System.Data.SqlClient.SqlCommand cmPlace;
		protected System.Data.SqlClient.SqlCommand cmType;
		protected System.Data.DataSet dsResults;
		protected System.Web.UI.WebControls.Panel pnlActions ;
		protected System.Web.UI.WebControls.ImageButton btnClearResults;
		protected System.Web.UI.WebControls.Label lblSelected;
		protected System.Web.UI.WebControls.ImageButton btnClearSelection;
		protected System.Web.UI.WebControls.ImageButton btnRequest;
		protected System.Web.UI.WebControls.Panel pnlSearch;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Configure the icon and message
			base.HeaderIconImageUrl = "~/Images/search.gif";
			base.HeaderMessage = "Search Users";

			cnFriends.Open();
      
			if (!Page.IsPostBack)
			{
				// Initialize comboboxes
				try
				{
					using (SqlDataReader r = cmPlace.ExecuteReader())
					{
						cbPlace.DataSource = r;
						cbPlace.DataBind();
						cbPlace.Items.Add(new ListItem("-- Not selected --", "0"));
						cbPlace.SelectedIndex = cbPlace.Items.Count - 1;
					}

					using (SqlDataReader r = cmType.ExecuteReader())
					{
						cbType.DataSource = r;
						cbType.DataBind();
						cbType.Items.Add(new ListItem("-- Not selected --", "0"));
						cbType.SelectedIndex = cbType.Items.Count - 1;
					}
				}
				finally
				{
					cnFriends.Close();  // Ensure connection is closed
				}
			}

			SetResultsState(Session["search"] != null);
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
			System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
			this.cnFriends = new System.Data.SqlClient.SqlConnection();
			this.cmPlace = new System.Data.SqlClient.SqlCommand();
			this.cmType = new System.Data.SqlClient.SqlCommand();
			this.dsResults = new System.Data.DataSet();
			((System.ComponentModel.ISupportInitialize)(this.dsResults)).BeginInit();
			this.grdResults.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdResults_ItemCommand);
			this.grdResults.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdResults_ItemDataBound);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnSearchResults.Click += new System.EventHandler(this.btnSearchResults_Click);
			this.btnClearResults.Click += new System.Web.UI.ImageClickEventHandler(this.btnClearResults_Click);
			this.btnClearSelection.Click += new System.Web.UI.ImageClickEventHandler(this.btnClearSelection_Click);
			this.btnRequest.Click += new System.Web.UI.ImageClickEventHandler(this.btnRequest_Click);
			// 
			// cnFriends
			// 
			this.cnFriends.ConnectionString = ((string)(configurationAppSettings.GetValue("cnFriends.ConnectionString", typeof(string))));
			// 
			// cmPlace
			// 
			this.cmPlace.CommandText = "SELECT PlaceID, Name FROM Place ORDER BY Name";
			this.cmPlace.Connection = this.cnFriends;
			// 
			// cmType
			// 
			this.cmType.CommandText = "SELECT TypeID, Name FROM PlaceType ORDER BY Name";
			this.cmType.Connection = this.cnFriends;
			// 
			// dsResults
			// 
			this.dsResults.DataSetName = "NewDataSet";
			this.dsResults.Locale = new System.Globalization.CultureInfo("en-US");
			this.Load += new System.EventHandler(this.Page_Load);
			((System.ComponentModel.ISupportInitialize)(this.dsResults)).EndInit();

		}
		#endregion

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			int limit = Convert.ToInt32(
				ConfigurationSettings.AppSettings["searchLimit"]);

			// Limit maximum resultset size
			string sql = "SELECT TOP " + limit + @"
					[User].UserID, [User].FirstName, [User].LastName, 
					Place.PlaceID, Place.Name AS PlaceName, 
					PlaceType.Name AS PlaceType, PlaceType.TypeID,
					TimeLapse.Name AS LapseName, TimeLapse.YearIn, 
					TimeLapse.MonthIn, TimeLapse.YearOut,
					TimeLapse.MonthOut 
				FROM [User] 
					LEFT OUTER JOIN TimeLapse ON 
						TimeLapse.UserID = [User].UserID
					LEFT OUTER JOIN Place ON
						Place.PlaceID = TimeLapse.PlaceID 
					LEFT OUTER JOIN PlaceType ON
						Place.TypeID = PlaceType.TypeID ";

			// Build the WHERE clause and accumulate parameters values now
			Hashtable values = new Hashtable();
			StringBuilder qry = new StringBuilder();
			if (txtFirstName.Text != String.Empty)
			{
				qry.Append("[User].FirstName LIKE @FName AND ");
				values.Add("@FName", "%" + txtFirstName.Text + "%");
			}
			if (txtLastName.Text != String.Empty)
			{
				qry.Append("[User].LastName LIKE @LName AND ");
				values.Add("@LName", "%" + txtLastName.Text + "%");
			}
			// All other values can take advantage of ADO.NET parameters.
			if (cbPlace.SelectedValue != "0")
			{
				qry.Append("[Place].PlaceID = @PlaceID AND ");
				values.Add("@PlaceID", cbPlace.SelectedValue);
			}
			if (cbType.SelectedValue != "0")
			{
				qry.Append("[PlaceType].TypeID = @TypeID AND ");
				values.Add("@TypeID", cbType.SelectedValue);
			}
			if (txtYearIn.Text != String.Empty)
			{
				qry.Append("TimeLapse.YearIn = @YearIN AND ");
				values.Add("@YearIN", txtYearIn.Text);
			}
			if (txtYearOut.Text != String.Empty)
			{
				qry.Append("TimeLapse.YearOut = @YearOut AND ");
				values.Add("@YearOut", txtYearOut.Text);
			}

			string filter = qry.ToString();
			if (filter.Length != 0) 
			{
				// Add the filter without the trailing AND
				sql += " WHERE " + filter.Remove(filter.Length - 4, 4);
			}

			SqlDataAdapter ad = new SqlDataAdapter(sql, cnFriends);
			// Now add all parameters to the select command.
			foreach (DictionaryEntry prm in values)
			{
				ad.SelectCommand.Parameters.Add( 
					prm.Key.ToString(), prm.Value);
			}

			dsResults = new DataSet();
			ad.Fill(dsResults, "User");

			// Adjust label for results
			if (dsResults.Tables["User"].Rows.Count < limit)
			{
				lblLimit.Text = "Found " + 
					dsResults.Tables["User"].Rows.Count + 
					" users matching your criteria on initial search.";
			}
			else
			{
				lblLimit.Text = "You're working with the first " + 
					limit + " results.<br/>" + 
					"If you're looking for someone who's not in this list, " +
					"please search again with a more precise search criterion.";
			}

			// Place results in session state
			Session["search"] = dsResults;

			SetResultsState(true);
		}

		private void btnSearchResults_Click(object sender, System.EventArgs e)
		{
			dsResults = Session["search"] as DataSet;

			// If we can't get the previous results then we lost session
			// information (failure), or no previous results were available.
			// Default to normal search.
			if (dsResults == null) btnSearch_Click(sender, e);

			// We can't use parameters as this is a common filter 
			// expression to use with the DataSet.
			StringBuilder qry = new StringBuilder();
			if (txtFirstName.Text != String.Empty)
			{
				qry.Append("FirstName LIKE '%");
				qry.Append(txtFirstName.Text).Append("%' AND ");
			}
			if (txtLastName.Text != String.Empty)
			{
				qry.Append("LastName LIKE '%");
				qry.Append(txtLastName.Text).Append("%' AND ");
			}
			if (cbPlace.SelectedItem.Value != "0")
			{
				qry.Append("PlaceID = '");
				qry.Append(cbPlace.SelectedItem.Value).Append("' AND ");
			}
			if (cbType.SelectedItem.Value != "0")
			{
				qry.Append("TypeID = '");
				qry.Append(cbType.SelectedItem.Value).Append("' AND ");
			}
			if (txtYearIn.Text != String.Empty)
			{
				qry.Append("YearIn = ");
				qry.Append(txtYearIn.Text).Append(" AND ");
			}
			if (txtYearOut.Text != String.Empty)
			{
				qry.Append("YearOut = ");
				qry.Append(txtYearOut.Text).Append(" AND ");
			}

			string filter = qry.ToString();      
			if (filter.Length != 0) 
			{
				// Remove trailing AND
				filter = filter.Remove(filter.Length - 4, 4);
			}

			DataRow[] rows = dsResults.Tables["User"].Select(filter);

			// Rebuild results with new filtered set of rows, maintaining
			// structure
			dsResults = dsResults.Clone();
			foreach (DataRow row in rows)
			{
				dsResults.Tables["User"].ImportRow(row);
			}

			// Place results in session state.
			Session["search"] = dsResults;

			BindFromSession();		
		}

		private void btnClearResults_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Session.Remove("search");
			ViewState.Remove("selected");
			SetResultsState(false);		
		}

		private void BindFromSession()
		{
			dsResults = (DataSet) Session["search"];
			grdResults.DataBind();
		}

		private void SetResultsState(bool visible)
		{
			pnlActions.Visible = visible;
			pnlResults.Visible = visible;
			btnSearchResults.Visible = visible;
			btnSearch.Text = visible ? "New search" : "Search";
      
			// If setting to visible, it's because there are results to bind to
			if (visible) BindFromSession();
		}

		private void grdResults_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (ViewState["selected"] == null) return;

			StringCollection sel = (StringCollection)ViewState["selected"];
			ImageButton img = e.Item.FindControl("imgSel") as ImageButton;
    
			if (img == null) return;
    
			if (sel.Contains(img.CommandArgument)) 
			{
				img.ImageUrl = "Images/ok.gif";
				img.CommandName = "DeselectUser";
				e.Item.ForeColor = Color.Red;
			}		
		}

		private void grdResults_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if (e.CommandName == "SelectUser")
			{
				StringCollection sel = ViewState["selected"] as StringCollection;
				if (sel == null)
				{
					sel = new StringCollection();
					ViewState["selected"] = sel;
				}

				if (!sel.Contains((string)e.CommandArgument))
					sel.Add((string)e.CommandArgument);

				BindFromSession();
			}
			else if (e.CommandName == "DeselectUser")
			{
				StringCollection sel = ViewState["selected"] as StringCollection;
				sel.Remove((string)e.CommandArgument);

				BindFromSession();
			}		
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (ViewState["selected"] != null)
			{
				lblSelected.Text = 
					((StringCollection)ViewState["selected"]).Count.ToString() +
					" users selected.";
			}
			base.OnPreRender (e);
		}

		private void btnClearSelection_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			ViewState.Remove("selected");
			BindFromSession();
		}

		private void btnRequest_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Context.Items["selected"] = ViewState["selected"];
			Server.Transfer("RequestContact.aspx");		
		}
	}
}
