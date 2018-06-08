using System;
using MongoDB.Driver;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Trend.Common;

namespace Trend.Persitent
{
	public class MongoDBContext : IDatabaseContext
	{
		private MongoClient client;
		private IMongoDatabase database;

		public MongoDBContext(string connectionString, string dbName)
		{
			Init(connectionString, dbName);
		}

		public void Init(string connectionString, string dbName)
		{
			client = new MongoClient(connectionString);
			database = client.GetDatabase(dbName);
		}

		public void Clear<T>()
		{
			if (database == null)
			{
				return;
			}

			string collectionName = GetCollectionName<T>();
			var filter = Builders<T>.Filter.Empty;

			var collection = database.GetCollection<T>(collectionName);

			if (collection.Find<T>(filter).Any())
			{
				collection.DeleteMany(filter);
			}
		}

		public List<T> Find<T>(List<Expression<Func<T, bool>>> conditions, int skip, int limit, string sort)
		{
			string collectionName = GetCollectionName<T>();

			if (conditions == null || conditions.Count == 0)
			{
				conditions = new List<Expression<Func<T, bool>>> { x => true };
			}
			var builder = Builders<T>.Filter;
			var filter = builder.And(conditions.Select(x => builder.Where(x)));

			var ret = new List<T>();
			try
			{
				List<SortDefinition<T>> sortDefList = new List<SortDefinition<T>>();
				if (sort != null)
				{
					var sortList = sort.Split(',');
					for (var i = 0; i < sortList.Length; i++)
					{
						var sl = Regex.Replace(sortList[i].Trim(), @"\s+", " ").Split(' ');
						if (sl.Length == 1 || (sl.Length >= 2 && sl[1].ToLower() == "asc"))
						{
							sortDefList.Add(Builders<T>.Sort.Ascending(sl[0]));
						}
						else if (sl.Length >= 2 && sl[1].ToLower() == "desc")
						{
							sortDefList.Add(Builders<T>.Sort.Descending(sl[0]));
						}
					}
				}


				var collection = database.GetCollection<T>(collectionName);
				var sortDef = Builders<T>.Sort.Combine(sortDefList);
				ret = collection.Find(filter).Sort(sortDef).Skip(skip).Limit(limit).ToListAsync().Result;
			}
			catch (Exception e)
			{
				//异常处理
			}
			return ret;
		}

		private static string GetCollectionName<T>()
		{
			var customAttribute = typeof(T).GetCustomAttributes(typeof(CollectionNameAttribute), false).FirstOrDefault();
			if (customAttribute == null)
			{
				return typeof(T).Name;
			}
			return (customAttribute as CollectionNameAttribute).CollectionName;
		}

		public void Insert<T>(T data)
		{
			string collectionName = GetCollectionName<T>();
			var collection = database.GetCollection<T>(collectionName);
			collection.InsertOne(data);
		}

		public long ReplaceOne<T>(List<Expression<Func<T, bool>>> conditions, T obj)
		{
			string collectionName = GetCollectionName<T>();
			if (conditions == null || conditions.Count == 0)
			{
				conditions = new List<Expression<Func<T, bool>>> { x => true };
			}
			var builderFilter = Builders<T>.Filter;
			var filter = builderFilter.And(conditions.Select(x => builderFilter.Where(x)));

			var collection = database.GetCollection<T>(collectionName);
			return collection.ReplaceOne(filter, obj).ModifiedCount;
		}
	}
}