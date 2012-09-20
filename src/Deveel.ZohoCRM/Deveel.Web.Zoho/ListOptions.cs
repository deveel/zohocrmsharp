using System;

namespace Deveel.Web.Zoho {
	public sealed class ListOptions {
		public int? FromIndex { get; set; }

		public int? ToIndex { get; set; }

		public SortColumn SortBy { get; set; }

		public DateTime? LastModified { get; set; }
	}
}