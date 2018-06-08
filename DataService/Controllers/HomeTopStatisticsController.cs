using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class HomeTopStatisticsController : ApiController
	{
		public HomeTopStatistics GetStatistics()
		{ 
			return App.GetHomeTopStatistics();
		}
	}
}
