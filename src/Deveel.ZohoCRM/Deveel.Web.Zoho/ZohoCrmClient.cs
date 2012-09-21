using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

using RestSharp;

namespace Deveel.Web.Zoho {
	public sealed partial class ZohoCrmClient {
		private readonly string authToken;

		private const string BaseUrl = "https://crm.zoho.com/crm/private/xml/";

		public ZohoCrmClient(string authToken) {
			if (authToken == null)
				throw new ArgumentNullException("authToken");

			this.authToken = authToken;
		}

		private static void AssertTypeIsNotAbstract(Type type) {
			if (type.IsAbstract)
				throw new ArgumentException("Abstract types cannot be used for this function");
		}

		private static void AssertAllowMultipleInserts<T>(Type type, IEnumerable<T> records) {
			var attr = Attribute.GetCustomAttribute(type, typeof (AllowMultipleInsertsAttribute)) as AllowMultipleInsertsAttribute;
			if (attr != null && 
			    (!attr.Allow && records.Count() > 1))
				throw new ArgumentException("Entity " + type.Name + " cannot be inserted more than one record at a time.");
		}

		private void AssertAllowInserts(Type type) {
			var attr = Attribute.GetCustomAttribute(type, typeof (AllowInsertsAttribute)) as AllowInsertsAttribute;
			if (attr != null && !attr.Allow)
				throw new ArgumentException("Entity " + type.Name + " cannot be inserted.");
		}

		private static string ModuleName(Type entityType) {
			var entityName = Attribute.GetCustomAttribute(entityType, typeof(ModuleNameAttribute)) as ModuleNameAttribute;
			if (entityName == null)
				throw new ArgumentException();

			return entityName.EntityName;
		}

		private static string ModuleName<T>() {
			return ModuleName(typeof (T));
		}

