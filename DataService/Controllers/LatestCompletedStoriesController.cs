using System.Collections.Generic;
using System.Web.Http;
using Unisys.Trend.DataModel;
using Unisys.Trend.AnalysisService;

namespace DataService.Controllers
{
	public class LatestCompletedStoriesController : ApiController
	{
		public IEnumerable<Story> GetCompletedStories()
		{
			return App.GetReleaseScrumData().CurrentSprintProxy.CompletedStories;
		}
	}
}
