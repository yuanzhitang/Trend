using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

namespace DataService.Controllers
{
    public class RiskController : ApiController
	{
		public RiskSummaryData GetRiskData()
		{
			RiskSummaryData model = new RiskSummaryData();
			if (App.GetReleaseScrumData().CurrentSprintProxy != null)
			{
				model.PersonInHighRisk = App.GetReleaseScrumData().CurrentSprintProxy.HighRiskPersons;
				model.StoryInHighRisk = App.GetReleaseScrumData().CurrentSprintProxy.HighRiskStories;
				model.StoryInException = App.GetReleaseScrumData().CurrentSprintProxy.StoriesInException;
				model.TaskInException = App.GetReleaseScrumData().CurrentSprintProxy.TasksInException;
			}

			return model;
		}
	}
}
