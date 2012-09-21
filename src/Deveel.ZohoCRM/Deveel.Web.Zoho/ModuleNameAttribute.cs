using System;

namespace Deveel.Web.Zoho {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	sealed class ModuleNameAttribute : Attribute {
		public ModuleNameAttribute(string entityName) {
			Name = entityName;
		}

		public string Name { get; private set; }
	}
}