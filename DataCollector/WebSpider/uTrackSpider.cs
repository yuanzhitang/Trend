using System;
using System.Collections.Generic;
using System.Linq;
using Trend.Common;
using Trend.DataModel;
using HADocument = HtmlAgilityPack.HtmlDocument;

namespace Trend.DataCollector
{
	public class uTrackSpider : IWebSpider
	{
		#region Private Fields

		private string url = string.Empty;
		private bool IsFirstFetch = true;

		private Release ReleaseModel;

		#endregion

		#region Event

		public event Action<int, int> FetchProgressChanged;
		public event Action FetchCompleted;

		#endregion

		public object Data
		{
			get
			{
				return ReleaseModel;
			}
		}

		public uTrackSpider(string productName, string sprintHomeURL)
		{
			ReleaseModel = new Release()
			{
				ProductName = productName,
				_id = productName
			};

			url = sprintHomeURL;
		}

		public void Start()
		{
			if (IsFirstFetch || NeedRefreshInitData())
			{
				ReleaseModel.Sprints.Clear();
				foreach (var sprintInfo in GetSprintItems())
				{
					Sprint sprint = new Sprint() { Name = sprintInfo.Key, Url = ConfigManager.uTrackRootUrl + sprintInfo.Value };
					ReleaseModel.Sprints.Add(sprint);
				}
			}

			int total = 0;
			int current = 0;

			if (IsFirstFetch)
			{
				total = ReleaseModel.Sprints.Count;

				FetchProgressChanged(total, current);
				foreach (var sprint in ReleaseModel.Sprints)
				{
					LoadStoryInformation(sprint);
					current++;

					FetchProgressChanged(total, current);
				}

				IsFirstFetch = false;
			}
			else
			{
				total = ReleaseModel.CurrentSprint.Stories.Count();
				FetchProgressChanged(total, current);
				ReleaseModel.CurrentSprint.Stories.Clear();
				LoadStoryInformation(ReleaseModel.CurrentSprint);
			}

			ReleaseModel.LastUpdateTime = DateTime.Now;
		}

		private bool NeedRefreshInitData()
		{
			if (ReleaseModel.CurrentSprint == Sprint.Null)
			{
				return true;
			}
			else
			{
				return ReleaseModel.CurrentSprint.EndTime < DateTime.Now;
			}
		}

		#region Fetch Data

		private Dictionary<string, string> GetSprintItems()
		{
			var result = new Dictionary<string, string>();
			HtmlAgilityPack.HtmlNode htmlNode = FeatureData(url);

			var menu = htmlNode.SelectSingleNode("//*[@id='navMenuBar']/div/ul");

			var interations = menu.ChildNodes.Where(t => t.Name == "li").ToList()[2];
			var allSprints = interations.Descendants().Where(t => t.Name == "li").ToList();
			foreach (var sprintItem in allSprints)
			{
				var sprintName = sprintItem.InnerText;
				var sprintUrl = sprintItem.ChildNodes.First().Attributes["href"].Value;
				result.Add(sprintName, sprintUrl);
			}

			return result;
		}

