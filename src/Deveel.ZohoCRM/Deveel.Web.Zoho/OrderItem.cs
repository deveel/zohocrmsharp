using System;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	[AllowInserts(false)]
	public sealed class OrderItem : ZohoEntity {
		public OrderItem(string productId, decimal unitPrice, int quantity) {
			if (productId == null)
				throw new ArgumentNullException("productId");

			ProductId = productId;
			UnitPrice = unitPrice;
			Quantity = quantity;
		}

		internal OrderItem() {
		}
		
		public string ProductId {
			get { return GetString("Product Id"); }
			private set { SetValue("Product Id", value); }
		}

		public decimal UnitPrice {
			get { return GetDecimal("Unit Price"); }
			set { SetValue("Unit Price", value); }
		}

		public int Quantity {
			get { return GetInt32("Quantity"); }
			set { SetValue("Quantity", value); }
		}

		// TODO: make this a readonly?
		public decimal Total {
			get { return GetDecimal("Total"); }
			set { SetValue("Total", value);}
		}

		public decimal Discount {
			get { return GetDecimal("Discount"); }
			set { SetValue("Discount", value); }
		}

		// TODO: make this a readonly?
		public decimal TotalAfterDiscount {
			get { return GetDecimal("Total After Discount"); }
			set { SetValue("Total After Discount", value);}
		}

		public decimal ListPrice {
			get { return GetDecimal("List Price"); }
			set { SetValue("List Price", value); }
		}

		// TODO: make this a readonly?
		public decimal NetTotal {
			get { return GetDecimal("Net Total"); }
			set { SetValue("Net Total", value); }
		}

		internal override void AppendTo(XElement parent, int prodNum = 1) {
			var productElement = new XElement("product");
			productElement.SetAttributeValue("no", prodNum);

			AppendFieldsToRow(productElement);
		}

		internal override void LoadFromXml(XElement element) {
			var childElement = element.Descendants("FL");
			foreach (var child in childElement) {
				LoadFieldFromXml(child);
			}
		}
	}
}