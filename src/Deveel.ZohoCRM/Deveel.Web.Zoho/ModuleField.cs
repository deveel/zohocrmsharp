using System;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	public sealed class ModuleField {
		internal ModuleField() {
		}

		public bool IsRequired { get; private set; }

		public FieldType FieldType { get; private set; }

		public string Label { get; private set; }

		public bool IsCustom { get; private set; }

		internal void LoadFromXml(XElement element) {
			
		}
	}
}