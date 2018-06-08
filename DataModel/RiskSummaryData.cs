using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.DataModel
{
	public class RiskSummaryData
	{
		public Dictionary<Story, string> StoryInHighRisk = new Dictionary<Story, string>();
		public List<PersonRiskItem> PersonInHighRisk = new List<PersonRiskItem>();
		public List<ExceptionItem> StoryInException = new List<ExceptionItem>();
		public List<ExceptionItem> TaskInException = new List<ExceptionItem>();
	}
}
