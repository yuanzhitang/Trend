using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

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
