using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Persitent
{
	public interface IRepository<T>
	{
		void Save(T obj);
		void Add(T Obj);
		void Remove(T obj);
		T FindBy(string id);
		List<T> FindAll();
	}
}
