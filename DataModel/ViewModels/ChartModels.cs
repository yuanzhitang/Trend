using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trend.DataModel
{
	public class SprintStoryCompletionRate
	{
		public string SprintName;
		public decimal CompletionRate;
	}

	public class StoryCountProgessItem
	{
		public string Name;
		public int Total;
		public int Left;
	}

	public class StorySuccessRate
	{
		public decimal Size;
		public decimal SucessRate;
	}

	public class StorySizeCount
	{
		public decimal Size;
		public decimal Count;
	}
}