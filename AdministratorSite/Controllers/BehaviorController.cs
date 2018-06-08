using System.Web.Mvc;
using Trend.AnalysisService.DataAnalyzer;
using Trend.DataModel;

namespace AdministratorSite.Controllers
{
	public class BehaviorController : Controller
	{
		// GET: Statistics
		public ActionResult Index()
		{
			AwardSummary awardSumamry = AwardAnalyzer.GetAwardSummary();

			ViewBag.PersonOfIndustrious = awardSumamry.PersonOfIndustrious;
			ViewBag.PersonOfSilent = awardSumamry.PersonOfSilent;
			ViewBag.PersonOfWarrior = awardSumamry.PersonOfWarrior;
			return View();
		}
	}
}