using System;

namespace Deveel.Web.Zoho {
	[ModuleName("Invoices")]
	public sealed class ZohoInvoice : ZohoOrderBase {
		public ZohoInvoice(string subject)
			: base(subject) {
		}

		internal ZohoInvoice() {
		}

        protected override string IdFieldName {
            get { return "INVOICEID"; }
        }

		public string AccountName {
			get { return GetString("Account Name"); }
			set { SetValue("Account Name", value); }
		}

		public string ContactID {
			get { return GetString("CONTACTID"); }
			set { SetValue("CONTACTID", value); }
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
			get { return GetString("Invoice Number"); }
			set { SetValue("Invoice Number", value); }
		}

		public string Status {
			get { return GetString("Status"); }
			set { SetValue("Status", value); }
		}

		public decimal Discount {
			get { return GetDecimal("Discount"); }
			set { SetValue("Discount", value); }
		}

		public decimal GrandTotal {
			get { return GetDecimal("Grand Total"); }
			set { SetValue("Grand Total", value); }
		}

		public decimal Tax {
			get { return GetDecimal("Tax"); }
			set { SetValue("Tax", value); }
		}

		public decimal SubTotal {
			get { return GetDecimal("Sub Total"); }
			set { SetValue("Sub Total", value); }
		}

		public decimal NetTotal {
			get { return GetDecimal("Net Total"); }
			set { SetValue("Net Total", value); }
		}

        public DateTime InvoiceDate
        {
            get { return GetDateTime("Invoice Date"); }
            set { SetValue("Invoice Date", value); }
        }

    }
}
