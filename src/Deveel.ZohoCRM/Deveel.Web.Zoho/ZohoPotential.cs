using System;

namespace Deveel.Web.Zoho {
	[ModuleName("Potentials")]
	public sealed class ZohoPotential : ZohoEntity {
		public ZohoPotential(string name) {
			if (name == null)
				throw new ArgumentNullException("name");

			Name = name;
		}

		protected override string IdFieldName {
			get { return "POTENTIALID"; }
		}

		public string Name {
			get { return GetString("Potential Name"); }
			private set { SetValue("Potential Name", value); }
		}

		public string AccountName {
			get { return GetString("Account Name"); }
			set { SetValue("Account Name", value); }
		}

		public decimal Amount {
			get { return GetDecimal("Amount"); }
			set { SetValue("Amount", value); }
		}

		public string Currency {
			get { return GetString("Currency"); }
			set { SetValue("Currency", value); }
		}

		public string CampaignSource {
			get { return GetString("Campaign Source"); }
			set { SetValue("Campaign Source", value); }
		}

		public string ContactName {
			get { return GetString("Contact Name"); }
			set { SetValue("Contact Name", value); }
		}

		public string Owner {
			get { return GetString("Potential Owner"); }
			set { SetValue("Potential Owner", value); }
		}

		public int ProbabilityRate {
			get { return GetInt32("Probability (%)"); }
			set { SetValue("Probability (%)", value); }
		}

		public decimal ExpectedRevenue {
			get { return GetDecimal("Expected Revenue"); }
			set { SetValue("Expected Revenue", value); }
		}

		public string Type {
			get { return GetString("Type"); }
			set { SetValue("Type", value); }
		}

		public string Stage {
			get { return GetString("Stage"); }
			set { SetValue("Stage", value); }
		}

		public string NextStep {
			get { return GetString("Next Step"); }
			set { SetValue("Next Step", value); }
		}
	}
}