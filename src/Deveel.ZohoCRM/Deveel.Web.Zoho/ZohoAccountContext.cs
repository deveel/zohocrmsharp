using System;

namespace Deveel.Web.Zoho {
	public sealed class ZohoAccountContext : ZohoEntityContext<ZohoAccount> {
		public ZohoAccountContext(ZohoCrmClient client) 
			: base(client) {
		}

		public ZohoEntityCollection<ZohoContact> ListContacts(string id) {
			return ListRelated<ZohoContact>(id);
		}

		public ZohoEntityCollection<ZohoSalesOrder> ListSalesOrders(string id) {
			return ListRelated<ZohoSalesOrder>(id);
		}

		/*
		public ZohoEntityCollection<ZohoPurchaseOrder> ListPurchaseOrdsers(string id) {
			return ListRelated<ZohoPurchaseOrder>(id);
		}
		*/

		public ZohoEntityCollection<ZohoAccount> ListChildAccounts(string id) {
			return ListRelated<ZohoAccount>(id);
		}

		public ZohoEntityCollection<ZohoProduct> ListProducts(string id) {
			return ListRelated<ZohoProduct>(id);
		}

		public ZohoInsertResponse AddChild(string id, ZohoAccount account) {
			if (account == null)
				throw new ArgumentNullException("account");

			account.ParentAccount = RequireById(id).Name;
			return Client.InsertRecord(account);
		}

		public bool TransferChild(string id, ZohoAccount account) {
			if (account == null)
				throw new ArgumentNullException("account");

			account.ParentAccount = RequireById(id).Name;
			return Update(account);
		}

		public ZohoInsertResponse AddSalesOrder(string id, ZohoSalesOrder salesOrder) {
			if (salesOrder == null)
				throw new ArgumentNullException("salesOrder");

			salesOrder.AccountName = RequireById(id).Name;
			return Client.InsertRecord(salesOrder);
		}

		public bool TransferSalesOrder(string id, ZohoSalesOrder salesOrder) {
			if (salesOrder == null)
				throw new ArgumentNullException("salesOrder");

			salesOrder.AccountName = RequireById(id).Name;
			return Client.UpdateRecord(salesOrder);
		}

		public ZohoInsertResponse AddContact(string id, ZohoContact contact) {
			if (contact == null)
				throw new ArgumentNullException("contact");

			contact.AccountName = RequireById(id).Name;
			return Client.InsertRecord(contact);
		}

		public bool TransferContact(string id, ZohoContact contact) {
			if (contact == null)
				throw new ArgumentNullException("contact");

			contact.AccountName = RequireById(id).Name;
			return Client.UpdateRecord(contact);
		}
	}
}