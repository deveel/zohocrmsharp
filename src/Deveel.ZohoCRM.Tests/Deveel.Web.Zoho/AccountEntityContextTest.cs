using System;

using NUnit.Framework;

namespace Deveel.Web.Zoho {
	[TestFixture]
	public sealed class AccountEntityContextTest : EntityContextTestBase<ZohoAccount> {
		protected override string ModuleName {
			get { return "Accounts"; }
		}

		protected override string IdColumn {
			get { return AccountDefaultFields.AccountId; }
		}

		protected override string NonDuplicateFieldName {
			get { return AccountDefaultFields.AccountName;}
		}

		protected override ZohoAccount CreateTestObject() {
			return new ZohoAccount("Test Account " + DateTime.Now.Ticks);
		}
	}
}