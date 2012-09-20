using System;
using System.Collections.Generic;

namespace Deveel.Web.Zoho {
	public sealed class ListOptions {
		public int? FromIndex { get; set; }

		public int? ToIndex { get; set; }

		public string SortColumn { get; set; }

		public SortOrder SortOrder { get; set; }

		public DateTime? LastModified { get; set; }

		public IEnumerable<string> SelectColumns { get; set; }
	}
}