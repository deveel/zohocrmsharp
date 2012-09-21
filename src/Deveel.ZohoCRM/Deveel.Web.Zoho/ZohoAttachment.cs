using System;

namespace Deveel.Web.Zoho {
	[ModuleName("Attachments")]
	[AllowInserts(false)]
	public sealed class ZohoAttachment : ZohoEntity {
		internal ZohoAttachment() {
		}

		protected override string IdFieldName {
			get { return "id"; }
		}

		public string FileName {
			get { return GetString("File Name"); }
		}

		//TODO: convert this to a numeric value
		public string Size {
			get { return GetString("Size"); }
		}

		public string AttachedBy {
			get { return GetString("Attached By"); }
		}

		public DateTime ModifiedTime {
			get { return GetDateTime("Modified Time"); }
		}
	}
}