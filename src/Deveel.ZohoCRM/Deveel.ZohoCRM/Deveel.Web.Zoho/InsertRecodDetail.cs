using System;

namespace Deveel.Web.Zoho {
	public sealed class RecordDetail {
		internal RecordDetail() {
		}

		public string CreatedBy { get; internal set; }

		public string ModifiedBy { get; internal set; }

		public DateTime CreatedDate { get; internal set; }

		public DateTime ModifiedDate { get; internal set; }

		public string Id { get; internal set; }
	}
}