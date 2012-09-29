using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	public sealed class ZohoUsersResponse : ZohoResponse {
		internal ZohoUsersResponse(string module, string method, string responseContent) 
			: base(module, method, responseContent) {
		}

		public IEnumerable<ZohoUser> Users { get; private set; }

		internal override void LoadFromXml(XElement parent) {
			if (parent.Name != "users")
				throw new FormatException();

			var userList = new List<ZohoUser>();
			var usersElements = parent.Descendants("user");
			foreach (var element in usersElements) {
				var name = element.Value;
				var id = element.Attribute("id").Value;
				var email = element.Attribute("email").Value;
				var role = element.Attribute("role").Value;
				var profile = element.Attribute("profile").Value;
				var isActive = element.Attribute("status").Value == "active";
				var confirm = element.Attribute("confirm").Value == "true";

				userList.Add(new ZohoUser(name, email, id, role, profile, isActive, confirm));
			}

			Users = userList.AsReadOnly();
		}
	}
}