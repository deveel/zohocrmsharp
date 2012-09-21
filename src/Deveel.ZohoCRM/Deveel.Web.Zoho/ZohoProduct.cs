using System;

namespace Deveel.Web.Zoho {
	[ModuleName("Products")]
	public sealed class ZohoProduct : ZohoEntity {
		public ZohoProduct(string name) {
			if (name == null)
				throw new ArgumentNullException("name");

			Name = name;
		}

		internal ZohoProduct() {
		}

		protected override string IdFieldName {
			get { return "PRODUCTID"; }
		}

		public string Name {
			get { return GetString("Product Name"); }
			private set { SetValue("Product Name", value); }
		}

		public string Currency {
			get { return GetString("Currency"); }
			set { SetValue("Currency", value); }
		}

		public decimal UnitPrice {
			get { return GetDecimal("Unit Price"); }
			set { SetValue("Unit Price", value); }
		}

		public string Code {
			get { return GetString("Product Code"); }
			set { SetValue("Product Code", value); }
		}

		public string Category {
			get { return GetString("Product Category"); }
			set { SetValue("Product Category", value); }
		}

		public string VendorName {
			get { return GetString("Vendor Name"); }
			set { SetValue("Vendor Name", value); }
		}

		public int ExchangeRate {
			get { return GetInt32("Exchange Rate"); }
			set { SetValue("Exchange Rate", value); }
		}
	}
}