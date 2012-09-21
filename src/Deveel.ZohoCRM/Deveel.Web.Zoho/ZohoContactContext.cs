using System;

namespace Deveel.Web.Zoho {
	public sealed class ZohoContactContext : ZohoEntityContext<ZohoContact> {
		public ZohoContactContext(ZohoCrmClient client) 
			: base(client) {
		}

		public ZohoEntityCollection<ZohoContact> GetByEmail(string email) {
			if (email == null)
				throw new ArgumentNullException("email");

			// Ideally, there shouldn't be more than one contact for the given email
			// but it seems Zoho doesn't forbids addition of multiple contacts for
			// the same email ... so we return a list...
			return Is("Email", email);
		}

		public ZohoEntityCollection<ZohoLead> ListLeads(string id) {
			return ListRelated<ZohoLead>(id);
		}

		public ZohoInsertResponse AddLead(string id, ZohoLead lead) {
			if (lead == null)
				throw new ArgumentNullException("lead");

			lead.Owner = RequireById(id).Email;
			return Client.InsertRecord(lead);
		}

		public bool TransferLead(string id, ZohoLead lead) {
			if (lead == null)
				throw new ArgumentNullException("lead");

			lead.Owner = RequireById(id).Email;
			return Client.UpdateRecord(lead);
		}

		public ZohoEntityCollection<ZohoSalesOrder> ListSalesOrders(string id) {
			return ListRelated<ZohoSalesOrder>(id);
		} 

		public ZohoInsertResponse AddSalesOrder(string id, ZohoSalesOrder salesOrder) {
			if (salesOrder == null)
				throw new ArgumentNullException("salesOrder");

			salesOrder.ContactName = RequireById(id).Email;
			return Client.InsertRecord(salesOrder);
		}
	}
}