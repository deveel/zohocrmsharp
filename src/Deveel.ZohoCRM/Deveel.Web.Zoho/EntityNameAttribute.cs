using System;

namespace Deveel.Web.Zoho {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	sealed class EntityNameAttribute : Attribute {
		public EntityNameAttribute(string entityName) {
			EntityName = entityName;
		}

		public string EntityName { get; private set; }
	}
}