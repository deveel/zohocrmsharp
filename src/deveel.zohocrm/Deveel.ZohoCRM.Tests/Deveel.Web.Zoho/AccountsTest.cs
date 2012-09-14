using System;

namespace Deveel.Web.Zoho {
	public sealed class AccountsTest : EntityTestBase<ZohoAccount> {
		protected override string Module {
			get { return "Accounts"; }
		}

		protected override ZohoAccount CreateEntry(int index) {
			return new ZohoAccount("TestAccount " + index);
		}
	}
}