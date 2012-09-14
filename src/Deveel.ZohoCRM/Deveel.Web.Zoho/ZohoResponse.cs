using System;
using System.Linq;
using System.Xml.Linq;

namespace Deveel.Web.Zoho
{
	public class ZohoResponse
	{
		internal ZohoResponse(string module, string method) {
			if (module == null)
				throw new ArgumentNullException("module");
			if (method == null)
				throw new ArgumentNullException("method");

			Method = method;
			Module = module;
		}

		public string Module { get; private set; }

		public string Method { get; private set; }

		internal virtual void LoadFromXml(XElement parent) {
			if (parent.Name != "response")
				throw new FormatException();

			var firstChild = parent.Elements().OfType<XElement>().First();
			if (firstChild.Name == "error") {
				//TODO:
			}
		}
	}
}