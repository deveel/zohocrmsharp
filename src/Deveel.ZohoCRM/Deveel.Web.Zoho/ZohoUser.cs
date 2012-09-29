using System;

namespace Deveel.Web.Zoho {
	public sealed class ZohoUser {
		internal ZohoUser(string name, string email, string id, string role, string profile, bool isActive, bool confirm) {
			Confirm = confirm;
			Role = role;
			IsActive = isActive;
			Profile = profile;
			Id = id;
			Email = email;
			Name = name;
		}

		public string Name { get; private set; }

		public string Email { get; private set; }

		public string Id { get; private set; }

		public string Role { get; private set; }

		public string Profile { get; private set; }

		public bool IsActive { get; private set; }

		public bool Confirm { get; private set; }
	}
}