using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Trend.Common;

namespace Trend.DataModel
{
	[Serializable]
	[CollectionName("DE5ScrumData")]
	public class Release
	{
		//[BsonRepresentation(BsonType.ObjectId)]
		public string _id { get; set; }

		public string ProductName { set; get; }

		public DateTime LastUpdateTime { get; set; }

		public List<Sprint> Sprints = new List<Sprint>();

		[XmlIgnore]
		[JsonIgnore]
		[BsonIgnore]
		public Sprint CurrentSprint
		{
			get
			{
				DateTime now = DateTime.Now;
				foreach (var sprint in Sprints)
				{
					if (sprint.StartTime <= now && now < sprint.EndTime.AddDays(1))
					{
						return sprint;
					}
				}

				if (Sprints.Any())
				{
					return Sprints.First();
				}

				return Sprint.Null;
			}
		}

		public List<Story> GetAllStories()
		{
			var stories = new List<Story>();
			foreach(var sprint in Sprints)
			{
				stories.AddRange(sprint.Stories);
			}

			return stories;
		}
	}
}
