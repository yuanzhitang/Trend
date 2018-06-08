using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

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
