using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Deveel.Web.Zoho
{
	public abstract class ZohoEntity
	{
		private readonly Dictionary<string, string> fields = new Dictionary<string, string>();

		protected ZohoEntity() {
			var entityName = Attribute.GetCustomAttribute(GetType(), typeof (EntityNameAttribute)) as EntityNameAttribute;
			if (entityName == null)
				throw new InvalidOperationException();

			EntityName = entityName.EntityName;
		}

		internal string EntityName { get; private set; }

		public string GetValue(string fieldName) {
			string value;
			if (!fields.TryGetValue(fieldName, out value))
				return null;

			return value;
		}

		public void SetValue(string fieldName, object value) {
			string strValue = null;
			if (value != null)
				strValue = value.ToString();

			if (String.IsNullOrEmpty(strValue)) {
				fields.Remove(fieldName);
			} else {
				fields[fieldName] = strValue;
			}
		}

		internal void ToXmlString(StringBuilder sb, int rowNum = 1) {
			sb.AppendFormat("<row no=\"{0}\">", rowNum);
			foreach (var pair in fields) {
				sb.AppendFormat("<FL val=\"{0}\">{1}</FL>", pair.Key, pair.Value);
			}
			sb.Append("</row>");
		}

		public string ToXmlString() {
			var sb = new StringBuilder();
			sb.AppendFormat("<{0}>", EntityName);
			ToXmlString(sb);
			sb.AppendFormat("</{0}>", EntityName);
			return sb.ToString();
		}

		internal virtual void LoadFromXml(XElement element)
		{
			var fieldNodes = element.Descendants();
			foreach (var node in fieldNodes)
			{
				var key = node.Attribute("val");
				var value = node.Nodes().OfType<XText>().FirstOrDefault();

				if (key == null)
					throw new FormatException();
				if (value == null)
					throw new FormatException();

				SetValue(key.Value, value.Value);
			}
		}
	}
}