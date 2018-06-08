using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trend.DataModel;

namespace Trend.Persitent
{
	public class ActivityRepository : IActivityRepository
	{
		internal IDatabaseContext context { get; set; }

		public ActivityRepository()
		{

		}

		public ActivityRepository(string connectionString, string dbName)
		{
			context = new MongoDBContext(connectionString, dbName);
		}

		public void Save(PersonActivity obj)
		{
			List<Expression<Func<PersonActivity, bool>>> conditions = new List<Expression<Func<PersonActivity, bool>>>();
			conditions.Add(x => x._id == obj._id);

			context.ReplaceOne(conditions, obj);
		}

		public void Add(PersonActivity Obj)
		{
			context.Insert(Obj);
		}

		public void Remove(PersonActivity obj)
		{
			throw new NotImplementedException();
		}

		public PersonActivity FindBy(string id)
		{
			List<Expression<Func<PersonActivity, bool>>> conditions = new List<Expression<Func<PersonActivity, bool>>>();
			conditions.Add(x => x._id == id);

			var release = context.Find(conditions, 0, 10, "asc");

			return release.FirstOrDefault();
		}

		public List<PersonActivity> FindAll()
		{
			throw new NotImplementedException();
		}
	}
}
