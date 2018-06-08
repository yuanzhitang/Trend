using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trend.AnalysisService;

namespace AdministratorSite.Controllers
{
	public class PredictionController : Controller
	{
		// GET: Prediction
		public ActionResult Index()
		{
			NaiveBayesAnalyzer analyzer = new NaiveBayesAnalyzer();
			analyzer.Analyze();

			LogisticRegressionAnalyzer lrAnalyzer = new LogisticRegressionAnalyzer();
			lrAnalyzer.Analyze();

			var currentSprintStories = App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint.Stories;
			//var invalidStories = currentSprintStories.Where(s => analyzer.ExcludedOwners.Contains(s.Owner));
			//var validStories = currentSprintStories.Except(invalidStories);
			return View(currentSprintStories);
		}
	}
}