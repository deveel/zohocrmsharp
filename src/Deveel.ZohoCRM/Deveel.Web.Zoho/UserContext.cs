using System;
using System.Collections.Generic;

namespace Deveel.Web.Zoho {
	public sealed class UserContext {
		internal UserContext(ZohoCrmClient client,  ZohoUser user) {
			if (user == null)
				throw new ArgumentNullException("user");

			Client = client;
			User = user;
		}

		private ZohoCrmClient Client { get; set; }

		public ZohoUser User { get; private set; }

		public ZohoInsertResponse InsertRecord<T>(T record) where T : ZohoEntity {
			if (record == null)
				throw new ArgumentNullException("record");

			record.SetValue("SMOWNERID", User.Id);
			return Client.InsertRecord(record);
		}

		public ZohoInsertResponse InsertRecords<T>(IEnumerable<T> records) where T : ZohoEntity {
			var list = new List<T>();
			foreach (var record in records) {
				record.SetValue("SMOWNERID", User.Id);
			}

			return Client.InsertRecords(list);
		}

		public bool GetOwnership<T>(T record) where T:ZohoEntity {
			record.SetValue("SMOWNERID", User.Id);
			return Client.UpdateRecord(record);
		}
	}
}