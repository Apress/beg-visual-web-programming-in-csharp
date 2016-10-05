using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for AssignPlaces.
	/// </summary>
	public class AssignPlaces : FriendsBase
	{
		protected System.Web.UI.WebControls.TextBox txtDescription;
		protected System.Web.UI.WebControls.TextBox txtYearIn;
		protected System.Web.UI.WebControls.TextBox txtMonthIn;
		protected System.Web.UI.WebControls.TextBox txtYearOut;
		protected System.Web.UI.WebControls.TextBox txtMonthOut;
		protected System.Web.UI.WebControls.DropDownList cbPlaces;
		protected System.Web.UI.WebControls.TextBox txtNotes;
		protected System.Web.UI.WebControls.PlaceHolder phPlaces;
		protected System.Web.UI.WebControls.Panel pnlExisting;
		protected System.Web.UI.WebControls.Button btnAdd;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.HeaderMessage = "Assign Places";

			LoadDataSet();
			InitPlaces();
			InitForm();
		}

		DataSet ds;

		private void LoadDataSet()
		{
			SqlConnection con = new SqlConnection(
				"data source=.;initial catalog=FriendsData;" + 
				"user id=sa;pwd=apress");

			// Select the place's timelapse records, descriptions, and type
			string sql = @"SELECT 
						TimeLapse.*, Place.Name AS Place, 
						PlaceType.Name AS Type 
					FROM 
						TimeLapse, Place, PlaceType 
					WHERE 
						TimeLapse.PlaceID = Place.PlaceID AND 
						Place.TypeID = PlaceType.TypeID AND 
						TimeLapse.UserID = '" + 
				Context.User.Identity.Name + "' ";

			// Initialize the adapters
			SqlDataAdapter adExisting = new SqlDataAdapter(sql, con);
			SqlDataAdapter adPlaces = new SqlDataAdapter(
				"SELECT * FROM Place ORDER BY TypeID", con);
			SqlDataAdapter adPlaceTypes = new SqlDataAdapter(
				"SELECT * FROM PlaceType", con);

			con.Open();
			ds = new DataSet();

			try 
			{
				// Proceed to fill the dataset
				adExisting.Fill(ds, "Existing");
				adPlaces.Fill(ds, "Places");
				adPlaceTypes.Fill(ds, "Types");
			} 
			finally 
			{
				con.Close();
			}
		}

		private void InitPlaces()
		{
			phPlaces.Controls.Clear();
			string msg =
				"Type: {0}, Place: {1}. From {2}/{3} to {4}/{5}. Description: {6}.";

			foreach (DataRow row in ds.Tables["Existing"].Rows)
			{
				LiteralControl lbl = new LiteralControl();

				// Format the msg variable with values in the row
				lbl.Text = string.Format(msg,
					row["Type"], row["Place"],
					row["MonthIn"], row["YearIn"], 
					row["MonthOut"], row["YearOut"], row["Name"]);

				LinkButton btn = new LinkButton();
				btn.Text = "Delete";
				// Pass the LapseID when the link is clicked
				btn.CommandArgument = row["LapseID"].ToString();
				// Attach the handler to the event
				btn.Command += new CommandEventHandler(OnDeletePlace);

				// Add the controls to the placeholder
				phPlaces.Controls.Add(lbl);
				phPlaces.Controls.Add(btn);
				phPlaces.Controls.Add(new LiteralControl("<br>"));
			}
			// Hide the panel if there are no rows
			if (ds.Tables["Existing"].Rows.Count > 0)
				pnlExisting.Visible = true;
			else 
				pnlExisting.Visible = false;
		}

		private void OnDeletePlace(Object sender, CommandEventArgs e)
		{
			// e.CommandArgument receives the LapseID to delete
			SqlConnection con = new SqlConnection(
				"data source=.;initial catalog=FriendsData;" + 
				"user id=sa;pwd=apress");
			SqlCommand cmd = new SqlCommand(
				"DELETE FROM TimeLapse WHERE LapseID='" +
				e.CommandArgument.ToString() + "'", con);
			con.Open();

			try
			{
				cmd.ExecuteNonQuery();
			}
			finally
			{
				con.Close();
			}

			LoadDataSet();
			InitPlaces();
		}

		private void InitForm()
		{
			// Initialize combo box
			if (!Page.IsPostBack)
			{
				// Access the table by index
				foreach (DataRow row in ds.Tables[1].Rows)
				{
					// Find the related row in Types data table
					DataRow[] types = ds.Tables["Types"].Select(
						"TypeID='" + row["TypeID"] + "'");

					// Access row columns by name.
					string text = types[0]["Name"] + ": " + row["Name"];
					// We can access the row's column by index too.
					string value = row[0].ToString();

					cbPlaces.Items.Add(new ListItem(text, value));
				}
			}
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				// Clear existing values
				txtDescription.Text = string.Empty;
				txtMonthIn.Text = string.Empty;
				txtMonthOut.Text = string.Empty;
				txtNotes.Text = string.Empty;
				txtYearIn.Text = string.Empty;
				txtYearOut.Text = string.Empty;
				cbPlaces.Items.Clear();
			}

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
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				ArrayList values = new ArrayList(9);

				string sql = @"INSERT INTO TimeLapse 
						(LapseID, PlaceID, UserID, Name, 
						YearIn, YearOut, MonthIn, MonthOut, Notes) 
						VALUES 
						('{0}', '{1}', '{2}', '{3}', 
						{4}, {5}, {6}, {7}, '{8}')";

				values.Add(Guid.NewGuid().ToString());
				values.Add(cbPlaces.SelectedItem.Value);
				values.Add(Context.User.Identity.Name);
				values.Add(txtDescription.Text);
				values.Add(txtYearIn.Text);
				values.Add(txtYearOut.Text);

				if (txtMonthIn.Text != string.Empty)
					values.Add(txtMonthIn.Text);
				else
					values.Add("Null");

				if (txtMonthOut.Text != string.Empty)
					values.Add(txtMonthOut.Text);
				else 
					values.Add("Null");
			
				if (txtNotes.Text != string.Empty)
					values.Add(txtNotes.Text);
				else 
					values.Add("Null");

				sql = String.Format(sql, values.ToArray());

				// Connect and execute the query
				SqlConnection con = new SqlConnection( 
					"data source=.;initial catalog=FriendsData;" + 
					"user id=sa;pwd=apress");
				SqlCommand cmd = new SqlCommand(sql, con);
				con.Open();
				try 
				{
					cmd.ExecuteNonQuery();
				} 
				finally 
				{
					con.Close();
				}

				LoadDataSet();
				InitPlaces();
			} 
			else 
			{
				throw new InvalidOperationException("Invalid page data.");
			}		
		}
	}
}
