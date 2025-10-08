using Humanizer.DateTimeHumanizeStrategy;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("Saa3idWebTests")]

namespace Saa3idWeb.Util
{
	/// <summary>
	/// This class is designed for SELECT operations without JOIN statements.
	/// </summary>
	public class SelectQueryBuilder
	{
		internal string query = "SELECT ";
		internal string[] fields = { };
		internal string[] condition = { };
		internal string tableName = "";
		internal string order = "";
		public SelectQueryBuilder() { }

		/// <summary>
		/// Add fields to perform query from.
		/// </summary>
		/// <param name="field">The field name as part of the query</param>
		/// <returns>QueryBuilder</returns>
		public SelectQueryBuilder AddField(Object field, bool lowercase = false)
		{
			String fieldname = nameof(field);

			if (lowercase)
			{
				fieldname = fieldname.ToLower();
			}

			this.fields.Append<Object>(lowercase);
			return this;
		}

		public SelectQueryBuilder AddStringField(String field)
		{
			this.fields.Append(field);
			return this;
		}

		public SelectQueryBuilder AddCondition(String condition, String connector = "")
		{
			this.condition.Append(condition + " " + connector);
			return this;
		}

		public String Build()
		{
			// Append the field names
			for (int i = 0; i < fields.Length; i++)
			{
				string? field = this.fields[i];

				if(i == fields.Length - 1)
				{
					this.query += field;
				}
				else
				{
					this.query += field + ",";
				}
			}

			// Append the table name.
			this.query += $" FROM {this.tableName}";

			if(this.fields.Length > 0)
			{
				this.query += " WHERE ";
			}

			// Append the conditions with connectors.
			for(int i = 0; i < fields.Length; i++)
			{
				string? condition = this.condition[i];

				if(i < fields.Length - 1)
				{
					this.query += condition;
				}
				else
				{
					this.query += condition + ",";
				}
			}

			// Append the order query.
			this.query += order;

			// Return the string query.
			return this.query;
		}

		public SelectQueryBuilder SetOrder(String order)
		{
			this.order = order;
			return this;
		}

		public SelectQueryBuilder SetTable(String tableName)
		{
			this.tableName = tableName;
			return this;
		}
	}
}
