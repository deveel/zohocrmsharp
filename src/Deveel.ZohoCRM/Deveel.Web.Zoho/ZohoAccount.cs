using System;

namespace Deveel.Web.Zoho {
	[ModuleName("Accounts")]
	public sealed class ZohoAccount : ZohoEntity {
		internal ZohoAccount() {
		}

		public ZohoAccount(string accountName) {
			if (accountName == null)
				throw new ArgumentNullException("accountName");

			Name = accountName;
		}

		protected override string IdFieldName {
			get { return AccountDefaultFields.AccountId; }
		}

		public string Name {
			get { return GetString(AccountDefaultFields.AccountName); }
			private set { SetValue(AccountDefaultFields.AccountName, value); }
		}

		public string Number {
			get { return GetString(AccountDefaultFields.AccountNumber); }
			set { SetValue(AccountDefaultFields.AccountNumber, value); }
		}

		public string Owner {
			get { return GetString(AccountDefaultFields.AccountOwner); }
			set { SetValue(AccountDefaultFields.AccountOwner, value); }
		}

		public string ParentAccount {
			get { return GetString(AccountDefaultFields.ParentAccount); }
			set { SetValue(AccountDefaultFields.ParentAccount, value); }
		}

		public string Type {
			get { return GetString(AccountDefaultFields.AccountType); }
			set { SetValue(AccountDefaultFields.AccountType, value); }
		}

		public string Site {
			get { return GetString(AccountDefaultFields.AccountSite); }
			set { SetValue(AccountDefaultFields.AccountSite, value); }
		}

		public string PhoneNumber {
			get { return GetString(AccountDefaultFields.Phone); }
			set { SetValue(AccountDefaultFields.Phone, value); }
		}

		public string BillingStreet {
			get { return GetString(AccountDefaultFields.BillingStreet); }
			set { SetValue(AccountDefaultFields.BillingStreet, value); }
		}

		public string BillingCity {
			get { return GetString(AccountDefaultFields.BillingCity); }
			set { SetValue(AccountDefaultFields.BillingCity, value); }
		}

		public string BillingState {
			get { return GetString(AccountDefaultFields.BillingState); }
			set { SetValue(AccountDefaultFields.BillingState, value); }
		}

		public string BillingCountry {
			get { return GetString(AccountDefaultFields.BillingCountry); }
			set { SetValue(AccountDefaultFields.BillingCountry, value); }
		}

		public string BillingZipCode {
			get { return GetString(AccountDefaultFields.BillingCode); }
			set { SetValue(AccountDefaultFields.BillingCode, value); }
		}

		public string ShippingStreet {
			get { return GetString(AccountDefaultFields.ShippingStreet); }
			set { SetValue(AccountDefaultFields.ShippingStreet, value); }
		}

		public string ShippingCity {
			get { return GetString(AccountDefaultFields.ShippingCity); }
			set { SetValue(AccountDefaultFields.ShippingCity, value); }
		}

		public string ShippingState {
			get { return GetString(AccountDefaultFields.ShippingState); }
			set { SetValue(AccountDefaultFields.ShippingState, value); }
		}

		public string ShippingCountry {
			get { return GetString(AccountDefaultFields.ShippingCountry); }
			set { SetValue(AccountDefaultFields.ShippingCountry, value); }
		}

		public string ShippingZipCode {
			get { return GetString(AccountDefaultFields.ShippingCode); }
			set { SetValue(AccountDefaultFields.ShippingCode, value); }
		}

		public int Employees {
			get { return GetInt32(AccountDefaultFields.Employees); }
			set { SetValue(AccountDefaultFields.Employees, value); }
		}

		public string Ownership {
			get { return GetString(AccountDefaultFields.Ownership); }
			set { SetValue(AccountDefaultFields.Ownership, value); }
		}

		public string Website {
			get { return GetString(AccountDefaultFields.Website); }
			set { SetValue(AccountDefaultFields.Website, value); }
		}
	}
}