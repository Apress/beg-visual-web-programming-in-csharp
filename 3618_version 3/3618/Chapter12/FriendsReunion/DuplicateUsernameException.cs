using System;

namespace FriendsReunion
{
	/// <summary>
	/// Summary description for DuplicateUsernameException.
	/// </summary>
	public class DuplicateUsernameException : FriendsReunionException
	{
		public DuplicateUsernameException()
		{
		}

		public DuplicateUsernameException(string message, Exception inner) : 
			base(message,inner)
		{
		}
	}
}
