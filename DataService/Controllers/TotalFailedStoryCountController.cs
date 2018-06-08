using System.Web.Http;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class TotalFailedStoryCountController : ApiController
	{
		public string GetTotalFailedStoryCount()
		{
			return App.TotalFailedStoryCount();
		}
	}
}
