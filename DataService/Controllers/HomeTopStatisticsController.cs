using System.Collections.Generic;
using System.Web.Http;
using Unisys.Trend.DataModel;
using Unisys.Trend.AnalysisService;

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
