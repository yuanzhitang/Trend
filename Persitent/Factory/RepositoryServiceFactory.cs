using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Common;

namespace Trend.Persitent
{
	public class RepositoryServiceFactory
	{
		public static ScrumDataService CreateScrumDataService()
		{
			var scrumDataRespository = new ScrumDataRepository(RepositoryConfig.DbServer, RepositoryConfig.TrendScrumDataDB);
			return new ScrumDataService(scrumDataRespository);
		}

		public static ActivityDataService CreateActitivyDataService()
		{
			var activityRepository = new ActivityRepository(RepositoryConfig.DbServer, RepositoryConfig.TrendActivityDB);
			return new ActivityDataService(activityRepository);
		}
	}
}
