using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class StoryProgressDataController : ApiController
	{
		public IEnumerable<TaskWallOfStory> GetAllStoryProgressData()
		{
			IEnumerable<TaskWallOfStory> result = App.GetTaskWallOfStorySummary(string.Empty);

			return result;
		}

		// GET: api/Default/5
		public IEnumerable<TaskWallOfStory> GetStoryProgressData(string personName)
		{
			IEnumerable<TaskWallOfStory> result = App.GetTaskWallOfStorySummary(personName);
			return result;
		}
	}
}
