using System;

namespace Deveel.Web.Zoho {
	public sealed class ZohoProductContext : ZohoEntityContext<ZohoProduct> {
		public ZohoProductContext(ZohoCrmClient client) 
			: base(client) {
		}

		public ZohoEntityCollection<ZohoProduct> GetByName(string productName) {
			if (productName == null)
				throw new ArgumentNullException("productName");

			return Is("Product Name", productName);
		}

		public ZohoEntityCollection<ZohoAccount> ListAccounts(string id) {
			return ListRelated<ZohoAccount>(id);
		} 

		public ZohoEntityCollection<ZohoContact> ListContacts(string id) {
			return ListRelated<ZohoContact>(id);
		}

		public ZohoEntityCollection<ZohoLead> ListLeads(string id) {
			return ListRelated<ZohoLead>(id);
		} 

		public ZohoEntityCollection<ZohoPotential> ListPotentials(string id) {
			return ListRelated<ZohoPotential>(id);
		}
	}
}