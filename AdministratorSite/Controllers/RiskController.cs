using System.Web.Mvc;
using Trend.AnalysisService;
using Trend.DataModel;

namespace AdministratorSite.Controllers
{
	public class RiskController : Controller
    {
        // GET: Risk
        public ActionResult Index()
        {
			RiskSummaryData model = new RiskSummaryData();
			if (App.GetReleaseScrumData().CurrentSprintProxy != null)
			{
				model.PersonInHighRisk = App.GetReleaseScrumData().CurrentSprintProxy.HighRiskPersons;
				model.StoryInHighRisk = App.GetReleaseScrumData().CurrentSprintProxy.HighRiskStories;
				model.StoryInException = App.GetReleaseScrumData().CurrentSprintProxy.StoriesInException;
				model.TaskInException = App.GetReleaseScrumData().CurrentSprintProxy.TasksInException;
			}
			return View(model);
		}
    }
}