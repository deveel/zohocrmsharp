using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	public sealed class ZohoInsertResponse<T> : ZohoResponse where T : ZohoEntity {
		private readonly List<RecordDetail> details = new List<RecordDetail>();

		internal ZohoInsertResponse(string module, XDocument doc)
			: base(module, "insertRecords") {
			LoadFromXml(doc.Root);
		}

		public string Message { get; private set; }

		public ICollection<RecordDetail> RecordDetails {
			get { return details.AsReadOnly(); }
		}

		internal override void LoadFromXml(XElement parent) {
			base.LoadFromXml(parent);

			var resultElement = parent.Elements().FirstOrDefault();
			if (resultElement == null ||
				resultElement.Name != "result")
				throw new FormatException();

			var childElements = resultElement.Elements();
			foreach (var child in childElements) {
				if (child.Name == "message") {
					Message = child.Value;
				} else if (child.Name == "recorddetail") {
					var detailElements = child.Elements().OfType<XElement>();
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
}