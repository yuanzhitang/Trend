using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trend.DataModel
{
	public class Story
	{
		public string ID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public StoryType Type { get; set; }
		public int Priority { get; set; }
		public string Owner { get; set; }
		public StoryStatus Status { get; set; }
		public String CreatedOwner { get; set; }
		public String ModifiedOwner { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public decimal Size { get; set; }
		public List<UTrackTask> Tasks = new List<UTrackTask>();

		[JsonIgnore]
		public string PredicateSuccessRate { get; set; }
		[JsonIgnore]
		public string LRPreSuccessRate { get; set; }

		public bool IsComplete()
		{
			return Status == StoryStatus.Accepted || Status == StoryStatus.Done;
		}

		public bool IsFailed()
		{
			return Status == StoryStatus.Incomplete;
		}

		public string GetDesc()
		{
			return Type.ToString() + " " + ID + " : " + Title;
		}

		public string GetBreifDesc()
		{
			return ID + " : " + Title;
		}

		public IEnumerable<UTrackTask> GetInCompleteTasks()
		{
			return Tasks.Where(t => !t.IsComplete());
		}

		public IEnumerable<UTrackTask> GetCompleteTasks()
		{
			return Tasks.Where(t => t.IsComplete());
		}
	}
}