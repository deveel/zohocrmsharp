using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Deveel.Web.Zoho {
	public sealed class ZohoEntityCollection<T> : ICollection<T> where T : ZohoEntity {
		private readonly ICollection<T> collection;

		public ZohoEntityCollection()
			: this(false) {
		}

		public ZohoEntityCollection(bool readOnly)
			: this(new List<T>(), readOnly) {
		}

		private ZohoEntityCollection(ICollection<T> collection, bool readOnly) {
			var entityName = Attribute.GetCustomAttribute(typeof (T), typeof (EntityNameAttribute)) as EntityNameAttribute;
			if (entityName == null)
				throw new InvalidOperationException();

			EntityName = entityName.EntityName;
			IsReadOnly = readOnly;
			this.collection = collection;
		}

		internal string EntityName { get; private set; }

		private void AssertNotReadOnly() {
			if (IsReadOnly)
				throw new InvalidOperationException("Collection is not read-only.");
		}

		public IEnumerator<T> GetEnumerator() {
			return collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void Add(T item) {
			AssertNotReadOnly();
			collection.Add(item);
		}

		public void Clear() {
			AssertNotReadOnly();
			collection.Clear();
		}

		public bool Contains(T item) {
			return collection.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			collection.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item) {
			AssertNotReadOnly();
			return collection.Remove(item);
		}

		public int Count {
			get { return collection.Count; }
		}

		public bool IsReadOnly { get; private set; }

		public ZohoEntityCollection<T> AsReadOnly() {
			return new ZohoEntityCollection<T>(collection, true);
		}

		internal void LoadFromXml(XElement parent) {
			if (parent.Name == "nodata")
				return;

			if (parent.Name != EntityName)
				throw new InvalidOperationException();

			foreach (var element in parent.Elements()) {
				if (element.Name != "row")
					continue;

				var entity = Activator.CreateInstance<T>();
				entity.LoadFromXml(element);
				collection.Add(entity);
			}
		}

		internal string ToXmlString() {
			var sb = new StringBuilder();
			sb.AppendFormat("<{0}>", EntityName);
			var rowNum = 0;
			foreach (var entity in this) {
				entity.ToXmlString(sb, rowNum++);
			}
			sb.AppendFormat("</{0}>", EntityName);
			return sb.ToString();
		}
	}
}