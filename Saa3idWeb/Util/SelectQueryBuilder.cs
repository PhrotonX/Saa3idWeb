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
		internal List<String> fields = new List<string>();
		internal List<String> condition = new List<string>();
		internal List<String> connector = new List<string>();
		internal string tableName = "";
		internal List<String> order = new List<String>();
		internal int limit = 0;
		public SelectQueryBuilder() { }

		/// <summary>
		/// Add fields to perform query from.
		/// </summary>
		/// <param name="field">The field name as part of the query</param>
		/// <returns>QueryBuilder</returns>
		public SelectQueryBuilder AddField(String fieldname, bool lowercase = false)
		{
			if (lowercase)
			{
				fieldname = fieldname.ToLower();
			}

			this.fields.Add(fieldname);
			return this;
		}

		public SelectQueryBuilder AddCondition(String condition, String connector = "")
		{
			this.condition.Add(condition);
			this.connector.Add(connector);
			return this;
		}

		public SelectQueryBuilder AddCondition(String lvalue, String op, String rvalue, String connector = "")
		{
			this.condition.Add(lvalue + " " + op + " " + rvalue);
			this.condition.Add(connector);
			return this;
		}

		public SelectQueryBuilder AddOrder(String fieldname, String order)
		{
			this.order.Add(fieldname + " " + order);
			return this;
		}

		public String Build()
		{
			// Append the field names
			for (int i = 0; i < this.fields.Count; i++)
			{
				string? field = this.fields[i];

				if(i == this.fields.Count - 1)
				{
					this.query += field;
				}
				else
				{
					this.query += field + ", ";
				}
			}

			// Append the table name.
			this.query += $" FROM {this.tableName}";

			if(this.condition.Count > 0)
			{
				this.query += " WHERE ";
			}

			// Append the conditions with connectors.
			for(int i = 0; i < this.condition.Count; i++)
			{
				string? condition = this.condition[i];
				string? connector = this.connector[i];

				if (i == this.condition.Count - 1)
				{
					this.query += condition;
				}
				else
				{
					this.query += condition + " " + connector + " ";
				}
			}

			// Append the order keyword.
			if(this.order.Count > 0)
			{
				this.query += " ORDER BY ";
			}

			// Append the order queries.
			for (int i = 0; i < this.order.Count; i++)
			{
				string? order = this.order[i];

				if (i == this.order.Count - 1)
				{
					this.query += order;
				}
				else
				{
					this.query += order + ", ";
				}
			}

			// Append the limit
			if(limit > 0)
			{
				this.query += " LIMIT " + limit;
			}

			// Return the string query.
			return this.query;
		}

		public SelectQueryBuilder SetLimit(int limit)
		{
			this.limit = limit;
			return this;
		}

		public SelectQueryBuilder SetTable(String tableName)
		{
			this.tableName = tableName;
			return this;
		}
	}
}
