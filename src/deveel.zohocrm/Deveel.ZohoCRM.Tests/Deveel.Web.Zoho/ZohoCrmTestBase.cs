using System;
using System.Configuration;

using NUnit.Framework;

namespace Deveel.Web.Zoho {
	[TestFixture]
	public abstract class ZohoCrmTestBase {
		protected string AuthToken { get; private set; }

		[TestFixtureSetUp]
		public void TextFixtureSetUp() {
			AuthToken = ConfigurationManager.AppSettings["authToken"];
		}

		protected ZohoCrmClient CreateClient() {
			return new ZohoCrmClient(AuthToken);
		}
	}
}