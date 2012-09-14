using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Xml.Linq;

using RestSharp;

namespace Deveel.Web.Zoho {
	public sealed class ZohoCrmClient {
		private readonly string authToken;

		private const string BaseUrl = "https://crm.zoho.com/crm/private/xml/";

		public ZohoCrmClient(string authToken) {
			if (authToken == null)
				throw new ArgumentNullException("authToken");

			this.authToken = authToken;
		}

		private XDocument PostData(string module, string method, string xmlData) {
			if (module == null)
				throw new ArgumentNullException("module");
			if (method == null)
				throw new ArgumentNullException("method");
			if (xmlData == null)
				throw new ArgumentNullException("xmlData");

			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "{module}/{method}";
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("method", method, ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken);
			request.AddParameter("scope", "crmapi");
			request.AddParameter("xmlData", xmlData);

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			return XDocument.Parse(response.Content);
		}

		private string SelectColumns(string module, IEnumerable<string> selectColumns) {
			var scList = selectColumns == null ? new List<string>() : new List<string>(selectColumns);
			var selectColumnsSb = new StringBuilder();
			if (scList.Count == 0) {
				selectColumnsSb.Append("All");
			} else {
				selectColumnsSb.Append(module);
				selectColumnsSb.Append('(');
				for (int i = 0; i < scList.Count; i++) {
					selectColumnsSb.Append(scList[i]);

					if (i < scList.Count - 1)
						selectColumnsSb.Append(',');
				}
				selectColumnsSb.Append(')');
			}

			return selectColumnsSb.ToString();
		}

		private ZohoEntityCollection<T> Search<T>(string module, IEnumerable<string> selectColumns, ZohoSerachCondition searchCondition) where T : ZohoEntity {
			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "{module}/{method}";
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("method", "getSearchRecords", ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken);
			request.AddParameter("scope", "crmapi");
			request.AddParameter("selectColumns", SelectColumns(module, selectColumns));
			request.AddParameter("searchCondition", searchCondition.ToString());

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			var doc = XDocument.Parse(response.Content);
			var collection = new ZohoEntityCollection<T>();
			collection.LoadFromXml(doc.Root);
			return collection.AsReadOnly();
		}

		private static string ModuleName(Type entityType) {
			var entityName = Attribute.GetCustomAttribute(entityType, typeof(EntityNameAttribute)) as EntityNameAttribute;
			if (entityName == null)
				throw new ArgumentException();

			return entityName.EntityName;
		}

		public ZohoEntityCollection<T> Search<T>(ZohoSerachCondition searchCondition) where T : ZohoEntity {
			return Search<T>(null, searchCondition);
		}

		public ZohoEntityCollection<T> Search<T>(IEnumerable<string> selectColumns, ZohoSerachCondition searchCondition) where T : ZohoEntity {
			return Search<T>(ModuleName(typeof(T)), selectColumns, searchCondition);
		}

		public ZohoInsertResponse<T> InsertRecords<T>(ICollection<T> records) where T : ZohoEntity {
			ZohoEntityCollection<T> collection;
			if (records is ZohoEntityCollection<T>) {
				collection = (ZohoEntityCollection<T>) records;
			} else {
				collection = new ZohoEntityCollection<T>();
				foreach (var record in records) {
					collection.Add(record);
				}
			}

			var moduleName = ModuleName(typeof (T));
			var xmlData = collection.ToXmlString();
			var response = PostData(moduleName, "insertRecords", xmlData);

			return new ZohoInsertResponse<T>(moduleName, response);
		}

		public ZohoInsertResponse<T> InsertRecord<T>(T record) where T : ZohoEntity {
			var collection = new ZohoEntityCollection<T>();
			collection.Add(record);
			return InsertRecords(collection);
		}
	}
}