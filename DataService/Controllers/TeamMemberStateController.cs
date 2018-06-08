using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class TeamMemberStateController : ApiController
	{
		public IEnumerable<MemberState> GetTeamMemberState()
		{
			var appData = App.GetReleaseScrumData();
			return appData.CurrentSprintProxy.GetTeamMemberState();
		}
	}
}
