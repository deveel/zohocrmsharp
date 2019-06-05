using System;

namespace Deveel.Web.Zoho {
	[ModuleName("Contacts")]
	public sealed class ZohoContact : ZohoEntity {
		internal ZohoContact() {
		}

		public ZohoContact(string email) 
			: this(email, null, null) {
		}

		public ZohoContact(string email, string firstName, string lastName) {
			if (email == null)
				throw new ArgumentNullException("email");

			Email = email;
			FirstName = firstName;
			LastName = lastName;
		}

		protected override string IdFieldName {
			get { return ContactDefaultFields.ContactId; }
		}

		public string ProductId {
			get { return ContactDefaultFields.ProductId; }
			set { SetValue(ContactDefaultFields.ProductId, value); }
		}

		public string FirstName {
			get { return GetString(ContactDefaultFields.FirstName); }
			set { SetValue(ContactDefaultFields.FirstName, value); }
		}

		public string LastName {
			get { return GetString(ContactDefaultFields.LastName); }
			set { SetValue(ContactDefaultFields.LastName, value); }
		}

		public string Email {
			get { return GetString(ContactDefaultFields.Email); }
			private set { SetValue(ContactDefaultFields.Email, value); }
		}

		public string AccountName {
			get { return GetString(ContactDefaultFields.AccountName); }
			set { SetValue(ContactDefaultFields.AccountName, value); }
		}

		public string AccountId {
			get { return GetString(ContactDefaultFields.AccountId); }
			set { SetValue(ContactDefaultFields.AccountId, value); }
		}

		public bool EmailOptOut {
			get { return GetBoolean(ContactDefaultFields.EmailOptOut); }
			set { SetValue(ContactDefaultFields.EmailOptOut, value); }
		}

		public string Phone {
			get { return GetString(ContactDefaultFields.Phone); }
			set { SetValue(ContactDefaultFields.Phone, value); }
		}

		public string HomePhone {
			get { return GetString(ContactDefaultFields.HomePhone); }
			set { SetValue(ContactDefaultFields.HomePhone, value); }
		}

		public string Mobile {
			get { return GetString(ContactDefaultFields.Mobile); }
			set { SetValue(ContactDefaultFields.Mobile, value); }
		}

		public string Assistant {
			get { return GetString(ContactDefaultFields.Assistant); }
			set { SetValue(ContactDefaultFields.Assistant, value); }
		}

		public string AssistantPhone {
			get { return GetString(ContactDefaultFields.AssistantPhone); }
			set { SetValue(ContactDefaultFields.AssistantPhone, value); }
		}

		public string Owner {
			get { return GetString(ContactDefaultFields.ContactOwner); }
			set { SetValue(ContactDefaultFields.ContactOwner, value); }
		}

		public DateTime DateOfBirth {
			get { return GetDateTime(ContactDefaultFields.DateOfBirth); }
			set { SetValue(ContactDefaultFields.DateOfBirth, value); }
		}

		public string Department {
			get { return GetString(ContactDefaultFields.Department); }
			set { SetValue(ContactDefaultFields.Department, value); }
		}

		public string Description {
			get { return GetString(ContactDefaultFields.Description); }
			set { SetValue(ContactDefaultFields.Description, value); }
		}

		public string ReportsTo {
			get { return GetString(ContactDefaultFields.ReportsTo); }
			set { SetValue(ContactDefaultFields.ReportsTo, value); }
		}

		public string LeadSource {
			get { return GetString(ContactDefaultFields.LeadSource); }
			set { SetValue(ContactDefaultFields.LeadSource, value); }
		}

		public string VendorName {
			get { return GetString(ContactDefaultFields.VendorName); }
			set { SetValue(ContactDefaultFields.VendorName, value); }
		}

		public string Salutation {
			get { return GetString(ContactDefaultFields.Salutation); }
			set { SetValue(ContactDefaultFields.Salutation, value); }
		}

		public string Title {
			get { return GetString(ContactDefaultFields.Title); }
			set { SetValue(ContactDefaultFields.Title, value); }
		}

		public string MailingCity {
			get { return GetString(ContactDefaultFields.MailingCity); }
			set { SetValue(ContactDefaultFields.MailingCity, value); }
		}

		public string MailingCountry {
			get { return GetString(ContactDefaultFields.MailingCountry); }
			set { SetValue(ContactDefaultFields.MailingCountry, value); }
		}

		public string MailingState {
			get { return GetString(ContactDefaultFields.MailingState); }
			set { SetValue(ContactDefaultFields.MailingState, value); }
		}

		public string MailingStreet {
			get { return GetString(ContactDefaultFields.MailingStreet); }
			set { SetValue(ContactDefaultFields.MailingStreet, value); }
		}

		public string MailingCode {
			get { return GetString(ContactDefaultFields.MailingCode); }
			set { SetValue(ContactDefaultFields.MailingCode, value);}
		}
	}
}