using Trend.DataModel;
using System.Web.Mvc;
using Trend.AnalysisService;

namespace AdministratorSite.Controllers
{
	public class ActivityController : Controller
	{
		// GET: Statistics
		public ActionResult Index()
		{
			return View(App.CurrentSprintActivity.Items);
		}

		public ActionResult GetActivityTitle()
		{
			string title = string.Empty;
			var appData = App.GetReleaseScrumData().ReleaseData.CurrentSprint;
			if (appData != null)
			{
				title = $"5.0 {appData.Name} Activity";
			}
			else
			{
				title = Resource.SprintNotStarted;
			}

			return Content(title);
		}
	}
}