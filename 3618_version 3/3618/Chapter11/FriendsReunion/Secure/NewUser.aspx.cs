using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace FriendsReunion.Secure
{
	/// <summary>
	/// Summary description for NewUser.
	/// </summary>
	public class NewUser : FriendsBase
	{
		protected System.Web.UI.WebControls.TextBox txtLogin;
		protected System.Web.UI.WebControls.TextBox txtPwd;
		protected System.Web.UI.WebControls.TextBox txtFName;
		protected System.Web.UI.WebControls.TextBox txtLName;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtPhone;
		protected System.Web.UI.WebControls.TextBox txtMobile;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtBirth;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqLogin;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqPwd;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqFName;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqLName;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqPhone;
		protected System.Web.UI.WebControls.RequiredFieldValidator reqEmail;
		protected System.Web.UI.WebControls.CompareValidator compBirth;
		protected System.Web.UI.WebControls.RegularExpressionValidator regPhone;
		protected System.Web.UI.WebControls.RegularExpressionValidator regEmail;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.ValidationSummary valErrors;
		protected System.Web.UI.WebControls.Button btnAccept;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			base.HeaderIconImageUrl = "~/Images/securekeys.gif";
			base.HeaderMessage = "Registration Form";

			// Postbacks will typically be caused by the validator
			// controls in non-IE browsers
			if (Page.IsPostBack)
				return;

			// If this is an update, preload the values
			if (Context.User.Identity.IsAuthenticated)
			{
				// Change the header message
				base.HeaderMessage = "Update my profile";

				SqlConnection con = new SqlConnection(
					ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
				SqlCommand cmd = new SqlCommand(
					"SELECT * FROM [User] WHERE UserID=@ID", con);
				cmd.Parameters.Add("@ID", Page.User.Identity.Name);

				con.Open(); 
				try
				{
					SqlDataReader reader = cmd.ExecuteReader();

					if (reader.Read())
					{
						// Retrieve a typed value using the column's ordinal position
						int pos = reader.GetOrdinal("Address");
						this.txtAddress.Text = reader.GetString(pos);

						// Avoid using the pos variable altogether, but
						// get the typed value
						this.txtBirth.Text = reader.GetDateTime(
							reader.GetOrdinal("DateOfBirth")).ToShortDateString();

						// Convert directly the untyped Object returned by the
						// indexer to a string
						this.txtEmail.Text = reader["Email"].ToString();
						this.txtFName.Text = reader["FirstName"].ToString();
						this.txtLName.Text = reader["LastName"].ToString();
						this.txtLogin.Text = reader["Login"].ToString();
						this.txtPhone.Text = reader["PhoneNumber"].ToString();
						this.txtPwd.Text = reader["Password"].ToString();

						// Use SQL Server type to have additional features 
						pos = reader.GetOrdinal("CellNumber");
						SqlString cel = reader.GetSqlString(pos);
						if (!cel.IsNull)
							this.txtMobile.Text = cel.Value;
					}
				}
				finally
				{
					// Ensure connection is ALWAYS closed
					con.Close();
				}
			}
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
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnAccept_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				Trace.Write("FriendsReunion", "Page data was validated ok");

				if (Context.User.Identity.IsAuthenticated)
				{
					UpdateUser();
				}
				else 
				{
					try
					{
						InsertUser();
					}
					catch(DuplicateUsernameException)
					{
						this.lblMessage.Visible = true;
						this.lblMessage.Text = 
							"You are trying to register using a username that has " + 
							"already been taken by someone else. " + 
							"Please choose a different username. ";
					}
				}
			}
			else
			{
				lblMessage.Text = "Fix the following errors and retry:";
			}
		}

		private void InsertUser()
		{
			Trace.Write("FriendsReunion", "We're entering the InsertUser() method");
			
			// Save new user to the database
			ArrayList values = new ArrayList(11);

			// Escape any quotation mark entered by the user
			txtLogin.Text = txtLogin.Text.Replace("'","''");
			txtPwd.Text = txtPwd.Text.Replace("'","''");
			txtFName.Text = txtFName.Text.Replace("'","''");
			txtLName.Text = txtLName.Text.Replace("'","''");
			txtPhone.Text = txtPhone.Text.Replace("'","''");
			txtMobile.Text = txtMobile.Text.Replace("'","''");
			txtEmail.Text = txtEmail.Text.Replace("'","''");
			txtAddress.Text = txtAddress.Text.Replace("'","''");
			txtBirth.Text = txtBirth.Text.Replace("'","''");

			// Optional values without quotes as they can be the Null value.
			string sql = @"
					INSERT INTO [User] 
						(UserID, Login, Password, FirstName, LastName,
						PhoneNumber, Email, IsAdministrator, Address,
						CellNumber, DateOfBirth) 
					VALUES 
						('{0}', '{1}', '{2}', '{3}', '{4}',
						'{5}', '{6}', '{7}', {8}, {9}, {10})";

			// Add required values to replace
			values.Add(Guid.NewGuid().ToString());
			values.Add(txtLogin.Text);
			values.Add(txtPwd.Text);
			values.Add(txtFName.Text);
			values.Add(txtLName.Text);
			values.Add(txtPhone.Text);
			values.Add(txtEmail.Text);
			values.Add(0);

			// Add the optional values or Null
			if (txtAddress.Text != string.Empty)
				values.Add("'" + txtAddress.Text + "'");
			else 
				values.Add("Null");

			if (txtMobile.Text != string.Empty)
				values.Add("'" + txtMobile.Text + "'");
			else 
				values.Add("Null");

			if (txtBirth.Text != string.Empty)
				values.Add("'" + txtBirth.Text + "'");
			else
				values.Add("Null");

			// Format the string with the array of values
			sql = String.Format(sql, values.ToArray());

			// Connect and execute the query
			SqlConnection con = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlCommand cmd = new SqlCommand(sql, con);
			con.Open();

			Trace.Write("FriendsReunion", 
				"Connection string in use: " + con.ConnectionString);

			bool doredirect = true;

			try
			{
				cmd.ExecuteNonQuery();
			}
			catch(OutOfMemoryException e)
			{
				doredirect = false;
				this.lblMessage.Visible = true;
				this.lblMessage.Text = "We just run of out memory, " + 
					"please restart the application!";
			}
			catch (SqlException e)
			{
				if (e.Number==2627)
				{
					throw new DuplicateUsernameException("Can't insert record", e);
				}
				else 
				{
					doredirect = false;
					this.lblMessage.Visible = true;
					this.lblMessage.Text = "Insert couldn't be performed.";
				}
			}
			catch(Exception e)
			{ 
				doredirect = false;
				this.lblMessage.Visible = true;
				this.lblMessage.Text = "Couldn't update your profile!";
			}
			finally
			{
				// Ensure connection is ALWAYS closed.
				con.Close();
			}

			if (doredirect)
				Response.Redirect("Login.aspx");

			Trace.Write("FriendsReunion", "We're leaving the InsertUser() method");
		}

		private void UpdateUser()
		{
			ArrayList values = new ArrayList();

			// Optional values without quotes as they can be the Null value.
			string sql = @"
					UPDATE [User] SET
						Login='{0}', Password='{1}', FirstName='{2}', 
						LastName='{3}', PhoneNumber='{4}', Email='{5}',	
						Address={6}, CellNumber={7}, DateOfBirth={8} 
					WHERE
						UserID='{9}'";

			// Add required values to replace
			values.Add(txtLogin.Text);
			values.Add(txtPwd.Text);
			values.Add(txtFName.Text);
			values.Add(txtLName.Text);
			values.Add(txtPhone.Text);
			values.Add(txtEmail.Text);

			// Add optional values
			if (txtAddress.Text == String.Empty)
				values.Add("Null");
			else
				values.Add("'" + txtAddress.Text + "'");

			if (txtMobile.Text == String.Empty)
				values.Add("Null");
			else
				values.Add("'" + txtMobile.Text + "'");

			if (txtBirth.Text == String.Empty)
			{
				values.Add("Null");
			}
			else
			{
				// Pass date in ISO format YYYYMMDD
				DateTime dt = DateTime.Parse(txtBirth.Text);
				values.Add("'" + dt.ToString("yyyyMMdd") + "'");
			}

			// Get the UserID from the context.
			values.Add(Context.User.Identity.Name);

			// Format the query with the values
			sql = String.Format(sql, values.ToArray());

			// Connect and execute the query
			SqlConnection con = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlCommand cmd = new SqlCommand(sql, con);
			con.Open();

			bool doredirect = true;

			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (SqlException)
			{
				doredirect = false;
				this.lblMessage.Visible = true;
				this.lblMessage.Text = "Couldn't update your profile!";
			}
			finally
			{
				// Ensure connection is ALWAYS closed
				con.Close();
			}

			if (doredirect)
				Response.Redirect("../Default.aspx");
		}
	}
}