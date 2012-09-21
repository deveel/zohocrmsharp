using System;
using System.Collections.Generic;
using System.IO;

namespace Deveel.Web.Zoho {
	public class ZohoEntityContext<T> where T : ZohoEntity {
		public ZohoEntityContext(ZohoCrmClient client) {
			Client = client;
		}

		protected ZohoCrmClient Client { get; private set; }

		public T GetById(string id) {
			return Client.GetRecordById<T>(id);
		}

		public T RequireById(string id) {
			var entity = GetById(id);
			if (entity == null)
				throw new ArgumentException("Entity not found for the given id.");

			return entity;
		}

		public ZohoEntityCollection<T> List(ListOptions options) {
			return Client.GetRecords<T>(options);
		}

		public ZohoEntityCollection<T> ListMy(ListOptions options) {
			return Client.GetMyRecords<T>(options);
		}

		public ZohoEntityCollection<TRelated> ListRelated<TRelated>(string id) where TRelated : ZohoEntity {
			return Client.GetRelatedRecordsTo<T, TRelated>(id);
		}

		public ZohoEntityCollection<ZohoAttachment> ListAttachments(string id) {
			return ListRelated<ZohoAttachment>(id);
		} 

		public ZohoInsertResponse Insert(T record) {
			return Client.InsertRecord(record);
		}

		public ZohoInsertResponse Insert(IEnumerable<T> records) {
			return Client.InsertRecords(records);
		}

		public bool Delete(T record) {
			return Client.DeleteRecord(record);
		}
		
		public bool DeleteById(string id) {
			return Client.DeleteRecordById<T>(id);
		}

		public bool Update(T record) {
			return Client.UpdateRecord(record);
		}

		public string UploadFile(string id, string fileName, string contentType, byte[] bytes) {
			return Client.UploadFileToRecord<T>(id, fileName, contentType, bytes);
		}

		public string UploadFile(string id, string fileName, string contentType, Stream inputStream) {
			return Client.UploadFileToRecord<T>(id, fileName, contentType, inputStream);
		}

		public string UploadFile(string id, string fileName, string contentType, Uri uri) {
			return Client.UploadFileToRecord<T>(id, fileName, contentType, uri);
		}

		public string UploadFile(string id, string fileName, string contentType, string filePath) {
			return Client.UploadFileToRecord<T>(id, fileName, contentType, filePath);
		}

		public string UploadPhoto(string id, string contentType, byte[] bytes) {
			return Client.UploadPhotoToRecord<T>(id, contentType, bytes);
		}

		public string UploadPhoto(string id, string contentType, Stream inputStream) {
			return Client.UploadPhotoToRecord<T>(id, contentType, inputStream);
		}

		public string UploadPhoto(string id, string contentType, Uri uri) {
			return Client.UploadPhotoToRecord<T>(id, contentType, uri);
		}

		public string UploadPhoto(string id, string contentType, string filePath) {
			return Client.UploadPhotoToRecord<T>(id, contentType, filePath);
		}


		public ZohoEntityCollection<T> Search(ZohoSearchCondition condition) {
			return Search(condition, null);
		}

		public ZohoEntityCollection<T> Search(ZohoSearchCondition condition, IEnumerable<string> columns) {
			return Client.Search<T>(condition, columns);
		}

		public ZohoEntityCollection<T> Search(string column, ConditionOperator @operator, object value) {
			return Search(new ZohoSearchCondition(column, @operator, value));
		}

		public ZohoEntityCollection<T> Is(string column, object value) {
			return Search(column, ConditionOperator.Is, value);
		} 

		public ZohoEntityCollection<T> IsNot(string column, object value) {
			return Search(column, ConditionOperator.IsNot, value);
		} 

		public ZohoEntityCollection<T> GreaterThan(string column, object value) {
			return Search(column, ConditionOperator.GreaterThan, value);
		}

		public ZohoEntityCollection<T> LesserThan(string column, object value) {
			return Search(column, ConditionOperator.LesserThan, value);
		}

		public ZohoEntityCollection<T> GreaterOrEqualThan(string column, object value) {
			return Search(column, ConditionOperator.GreaterOrEqualThan, value);
		} 

		public ZohoEntityCollection<T> LesserOrEqualThan(string column, object value) {
			return Search(column, ConditionOperator.LesserOrEqualsThan, value);
		} 

		public ZohoEntityCollection<T> Contains(string column, string value) {
			return Search(column, ConditionOperator.Contains, value);
		} 

		public ZohoEntityCollection<T> NotContains(string column, string value) {
			return Search(column, ConditionOperator.NotContains, value);
		}

		public ZohoEntityCollection<T> StartsWith(string column, string value) {
			return Search(column, ConditionOperator.StartsWith, value);
		}

		public ZohoEntityCollection<T> EndsWith(string column, string value) {
			return Search(column, ConditionOperator.EndsWith, value);
		} 

		public DownloadFile DownloadFile(string id) {
			return Client.DownloadRecordFile<T>(id);
		}
	}
}