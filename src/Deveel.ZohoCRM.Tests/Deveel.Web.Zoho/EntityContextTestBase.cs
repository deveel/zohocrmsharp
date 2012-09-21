using System;
using System.Linq;

using NUnit.Framework;

namespace Deveel.Web.Zoho {
	[TestFixture]
	public abstract class EntityContextTestBase<T> : ZohoCrmTestBase where T : ZohoEntity {
		protected abstract string ModuleName { get; }

		protected abstract T CreateTestObject();

		protected virtual  string IdColumn {
			get { return null; }
		}

		protected virtual string NonDuplicateFieldName {
			get { return null; }
		}

		[Test]
		public void CreateAndSearchByUniqueField() {
			var client = CreateClient();

			var context = client.GetContext<T>();

			var obj = CreateTestObject();
			var response = context.Insert(obj);
			
			Assert.AreEqual(1, response.RecordDetails.Count());

			var objId = response.RecordDetails.First().Id;
			var uniqueValue = obj.GetValue<object>(NonDuplicateFieldName);
			var result = context.Is(NonDuplicateFieldName, uniqueValue);

			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(objId, result.First().Id);
		}

		[Test]
		public void SearchNonExisting() {
			var client = CreateClient();

			var context = client.GetContext<T>();

			var obj = CreateTestObject();
			var uniqueValue = obj.GetValue<object>(NonDuplicateFieldName);
			var result = context.Is(NonDuplicateFieldName, uniqueValue);

			Assert.AreEqual(0, result.Count);
		}
	}
}