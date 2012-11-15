using System;

namespace Deveel.Web.Zoho {
	[ModuleName("SalesOrders")]
	public sealed class ZohoSalesOrder : ZohoOrderBase {
		public ZohoSalesOrder(string subject)
			: base(subject) {
		}

		internal ZohoSalesOrder() {
		}

        protected override string IdFieldName {
            get { return "SALESORDERID"; }
        }

		public string AccountName {
			get { return GetString("Account Name"); }
			set { SetValue("Account Name", value); }
		}

		public string CustomerNumber {
			get { return GetString("Customer No"); }
			set { SetValue("Customer No", value); }
		}

		public string PurchaseOrder {
			get { return GetString("Purchase Order"); }
			set { SetValue("Purchase Order", value); }
		}

		public string QuoteName {
			get { return GetString("Quote Name"); }
			set { SetValue("Quote Name", value); }
		}

		public string Owner {
			get { return GetString("Sales Order Owner"); }
			set { SetValue("Sales Order Owner", value); }
		}

		public decimal Commission {
			get { return GetDecimal("Sales Commission"); }
			set { SetValue("Sales Commission", value); }
		}
	}
}