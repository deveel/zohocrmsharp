using System;

using NUnit.Framework;

namespace Deveel.Web.Zoho {
	[TestFixture]
	public sealed class UsersTest : ZohoCrmTestBase {
		[Test]
		public void GetUsers() {
			var client = CreateClient();

			var response = client.GetUsers(UserType.AllUsers);
			foreach (var user in response.Users) {
				Console.Out.WriteLine("User {0} ({1}): {2}", user.Name, user.Email, user.Id);
			}
		}
	}
}