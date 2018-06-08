using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;

namespace DataService.Controllers
{
	public class TaskProgressDataController : ApiController
	{
		public List<List<TaskWallOfTask>> GetAllTaskProgressData()
		{
			List<List<TaskWallOfTask>> result = App.GetTaskWallOfTaskSummary(string.Empty);

			return result;
		}

		// GET: api/Default/5
		public List<List<TaskWallOfTask>> GetPersonTaskProgressData(string personName)
		{
			List<List<TaskWallOfTask>> result = App.GetTaskWallOfTaskSummary(personName);
			return result;
		}
	}
}
