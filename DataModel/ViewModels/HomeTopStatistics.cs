using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.DataModel
{
	public class HomeTopStatistics
	{
		public string CurrentSprintName;

		public int TotalStories;
		public decimal TotalStoryPoints;
		public int CompletedStories;
		public decimal TotalTaskSizes;
		public decimal TotalCompletedTaskSizes;
		public int HighRiskStories;
		public int HighRiskPerson;
	}
}
