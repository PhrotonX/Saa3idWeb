using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Saa3idWeb.Models;
using Saa3idWeb.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saa3idWeb.Util.Tests
{
	[TestClass()]
	public class SelectQueryBuilderTests
	{
		[TestMethod()]
		public void AddField_AddOneItem_FieldsAreGreaterThanOne()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();
			int Sample = 0;

			//Act
			selectQueryBuilder.AddField("Sample");

			//Assert
			Assert.IsTrue(selectQueryBuilder.fields.Count > 0);
		}

		[TestMethod()]
		public void AddField_AddManyItems_FieldsAreGreaterThanOne()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			//Act
			for(int i = 0; i < 100; i++)
			{
				selectQueryBuilder.AddField("Sample" + i);
			}

			//Assert
			Assert.IsTrue(selectQueryBuilder.fields.Count > 0);
		}

		[TestMethod()]
		public void AddCondition_AddOneItem_FieldsAreGreaterThanOne()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			//Act
			selectQueryBuilder.AddCondition("Sample > 1");

			//Assert
			Assert.IsTrue(selectQueryBuilder.condition.Count > 0);
		}

		[TestMethod()]
		public void AddCondition_AddManyItems_FieldsAreGreaterThanOne()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			//Act
			for(int i = 0; i < 5; i++)
			{
				selectQueryBuilder.AddCondition("Sample"+i+" < 1");
			}

			//Assert
			Assert.IsTrue(selectQueryBuilder.condition.Count > 0);
		}

		[TestMethod()]
		public void AddOrder_AddOneItem_FieldsAreGreaterThanOne()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			//Act
			selectQueryBuilder.AddOrder("F1", "DESC");

			//Assert
			Assert.IsTrue(selectQueryBuilder.order.Count > 0);
		}

		[TestMethod()]
		public void AddOrder_AddManyItems_FieldsAreGreaterThanOne()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			//Act
			for (int i = 0; i < 3; i++)
			{
				selectQueryBuilder.AddOrder("F" + i, "DESC");
			}
			for (int i = 0; i < 2; i++)
			{
				selectQueryBuilder.AddOrder("F" + i, "ASC");
			}

			//Assert
			Assert.IsTrue(selectQueryBuilder.order.Count > 0);
		}

		[TestMethod()]
		public void BuildTest_UseSingleField_IsValidSqlQuery()
		{
			// Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder()
				.AddField("FieldName")
				.SetTable("TableName");

			// Act
			var result = selectQueryBuilder.Build();

			// Assert
			Assert.AreEqual("SELECT FieldName FROM TableName", result);
		}

		[TestMethod()]
		public void BuildTest_UseMultipleFields_IsValidSqlQuery()
		{
			// Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder()
				.AddField("F1")
				.AddField("F2")
				.AddField("F3")
				.AddField("F4")
				.AddField("F5")
				.SetTable("TableName");

			// Act
			var result = selectQueryBuilder.Build();

			// Assert
			Assert.AreEqual("SELECT F1, F2, F3, F4, F5 FROM TableName", result);
		}

		[TestMethod()]
		public void BuildTest_UseAllFieldValueOneCondition_IsValidSqlQuery()
		{
			// Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder()
				.AddField("*")
				.AddCondition("FieldName > 1")
				.SetTable("TableName");

			// Act
			var result = selectQueryBuilder.Build();

			// Assert
			Assert.AreEqual("SELECT * FROM TableName WHERE FieldName > 1", result);
		}

		[TestMethod()]
		public void BuildTest_UseAllFieldValueMultipleCondition_IsValidSqlQuery()
		{
			// Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder()
				.AddField("*")
				.AddCondition("F1 > 1", "AND")
				.AddCondition("F2 < 1", "AND")
				.AddCondition("F3 = 1", "AND")
				.AddCondition("F4 <> 1", "AND")
				.AddCondition("F5 <= 1", "AND")
				.SetTable("TableName");

			// Act
			var result = selectQueryBuilder.Build();

			// Assert
			Assert.AreEqual("SELECT * FROM TableName WHERE F1 > 1 AND F2 < 1 AND F3 = 1 AND F4 <> 1 AND F5 <= 1", result);
		}

		[TestMethod()]
		public void BuildTest_UseOneOrderWithAllFieldValueOneCondition_IsValidSqlQuery()
		{
			// Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder()
				.AddField("*")
				.AddCondition("F1 > 1")
				.AddOrder("F1", "DESC")
				.SetTable("TableName");

			// Act
			var result = selectQueryBuilder.Build();

			// Assert
			Assert.AreEqual("SELECT * FROM TableName WHERE F1 > 1 ORDER BY F1 DESC", result);
		}

		[TestMethod()]
		public void BuildTest_UseMultipleOrderWithAllFieldValueNoCondition_IsValidSqlQuery()
		{
			// Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder()
				.AddField("*")
				.AddOrder("F1", "DESC")
				.AddOrder("F2", "DESC")
				.AddOrder("F3", "ASC")
				.SetTable("TableName");

			// Act
			var result = selectQueryBuilder.Build();

			// Assert
			Assert.AreEqual("SELECT * FROM TableName ORDER BY F1 DESC, F2 DESC, F3 ASC", result);
		}

		[TestMethod()]
		public void BuildTest_UseLimitWithMultipleOrderAndAllFieldValueNoCondition_IsValidSqlQuery()
		{
			// Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder()
				.AddField("*")
				.AddOrder("F1", "DESC")
				.AddOrder("F2", "DESC")
				.AddOrder("F3", "ASC")
				.SetLimit(5)
				.SetTable("TableName");

			// Act
			var result = selectQueryBuilder.Build();

			// Assert
			Assert.AreEqual("SELECT * FROM TableName ORDER BY F1 DESC, F2 DESC, F3 ASC LIMIT 5", result);
		}

		[TestMethod()]
		public void SetLimit_SetValue_IsNotNull()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			//Act
			selectQueryBuilder.SetLimit(5);

			//Assert
			Assert.IsNotNull(selectQueryBuilder.limit);
		}

		[TestMethod()]
		public void SetTable_SetValue_IsNotNull()
		{
			//Arrange
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			//Act
			selectQueryBuilder.SetTable("tablename");

			//Assert
			Assert.IsNotNull(selectQueryBuilder.tableName);
		}
	}
}