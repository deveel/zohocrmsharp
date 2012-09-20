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

			Assert.AreEqual(1, response.RecordDetails.Count());

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

			Assert.AreEqual(3, response.RecordDetails.Count());

			foreach (var detail in response.RecordDetails) {
				Console.Out.WriteLine("(ID = {0})  Created On {1} by {2}", detail.Id, detail.CreatedDate, detail.CreatedBy);
			}
		}

		[Test]
		public void InsertTwiceTheSameRecordAtTheSameTime() {
			var client = CreateClient();
			var records = new ZohoEntityCollection<T> {
				CreateEntry(0), 
				CreateEntry(0)
			};

			var response = client.InsertRecords(records);
		}

		[Test]
		public void InsertAndGetById() {
			var client = CreateClient();

			var response = client.InsertRecord(CreateEntry(0));

			Assert.AreEqual(1, response.RecordDetails.Count());

			var id = response.RecordDetails.First().Id;
			var record = client.GetRecordById<T>(id);

			Assert.IsNotNull(record);
			Assert.AreEqual(id, record.Id);
		}

		[Test]
		public void InsertAndDeleteById() {
			var client = CreateClient();

			var response = client.InsertRecord(CreateEntry(0));

			Assert.AreEqual(1, response.RecordDetails.Count());

			var id = response.RecordDetails.First().Id;
			Assert.IsTrue(client.DeleteRecordById<T>(id));
		}

		[Test]
		public void InsertAndDeleteByObject() {
			var client = CreateClient();

			var response = client.InsertRecord(CreateEntry(0));

			Assert.AreEqual(1, response.RecordDetails.Count());

			var id = response.RecordDetails.First().Id;
			var record = client.GetRecordById<T>(id);

			Assert.IsNotNull(record);
			Assert.AreEqual(id, record.Id);

			Assert.IsTrue(client.DeleteRecord(record));

			record = client.GetRecordById<T>(id);

			Assert.IsNull(record);
		}

		[Test]
		public void DeleteByInvalidId() {
			var client = CreateClient();

			Assert.IsFalse(client.DeleteRecordById<T>("invalidId"));
		}
	}
}