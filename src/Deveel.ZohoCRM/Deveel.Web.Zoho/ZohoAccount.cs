using System;

namespace Deveel.Web.Zoho {
	[EntityName("Accounts")]
	public sealed class ZohoAccount : ZohoEntity {
		internal ZohoAccount() {
		}

		public ZohoAccount(string accountName) {
			if (accountName == null)
				throw new ArgumentNullException("accountName");

			AccountName = accountName;
		}

		public string AccountName {
			get { return GetValue("Account Name"); }
			private set { SetValue("Account Name", value); }
		}

		public string AccountNumber {
			get { return GetValue("Account Number"); }
			set { SetValue("Account Number", value); }
		}

		public string AccountOwner {
			get { return GetValue("Account Owner"); }
			set { SetValue("Account Owner", value); }
		}

		public string AccountType {
			get { return GetValue("Account Type"); }
			set { SetValue("Account Type", value); }
		}

		public string BillingStreet {
			get { return GetValue("Billing Street"); }
			set { SetValue("Billing Street", value); }
		}

		public string BillingCity {
			get { return GetValue("Billing City"); }
			set { SetValue("Billing City", value); }
		}

		public string BillingState {
			get { return GetValue("Billing State"); }
			set { SetValue("Billing State", value); }
		}

		public string BillingCountry {
			get { return GetValue("Billing Country"); }
			set { SetValue("Billing Country", value); }
		}
	}
}