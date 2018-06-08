
using System;
using System.Linq;
using System.Web.Mvc;
using Trend.Common;
using Trend.DataModel;
using Trend.AnalysisService;

namespace AdministratorSite.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var homeTopStatistics = App.GetHomeTopStatistics();

			ViewBag.CurrentSprintName = homeTopStatistics.CurrentSprintName;
			ViewBag.TotalStories = homeTopStatistics.TotalStories;
			ViewBag.TotalStoryPoints = homeTopStatistics.TotalStoryPoints;
			ViewBag.CompletedStories = homeTopStatistics.CompletedStories;
			ViewBag.TotalTaskSizes = homeTopStatistics.TotalTaskSizes;
			ViewBag.TotalCompletedTaskSizes = homeTopStatistics.TotalCompletedTaskSizes;
			ViewBag.HighRiskStories = homeTopStatistics.HighRiskStories;
			ViewBag.HighRiskPerson = homeTopStatistics.HighRiskPerson;

			var appData = App.GetReleaseScrumData();
			ViewBag.RecentActivity = appData.CurrentSprintProxy.GetRecentActivity();
			ViewBag.MemberState = appData.CurrentSprintProxy.GetTeamMemberState();

			return View();
		}

		public ActionResult LastUpdatedTime()
		{
			return Content(App.LastUpdatedTime());
		}

		[ChildActionOnly]
		public PartialViewResult StoryProgressView()
		{
			return PartialView("_StoryProgress", App.GetReleaseScrumData().CurrentSprintProxy.ProgressOfStories);
		}

		[ChildActionOnly]
		public PartialViewResult LatestCompletedTaskView()
		{
			return PartialView("_LatestCompletedStory", App.GetReleaseScrumData().CurrentSprintProxy.CompletedStories);
		}
	}
}