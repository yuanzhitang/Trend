using System.Collections.Generic;
using System.Web.Http;
using Unisys.Trend.DataModel;
using Unisys.Trend.AnalysisService;

namespace DataService.Controllers
{
	public class RecentActivityController : ApiController
	{
		public IEnumerable<ActivityItem> GetRecentActivities()
		{
			var appData = App.GetReleaseScrumData();
			return appData.CurrentSprintProxy.GetRecentActivity();
		}
	}
}
