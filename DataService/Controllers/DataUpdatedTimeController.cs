using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService.DataAnalyzer;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class DataUpdatedTimeController : ApiController
	{
		public string GetDataUpdatedTime()
		{
			return App.LastUpdatedTime();
		}
	}
}
