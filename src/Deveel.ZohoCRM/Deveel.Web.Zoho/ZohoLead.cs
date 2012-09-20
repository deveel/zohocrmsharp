using System;

namespace Deveel.Web.Zoho {
	[EntityName("Leads")]
	public sealed class ZohoLead : ZohoEntity {
		public ZohoLead(string firstName, string lastName, string emailAddress) {
			if (firstName == null)
				throw new ArgumentNullException("firstName");
			if (lastName == null)
				throw new ArgumentNullException("lastName");
			if (emailAddress == null)
				throw new ArgumentNullException("emailAddress");

			FirstName = firstName;
			LastName = lastName;
			Email = emailAddress;
		}

		internal ZohoLead() {
		}

		public string FirstName {
			get { return GetString("First Name"); }
			private set { SetValue("First Name", value); }
		}

		public string LastName {
			get { return GetString("Last Name"); }
			private set { SetValue("Last Name", value); }
		}

		public string Email {
			get { return GetString("Email"); }
			private set { SetValue("Email", value); }
		}

		public bool EmailOptOut {
			get { return GetBoolean("Email Opt Out"); }
			set { SetValue("Email Opt Out", value); }
		}

		public string Title {
			get { return GetString("Title"); }
			set { SetValue("Title", value); }
		}
		
		public string Owner {
			get { return GetString("Lead Owner"); }
			set { SetValue("Lead Owner", value); }
		}

		public string Source {
			get { return GetString("Lead Source"); }
			set { SetValue("Lead Source", value); }
		}

		public string JobTitle {
			get { return GetString("Designation"); }
			set { SetValue("Designation", value); }
		}

		public string Company {
			get { return GetString("Company"); }
			set { SetValue("Company", value); }
		}

		public string PhoneNumber {
			get { return GetString("Phone"); }
			set { SetValue("Phone", value); }
		}

		public string MobileNumber {
			get { return GetString("Mobile"); }
			set { SetValue("Mobile", value); }
		}

		public string Street {
			get { return GetString("Street"); }
			set { SetValue("Street", value); }
		}

		public string City {
			get { return GetString("City"); }
			set { SetValue("City", value); }
		}

		public string Country {
			get { return GetString("Cuntry"); }
			set { SetValue("Country", value); }
		}

		public string State {
			get { return GetString("State"); }
			set { SetValue("State", value); }
		}

		public string Zip {
			get { return GetString("Zip Code"); }
			set { SetValue("Zip Code", value); }
		}

		public string Salutation {
			get { return GetString("Salutation"); }
			set { SetValue("Salutation", value); }
		}

		public string Website {
			get { return GetString("Website"); }
			set { SetValue("Website", value); }
		}
	}
}