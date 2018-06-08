using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class ProgressOfStoriesController : ApiController
	{
		public IEnumerable<StoryProgressItem> GetProgressOfStories()
		{
			return App.GetReleaseScrumData().CurrentSprintProxy.ProgressOfStories;
		}

		//// GET: api/Default/5
		//public PersonActivityItem GetBookChapter(int id)
		//{

		//	return chapters.Where(c => c.Number == id).SingleOrDefault();
		//}

		//// POST: api/Default
		//public void Post([FromBody]BookChapter value)
		//{
		//	chapters.Add(value);
		//}

		//// PUT: api/Default/5
		//public void Put(int id, [FromBody]BookChapter value)
		//{
		//	var chapter = chapters.FirstOrDefault(c => c.Number == id);
		//	if (chapter != null)
		//	{
		//		chapters.Remove(chapter);
		//	}
		//	chapters.Add(value);
		//}

		//// DELETE: api/Default/5
		//public void Delete(int id)
		//{
		//	chapters.Remove(chapters.Where(c => c.Number == id).Single());
		//}
	}
}
