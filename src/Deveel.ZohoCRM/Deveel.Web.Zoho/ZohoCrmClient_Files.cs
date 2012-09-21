using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using RestSharp;

namespace Deveel.Web.Zoho {
	public sealed partial class ZohoCrmClient {
		public string UploadFileToRecord<T>(string id, string fileName, string contentType, Stream inputStream) where T : ZohoEntity {
			if (inputStream == null)
				throw new ArgumentNullException("inputStream");

			var bytes = ReadFromStream(inputStream);
			return UploadFileToRecord<T>(id, fileName, contentType, bytes);
		}

		public string UploadFileToRecord<T>(string id, string fileName, string contentType, byte[] content) where T : ZohoEntity {
			if (content == null)
				throw new ArgumentNullException("content");

			var response = PostFile(ModuleName<T>(), "uploadFile", id, content, fileName, contentType);
			response.ThrowIfError();
			return response.RecordDetails.First().Id;
		}

		public string UploadFileToRecord<T>(string recordId, string fileName, string contentType, Uri uri) where T : ZohoEntity {
			if (uri == null)
				throw new ArgumentNullException("uri");

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

		public DownloadFile DownloadRecordFile<T>(string fileId) where T : ZohoEntity {
			return DownloadFile(ModuleName<T>(), fileId);
		}

		public bool DeleteRecordFile<T>(string fileId) where T : ZohoEntity {
			var response = GetResponse(ModuleName<T>(), "deleteFile", new Dictionary<string, string> { { "id", fileId } });
			return response.Code == "4800";
		}

		public string UploadPhotoToRecord<T>(string id, string contentType, byte[] data) {
			if (typeof(T) != typeof(ZohoLead) &&
				typeof(T) != typeof(ZohoContact))
				throw new ArgumentException("Photos can be uploaded only to Leads and Contacts.");

			var response = PostFile(ModuleName<T>(), "uploadPhoto", id, data, "photo", contentType);
			response.ThrowIfError();
			return response.RecordDetails.First().Id;
		}

		public string UploadPhotoToRecord<T>(string id, string contentType, Stream inputStream) where T : ZohoEntity {
			if (inputStream == null)
				throw new ArgumentNullException("inputStream");

			var bytes = ReadFromStream(inputStream);
			return UploadPhotoToRecord<T>(id, contentType, bytes);
		}

		public string UploadPhotoToRecord<T>(string id, string contentType, string filePath) where T : ZohoEntity {
			if (!File.Exists(filePath))
				throw new ArgumentException("File specified was not found in the system.");

			byte[] bytes;
			using (var inputStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
				bytes = ReadFromStream(inputStream);
			}

			return UploadPhotoToRecord<T>(id, contentType, bytes);
		}

		public string UploadPhotoToRecord<T>(string id, string contentType, Uri uri) where T : ZohoEntity {
			if (uri == null)
				throw new ArgumentNullException("uri");

			byte[] bytes;

			using (var client = new WebClient()) {
				var inputStream = client.OpenRead(uri);
				if (inputStream == null)
					throw new FileNotFoundException("The uri specified returned no file.");

				bytes = ReadFromStream(inputStream);
			}

			return UploadPhotoToRecord<T>(id, contentType, bytes);
		}

		private ZohoInsertResponse PostFile(string module, string method, string id, byte[] bytes, string fileName, string contentType) {
			if (module == null)
				throw new ArgumentNullException("module");

			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.POST);
			request.Resource = "{module}/{method}?authtoken={authtoken}&scope={scope}&id={id}";
			request.AddParameter("method", method, ParameterType.UrlSegment);
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken, ParameterType.UrlSegment);
			request.AddParameter("scope", "crmapi", ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);
			request.AddFile("content", bytes, fileName, contentType);

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			return new ZohoInsertResponse(module, "uploadFile", response.Content);
		}

		private DownloadFile DownloadFile(string module, string id) {
			if (module == null)
				throw new ArgumentNullException("module");

			var client = new RestClient(BaseUrl);
			var request = new RestRequest(Method.GET);
			request.Resource = "{module}/downloadFile?authtoken={authtoken}&scope={scope}&id={id}";
			request.AddParameter("module", module, ParameterType.UrlSegment);
			request.AddParameter("authtoken", authToken, ParameterType.UrlSegment);
			request.AddParameter("scope", "crmapi", ParameterType.UrlSegment);
			request.AddParameter("id", id, ParameterType.UrlSegment);

			var response = client.Execute(request);
			if (response.StatusCode != HttpStatusCode.OK)
				throw response.ErrorException;

			return new DownloadFile(id, response.ContentType, (int)response.ContentLength, response.RawBytes);
		}
	}
}