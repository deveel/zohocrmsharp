using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	public sealed class ModuleSection {
		private readonly List<ModuleField> fields = new List<ModuleField>(12);

		internal ModuleSection(string sectionName) {
			SectionName = sectionName;
		}

		public string SectionName { get; private set; }

		public IEnumerable<ModuleField> Fields {
			get { return fields.AsReadOnly(); }
		} 

		internal void LoadFromXml(XElement element) {
			
		}
	}
}