using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trend.DataModel;

namespace Trend.Persitent
{
	public class ScrumDataService
	{
		private IScrumDataRepository repository;
		public ScrumDataService(IScrumDataRepository _repository)
		{
			repository = _repository;
		}

		public void Save(Release data)
		{
			var release = repository.FindBy(data._id);
			if (release == null)
			{
				repository.Add(data);
			}
			else
			{
				repository.Save(data);
			}
		}

		public Release FindBy(string _id)
		{
			return repository.FindBy(_id);
		}
	}
}
