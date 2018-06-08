using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trend.Persitent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnitTest
{
	[TestClass]
	public class MongoDBTest
	{
		private const string MongoDBConnectionStr = "mongodb://cn-shenje-1";
		private static string DefaultCollectionName = "testCollection";

		private IDatabaseContext context;

		public MongoDBTest()
		{
			context = new MongoDBContext(MongoDBConnectionStr, "test");
		}

		[TestInitialize]
		public void TestClear()
		{
			context.Clear<OBJ>();
		}

		[TestMethod]
		public void InsertRecord()
		{
			var obj = new OBJ() { Name = "Mike" };
			context.Insert<OBJ>(obj);

			var n = typeof(OBJ).Name;
			//var n = obj.GetType().Name;

			List<Expression<Func<OBJ, bool>>> conditions = new List<Expression<Func<OBJ, bool>>>();
			conditions.Add(x => x.Name == "Mike");
			var result = context.Find(conditions, 0, 10, "asc");
			Assert.AreEqual(1, result.Count);
		}

		[TestMethod]
		public void ReplaceOneRecord()
		{
			var obj = new OBJ() { Name = "Mike" };
			context.Insert<OBJ>(obj);

			var newRecord = new OBJ() { Name = "NewName" };

			List<Expression<Func<OBJ, bool>>> conditions = new List<Expression<Func<OBJ, bool>>>();
			conditions.Add(x => x.Name == "Mike");

			var modifiedCount = context.ReplaceOne<OBJ>(conditions, newRecord);

			Assert.AreEqual(1, modifiedCount);

			List<Expression<Func<OBJ, bool>>> conditionsForNewRecord = new List<Expression<Func<OBJ, bool>>>();
			conditionsForNewRecord.Add(x => x.Name == "NewName");

			var result = context.Find(conditionsForNewRecord, 0, 10, "asc");
			Assert.AreEqual(1, result.Count);
		}
	}

	public class OBJ
	{
		public string _id;
		public string Name;
		public string Age;
	}
}
