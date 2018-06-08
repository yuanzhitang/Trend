using System;
using System.Collections.Generic;
using System.Linq;

namespace Trend.DataModel
{
	public class Sprint
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public List<Story> Stories = new List<Story>();

		public List<Story> GetUnCompletedStories()
		{
			return Stories.Where(t => !t.IsComplete()).ToList();
		}

		public decimal GetStoryPoints()
		{
			decimal total = 0;
			foreach(var story in Stories)
			{
				total += story.Size;
			}

			return total;
		}

		public static Sprint Null = new Sprint();
	}

	public class MemberState
	{
		public string Name;
		public string StatusColor;
		public string StatusImg;
		public string StatusTip;
		public string Detail;
	}

	public class PersonRiskItem
	{
		public string Owner;
		public string RiskDetail;
		public decimal ExceedDay;
		public string OwningStories;
	}

    public class ExceptionItem
    {
        public string Owner;
        public string Item;
        public string ExceptionCategory;
        public string ExceptionImg;
        public string ExceptionDetail;
        public decimal Diff;
    }

	public class StoryProgressItem
	{
        public string StoryType;
		public string StoryDesc;
		public string Owner;
		public int Progress;
	}

	public class StoryProgressItemComparer : IComparer<StoryProgressItem>
	{
		public int Compare(StoryProgressItem x, StoryProgressItem y)
		{
			return y.Progress.CompareTo(x.Progress);
		}
	}

	public class TaskWallOfStory
	{
		public Story Story;
		public List<TaskWallOfTask> Tasks=new List<TaskWallOfTask>();
	}

	public class TaskWallOfTask
	{
		public UTrackTask Task;
		public int Percent;
	}
}
