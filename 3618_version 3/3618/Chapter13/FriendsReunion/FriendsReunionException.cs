using System;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for FriendsReunionException.
	/// </summary>
	public class FriendsReunionException : ApplicationException
	{
		public FriendsReunionException()
		{
		}

		public FriendsReunionException(string message, Exception inner) : 
			base(message, inner)
		{
		}
	}
}
