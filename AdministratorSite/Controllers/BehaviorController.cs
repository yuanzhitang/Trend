using System.Web.Mvc;
using Unisys.Trend.AnalysisService.DataAnalyzer;
using Unisys.Trend.DataModel;

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