		private void LoadStoryInformation(Sprint sprint)
		{
			HtmlAgilityPack.HtmlNode htmlNode = FeatureData(sprint.Url);

			LoadSprintInformation(sprint, htmlNode);

			var storyList = htmlNode.SelectSingleNode("//*[@id='iterationfeatures']").Descendants().Where(t => t.Name == "tr").ToList();

			int total = storyList.Count();
			int current = 0;
			foreach (var storyItem in storyList)
			{
				var storyItemColumn = storyItem.ChildNodes.Where(t => t.Name == "td").ToList();
				var Id = SpiderUtil.TrimHtmlTag(storyItemColumn[1].ChildNodes.Where(t => t.Name == "a").ToList().First().InnerText);
				if (string.IsNullOrEmpty(Id))
				{
					Id = SpiderUtil.TrimHtmlTag(storyItemColumn[1].ChildNodes.First().InnerText);
				}
				var name = SpiderUtil.TrimHtmlTag(storyItemColumn[2].ChildNodes.Where(t => t.Name == "div").ToList().First().InnerText);
				var type = SpiderUtil.TrimHtmlTag(storyItemColumn[3].Attributes["title"].Value);
				var priority = int.Parse(SpiderUtil.TrimHtmlTag(storyItemColumn[4].InnerText));
				var owner = SpiderUtil.TrimHtmlTag(storyItemColumn[5].ChildNodes.Last().InnerText);
				var status = SpiderUtil.TrimHtmlTag(storyItemColumn[6].Attributes["title"].Value);
				decimal size = decimal.Parse(SpiderUtil.TrimHtmlTag(storyItemColumn[7].InnerText));

				Story story = new Story();
				story.ID = Id;
				story.Title = name;
				story.Type = ConvertToStoryType(type);
				story.Priority = priority;
				story.Owner = owner;
				story.Status = (StoryStatus)Enum.Parse(typeof(StoryStatus), status.Replace(" ", ""));
				story.Size = size;

				LoadStoryCreateAndModifiedTime(story);
				LoadStoryTaskInfomation(story);
				sprint.Stories.Add(story);

				if (!IsFirstFetch)
				{
					current++;
					FetchProgressChanged(total, current);
				}
			}
		}

		private void LoadStoryTaskInfomation(Story story)
		{
			string url = string.Format("https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/TaskList?fid={0}", story.ID);
			HtmlAgilityPack.HtmlNode htmlNode = FeatureData(url);

			var taskList = htmlNode.Descendants().Where(t => t.Name == "tr").ToList();

			bool alreadySkippedHeader = false;
			foreach (var taskItem in taskList)
			{
				if (!alreadySkippedHeader)
				{
					alreadySkippedHeader = true;
					continue;
				}

				var taskItemColumn = taskItem.ChildNodes.Where(t => t.Name == "td").ToList();
				var Id = SpiderUtil.TrimHtmlTag(taskItemColumn[0].ChildNodes.Where(t => t.Name == "a").ToList().First().InnerText);
				if (string.IsNullOrEmpty(Id))
				{
					Id = SpiderUtil.TrimHtmlTag(taskItemColumn[0].ChildNodes.First().InnerText);
				}
				var name = SpiderUtil.TrimHtmlTag(taskItemColumn[1].ChildNodes.First().InnerText);
				var type = SpiderUtil.TrimHtmlTag(taskItemColumn[2].Attributes["title"].Value);
				var priority = int.Parse(SpiderUtil.TrimHtmlTag(taskItemColumn[3].InnerText));
				var owner = SpiderUtil.TrimHtmlTag(taskItemColumn[4].ChildNodes.Last().InnerText);
				var status = SpiderUtil.TrimHtmlTag(taskItemColumn[5].Attributes["title"].Value);
				decimal estimate = decimal.Parse(SpiderUtil.TrimHtmlTag(taskItemColumn[6].InnerText));
				decimal workDone = decimal.Parse(SpiderUtil.TrimHtmlTag(taskItemColumn[7].InnerText));
				decimal workTodo = decimal.Parse(SpiderUtil.TrimHtmlTag(taskItemColumn[8].InnerText));

				UTrackTask task = new UTrackTask();
				task.ID = Id;
				task.Description = name;
				task.Type = ConvertToTaskType(type);
				task.Priority = priority;
				task.Owner = owner;
				task.Status = (Trend.DataModel.TaskStatus)Enum.Parse(typeof(Trend.DataModel.TaskStatus), status.Replace(" ", ""));
				task.Estimate = estimate;
				task.WorkDone = workDone;
				task.WorkToDo = workTodo;

				if (story.Status == StoryStatus.Done || story.Status == StoryStatus.Pending || story.Status == StoryStatus.InProcess)
				{
					LoadTaskCreateAndModifyTime(task);
				}

				story.Tasks.Add(task);
			}
		}

		private void LoadTaskCreateAndModifyTime(UTrackTask task)
		{
			string url = string.Format("https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/TaskAdmin?tid={0}", task.ID);

			HtmlAgilityPack.HtmlNode htmlNode = FeatureData(url);

			var allRow = htmlNode.Descendants().Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "row").ToList();

