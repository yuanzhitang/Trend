using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unisys.Trend.DataModel;

namespace Unisys.Trend.AnalysisService
{
	public class DataResult
	{
		public Release ReleaseData { get; set; }
		public SprintProxy CurrentSprintProxy { get; set; }
	}
}
