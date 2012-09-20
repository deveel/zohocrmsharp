using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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

		private ZohoEntityCollection<T> GetEntities<T>(string module, string method, IEnumerable<string> selectColumns, IEnumerable<KeyValuePair<string, string>> parameters) where T : ZohoEntity {
			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "{module}/{method}";
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("method", method, ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken);
			request.AddParameter("scope", "crmapi");
			if (selectColumns != null)
				request.AddParameter("selectColumns", SelectColumns(module, selectColumns));
			foreach (var parameter in parameters) {
				request.AddParameter(parameter.Key, parameter.Value);
			}

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			var doc = XDocument.Parse(response.Content);
			var root = doc.Root;
			if (root.Name == "response")
				root = root.Descendants().First();
			if (root.Name == "result")
				root = root.Descendants().First();

			var collection = new ZohoEntityCollection<T>();
			collection.LoadFromXml(root);
			return collection.AsReadOnly();
		}

		private ZohoEntityCollection<T> Search<T>(string module, IEnumerable<string> selectColumns, ZohoSearchCondition searchCondition) where T : ZohoEntity {
			return GetEntities<T>(module, "getSearchRecords", selectColumns,
			                      new Dictionary<string, string> {{"searchContion", searchCondition.ToString()}});
		}

		private static string ModuleName(Type entityType) {
			var entityName = Attribute.GetCustomAttribute(entityType, typeof(EntityNameAttribute)) as EntityNameAttribute;
			if (entityName == null)
				throw new ArgumentException();

			return entityName.EntityName;
		}

		public ZohoEntityCollection<T> Search<T>(ZohoSearchCondition searchCondition) where T : ZohoEntity {
			return Search<T>(null, searchCondition);
		}

		public ZohoEntityCollection<T> Search<T>(IEnumerable<string> selectColumns, ZohoSearchCondition searchCondition) where T : ZohoEntity {
			return Search<T>(ModuleName(typeof(T)), selectColumns, searchCondition);
		}

		public ZohoInsertResponse<T> InsertRecords<T>(ICollection<T> records) where T : ZohoEntity {
			AssertTypeIsNotAbstract(typeof (T));
			AssertAllowInserts(typeof (T));
			AssertAllowMultipleInserts(typeof (T), records);

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

		private static void AssertTypeIsNotAbstract(Type type) {
			if (type.IsAbstract)
				throw new ArgumentException("Abstract types cannot be used for this function");
		}

		private static void AssertAllowMultipleInserts<T>(Type type, ICollection<T> records) {
			var attr = Attribute.GetCustomAttribute(type, typeof (AllowMultipleInsertsAttribute)) as AllowMultipleInsertsAttribute;
			if (attr != null && 
				(!attr.Allow && records.Count > 1))
				throw new ArgumentException("Entity " + type.Name + " cannot be inserted more than one record at a time.");
		}

		private void AssertAllowInserts(Type type) {
			var attr = Attribute.GetCustomAttribute(type, typeof (AllowInsertsAttribute)) as AllowInsertsAttribute;
			if (attr != null && !attr.Allow)
				throw new ArgumentException("Entity " + type.Name + " cannot be inserted.");
		}

		public ZohoInsertResponse<T> InsertRecord<T>(T record) where T : ZohoEntity {
			return InsertRecords(new ZohoEntityCollection<T> { record });
		}

		public T GetRecordById<T>(string id) where T :ZohoEntity {
			var collection = GetEntities<T>(ModuleName(typeof (T)), "getRecordById", null, new Dictionary<string, string> {{"id", id}});
			if (collection.Count == 0)
				return null;
			if (collection.Count > 1)
				throw new AmbiguousMatchException("More than one entity was found for the given id");

			return collection.SingleOrDefault();

		}
	}
}