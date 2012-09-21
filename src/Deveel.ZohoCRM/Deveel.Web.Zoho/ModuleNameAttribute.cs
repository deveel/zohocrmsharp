using System;

namespace Deveel.Web.Zoho {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	sealed class ModuleNameAttribute : Attribute {
		public ModuleNameAttribute(string entityName) {
			EntityName = entityName;
		}

		public string EntityName { get; private set; }
	}
}