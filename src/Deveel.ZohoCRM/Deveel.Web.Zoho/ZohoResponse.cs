using System;
using System.Linq;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	public class ZohoResponse {
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

		public bool IsError { get; private set; }

		public string Message { get; private set; }

		public string Code { get; private set; }

		internal void LoadFromXml(XElement parent) {
			if (parent.Name != "response")
				throw new FormatException();

			var firstChild = parent.Elements().First();
			if (firstChild.Name == "error") {
				IsError = true;
				var code = firstChild.Descendants("code").First();
				var message = firstChild.Descendants("message").First();
				if (code != null)
					Code = code.Value;
				if (message != null)
					Message = message.Value;			
			} else if (firstChild.Name == "result") {
				LoadResultFromXml(firstChild);
			}
		}

		internal virtual void LoadResultFromXml(XElement resultElement) {
			var code = resultElement.Descendants("code").First();
			var message = resultElement.Descendants("message").First();
			if (code != null)
				Code = code.Value;
			if (message != null)
				Message = message.Value;			
		}
	}
}