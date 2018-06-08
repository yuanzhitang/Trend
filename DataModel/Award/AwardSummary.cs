using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.DataModel
{
	public class AwardSummary
	{
		public IEnumerable<String> PersonOfIndustrious { get; set; }
		public IEnumerable<String> PersonOfSilent { get; set; }
		public IEnumerable<String> PersonOfWarrior { get; set; }
	}
}
