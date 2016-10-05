using System;
using System.Configuration.Install;
using System.Diagnostics;

namespace FriendsReunionInstaller
{
	[System.ComponentModel.RunInstaller(true)]
	public class EventLogInstaller : Installer
	{
		public override void Install(System.Collections.IDictionary stateSaver)
		{
			base.Install(stateSaver);
            EventLog.CreateEventSource("FriendsReunion", "Application");
		}

		public override void Uninstall(System.Collections.IDictionary savedState)
		{
			EventLog.DeleteEventSource("FriendsReunion");
			base.Uninstall(savedState);
		}
	}
}