using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FriendsReunionInstaller
{
	/// <summary>
	/// Manages database installation process.
	/// </summary>
	[System.ComponentModel.RunInstaller(true)]
	public class DbInstaller : Installer
	{
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);

			// Warn the user that we need the service to be running.
			MessageBox.Show(
				"Ensure the SQL Server / MSDE service is running for a proper installation.", 
				"Database Service", MessageBoxButtons.OK, MessageBoxIcon.Warning );
			string path = base.Context.Parameters["db"];
			string args = String.Format(
				"-S (local) -E -Q \"sp_attach_db N'FriendsData', N'{0}', N'{1}'", 
				Path.Combine(path, "Friends_Data.mdf"), 
				Path.Combine(path, "Friends_Log.ldf"));

			// Execute the attach DB command.
			Process.Start("osql", args);
		}

		public override void Uninstall(System.Collections.IDictionary savedState)
		{
			MessageBox.Show(
				"Ensure the SQL Server / MSDE service is running for a proper uninstallation.", 
				"Database Service", MessageBoxButtons.OK, MessageBoxIcon.Warning );
            Process.Start("osql", "-S (local) -E -Q \"sp_detach_db N'FriendsData'\"");
			base.Uninstall(savedState);
		}

		public override void Rollback(System.Collections.IDictionary savedState)
		{
			Process.Start("osql", "-S (local) -E -Q \"sp_detach_db N'FriendsData'\"");
			base.Rollback(savedState);
		}
	}
}
