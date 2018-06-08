using System.Web.Http;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class TotalCompletedStoryCountController : ApiController
	{
		public string GetTotalCompletedStoryCount()
		{
			return App.TotalCompletedStoryCount();
		}
	}
}
