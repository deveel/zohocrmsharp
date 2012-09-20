using System;

namespace Deveel.Web.Zoho {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	class AllowInsertsAttribute : Attribute {
		public AllowInsertsAttribute(bool allow) {
			Allow = allow;
		}

		public bool Allow { get; private set; }
	}
}