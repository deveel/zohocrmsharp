using System;

namespace Deveel.Web.Zoho {
	public sealed class SortColumn {
		public SortColumn(string columnName) 
			: this(columnName, SortOrder.Ascending) {
		}

		public SortColumn(string columnName, SortOrder order) {
			if (columnName == null)
				throw new ArgumentNullException("columnName");

			Order = order;
			ColumnName = columnName;
		}

		public string ColumnName { get; private set; }

		public SortOrder Order { get; private set; }
	}
}