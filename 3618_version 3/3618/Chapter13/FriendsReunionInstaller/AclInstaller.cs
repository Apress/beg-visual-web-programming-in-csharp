using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;

namespace FriendsReunionInstaller
{
	/// <summary>
	/// Manages database installation process.
	/// </summary>
	[System.ComponentModel.RunInstaller(true)]
	public class AclInstaller : Installer
	{
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);
			System.Diagnostics.Debugger.Break();
			string path = base.Context.Parameters["path"];
			string[] files = base.Context.Parameters["files"].Split(',');

			ProcessStartInfo info = new ProcessStartInfo("cacls");
			info.CreateNoWindow = true;
			info.WindowStyle = ProcessWindowStyle.Hidden;

			foreach (string file in files)
			{
				// Assign permissions to everyone to read the file.
				info.Arguments = Path.Combine(path, file) + 
                    " /E /G Everyone:R";
				Process.Start(info);
			}
		}
	}
}
