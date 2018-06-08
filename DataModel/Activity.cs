using System;
using System.Collections.Generic;
using Trend.Common;

namespace Trend.DataModel
{
	[CollectionName("DE5Activity")]
	public class PersonActivity
	{
		public string _id { get; set; }

		public List<PersonActivityItem> Items = new List<PersonActivityItem>();

		public static PersonActivity Empty = new PersonActivity();
	}

	public class ActivityItem
	{
		public string Name;
		public string EventObject;
		public DateTime ModifiedTime;
		public string StatusImg;
		public string ReadableDateTime;
		public string GetReadableDateTimeText(int days)
		{
			if (days <= 0)
			{
				return "Today";
			}

			if (days == 1)
			{
				return "Yesterday";
			}

			return $" {days} days ago";
		}
	}

	public class PersonActivityItem
	{
		public long ID;
		public DateTime Time;
		public string Name;
		public string Activity;
		public string Comment;
		public string Sprint;
	}
}
