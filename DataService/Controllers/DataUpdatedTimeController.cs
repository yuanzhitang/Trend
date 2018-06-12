using System.Web.Http;
using Unisys.Trend.DataModel;
using Unisys.Trend.AnalysisService.DataAnalyzer;
using Unisys.Trend.AnalysisService;

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
