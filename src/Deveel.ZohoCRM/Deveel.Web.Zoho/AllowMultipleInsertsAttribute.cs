using System;

namespace Deveel.Web.Zoho {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	class AllowMultipleInsertsAttribute : Attribute {
		public AllowMultipleInsertsAttribute(bool allow) {
			Allow = allow;
		}

		public bool Allow { get; private set; }
	}
}