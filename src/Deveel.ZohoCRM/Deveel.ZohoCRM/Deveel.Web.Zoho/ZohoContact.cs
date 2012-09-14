using System;

namespace Deveel.Web.Zoho
{
	[EntityName("Contacts")]
	public sealed class ZohoContact : ZohoEntity
	{
		internal ZohoContact() {
		}

		public ZohoContact(string email) {
			if (email == null)
				throw new ArgumentNullException("email");

			Email = email;
		}

		public string Email {
			get { return GetValue("Email"); }
			private set { SetValue("Email", value); }
		}
	}
}