		private static string SelectColumns(string module, IEnumerable<string> selectColumns) {
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

		private static IDictionary<string, string> GetListOptionsParameters(string module, ListOptions options) {
			var parameters = new Dictionary<string, string>();
			if (options.FromIndex != null)
				parameters.Add("fromIndex", options.FromIndex.Value.ToString(CultureInfo.InvariantCulture));
			if (options.ToIndex != null)
				parameters.Add("toIndex", options.ToIndex.Value.ToString(CultureInfo.InvariantCulture));
			if (options.LastModified != null)
				parameters.Add("lastModifiedTime", options.LastModified.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			if (!String.IsNullOrEmpty(options.SortColumn))
				parameters.Add("sortColumnString", options.SortColumn);
			if (options.SortOrder != SortOrder.Default)
				parameters.Add("sortOrderString", options.SortOrder == SortOrder.Ascending ? "asc" : "desc");
			if (options.SelectColumns != null) 
				parameters.Add("selectColumns", SelectColumns(module, options.SelectColumns));
			return parameters;			
		}

		private string GetData(string module, string method, IEnumerable<KeyValuePair<string, string>> parameters) {
			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.GET);
			request.Resource = "{module}/{method}";
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("method", method, ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken);
			request.AddParameter("scope", "crmapi");
			foreach (var parameter in parameters) {
				request.AddParameter(parameter.Key, parameter.Value);
			}

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			return response.Content;
		}

		private ZohoResponse GetResponse(string module, string method, IEnumerable<KeyValuePair<string, string>> parameters) {
			var content = GetData(module, method, parameters);
			return new ZohoResponse(module, method, content);
		}

		private ZohoEntityCollection<T> GetEntities<T>(string module, string method, IEnumerable<KeyValuePair<string, string>> parameters) where T : ZohoEntity {
			var response = GetData(module, method, parameters);

			var doc = XDocument.Parse(response);
			var root = doc.Root;
			if (root.Name == "response")
				root = root.Descendants().First();
			if (root.Name == "result")
				root = root.Descendants().First();

			var collection = new ZohoEntityCollection<T>();
			collection.LoadFromXml(root);
			return collection.AsReadOnly();
		}

		private ZohoInsertResponse PostData(string module, string method, IDictionary<string, string> parameters, string xmlData) {
			if (module == null)
				throw new ArgumentNullException("module");
			if (method == null)
				throw new ArgumentNullException("method");

			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "{module}/{method}";
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("method", method, ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken);
			request.AddParameter("scope", "crmapi");
			if (parameters != null) {
				foreach (var parameter in parameters) {
					request.AddParameter(parameter.Key, parameter.Value);
				}
			}

			if (!String.IsNullOrEmpty(xmlData))
				request.AddParameter("xmlData", xmlData);

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			return new ZohoInsertResponse(module, method, response.Content);
		}

		private ZohoEntityCollection<T> Search<T>(string module, IEnumerable<string> selectColumns, ZohoSearchCondition searchCondition) where T : ZohoEntity {
			var parameters = GetListOptionsParameters(module, new ListOptions {SelectColumns = selectColumns});
			parameters.Add("searchCondition", searchCondition.ToString());
			return GetEntities<T>(module, "getSearchRecords", parameters);
		}

		public ZohoEntityCollection<T> Search<T>(ZohoSearchCondition searchCondition) where T : ZohoEntity {
			return Search<T>(searchCondition, null);
		}

		public ZohoEntityCollection<T> Search<T>(ZohoSearchCondition searchCondition, IEnumerable<string> selectColumns) where T : ZohoEntity {
			return Search<T>(ModuleName(typeof(T)), selectColumns, searchCondition);
		}

		public ZohoEntityCollection<T> Search<T>(string column, ConditionOperator @operator, object value) where T : ZohoEntity {
			return Search<T>(new ZohoSearchCondition(column, @operator, value));
		}

		public ZohoInsertResponse InsertRecords<T>(IEnumerable<T> records) where T : ZohoEntity {
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
			return PostData(moduleName, "insertRecords", null, xmlData);
		}

		public ZohoInsertResponse InsertRecord<T>(T record) where T : ZohoEntity {
			return InsertRecords(new ZohoEntityCollection<T> { record });
		}

		public T GetRecordById<T>(string id) where T :ZohoEntity {
			var collection = GetEntities<T>(ModuleName(typeof (T)), "getRecordById", new Dictionary<string, string> {{"id", id}});
			if (collection.Count == 0)
				return null;
			if (collection.Count > 1)
				throw new AmbiguousMatchException("More than one entity was found for the given id");

			return collection.SingleOrDefault();
		}

		public ZohoEntityCollection<T> GetRecords<T>(ListOptions options) where T : ZohoEntity {
			var moduleName = ModuleName<T>();
			return GetEntities<T>(moduleName, "getRecords", GetListOptionsParameters(moduleName, options));
		}

		public ZohoEntityCollection<T> GetMyRecords<T>(ListOptions options) where T : ZohoEntity {
			var moduleName = ModuleName<T>();
			return GetEntities<T>(moduleName, "getMyRecords", GetListOptionsParameters(moduleName, options));			
		}

		public bool UpdateRecord<T>(T record) where T : ZohoEntity {
			if (record == null)
				throw new ArgumentNullException("record");

			var id = record.Id;
			if (String.IsNullOrEmpty(id))
				throw new ArgumentException("The record specified has no identification set");

			return UpdateRecord(id, record);
		}

		public ZohoEntityCollection<TRelated> GetRelatedRecordsTo<TSource, TRelated>(string id) where TSource : ZohoEntity where TRelated : ZohoEntity {
			return GetRelatedRecordsTo<TSource, TRelated>(id, null);
		}

		public ZohoEntityCollection<TRelated> GetRelatedRecordsTo<TSource, TRelated>(string id, int? toIndex) where TSource : ZohoEntity where TRelated : ZohoEntity {
			return GetRelatedRecordsTo<TSource, TRelated>(id, null, toIndex);
		}

		public ZohoEntityCollection<TRelated> GetRelatedRecordsTo<TSource, TRelated>(string id, int? fromIndex, int? toIndex) where TSource : ZohoEntity where TRelated : ZohoEntity {
			var parentModuleName = ModuleName<TSource>();
			var paremeters = GetListOptionsParameters(null, new ListOptions {FromIndex = fromIndex, ToIndex = toIndex});
			paremeters.Add("parentModule", parentModuleName);
			paremeters.Add("id", id);

			return GetEntities<TRelated>(ModuleName<TRelated>(), "getRelatedRecords", paremeters);
		}

		public ZohoEntityCollection<ZohoAttachment> GetRecordAttachments<T>(string id) where T : ZohoEntity {
			return GetRelatedRecordsTo<T, ZohoAttachment>(id, null, null);
		}

		public bool UpdateRecord<T>(string id, T record) where T : ZohoEntity {
			AssertTypeIsNotAbstract(typeof(T));
			AssertAllowInserts(typeof(T));

			var collection = new ZohoEntityCollection<T> {record};
			var moduleName = ModuleName(typeof(T));
			var xmlData = collection.ToXmlString();
			var response = PostData(moduleName, "updateRecords", new Dictionary<string, string> {{"id", id}}, xmlData);
			if (response.RecordDetails.Count() != 1)
				throw new InvalidOperationException("Invalid number of details returned");

			var detail = response.RecordDetails.First();
			return detail.Id == id;
		}

		public bool DeleteRecord<T>(T record) where T : ZohoEntity {
			if (record == null)
				throw new ArgumentNullException("record");

			var id = record.Id;
			if (String.IsNullOrEmpty(id))
				throw new ArgumentException("The record specified has no identification set");

			return DeleteRecordById<T>(id);
		}

		public bool DeleteRecordById<T>(string id) {
			AssertTypeIsNotAbstract(typeof(T));
			AssertAllowInserts(typeof(T));

			var moduleName = ModuleName(typeof(T));
			var response = GetResponse(moduleName, "deleteRecords", new Dictionary<string, string> { { "id", id } });
			return response.Code == "5000";
		}

		public ZohoEntityContext<T> GetContext<T>() where T : ZohoEntity {
			return new ZohoEntityContext<T>(this);
		}
	}
}