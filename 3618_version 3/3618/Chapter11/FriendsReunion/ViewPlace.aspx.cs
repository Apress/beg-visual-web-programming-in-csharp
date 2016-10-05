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
	/// Summary description for ViewPlace.
	/// </summary>
	public class ViewPlace : FriendsBase
	{
		protected System.Data.SqlClient.SqlDataAdapter adPlaces;
		protected System.Data.SqlClient.SqlCommand cmSelect;
		protected System.Data.SqlClient.SqlCommand cmUpdate;
		protected System.Data.SqlClient.SqlConnection cnFriends;
		protected System.Web.UI.WebControls.DataList dlPlaces;
		protected FriendsReunion.PlaceData dsPlaces;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
				BindPlaces();
		}

		private void BindPlaces()
		{
			adPlaces.Fill(dsPlaces);
			if (dsPlaces.Place.Rows.Count == 0)
				dlPlaces.Visible = false;
			else 
				dlPlaces.DataBind();
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
			this.adPlaces = new System.Data.SqlClient.SqlDataAdapter();
			this.cmSelect = new System.Data.SqlClient.SqlCommand();
			this.cnFriends = new System.Data.SqlClient.SqlConnection();
			this.cmUpdate = new System.Data.SqlClient.SqlCommand();
			this.dsPlaces = new FriendsReunion.PlaceData();
			((System.ComponentModel.ISupportInitialize)(this.dsPlaces)).BeginInit();
			this.dlPlaces.CancelCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlPlaces_CancelCommand);
			this.dlPlaces.EditCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlPlaces_EditCommand);
			this.dlPlaces.UpdateCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlPlaces_UpdateCommand);
			this.dlPlaces.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlPlaces_ItemDataBound);
			this.dlPlaces.SelectedIndexChanged += new System.EventHandler(this.dlPlaces_SelectedIndexChanged);
			// 
			// adPlaces
			// 
			this.adPlaces.SelectCommand = this.cmSelect;
			this.adPlaces.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							   new System.Data.Common.DataTableMapping("Table", "Place", new System.Data.Common.DataColumnMapping[] {
																																																		new System.Data.Common.DataColumnMapping("PlaceID", "PlaceID"),
																																																		new System.Data.Common.DataColumnMapping("TypeID", "TypeID"),
																																																		new System.Data.Common.DataColumnMapping("Name", "Name"),
																																																		new System.Data.Common.DataColumnMapping("Address", "Address"),
																																																		new System.Data.Common.DataColumnMapping("Notes", "Notes"),
																																																		new System.Data.Common.DataColumnMapping("AdministratorID", "AdministratorID")})});
			this.adPlaces.UpdateCommand = this.cmUpdate;
			// 
			// cmSelect
			// 
			this.cmSelect.CommandText = "SELECT Place.PlaceID, Place.TypeID, Place.Name, Place.Address, Place.Notes, Place" +
				".AdministratorID, PlaceType.Name AS TypeName FROM Place INNER JOIN PlaceType ON " +
				"Place.TypeID = PlaceType.TypeID";
			this.cmSelect.Connection = this.cnFriends;
			// 
			// cnFriends
			// 
			this.cnFriends.ConnectionString = ((string)(configurationAppSettings.GetValue("cnFriends.ConnectionString", typeof(string))));
			// 
			// cmUpdate
			// 
			this.cmUpdate.CommandText = "UPDATE Place SET PlaceID = @PlaceID, TypeID = @TypeID, Name = @Name, Address = @A" +
				"ddress, Notes = @Notes, AdministratorID = @AdministratorID WHERE (PlaceID = @Ori" +
				"ginal_PlaceID)";
			this.cmUpdate.Connection = this.cnFriends;
			this.cmUpdate.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PlaceID", System.Data.SqlDbType.VarChar, 36, "PlaceID"));
			this.cmUpdate.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TypeID", System.Data.SqlDbType.VarChar, 36, "TypeID"));
			this.cmUpdate.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 30, "Name"));
			this.cmUpdate.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address", System.Data.SqlDbType.VarChar, 300, "Address"));
			this.cmUpdate.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Notes", System.Data.SqlDbType.VarChar, 300, "Notes"));
			this.cmUpdate.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AdministratorID", System.Data.SqlDbType.VarChar, 36, "AdministratorID"));
			this.cmUpdate.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Original_PlaceID", System.Data.SqlDbType.VarChar, 36, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "PlaceID", System.Data.DataRowVersion.Original, null));
			// 
			// dsPlaces
			// 
			this.dsPlaces.DataSetName = "PlaceData";
			this.dsPlaces.Locale = new System.Globalization.CultureInfo("en-US");
			this.Load += new System.EventHandler(this.Page_Load);
			((System.ComponentModel.ISupportInitialize)(this.dsPlaces)).EndInit();

		}
		#endregion

		private void dlPlaces_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// Remove the edit index just in case we were editing
			dlPlaces.EditItemIndex = -1;
			BindPlaces();		
		}

		private void dlPlaces_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			// Is the item selected?
			if (e.Item.ItemType == ListItemType.SelectedItem) 
			{
				// Locate the hidden Label containing the AdministratorID
				Label admin = (Label) e.Item.FindControl("lblAdministratorID");
				// If it matches the current user, show the Edit button
				if (admin.Text == Page.User.Identity.Name)
					e.Item.FindControl("cmdEdit").Visible = true; 
			}		
		}

		private void dlPlaces_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			// Save the edit index
			dlPlaces.EditItemIndex = e.Item.ItemIndex;
			BindPlaces();		
		}

		private void dlPlaces_CancelCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			// Reset the edit index
			dlPlaces.EditItemIndex = -1;

			// Set the selected item to the currently editing item
			dlPlaces.SelectedIndex = e.Item.ItemIndex;
			BindPlaces();		
		}

		private void dlPlaces_UpdateCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			// Find the updated controls
			TextBox addr = (TextBox) e.Item.FindControl("txtAddress");
			TextBox notes = (TextBox) e.Item.FindControl("txtNotes");
			Label place = (Label) e.Item.FindControl("lblPlaceID");

			// Reload the dataset and locate the relevant row
			adPlaces.Fill(dsPlaces);
			string sql = "PlaceID = '" + place.Text + "'";
			PlaceData.PlaceRow row = (PlaceData.PlaceRow)
				dsPlaces.Place.Select(sql)[0];

			// Set the values using the typed properties
			row.Address = addr.Text;
			row.Notes = notes.Text;

			// Update the row in the database
			adPlaces.Update(new DataRow[] {row});

			// Reset datalist state and bind
			dlPlaces.EditItemIndex = -1;
			dlPlaces.SelectedIndex = e.Item.ItemIndex;
			dlPlaces.DataBind();		
		}
	}
}
