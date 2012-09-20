using System;
using System.Configuration;
using System.IO;

using NUnit.Framework;

namespace Deveel.Web.Zoho {
	[TestFixture]
	public abstract class ZohoCrmTestBase {
		protected string AuthToken { get; private set; }

		[TestFixtureSetUp]
		public void TextFixtureSetUp() {
			var fileName = ConfigurationManager.AppSettings["authTokenFileName"];
			if (String.IsNullOrEmpty(fileName))
				fileName = "authToken.txt";

			var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
			if (!File.Exists(filePath))
				Assert.Fail("File " + filePath + " was not found!");

			using (var reader = new StreamReader(filePath)) {
				AuthToken = reader.ReadLine();
			}

			if (String.IsNullOrEmpty(AuthToken))
				Assert.Fail("No authToken set fund for testing");
		}

		protected ZohoCrmClient CreateClient() {
			return new ZohoCrmClient(AuthToken);
		}
	}
}