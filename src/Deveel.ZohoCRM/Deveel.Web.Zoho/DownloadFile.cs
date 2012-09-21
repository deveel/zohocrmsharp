using System;
using System.IO;

namespace Deveel.Web.Zoho {
	public sealed class DownloadFile {
		internal DownloadFile(string id, string contentType, int contentLength, byte[] content) {
			Content = content;
			ContentLength = contentLength;
			ContentType = contentType;
			Id = id;
		}

		public string Id { get; private set; }

		public string ContentType { get; private set; }

		public int ContentLength { get; private set; }

		public byte[] Content { get; private set; }

		public Stream ContentStream {
			get { return new MemoryStream(Content, false); }
		}
	}
}