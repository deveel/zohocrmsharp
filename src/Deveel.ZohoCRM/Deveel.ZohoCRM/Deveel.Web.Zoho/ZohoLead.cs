using System;

namespace Deveel.Web.Zoho
{
	[EntityName("Leads")]
	public sealed class ZohoLead : ZohoEntity
	{
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

		public string FirstName
		{
			get { return GetValue("First Name"); }
			private set { SetValue("First Name", value); }
		}

		public string LastName
		{
			get { return GetValue("Last Name"); }
			private set { SetValue("Last Name", value); }
		}

		public string Email
		{
			get { return GetValue("Email"); }
			private set { SetValue("Email", value); }
		}

		public string Owner
		{
			get { return GetValue("Lead Owner"); }
			set { SetValue("Lead Owner", value); }
		}

		public string Source
		{
			get { return GetValue("Lead Source"); }
			set { SetValue("Lead Source", value); }
		}

		public string JobTitle
		{
			get { return GetValue("Designation"); }
			set { SetValue("Designation", value); }
		}

		public string Company
		{
			get { return GetValue("Company"); }
			set { SetValue("Company", value); }
		}

		public string PhoneNumber
		{
			get { return GetValue("Phone"); }
			set { SetValue("Phone", value); }
		}

		public string MobileNumber
		{
			get { return GetValue("Mobile"); }
			set { SetValue("Mobile", value); }
		}

		public string Street
		{
			get { return GetValue("Street"); }
			set { SetValue("Street", value); }
		}

		public string City
		{
			get { return GetValue("City"); }
			set { SetValue("City", value); }
		}

		public string Country
		{
			get { return GetValue("Cuntry"); }
			set { SetValue("Country", value); }
		}

		public string State
		{
			get { return GetValue("State"); }
			set { SetValue("State", value); }
		}

		public string Zip
		{
			get { return GetValue("Zip Code"); }
			set { SetValue("Zip Code", value); }
		}
	}
}