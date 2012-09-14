using System;
using System.Linq;

using NUnit.Framework;

namespace Deveel.Web.Zoho {
	[TestFixture]
	public abstract class EntityTestBase<T> : ZohoCrmTestBase where T : ZohoEntity {
		protected abstract string Module { get; }

		protected abstract T CreateEntry(int index);

		[Test]
		public void InsertSingleRecord() {
			var client = CreateClient();
			var entry = CreateEntry(0);
			var response = client.InsertRecord(entry);

			Assert.AreEqual(1, response.RecordDetails.Count);

			var detail = response.RecordDetails.First();
			Console.Out.WriteLine("(ID = {0})  Created On {1} by {2}", detail.Id, detail.CreatedDate, detail.CreatedBy);
		}

		[Test]
		public void InsertMultipleRecords() {
			var client = CreateClient();
			var entries = new ZohoEntityCollection<T>();
			for (int i = 0; i < 3; i++) {
				entries.Add(CreateEntry(i));
			}
			
			var response = client.InsertRecords(entries);

			Assert.AreEqual(3, response.RecordDetails.Count);

			foreach (var detail in response.RecordDetails) {
				Console.Out.WriteLine("(ID = {0})  Created On {1} by {2}", detail.Id, detail.CreatedDate, detail.CreatedBy);
			}
		}
	}
}