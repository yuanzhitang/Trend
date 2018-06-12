using System.Collections.Generic;
using System.Web.Http;
using Unisys.Trend.DataModel;
using Unisys.Trend.AnalysisService;

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
