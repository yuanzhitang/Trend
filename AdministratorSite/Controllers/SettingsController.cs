
using Trend.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trend.AnalysisService;

namespace AdministratorSite.Controllers
{
	public class SettingsController : Controller
	{
		// GET: Statistics
		public ActionResult Index()
		{
			//var activies = new List<PersonActivityItem>();

			//for (int i = 0; i < 1000; i++)
			//{
			//	activies.Add(new PersonActivityItem()
			//	{
			//		ID = i,
			//		Time = DateTime.Now,
			//		Name = "Michael",
			//		Activity = "Modi the task of ..."
			//	});
			//}
			//var appData = ;
			//activies = activies.OrderByDescending(t => t.Time).ToList();

			return View(App.Availability);
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult SavePublicHoliday(Availability availability)
		{
			if(ModelState.IsValid)
			{
				App.SaveAvailability(availability);
			}

			return View("Index",availability);
		}

		public ActionResult GetTeamWorkingDaysTitle()
		{
			string title = string.Empty;
			var currentSprint = App.GetReleaseScrumData().ReleaseData.CurrentSprint;
			if (currentSprint != null)
			{
				title = $"5.0 {currentSprint.Name} Team Availability";
			}
			else
			{
				title = Resource.SprintNotStarted;
			}

			return Content(title);
		}
	}
}