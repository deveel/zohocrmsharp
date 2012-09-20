using System;

namespace Deveel.Web.Zoho {
	[EntityName("Accounts")]
	public sealed class ZohoAccount : ZohoEntity {
		internal ZohoAccount() {
		}

		public ZohoAccount(string accountName) {
			if (accountName == null)
				throw new ArgumentNullException("accountName");

			Name = accountName;
		}

		protected override string IdFieldName {
			get { return "ACCOUNTID"; }
		}

		public string Name {
			get { return GetString("Account Name"); }
			private set { SetValue("Account Name", value); }
		}

		public string Number {
			get { return GetString("Account Number"); }
			set { SetValue("Account Number", value); }
		}

		public string Owner {
			get { return GetString("Account Owner"); }
			set { SetValue("Account Owner", value); }
		}

		public string ParentAccount {
			get { return GetString("Parent Account"); }
			set { SetValue("Parent Account", value); }
		}

		public string Type {
			get { return GetString("Account Type"); }
			set { SetValue("Account Type", value); }
		}

		public string Site {
			get { return GetString("Account Site"); }
			set { SetValue("Account Site", value); }
		}

		public string PhoneNumber {
			get { return GetString("Phone"); }
			set { SetValue("Phone", value); }
		}

		public string BillingStreet {
			get { return GetString("Billing Street"); }
			set { SetValue("Billing Street", value); }
		}

		public string BillingCity {
			get { return GetString("Billing City"); }
			set { SetValue("Billing City", value); }
		}

		public string BillingState {
			get { return GetString("Billing State"); }
			set { SetValue("Billing State", value); }
		}

		public string BillingCountry {
			get { return GetString("Billing Country"); }
			set { SetValue("Billing Country", value); }
		}

		public string BillingZipCode {
			get { return GetString("Billing Code"); }
			set { SetValue("Billing Code", value); }
		}

		public string ShippingStreet {
			get { return GetString("Shipping Street"); }
			set { SetValue("Shipping Street", value); }
		}

		public string ShippingCity {
			get { return GetString("Shipping City"); }
			set { SetValue("Shipping City", value); }
		}

		public string ShippingState {
			get { return GetString("Shipping State"); }
			set { SetValue("Shipping State", value); }
		}

		public string ShippingCountry {
			get { return GetString("Shipping Country"); }
			set { SetValue("Shipping Country", value); }
		}

		public string ShippingZipCode {
			get { return GetString("Shipping Code"); }
			set { SetValue("Shipping Code", value); }
		}

		public int Employees {
			get { return GetInt32("Employees"); }
			set { SetValue("Employees", value); }
		}

		public string Ownership {
			get { return GetString("Ownership"); }
			set { SetValue("Ownership", value); }
		}

		public string Website {
			get { return GetString("Website"); }
			set { SetValue("Website", value); }
		}
	}
}