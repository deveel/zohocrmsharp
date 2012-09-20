using System;

namespace Deveel.Web.Zoho {
	[EntityName("Contacts")]
	public sealed class ZohoContact : ZohoEntity {
		internal ZohoContact() {
		}

		public ZohoContact(string email) {
			if (email == null)
				throw new ArgumentNullException("email");

			Email = email;
		}

		public string FirstName {
			get { return GetString("First Name"); }
			set { SetValue("First Name", value); }
		}

		public string LastName {
			get { return GetString("Last Name"); }
			set { SetValue("Last Name", value); }
		}

		public string Email {
			get { return GetString("Email"); }
			private set { SetValue("Email", value); }
		}

		public bool EmailOptOut {
			get { return GetBoolean("Email Opt Out"); }
			set { SetValue("Email Opt Out", value); }
		}

		public string Phone {
			get { return GetString("Phone"); }
			set { SetValue("Phone", value); }
		}

		public string HomePhone {
			get { return GetString("Home Phone"); }
			set { SetValue("Home Phone", value); }
		}

		public string Mobile {
			get { return GetString("Mobile"); }
			set { SetValue("Mobile", value); }
		}

		public string Assistant {
			get { return GetString("Assistant"); }
			set { SetValue("Assistant", value); }
		}

		public string AssistantPhone {
			get { return GetString("Asst Phone"); }
			set { SetValue("Asst Phone", value); }
		}

		public string ContactOwner {
			get { return GetString("Contact Owner"); }
			set { SetValue("Contact Owner", value); }
		}

		public DateTime BirthDate {
			get { return GetDateTime("Date of Birth"); }
			set { SetValue("Date of Birth", value); }
		}

		public string Department {
			get { return GetString("Department"); }
			set { SetValue("Department", value); }
		}

		public string Description {
			get { return GetString("Description"); }
			set { SetValue("Description", value); }
		}

		public string ReportsTo {
			get { return GetString("Reports To"); }
			set { SetValue("Reports To", value); }
		}

		public string LeadSource {
			get { return GetString("Lead Source"); }
			set { SetValue("Lead Source", value); }
		}

		public string VendorName {
			get { return GetString("Vendor Name"); }
			set { SetValue("Vendor Name", value); }
		}

		public string Salutation {
			get { return GetString("Salutation"); }
			set { SetValue("Salutation", value); }
		}

		public string Title {
			get { return GetString("Title"); }
			set { SetValue("Title", value); }
		}

		public string MailingCity {
			get { return GetString("Mailing City"); }
			set { SetValue("Mailing City", value); }
		}

		public string MailingCuntry {
			get { return GetString("MailingCountry"); }
			set { SetValue("Mailing Country", value); }
		}

		public string MailingState {
			get { return GetString("Mailing State"); }
			set { SetValue("Mailing State", value); }
		}

		public string MailingStreet {
			get { return GetString("Mailing Street"); }
			set { SetValue("Mailing Street", value); }
		}
	}
}