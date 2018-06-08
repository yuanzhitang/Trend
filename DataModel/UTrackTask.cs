using System;

namespace Trend.DataModel
{
	public class UTrackTask
	{
		public string ID { get; set; }
		public string Description { get; set; }
		public TaskType Type { get; set; }
		public int Priority { get; set; }
		public string Owner { get; set; }
		public TaskStatus Status { get; set; }
		public decimal Estimate { get; set; }
		public decimal WorkDone { get; set; }
		public decimal WorkToDo { get; set; }
		public String CreatedOwner { get; set; }
		public String ModifiedOwner { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public bool IsComplete()
		{
			return Status == TaskStatus.Accept || Status == TaskStatus.Done || Status == TaskStatus.Pass;
		}
	}
}