using System.Collections.Generic;
using System.Web.Mvc;
using Trend.DataModel;
using Trend.AnalysisService;

namespace AdministratorSite.Controllers
{
	public class TaskWallController : Controller
	{
		public ActionResult Index()
		{
			Dictionary<string, int> personToTaskCount = App.TaskWallOfPersonTasks();

			return View(personToTaskCount);
		}

		public ActionResult GetTaskWallTitle()
		{
			string title = string.Empty;
			var appData = App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint;
			if (appData != null)
			{
				//title = $"{App.GetRelease5Data().ReleaseData.ProductName} {appData.CurrentSprint.Name} Story Task Wall";
				title = $"Task Wall";
			}
			else
			{
				title = "Sprint is not started";
			}

			return Content(title);
		}

		[ChildActionOnly]
		public PartialViewResult StoryView(string personName)
		{
			List<TaskWallOfStory> result = App.GetTaskWallOfStorySummary(personName);
			return PartialView("_StoryView", result);
		}

		[ChildActionOnly]
		public PartialViewResult TaskView(string personName)
		{
			List<List<TaskWallOfTask>> listOfFourItem = App.GetTaskWallOfTaskSummary(personName);
			return PartialView("_TaskView", listOfFourItem);
		}
	}
}