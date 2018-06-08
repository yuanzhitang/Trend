
using Trend.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trend.Common;
using Trend.AnalysisService;

namespace AdministratorSite.Controllers
{
	public class StatisticsController : Controller
	{
		//private static List<CurrentSprintProxy> sprintProxyList = new List<CurrentSprintProxy>();

		// GET: Statistics
		public ActionResult Index()
		{
			var appData = App.GetReleaseScrumData();
			//foreach (var sprint in appData.Item1.Sprints)
			//{
			//	var sprintProxy = new CurrentSprintProxy() { CurrentSprint = sprint };
			//	sprintProxy.AnalyzeCompletedStory();
			//	sprintProxy.AnalyzeFailedStory();

			//	sprintProxyList.Add(sprintProxy);
			//}

			Dictionary<string, List<Story>> personToStories = new Dictionary<string, List<Story>>();
			foreach (var sprint in appData.ReleaseData.Sprints)
			{
				foreach (var story in sprint.Stories.Where(t => t.IsComplete()))
				{
					if(NameUtil.InValidNames.Contains(story.Owner))
					{
						continue;
					}

					var engName = NameUtil.ConvertToEngName(story.Owner);
					if (!personToStories.ContainsKey(engName))
					{
						personToStories[engName] = new List<Story>();
						personToStories[engName].Add(story);
					}
					else
					{
						personToStories[engName].Add(story);
					}
				}
			}

			personToStories = personToStories.OrderByDescending(t => t.Value.Count).ToDictionary(p => p.Key, o => o.Value);
			return View(personToStories);
		}

		[ChildActionOnly]
		public PartialViewResult FrequentWordView()
		{
			var model = new List<Dictionary<string, int>>();
			var r5 = WordCloudAnalyzer.Instance.GetWordCloudData(App.GetReleaseScrumData().ReleaseData);
			var r4 = WordCloudAnalyzer.Instance.GetWordCloudData(App.GetRelease4Data());
			var r3 = WordCloudAnalyzer.Instance.GetWordCloudData(App.GetRelease3Data());
			var r2 = WordCloudAnalyzer.Instance.GetWordCloudData(App.GetRelease2Data());
			var r1 = WordCloudAnalyzer.Instance.GetWordCloudData(App.GetRelease1Data());

			model.Add(r5);
			model.Add(r4);
			model.Add(r3);
			model.Add(r2);
			model.Add(r1);

			return PartialView("_FrequentWord", model);
		}

		[ChildActionOnly]
		public PartialViewResult StorySuccessRateView()
		{
			return PartialView("_StorySizeSuccessRate");
		}

		[ChildActionOnly]
		public PartialViewResult StorySizeCountView()
		{
			return PartialView("_StorySizeCountView");
		}

		[ChildActionOnly]
		public PartialViewResult SprintCompleteRateView()
		{
			return PartialView("_SprintCompleteRateView");
		}

		public ActionResult TotalCompletedStoryCount()
		{
			var count = App.TotalCompletedStoryCount();
			return Content(count);
		}

		public ActionResult TotalFailedStoryCount()
		{
			var count = App.TotalFailedStoryCount();
			return Content(count);
		}

		public ActionResult TotalTaskCount()
		{
			var appData = App.GetReleaseScrumData();
			var currentSprint = App.GetReleaseScrumData().ReleaseData.CurrentSprint;

			int total = 0;
			foreach (var story in currentSprint.Stories)
			{
				total += story.Tasks.Count;

			}
			return Content(total.ToString());
		}

		public ActionResult TotalCompletedTaskCount()
		{
			var appData = App.GetReleaseScrumData();
			var currentSprint = App.GetReleaseScrumData().ReleaseData.CurrentSprint;

			int total = 0;
			foreach (var story in currentSprint.Stories)
			{
				foreach (var task in story.Tasks)
				{
					if (task.IsComplete())
					{
						total++;
					}
				}
			}
			return Content(total.ToString());
		}

		public ActionResult TotalCompletedTaskSize()
		{
			var appData = App.GetReleaseScrumData();
			var currentSprint = App.GetReleaseScrumData().CurrentSprintProxy;

			string total = currentSprint.GetCompletedTaskSizes().ToString();
			return Content(total.ToString());
		}

		public ActionResult TotalLeftTaskSize()
		{
			var appData = App.GetReleaseScrumData();
			var currentSprint = App.GetReleaseScrumData().ReleaseData.CurrentSprint;

			decimal total = 0;
			foreach (var story in currentSprint.Stories)
			{
				foreach(var task in story.Tasks)
				{
					if(!task.IsComplete())
					{
						total += task.WorkToDo;
					}
				}
			}
			return Content(total.ToString());
		}

		public ActionResult TotalStoryInRiskORException()
		{
			int total = 0;
			var currentSprint = App.GetReleaseScrumData().CurrentSprintProxy;
			if(currentSprint!=null)
			{
				total += currentSprint.HighRiskPersons.Count;
				total += currentSprint.HighRiskStories.Count;
				total += currentSprint.StoriesInException.Count;
				total += currentSprint.TasksInException.Count;
			}

			return Content(total.ToString());
		}
	}
}