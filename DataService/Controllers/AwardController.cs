using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService.DataAnalyzer;

namespace DataService.Controllers
{
	public class AwardController : ApiController
	{
		public AwardSummary GetAwards()
		{
			AwardSummary awardSumamry = AwardAnalyzer.GetAwardSummary();
			return awardSumamry;
		}
	}
}
