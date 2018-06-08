using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trend.AnalysisService;
using Trend.DataModel;

namespace DataService.Controllers
{
    public class PredictionController : ApiController
    {
		public IEnumerable<Story> GetPredictionData()
		{
			NaiveBayesAnalyzer analyzer = new NaiveBayesAnalyzer();
			analyzer.Analyze();
			var currentSprintStories = App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint.Stories;
			return currentSprintStories;
		}
	}
}
