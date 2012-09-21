using System;
using System.Text;

namespace Deveel.Web.Zoho {
	public sealed class ZohoSearchCondition {
		public ZohoSearchCondition(string label, ConditionOperator @operator, object value) {
			if (label == null)
				throw new ArgumentNullException("label");
			if (value == null)
				throw new ArgumentNullException("value");

			Label = label;
			Operator = @operator;
			Value = value;
		}

		public string Label { get; private set; }

		public ConditionOperator Operator { get; private set; }

		public object Value { get; private set; }

		public override string ToString() {
			var sb = new StringBuilder();
			sb.Append("(");
			sb.Append(Label);
			sb.Append('|');
			sb.Append(OperatorString(Operator));
			sb.Append('|');
			sb.Append(ValueString(Operator, Value));
			sb.Append(")");
			return sb.ToString();
		}

		private static string ValueString(ConditionOperator @operator, object value) {
			string stringValue;
			if (value is string) {
				stringValue = (string) value;
			} else {
				stringValue = Convert.ToString(value);
			}

			if (@operator == ConditionOperator.Contains ||
			    @operator == ConditionOperator.NotContains)
				return String.Format("*{0}*", stringValue);
			if (@operator == ConditionOperator.StartsWith)
				return String.Format("{0}*", stringValue);
			if (@operator == ConditionOperator.EndsWith)
				return String.Format("*{0}", stringValue);

			return stringValue;
		}

		private static string OperatorString(ConditionOperator @operator) {
			if (@operator == ConditionOperator.Is)
				return "=";
			if (@operator == ConditionOperator.IsNot)
				return "<>";
			if (@operator == ConditionOperator.Contains)
				return "contains";
			if (@operator == ConditionOperator.NotContains)
				return "doesn't contain";
			if (@operator == ConditionOperator.StartsWith)
				return "starts with";
			if (@operator == ConditionOperator.EndsWith)
				return "ends with";
			if (@operator == ConditionOperator.GreaterThan)
				return ">";
			if (@operator == ConditionOperator.GreaterOrEqualThan)
				return ">=";
			if (@operator == ConditionOperator.LesserThan)
				return "<";
			if (@operator == ConditionOperator.LesserOrEqualsThan)
				return "<=";

			throw new NotSupportedException();
		}
	}
}