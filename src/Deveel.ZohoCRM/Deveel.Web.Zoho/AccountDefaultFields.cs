using System;
using System.Collections.Generic;

namespace Deveel.Web.Zoho {
	public static class AccountDefaultFields {
		public const string AccountId = "ACCOUNTID";
		public const string AccountName = "Account Name";
		public const string AccountNumber = "Account Number";
		public const string AccountOwner = "Account Owner";
		public const string ParentAccount = "Parent Account";
		public const string AccountType = "Account Type";
		public const string AccountSite = "Account Site";
		public const string Phone = "Phone";
		public const string BillingStreet = "Billing Street";
		public const string BillingCity = "Billing City";
		public const string BillingState = "Billing State";
		public const string BillingCountry = "Billing Country";
		public const string BillingCode = "Billing Code";
		public const string ShippingStreet = "Shipping Street";
		public const string ShippingCity = "Shipping City";
		public const string ShippingState = "Shipping State";
		public const string ShippingCountry = "Shipping Country";
		public const string ShippingCode = "Shipping Code";
		public const string Emplyees = "Employees";
		public const string Ownership = "Ownership";
		public const string Website = "Website";

		public static readonly IEnumerable<string> All = new[] {
			AccountName,
			AccountNumber,
			AccountOwner,
			ParentAccount,
			AccountType,
			AccountSite,
			Phone,
			BillingStreet,
			BillingCity,
			BillingState,
			BillingCountry,
			BillingCode,
			ShippingStreet,
			ShippingCity,
			ShippingState,
			ShippingCountry,
			ShippingCode,
			Emplyees,
			Ownership,
			Website
		};
	}
}