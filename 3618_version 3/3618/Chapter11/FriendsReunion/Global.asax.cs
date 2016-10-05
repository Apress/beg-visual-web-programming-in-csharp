using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Web;
using System.Web.SessionState;

namespace FriendsReunion 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			// Get the connection string from the existing key in Web.config
			SqlConnection con = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlCommand cmd = new SqlCommand("SELECT Visitors FROM Counter", con);
			con.Open();

			try 
			{
				// Retrieve the counter
				Application["counter"] = (int) cmd.ExecuteScalar();
			}
			finally 
			{
				con.Close();
			}
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			Application.Lock();
			Application["counter"] = ((int)Application["counter"]) + 1; 
			Application.UnLock();
		}

		protected void Application_End(Object sender, EventArgs e)
		{
			// Get the connection string from the existing key in Web.config
			SqlConnection con = new SqlConnection(
				ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
			SqlCommand cmd = new SqlCommand("UPDATE Counter SET Visitors=" +
				Application["counter"].ToString(), con);
			con.Open();

			try 
			{
				cmd.ExecuteNonQuery();
			}
			finally 
			{
				con.Close();
			}
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			try
			{
				System.Threading.Thread.CurrentThread.CurrentCulture = 
					new System.Globalization.CultureInfo(Context.Request.UserLanguages[0]);
			}
			catch 
			{
				// Will simply use the default language.
			}
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			// Cast the sender to the application
			HttpApplication app = (HttpApplication) sender;

			// Only replace the context if it has already been handled
			// by forms authentication module (user is authenticated)
			if (app.Request.IsAuthenticated)
			{
				SqlConnection con = new SqlConnection(
					ConfigurationSettings.AppSettings["cnFriends.ConnectionString"]);
				SqlCommand cmd = new SqlCommand(
					"SELECT IsAdministrator FROM [User] WHERE UserId=@ID", con);
				// Set the parameter value.
				cmd.Parameters.Add("@ID", Context.User.Identity.Name);
				con.Open();

				try
				{
					object admin = cmd.ExecuteScalar();

					// Was it a valid UserID?
					if (admin != null)
					{
						string[] roles;

						// If IsAdministrator field is true, add both roles
						if (((bool)admin) == true)
						{
							roles = new string[] {"User", "Admin"};
						} 
						else 
						{
							roles = new string[] {"User"};
						}

						// We reuse the identity created by Forms authentication.
						GenericPrincipal ppal = new GenericPrincipal(
							Context.User.Identity, roles);
						Context.User = ppal;
					} 
					else 
					{
						// If UserID was invalid, clear the context so they log on again
						Context.User = null;
					}
				} 
				finally 
				{
					// Ensure closing the connection
					con.Close();
				}
			}
		}

		protected void Application_Error(Object sender, EventArgs e)
		{
			System.Diagnostics.EventLog.WriteEntry("FriendsReunion", 
				Server.GetLastError().InnerException.ToString(), 
				System.Diagnostics.EventLogEntryType.Error);
		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

