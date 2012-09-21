using System;

namespace Deveel.Web.Zoho {
	[ModuleName("PurchaseOrders")]
	public sealed class ZohoPurchaseOrder : ZohoOrderBase {
		public ZohoPurchaseOrder(string subject)
			: base(subject) {
		}

		internal ZohoPurchaseOrder() {
		}

		public string Number {
			get { return GetString("PO Number"); }
			set { SetValue("PO Number", value); }
		}

		public DateTime Date {
			get { return GetDateTime("PO Date"); }
			set { SetValue("PO Date", value); }
		}

		public string VendorName {
			get { return GetString("Vendor Name"); }
			set { SetValue("Vendor Name", value); }
		}

		public string Owner {
			get { return GetString("Purchase Order Owner"); }
			set { SetValue("Purchase Order Owner", value); }
		}

		public string Status {
			get { return GetString("Status"); }
			set { SetValue("Status", value); }
		}
	}
}