using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using FriendsReunion.Controls;

namespace FriendsReunion
{
	public class FriendsBase : System.Web.UI.Page
	{
		protected string HeaderMessage = String.Empty;
		protected string HeaderIconImageUrl = String.Empty;

		FriendsFooter _footer;
		FriendsHeader _header;
		SubHeader _subheader;

		protected override void OnInit(EventArgs e)
		{
			_header = (FriendsHeader) LoadControl("~/Controls/FriendsHeader.ascx");
			_footer = (FriendsFooter) LoadControl("~/Controls/FriendsFooter.ascx");

			_subheader = new SubHeader();

			// Add to the Controls hierarchy to get proper
			// event handling, on rendering we reposition them
			Page.Controls.Add(_header);
			Page.Controls.Add(_subheader);
			Page.Controls.Add(_footer);
			base.OnInit(e);
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			// Remove the controls from their current place in the hierarchy
			Page.Controls.Remove(_header);
			Page.Controls.Remove(_subheader);
			Page.Controls.Remove(_footer);

			Debug.Assert(
				Page.Controls[1] is HtmlForm, 
				"Form control not found",
				"Any FriendsReunion page requires that a form tag be " + 
				"the first child of the page body.");

			// Get a reference to the form control
			HtmlForm form = (HtmlForm)Page.Controls[1];

			// Reposition the controls on the page
			form.Controls.AddAt(0, _header );
			form.Controls.AddAt(1, _subheader );
			
			// Add space separating the main content.
			form.Controls.AddAt(2, new LiteralControl("<p/>"));
			form.Controls.AddAt(form.Controls.Count, new LiteralControl("<p/>"));
			
			// Add the footer finally.
			form.Controls.AddAt(form.Controls.Count, _footer );

			//Set current values
			_header.Message = HeaderMessage;
			_header.IconImageUrl = HeaderIconImageUrl;

			#region UI customization
			// New cookies are set to Response by the color selector
			string bg = Response.Cookies["backcolor"].Value;

			// if not, check Request for a previously saved cookie
			if (bg == null && 
				Request.Cookies["backcolor"] != null && 
				Request.Cookies["backcolor"].Value != null &&
				Request.Cookies["backcolor"].Value != String.Empty)
			{
				bg = Request.Cookies["backcolor"].Value;

				// preserve cookie in the response
				Response.Cookies.Add(Request.Cookies["backcolor"]);
			}

			// Do we have a value to work with?
			if (bg != null && bg != String.Empty)
			{
				// Enclose form in a DIV to display the backcolor
				HtmlGenericControl div = new HtmlGenericControl("div");
				div.Style.Add("background-color", bg);

				// Relocate the form inside the DIV
				Page.Controls.Remove(form);
				Page.Controls.AddAt(1, div);
				div.Controls.Add(form);
				Response.Cookies["backcolor"].Expires = DateTime.Now.AddYears(1);
			}
			#endregion

			// Render as usual
			base.Render(writer);
		}

		/// <summary>
		/// Retrieves and handles caching of the places dataset. 
		/// </summary>
		protected DataSet GetPlacesDataSet()
		{
			// If it's already cached, return it.
			DataSet ds = (DataSet) Cache["Places"];
			if ( ds != null ) return ds;

			// Generate the new dataset.
			SqlConnection con = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);

			SqlDataAdapter adPlaces;
			adPlaces = new 
				SqlDataAdapter("SELECT * FROM Place ORDER BY TypeID", con);
			adPlaces.Fill(ds, "Places");

			// Reset the dependency flag.
			Cache["PlacesChanged"] = false;

			// Create a dependency based on the "PlacesChanged" cache key
			string[] dependencyKeys = new string[] { "PlacesChanged" };
			CacheDependency dependency = new CacheDependency(null, dependencyKeys);

			// Insert the dataset into the cache, 
			// with a dependency to the "PlacesChanged" key
			Cache.Insert("Places", ds, dependency);

			return ds;
		}
	}
}