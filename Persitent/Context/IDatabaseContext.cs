using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Trend.Persitent
{
	public interface IDatabaseContext
	{
		void Init(string connectionString, string dbName);

		void Insert<T>(T data);

		void Clear<T>();

		List<T> Find<T>(List<Expression<Func<T, bool>>> conditions, int skip, int limit, string sort);

		long ReplaceOne<T>(List<Expression<Func<T, bool>>> conditions, T obj);
	}
}