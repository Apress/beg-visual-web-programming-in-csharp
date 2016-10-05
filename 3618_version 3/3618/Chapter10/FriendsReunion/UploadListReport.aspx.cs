using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for UploadListReport.
	/// </summary>
	public class UploadListReport : FriendsBase
	{
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Image Image2;
		protected System.Web.UI.WebControls.Panel pnlError;
		protected System.Web.UI.WebControls.Table tbReport;
		protected System.Web.UI.WebControls.TextBox txtYearFrom;
		protected System.Web.UI.WebControls.TextBox txtYearTo;
		protected System.Web.UI.WebControls.LinkButton btnExecute;
		protected System.Web.UI.WebControls.LinkButton btnBackLink;
		protected System.Web.UI.WebControls.ImageButton btnBackImg;
		protected System.Web.UI.WebControls.Table tbDates;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Configure header
			base.HeaderIconImageUrl = "~/Images/print.gif";
			base.HeaderMessage = "Upload Attendees - Report";

			string ns = "http://www.apress.com/schemas/friendsreunion";

			try
			{
				// Retrieve the reader object and initialize the DOM document
				XmlReader reader = GetReader();
				XmlDocument doc = new XmlDocument();
				doc.Load(reader);

				// Initialize the namespace manager for the document
				XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
				mgr.AddNamespace("af", ns);

				// List of new users
				XmlNodeList nodes = doc.SelectNodes("/af:Friends/af:User", mgr);
				TableRow row = new TableRow();
				TableCell cell = new TableCell();
				cell.Text = "Users: " + nodes.Count.ToString();
				row.Cells.Add(cell);

				StringBuilder sb = new StringBuilder();
				foreach (XmlNode node in nodes)
				{
					sb.AppendFormat("{0}, {1} ({2})<br/>", 
						node["LastName", ns].InnerText, 
						node["FirstName", ns].InnerText,
						node["Email", ns].InnerText);
				}

				// Add the cell with the accumulated list.
				cell = new TableCell();
				cell.Text = sb.ToString();
				row.Cells.Add(cell);
				this.tbReport.Rows.Add(row);

				// Queries returning XPath intrinsic types.
				XPathNavigator nav = doc.CreateNavigator();

				// Total number of attendees anywhere in the document
				XPathExpression expr = nav.Compile("count(//af:Attended)");
				// Set the manager to resolve namespace.
				expr.SetContext(mgr);
				// Execute expression.
				object count = nav.Evaluate(expr);

				// Build the cell and row that shows the result.
				row = new TableRow();
				cell = new TableCell();
				cell.Text = "Global count of attendees: " + count;
				cell.ColumnSpan = 2;
				row.Cells.Add(cell);
				this.tbReport.Rows.Add(row);

				// The last attendee in the file, in document order
				expr = nav.Compile(
					"string(/af:Friends/af:User[position() = last()]/@ID)");
				expr.SetContext(mgr);
				object last = nav.Evaluate(expr);

				// Build the cell and row that shows the result.
				row = new TableRow();
				cell = new TableCell();
				cell.Text = "Last attendee ID in file: " + last;
				cell.ColumnSpan = 2;
				row.Cells.Add(cell);
				this.tbReport.Rows.Add(row);
			}
			catch (Exception ex)
			{
				this.lblError.Text = ex.Message;
				this.pnlError.Visible = true;
			}

			if (this.tbReport.Rows.Count == 1)
				this.tbReport.Visible = false;
		}

		private XmlReader GetReader()
		{
			if (Session["xml"] == null)
				throw new InvalidOperationException(
					"No Xml file has been uploaded.");

			// Build the XmlReader from the in-memory string saved 
			// by the previous page.
			StringReader xmlinput = new StringReader((string)Session["xml"]);
			return new XmlTextReader(xmlinput);
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
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			this.btnBackLink.Click += new System.EventHandler(this.btnBackLink_Click);
			this.btnBackImg.Click += new System.Web.UI.ImageClickEventHandler(this.btnBackImg_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnBackImg_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect("UploadList.aspx");		
		}

		private void btnBackLink_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("UploadList.aspx");
		}

		private void btnExecute_Click(object sender, System.EventArgs e)
		{
			string ns = "http://www.apress.com/schemas/friendsreunion";

			try
			{
				// Clear any previous state.
				TableRow row = this.tbDates.Rows[0];
				this.tbDates.Rows.Clear();
				this.tbDates.Rows.Add(row);

				// Set up the document.
				XPathDocument doc = new XPathDocument(GetReader());

				// Get the navigator over the document.
				XPathNavigator nav = doc.CreateNavigator();

				// Set up the manager.
				XmlNamespaceManager mgr = new XmlNamespaceManager(nav.NameTable);
				mgr.AddNamespace("af", ns);

				string path = String.Format(
					"/af:Friends/af:User/af:Attended[af:YearIn >= {0} and af:YearOut <= {1}]", 
					this.txtYearFrom.Text, 
					this.txtYearTo.Text);

				XPathExpression expr = nav.Compile(path);
				expr.SetContext(mgr);

				XPathNodeIterator it = nav.Select(expr);

				while (it.MoveNext())
				{
					// Create the empty row and cells.
					row = new TableRow();
					row.Cells.Add(new TableCell());
					row.Cells.Add(new TableCell());
					row.Cells.Add(new TableCell());
					row.Cells.Add(new TableCell());

					// Grab current navigator.
					XPathNavigator attended = it.Current;

					// Get the name attribute.
					row.Cells[0].Text = attended.GetAttribute("Name", String.Empty);
					
					// Iterate children of current Attended element
					attended.MoveToFirstChild();
					do
					{
						// Check current element.
						if (attended.LocalName == "YearIn" && 
							attended.NamespaceURI == ns)
							row.Cells[1].Text = attended.Value;
						else if (attended.LocalName == "YearOut" && 
							attended.NamespaceURI == ns)
							row.Cells[2].Text = attended.Value;

					} while (attended.MoveToNext());

					// We have moved to Attended children. Reposition to Attended node.
					attended.MoveToParent();
					// Get the parent (User) ID attribute.
					attended.MoveToParent();
					row.Cells[3].Text = attended.GetAttribute("ID", String.Empty);
					
					// Finally add the new row.
					this.tbDates.Rows.Add(row);
				}

				this.tbDates.Visible = true;
			}
			catch (Exception ex)
			{
				this.lblError.Text = ex.Message;
				this.pnlError.Visible = true;
			}		
		}
	}
}
