using System;
using System.Text;

namespace Deveel.Web.Zoho
{
	public sealed class ZohoSerachCondition
	{
		public ZohoSerachCondition(string label, ConditionOperator @operator, object value)
		{
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

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			sb.Append(Label);
			sb.Append('|');
			sb.Append(OperatorString(Operator));
			sb.Append('|');
			sb.Append(ValueString(Value));
			sb.Append(")");
			return sb.ToString();
		}

		private static string ValueString(object value)
		{
			if (value is string)
				return (string) value;

			return Convert.ToString(value);
		}

		private static string OperatorString(ConditionOperator @operator)
		{
			if (@operator == ConditionOperator.Is)
				return "=";
			if (@operator == ConditionOperator.IsNot)
				return "<>";

			throw new NotSupportedException();
		}
	}
}