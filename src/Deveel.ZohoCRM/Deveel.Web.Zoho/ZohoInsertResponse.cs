using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	public sealed class ZohoInsertResponse : ZohoResponse {
		private readonly List<RecordDetail> details = new List<RecordDetail>();

		internal ZohoInsertResponse(string module, string method, XDocument doc)
			: base(module, method) {
			LoadFromXml(doc.Root);
		}

		public IEnumerable<RecordDetail> RecordDetails {
			get { return details.AsReadOnly(); }
		}

		internal override void LoadResultFromXml(XElement resultElement) {
			var recordDetailsElements = resultElement.Descendants("recorddetail");
			foreach (var element in recordDetailsElements) {
				var detailElements = element.Descendants();
				var detail = new RecordDetail();

				foreach (var detailElement in detailElements) {
					var detailKey = detailElement.Attribute("val").Value;
					var detailValue = detailElement.Value;

					if (detailKey == "Id") {
						detail.Id = detailValue;
					} else if (detailKey == "Created Time") {
						DateTime createdDate;
						if (DateTime.TryParse(detailValue, out createdDate))
							detail.CreatedDate = createdDate;
					} else if (detailKey == "Modified Time") {
						DateTime modifiedDate;
						if (DateTime.TryParse(detailValue, out modifiedDate))
							detail.ModifiedDate = modifiedDate;
					} else if (detailKey == "Created By") {
						detail.CreatedBy = detailValue;
					} else if (detailKey == "Modified By") {
						detail.ModifiedBy = detailValue;
					}
				}

				details.Add(detail);				
			}
		}
	}
}