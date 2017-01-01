using System;
using System.Collections.Generic;

namespace Deveel.Web.Zoho {
	public static class ContactDefaultFields {
		public const string ContactId = "CONTACTID";
		public const string Email = "Email";
		public const string FirstName = "First Name";
		public const string LastName = "Last Name";
		public const string AccountName = "Account Name";
		public const string EmailOptOut = "Email Opt Out";
		public const string Phone = "Phone";
		public const string HomePhone = "Home Phone";
		public const string Mobile = "Mobile";
		public const string Assistant = "Assistant";
		public const string AssistantPhone = "Asst Phone";
		public const string ContactOwner = "Contact Owner";
		public const string DateOfBirth = "Date of Birth";
		public const string Department = "Department";
		public const string Description = "Description";
		public const string ReportsTo = "Reports To";
		public const string LeadSource = "Lead Source";
		public const string VendorName = "Vendor Name";
		public const string Salutation = "Salutation";
		public const string Title = "Title";
		public const string MailingCity = "Mailing City";
		public const string MailingCountry = "Mailing Country";
		public const string MailingState = "Mailing State";
		public const string MailingStreet = "Mailing Street";
		public const string MailingCode = "Mailing Zip";

		public static readonly IEnumerable<string> All = new[] {
			Email,
			FirstName,
			LastName,
			AccountName,
			EmailOptOut,
			Phone,
			HomePhone,
			Mobile,
			Assistant,
			AssistantPhone,
			ContactOwner,
			DateOfBirth,
			Department,
			Description,
			ReportsTo,
			LeadSource,
			VendorName,
			Salutation,
			Title,
			MailingCity,
			MailingCountry,
			MailingCode,
			MailingStreet,
			MailingState
		};
	}
}