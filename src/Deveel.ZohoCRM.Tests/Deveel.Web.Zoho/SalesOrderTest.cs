using System;
using System.Linq;

using NUnit.Framework;

namespace Deveel.Web.Zoho {
	public sealed class SalesOrderTest : EntityTestBase<ZohoSalesOrder> {
		protected override string Module {
			get { return "SalesOrders"; }
		}

		protected override ZohoSalesOrder CreateEntry(int index) {
			return new ZohoSalesOrder("Order " + index);
		}

		[Test]
		public void CreateProductsAndPlaceOrder() {
			var client = CreateClient();

			var testProduct = new ZohoProduct("Test Product " + DateTime.Now.Ticks);
			testProduct.Code = Guid.NewGuid().ToString("N");
			testProduct.Currency = "USD";
			testProduct.UnitPrice = new decimal(22.34);

			var response = client.InsertRecord(testProduct);

			Assert.AreEqual(1, response.RecordDetails.Count());

			var productId = response.RecordDetails.First().Id;

			var order = CreateEntry(0);
			order.Items.Add(new OrderItem(productId, testProduct.UnitPrice, 2));
			response = client.InsertRecord(order);

			Assert.AreEqual(1, response.RecordDetails.Count());

			var orderId = response.RecordDetails.First().Id;
			var placedOrder = client.GetRecordById<ZohoSalesOrder>(orderId);

			Assert.IsNotNull(placedOrder);
			Assert.AreEqual(order.Subject, placedOrder.Subject);
			Assert.AreEqual(1, placedOrder.Items.Count);
		}
	}
}