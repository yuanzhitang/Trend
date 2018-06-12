using System.Web.Http;
using Unisys.Trend.DataModel;
using Unisys.Trend.AnalysisService.DataAnalyzer;

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
