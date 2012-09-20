using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

		private static string ModuleName(Type entityType) {
			var entityName = Attribute.GetCustomAttribute(entityType, typeof(EntityNameAttribute)) as EntityNameAttribute;
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

		private ZohoEntityCollection<T> GetEntities<T>(string module, string method, IEnumerable<KeyValuePair<string, string>> parameters) where T : ZohoEntity {
			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
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

		private XDocument PostData(string module, string method, IDictionary<string, string> parameters, string xmlData) {
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

			return XDocument.Parse(response.Content);
		}

		private ZohoInsertResponse PostFile(string module, string id, byte[] bytes, string fileName, string contentType) {
			if (module == null)
				throw new ArgumentNullException("module");

			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "{module}/uploadFile?authtoken={authtoken}&scope={scope}&id={id}";
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken, ParameterType.UrlSegment);
			request.AddParameter("scope", "crmapi", ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);
			request.AddFile("content", bytes, fileName, contentType);
			request.Timeout = 5000;

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			var xmlResponse = XDocument.Parse(response.Content);
			return new ZohoInsertResponse(module, "uploadFile", xmlResponse);
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

		public ZohoInsertResponse InsertRecords<T>(ICollection<T> records) where T : ZohoEntity {
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
			var response = PostData(moduleName, "insertRecords", null, xmlData);

			return new ZohoInsertResponse(moduleName, "insertRecords", response);
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

		public RecordDetail UpdateRecord<T>(T record) where T : ZohoEntity {
			if (record == null)
				throw new ArgumentNullException("record");

			var id = record.Id;
			if (String.IsNullOrEmpty(id))
				throw new ArgumentException("The record specified has no identification set");

			return UpdateRecord(id, record);
		}

		public RecordDetail UpdateRecord<T>(string id, T record) where T : ZohoEntity {
			AssertTypeIsNotAbstract(typeof(T));
			AssertAllowInserts(typeof(T));

			var collection = new ZohoEntityCollection<T> {record};
			var moduleName = ModuleName(typeof(T));
			var xmlData = collection.ToXmlString();
			var xmlResponse = PostData(moduleName, "updateRecords", new Dictionary<string, string> {{"id", id}}, xmlData);

			var response = new ZohoInsertResponse(moduleName, "updateRecords", xmlResponse);
			if (response.RecordDetails.Count() != 1)
				throw new InvalidOperationException("Invalid number of details returned");

			return response.RecordDetails.First();
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
			var xmlResponse = PostData(moduleName, "deleteRecords", new Dictionary<string, string> { { "id", id } }, null);

			var response = new ZohoResponse(moduleName, "deleteRecords");
			response.LoadFromXml(xmlResponse.Root);

			return response.Code == "5000";
		}

		public string UploadFileToRecord<T>(string id, string fileName, string contentType, Stream inputStream) where  T :ZohoEntity{
			if (inputStream == null)
				throw new ArgumentNullException("inputStream");

			var bytes = ReadFromStream(inputStream);
			return UploadFileToRecord<T>(id, fileName, contentType, bytes);
		}

		public string UploadFileToRecord<T>(string id, string fileName, string contentType, byte[] content) where T : ZohoEntity {
			if (content == null)
				throw new ArgumentNullException("content");

			var response = PostFile(ModuleName<T>(), id, content, fileName, contentType);
			if (response.IsError)
				throw new InvalidOperationException("An error occurred while uploading the file");

			return response.RecordDetails.First().Id;
		}

		public string UploadFileToRecord<T>(string recordId, string fileName, string contentType, Uri uri) where T : ZohoEntity {
			byte[] bytes;

			using (var client = new WebClient()) {
				var inputStream = client.OpenRead(uri);
				if (inputStream == null)
					throw new FileNotFoundException("The uri specified returned no file.");

				bytes = ReadFromStream(inputStream);
			}

			return UploadFileToRecord<T>(recordId, fileName, contentType, bytes);
		}

		public string UploadFileToRecord<T>(string recordId, string fileName, string contentType, string filePath) where T : ZohoEntity {
			if (!File.Exists(filePath))
				throw new ArgumentException("File specified was not found in the system.");

			byte[] bytes;
			using (var inputStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				bytes = ReadFromStream(inputStream);
			}

			return UploadFileToRecord<T>(recordId, fileName, contentType, bytes);
		}

		private static byte[] ReadFromStream(Stream inputStream) {
			if (!inputStream.CanRead)
				throw new ArgumentException("The given stream cannot read");

			var memoryStream = new MemoryStream();
			int readCount;
			byte[] readBuffer = new byte[1024];
			while ((readCount = inputStream.Read(readBuffer, 0, readBuffer.Length)) != 0) {
				memoryStream.Write(readBuffer, 0, readCount);
			}

			memoryStream.Flush();
			return memoryStream.ToArray();			
		}
	}
}