			if (allRow.Any())
			{
				var createLabelText = SpiderUtil.TrimHtmlTag(allRow[10].InnerText);
				var modifiedLabelText = SpiderUtil.TrimHtmlTag(allRow[11].InnerText);
				var createdLable = createLabelText.Substring(createLabelText.Length - 10, 10);
				var modifiedLabel = modifiedLabelText.Substring(modifiedLabelText.Length - 10, 10);

				task.CreatedOwner = createLabelText.Substring(0, createLabelText.Length - 10).Replace("Created:", string.Empty).Trim();
				task.ModifiedOwner = modifiedLabelText.Substring(0, modifiedLabelText.Length - 10).Replace("Modified:", string.Empty).Trim();
				if (string.IsNullOrEmpty(task.ModifiedOwner))
				{
					task.ModifiedOwner = task.CreatedOwner;
				}
				task.Created = DateTime.Parse(createdLable);
				task.Modified = DateTime.Parse(modifiedLabel);
			}
		}

		private void LoadStoryCreateAndModifiedTime(Story story)
		{
			string url = string.Format("https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/FeatureAdmin?fid={0}", story.ID);

			HtmlAgilityPack.HtmlNode htmlNode = FeatureData(url);

			var allRow = htmlNode.Descendants().Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value == "row").ToList();
			var createLabelText = SpiderUtil.TrimHtmlTag(allRow[11].InnerText);
			var modifiedLabelText = SpiderUtil.TrimHtmlTag(allRow[12].InnerText);
			var createdLable = createLabelText.Substring(createLabelText.Length - 10, 10);
			var modifiedLabel = modifiedLabelText.Substring(modifiedLabelText.Length - 10, 10);

			story.CreatedOwner = createLabelText.Substring(0, createLabelText.Length - 10).Replace("Created:", string.Empty).Trim();
			story.ModifiedOwner = modifiedLabelText.Substring(0, modifiedLabelText.Length - 10).Replace("Modified:", string.Empty).Trim();
			if (string.IsNullOrEmpty(story.ModifiedOwner))
			{
				story.ModifiedOwner = story.CreatedOwner;
			}
			story.Created = DateTime.Parse(createdLable);
			story.Modified = DateTime.Parse(modifiedLabel);
		}

		private void LoadSprintInformation(Sprint sprint, HtmlAgilityPack.HtmlNode htmlNode)
		{
			var sectionList = htmlNode.SelectSingleNode("//*[@id='infoPanel']").Descendants().Where(t => t.Name == "tr").ToList();
			var IterationSection = sectionList[3];
			var durationItem = IterationSection.ChildNodes.Where(t => t.Name == "td").ToList()[1];

			var startAndEndTime = durationItem.InnerText.Split('-');

			sprint.StartTime = DateTime.Parse(startAndEndTime[0]);
			sprint.EndTime = DateTime.Parse(startAndEndTime[1]);
		}

		#endregion

		#region Helper

		private StoryType ConvertToStoryType(string type)
		{
			if (type == "Spike")
			{
				return StoryType.Spike;
			}
			else if (type == "User Story")
			{
				return StoryType.Story;
			}

			return StoryType.Story;
		}

		private TaskType ConvertToTaskType(string type)
		{
			if (type == "Task")
			{
				return TaskType.Task;
			}
			else if (type == "Test")
			{
				return TaskType.Test;
			}

			return TaskType.Task;
		}

		private HtmlAgilityPack.HtmlNode FeatureData(string url)
		{
			string cookieData = CookieUtil.GetCookies(ConfigManager.uTrackRootUrl);
			string reponseTaskHtml = SpiderUtil.GetHtml(url, cookieData, string.Empty);


			HADocument hadoc = new HADocument();
			hadoc.LoadHtml(reponseTaskHtml);

			HtmlAgilityPack.HtmlNode htmlNode = hadoc.DocumentNode;
			return htmlNode;
		}

		#endregion
	}
}
