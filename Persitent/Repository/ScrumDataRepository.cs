using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trend.DataModel;

namespace Trend.Persitent
{
	public class ScrumDataRepository : IScrumDataRepository
	{
		internal IDatabaseContext context { get; set; }

		public ScrumDataRepository()
		{

		}

		public ScrumDataRepository(string connectionString, string dbName)
		{
			context = new MongoDBContext(connectionString, dbName);
		}

		public void Save(Release obj)
		{
			List<Expression<Func<Release, bool>>> conditions = new List<Expression<Func<Release, bool>>>();
			conditions.Add(x => x._id == obj._id);

			context.ReplaceOne(conditions, obj);
		}

		public void Add(Release Obj)
		{
			context.Insert(Obj);
		}

		public void Remove(Release obj)
		{
			throw new NotImplementedException();
		}

		public Release FindBy(string id)
		{
			List<Expression<Func<Release, bool>>> conditions = new List<Expression<Func<Release, bool>>>();
			conditions.Add(x => x._id == id);

			var release = context.Find(conditions, 0, 10, "asc");

			return release.FirstOrDefault();
		}

		public List<Release> FindAll()
		{
			throw new NotImplementedException();
		}
	}
}
