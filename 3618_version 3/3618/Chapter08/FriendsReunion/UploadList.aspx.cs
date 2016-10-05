using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Web.UI.WebControls;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for UploadList.
	/// </summary>
	public class UploadList : FriendsBase
	{
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.HyperLink hySchema;
		protected System.Web.UI.WebControls.Button btnLoad;
		protected Microsoft.Web.UI.WebControls.TreeView tvXmlView;
		protected System.Web.UI.WebControls.Button btnAccept;
		protected System.Web.UI.WebControls.Panel pnlError;
		protected System.Web.UI.WebControls.LinkButton btnDefaultXml;
		protected System.Web.UI.WebControls.HyperLink hyXmlFile;
		protected System.Web.UI.HtmlControls.HtmlInputFile fldUpload;
		protected System.Web.UI.WebControls.Button btnReport;
		protected System.Web.UI.WebControls.Label lblError;
	
		private StringBuilder _errors = new StringBuilder();

		#region Schema initialization

		private XmlSchema SchemaInstance
		{
			get 
			{
				if (_schema == null)
				{
					using (Stream fs = File.OpenRead(Server.MapPath("~/Friends.xsd")))
					{
						_schema = XmlSchema.Read(fs, null);
					}
				}
				return _schema;
			}
		} static XmlSchema _schema;

		#endregion Schema initialization
        
		private void Page_Load(object sender, System.EventArgs e)
		{
			HeaderIconImageUrl = "~/Images/pctransfer.gif";
			HeaderMessage = "Upload Attendees";
		}

		// Save the input file if appropriate
		private void SaveXml()
		{
			if (Request.Files[0].FileName != String.Empty)
			{
				//Save the uploaded stream to Session for further postbacks
				using (StreamReader stm = new StreamReader(Request.Files[0].InputStream))
				{
					Session["xml"] = stm.ReadToEnd();
					Session["file"] = Request.Files[0].FileName;
				}
			}
		}

		private void OnValidation(object sender, ValidationEventArgs e) 
		{
			_errors.AppendFormat("<b>{0}</b>: {1}<br/>", 
				e.Severity.ToString(), e.Message);
		}

		private void btnLoad_Click(object sender, System.EventArgs e)
		{
			BuildTreeView();
		}

		private XmlReader GetReader() 
		{
			if (Session["xml"] == null) 
				throw new InvalidOperationException( 
					"No XML file has been uploaded yet.");

			// Build the XmlTextReader from the in-memory string saved above
			StringReader xmlinput = new StringReader((string)Session["xml"]);
			XmlTextReader reader = new XmlTextReader(xmlinput);

			// Configure the validating reader
			XmlValidatingReader validator = new XmlValidatingReader(reader);
			validator.ValidationEventHandler += 
				new ValidationEventHandler(OnValidation);

			validator.Schemas.Add(SchemaInstance);
			validator.ValidationType = ValidationType.Schema;
			return validator;
		}

		private void BuildTreeView()
		{
			// Will keep the current node and its parents.
			Stack hierarchy = new Stack(5);
			TreeNode node = null;
			XmlReader reader;

			this.pnlError.Visible = false;

			// Save the incoming file if appropriate.
			SaveXml();

			try
			{
				reader = GetReader();

				// Clear the tree view.
				this.tvXmlView.Nodes.Clear();
				
				while (reader.Read())
				{
					// We create new nodes for all elements.
					if (reader.NodeType == XmlNodeType.Element)	
					{
						// Create a new node.
						node = new TreeNode();
						node.Text = reader.LocalName;
						AddAttributes(reader, node);

						// Anchor to its parent.
						if (hierarchy.Count > 0)
							((TreeNode) hierarchy.Peek()).Nodes.Add(node);

						// Set it as the last node in the stack.
						hierarchy.Push(node);
					}
					else if (reader.NodeType == XmlNodeType.Text) 
					{
						// If it's a text node, set the text value of the last node
						node = (TreeNode) hierarchy.Peek();
						node.Text += ": " + reader.Value;
					}
					else if (reader.NodeType == XmlNodeType.EndElement)
					{
						// Remove the element as we're done with it.
						node = (TreeNode) hierarchy.Pop();
					}
				}

				// Last node will be the root one, with the whole 
				// hierarchy properly built. Append the file name to it.
				node.Text += " (" + Session["file"].ToString() + ")";

				this.tvXmlView.Nodes.Add(node);
				this.tvXmlView.Visible = true;

				// Check for errors accumulated during validation.
				string msg = _errors.ToString();
				if (msg != String.Empty)
				{
					pnlError.Visible = true;
					lblError.Text = msg;
					// Remove invalid document from session.
					Session.Remove("xml");
				}
				else
				{
					pnlError.Visible = false;
				}
			}
			catch (Exception ex)
			{
				this.lblError.Text = ex.Message;
				this.pnlError.Visible = true;
				// Remove invalid document from session.
				Session.Remove("xml");
			}
		}


		// Helper method of BuildTreeView that adds the attributes found as
		// child nodes of the passed node, using a different icon
		private void AddAttributes(XmlReader reader, TreeNode node)
		{
			if (!(reader.HasAttributes))
				return;

			TreeNode child;
			TreeNode attrs = new TreeNode();
			// Define the node that will contain all attributes.
			attrs.Text = "Attributes (" + reader.AttributeCount.ToString() + ")";
			attrs.ImageUrl = "Images/attributes.gif";
			attrs.ExpandedImageUrl = "Images/attributes.gif";

			for (int i = 0; i < reader.AttributeCount; i++)
			{
				child = new TreeNode();
				// Move to the appropriate attribute.
				reader.MoveToAttribute(i);
				// Configure the node and add it to the list of attributes.
				child.Text = reader.Name + ": " + reader.Value;
				child.ImageUrl = "Images/emptyfile.gif";
				attrs.Nodes.Add(child);
			}

			node.Nodes.Add(attrs);
			// Reposition the reader on the element.
			reader.MoveToElement();
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
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
			this.btnDefaultXml.Click += new System.EventHandler(this.btnDefaultXml_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnReport_Click(object sender, System.EventArgs e)
		{
			SaveXml();
			Response.Redirect("UploadListReport.aspx");
		}

		private void btnDefaultXml_Click(object sender, System.EventArgs e)
		{
			using (StreamReader sr = new StreamReader(Server .MapPath("~/upload.xml")))
			{
				Session["xml"] = sr.ReadToEnd();
				Session["file"] = "Sample file";
			}
			BuildTreeView();
		}
	}
}
