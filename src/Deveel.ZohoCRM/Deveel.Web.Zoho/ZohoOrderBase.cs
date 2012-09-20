using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	[AllowMultipleInserts(false)]
	public abstract class ZohoOrderBase : ZohoEntity {
		private readonly List<OrderItem> items;

		internal ZohoOrderBase(string subject)
			: this() {
			if (subject == null)
				throw new ArgumentNullException("subject");

			Subject = subject;
		}

		internal ZohoOrderBase() {
			items = new List<OrderItem>(12);
		}

		public string Subject {
			get { return GetString("Subject"); }
			private set { SetValue("Subject", value); }
		}

		public string ContactName {
			get { return GetString("Contact Name"); }
			set { SetValue("Contact Name", value); }
		}

		public string BillingStreet {
			get { return GetString("Billing Street"); }
			set { SetValue("Billing Street", value); }
		}

		public string BillingCity {
			get { return GetString("Billing City"); }
			set { SetValue("Billing City", value); }
		}

		public string BillingState {
			get { return GetString("Billing State"); }
			set { SetValue("Billing State", value); }
		}

		public string BillingCountry {
			get { return GetString("Billing Country"); }
			set { SetValue("Billing Country", value); }
		}

		public string BillingZipCode {
			get { return GetString("Billing Code"); }
			set { SetValue("Billing Code", value); }
		}

		public string ShippingStreet {
			get { return GetString("Shipping Street"); }
			set { SetValue("Shipping Street", value); }
		}

		public string ShippingCity {
			get { return GetString("Shipping City"); }
			set { SetValue("Shipping City", value); }
		}

		public string ShippingState {
			get { return GetString("Shipping State"); }
			set { SetValue("Shipping State", value); }
		}

		public string ShippingCountry {
			get { return GetString("Shipping Country"); }
			set { SetValue("Shipping Country", value); }
		}

		public string ShippingZipCode {
			get { return GetString("Shipping Code"); }
			set { SetValue("Shipping Code", value); }
		}

		public DateTime DueDate {
			get { return GetDateTime("Due Date"); }
			set { SetValue("Due Date", value); }
		}

		public ICollection<OrderItem> Items {
			get { return items; }
		}

		internal override void AppendFieldsToRow(XElement rowElement) {
			base.AppendFieldsToRow(rowElement);

			if (items.Count > 0) {
				var prodDetailsElement = new XElement("FL");
				prodDetailsElement.SetAttributeValue("val", "Product Details");

				int prodNum = 1;
				foreach (var item in Items) {
					item.AppendTo(prodDetailsElement, prodNum++);
				}

				rowElement.Add(prodDetailsElement);
			}
		}

		internal override void LoadFieldFromXml(XElement fieldElement) {
			var valAttr = fieldElement.Attribute("val");
			if (valAttr != null && valAttr.Value == "Product Details") {
				var productElements = fieldElement.Descendants("product");
				foreach (var element in productElements) {
					var item = new OrderItem();
					item.LoadFromXml(element);
					Items.Add(item);
				}
			} else {
				base.LoadFieldFromXml(fieldElement);
			}
		}
	}
}