using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Trend.Common;
using Trend.DataModel;

namespace Trend.AnalysisService
{
	public class SprintProxy
	{
		public Sprint CurrentSprint { get;set; }

		private List<Story> Stories
		{
			get
			{
				return CurrentSprint.Stories;
			}
		}

		[XmlIgnore]
		[JsonIgnore]
		public List<String> Members = new List<string>();

		[XmlIgnore]
		[JsonIgnore]
		public Dictionary<Story, string> HighRiskStories = new Dictionary<Story, string>();

		[XmlIgnore]
		[JsonIgnore]
		public List<ExceptionItem> StoriesInException = new List<ExceptionItem>();

		[XmlIgnore]
		[JsonIgnore]
		public List<ExceptionItem> TasksInException = new List<ExceptionItem>();

		[XmlIgnore]
		[JsonIgnore]
		public List<ActivityItem> RecentActivities = new List<ActivityItem>();

		[XmlIgnore]
		[JsonIgnore]
		public List<PersonRiskItem> HighRiskPersons = new List<PersonRiskItem>();

		[XmlIgnore]
		[JsonIgnore]
		public List<Story> CompletedStories = new List<Story>();

		[XmlIgnore]
		[JsonIgnore]
		public List<Story> FailedStories = new List<Story>();

		[XmlIgnore]
		[JsonIgnore]
		public List<StoryProgressItem> ProgressOfStories = new List<StoryProgressItem>();

		[XmlIgnore]
		[JsonIgnore]
		public List<TaskSizeProgressItem> PersonTasks = new List<TaskSizeProgressItem>();

		[XmlIgnore]
		[JsonIgnore]
		public List<UTrackTask> HighRiskTasks = new List<UTrackTask>();

		public List<ActivityItem> GetRecentActivity()
		{
			RecentActivities.Clear();

			foreach (var story in Stories)
			{
				var engName = NameUtil.ConvertToEngName(story.ModifiedOwner);

				var existedOne = RecentActivities.FirstOrDefault(t => t.Name == engName);

				if (existedOne == null)
				{
					RecentActivities.Add(new ActivityItem()
					{
						Name = engName,
						ModifiedTime = story.Modified,
						EventObject = story.Type + " " + story.ID + " " + story.Title
					});
				}
				else
				{
					if (story.Modified > existedOne.ModifiedTime)
					{
						existedOne.ModifiedTime = story.Modified;
						existedOne.EventObject = story.Type + " " + story.ID + " " + story.Title;
					}
				}

				foreach (var tsk in story.Tasks)
				{
					engName = NameUtil.ConvertToEngName(tsk.ModifiedOwner);

					existedOne = RecentActivities.FirstOrDefault(t => t.Name == engName);

					if (existedOne == null)
					{
						RecentActivities.Add(new ActivityItem()
						{
							Name = engName,
							ModifiedTime = tsk.Modified,
							EventObject = tsk.Type + " " + tsk.ID + " " + tsk.Description
						});
					}
					else
					{
						if (tsk.Modified > existedOne.ModifiedTime)
						{
							existedOne.ModifiedTime = tsk.Modified;
							existedOne.EventObject = tsk.Type + " " + tsk.ID + " " + tsk.Description;
						}
					}
				}
			}

			RecentActivities = RecentActivities.OrderByDescending(t => t.ModifiedTime).ToList();
			RecentActivities.ForEach(activity =>
			{
				int subDays = DateTimeUtil.GetSubstractDays(activity.ModifiedTime);
				int subWorkingDays = DateTimeUtil.GetSubstractWorkingDays(activity.ModifiedTime);
				activity.ReadableDateTime = activity.GetReadableDateTimeText(subDays);
				if (subWorkingDays > 4)
				{
					activity.StatusImg = "Deep Sleep";
				}
				else if (subWorkingDays > 1)
				{
					var recentActivity = App.CurrentSprintActivity.Items.FirstOrDefault(t => t.Name == activity.Name);
					if (recentActivity != null)
					{
						subWorkingDays = DateTimeUtil.GetSubstractWorkingDays(recentActivity.Time);
						if (subWorkingDays <= 2)
						{
							//Updated uTrack yesterday, if we calculate this working days today, the subworkingday is 2
							activity.StatusImg = "work";
						}
						else
						{
							activity.StatusImg = "sleep";
						}
					}
					else
					{
						activity.StatusImg = "work";
					}
				}
				else
				{
					activity.StatusImg = "work";
				}

				var personTasks = PersonTasks.FirstOrDefault(t => t.Name == activity.Name);
				if (personTasks != null && personTasks.Left == 0)
				{
					activity.StatusImg = "Free";
				}
			});

			return RecentActivities;
		}

		public List<MemberState> GetTeamMemberState()
		{
			var memberStateList = new List<MemberState>();
			foreach (var item in HighRiskPersons.OrderByDescending(t => t.ExceedDay))
			{
				var memberState = new MemberState();
				memberState.Name = item.Owner;
				memberState.Detail = item.RiskDetail;
				if (item.ExceedDay >= 5)
				{
					memberState.StatusColor = "blueviolet";
				}
				if (item.ExceedDay >= 3)
				{
					memberState.StatusColor = "red";
				}
				else
				{
					memberState.StatusColor = "orange";
				}

				var activityITem = RecentActivities.FirstOrDefault(t => t.Name == memberState.Name);
				if (activityITem != null)
				{
					memberState.StatusImg = activityITem.StatusImg;

					if (memberState.StatusImg == "Free")
					{
						memberState.StatusTip = "Completed All Tasks";
					}
					else
					{
						memberState.StatusTip = activityITem.ReadableDateTime + ", updated " + activityITem.EventObject;
					}
				}
				else
				{
					memberState.StatusImg = "work";
				}

				memberStateList.Add(memberState);
			}

			foreach (var member in Members)
			{
				if (member == NameUtil.NoOwner)
				{
					continue;
				}
				if (!memberStateList.Exists(t => t.Name == member))
				{
					var memberState = new MemberState();
					memberState.Name = member;
					memberState.StatusColor = "green";
					memberState.Detail = "No Risk";

					var activityITem = RecentActivities.FirstOrDefault(t => t.Name == memberState.Name);
					if (activityITem != null)
					{
						memberState.StatusImg = activityITem.StatusImg;
						if (memberState.StatusImg == "Free")
						{
							memberState.StatusTip = $"{activityITem.Name} have completed all tasks.";
						}
						else
						{
							memberState.StatusTip = activityITem.ReadableDateTime + ", updated " + activityITem.EventObject;
						}
					}
					else
					{
						var personTasks = PersonTasks.FirstOrDefault(t => t.Name == memberState.Name);
						if (personTasks != null && personTasks.Left == 0)
						{
							memberState.StatusImg = "Free";
						}
						else
						{
							memberState.StatusImg = "work";
						}
					}

					memberStateList.Add(memberState);
				}
			}

			return memberStateList.Where(t => !AppConfig.IgnoredMembers.Contains(t.Name)).ToList();
		}

		public IEnumerable<Story> GetCompletedStories()
		{
			return Stories.Where(t => t.Status == StoryStatus.Accepted || t.Status == StoryStatus.Done);
		}

		public void AnalyzePersonTasks()
		{
			PersonTasks.Clear();
			foreach (var story in Stories)
			{
				foreach (var task in story.Tasks)
				{
					var engName = NameUtil.ConvertToEngName(task.Owner);
					var currentPerson = PersonTasks.FirstOrDefault(t => t.Name == engName);
					if (currentPerson == null)
					{
						PersonTasks.Add(new TaskSizeProgressItem() { Name = engName, Total = task.Estimate, Left = task.WorkToDo });
					}
					else
					{
						currentPerson.Total += task.Estimate;
						currentPerson.Left += task.WorkToDo;
					}
				}
			}
		}

		public decimal GetTaskSizes()
		{
			decimal totalSize = 0;
			foreach (var story in Stories)
			{
				foreach (var tsk in story.Tasks)
				{
					totalSize += tsk.Estimate;
				}
			}

			return totalSize;
		}

		public List<UTrackTask> GetInCompleteTasks()
		{
			var result = new List<UTrackTask>();
			Stories.ForEach(t =>
			{
				result.AddRange(t.GetInCompleteTasks());
			});

			return result;
		}

		public List<UTrackTask> GetCompletedTasks()
		{
			var result = new List<UTrackTask>();
			Stories.ForEach(t =>
			{
				result.AddRange(t.GetCompleteTasks());
			});

			return result;
		}

		public decimal GetCompletedTaskSizes()
		{
			decimal totalSize = 0;
			foreach (var story in Stories)
			{
				foreach (var tsk in story.Tasks.Where(t => t.IsComplete()))
				{
					totalSize += tsk.WorkDone;
				}
			}

			return totalSize;
		}

		public void Analyze()
		{
			AnalyzeProgressForStory();
			AnalyzeHighRiskForPersonByStory();
			AnalyzeHighRiskForPersonByTask();
			AnalyzeHighRiskForStory();
			AnalyzeStoryTaskWithException();
			AnalyzeCompletedStory();
			AnalyzeFailedStory();
			AnalyzePersonTasks();
			InitAllMembers();
		}

		private void InitAllMembers()
		{
			foreach (var story in Stories)
			{
				string engName = NameUtil.ConvertToEngName(story.Owner);
				if (!Members.Contains(engName))
				{
					Members.Add(engName);
				}

				foreach (var tsk in story.Tasks)
				{
					engName = NameUtil.ConvertToEngName(tsk.Owner);
					if (!Members.Contains(engName))
					{
						Members.Add(engName);
					}
				}
			}
		}

		public void AnalyzeFailedStory()
		{
			FailedStories.Clear();

			foreach (var story in Stories)
			{
				if (story.IsFailed())
				{
					FailedStories.Add(story);
				}
			}

			FailedStories.Sort((x, y) =>
			{
				return y.Modified.CompareTo(x.Modified);
			});
		}

		public void AnalyzeCompletedStory()
		{
			CompletedStories.Clear();

			foreach (var story in Stories)
			{
				if (story.IsComplete())
				{
					CompletedStories.Add(story);
				}
			}

			CompletedStories.Sort((x, y) =>
			{
				return y.Modified.CompareTo(x.Modified);
			});
		}

		private void AnalyzeHighRiskForStory()
		{
			foreach (var story in CurrentSprint.GetUnCompletedStories())
			{
				var errorDesc = string.Empty;
				var errorType = string.Empty;
				if (IsHighRisk(story, ref errorDesc))
				{
					HighRiskStories.Add(story, errorDesc);
				}
			}
		}

		private void AnalyzeStoryTaskWithException()
		{
			foreach (var story in CurrentSprint.GetUnCompletedStories())
			{
				var errorDesc = string.Empty;
				var errorType = string.Empty;
				var errorImg = string.Empty;
				decimal diffDesc = 0;

				if (IsSizeMismatch(story, ref errorType, ref errorDesc, ref errorImg, ref diffDesc))
				{
					var exItem = new ExceptionItem()
					{
						Owner = NameUtil.ConvertToEngName(story.Owner),
						Item = story.Type + " " + story.ID + " " + story.Title,
						ExceptionCategory = errorType,
						ExceptionImg = errorImg,
						ExceptionDetail = errorDesc,
						Diff = diffDesc
					};
					StoriesInException.Add(exItem);
				}

				foreach (var task in story.GetInCompleteTasks())
				{
					if (NameUtil.ConvertToEngName(task.Owner) == NameUtil.NoOwner)
					{
						var exItem = new ExceptionItem()
						{
							Owner = NameUtil.ConvertToEngName(task.Owner),
							Item = "Task " + task.ID + " " + task.Description,
							ExceptionCategory = "No Owner",
							ExceptionImg = "No Owner",
							ExceptionDetail = "This task has no owner. It belongs to " + story.Type + " " + story.GetBreifDesc()
						};
						TasksInException.Add(exItem);
					}
					else if (task.Estimate == 0)
					{
						var exItem = new ExceptionItem()
						{
							Owner = NameUtil.ConvertToEngName(task.Owner),
							Item = "Task " + task.ID + " " + task.Description,
							ExceptionCategory = "This task's estimation is 0",
							ExceptionImg = "Zero",
							ExceptionDetail = "This task's estimation is 0. It belongs to " + story.Type + " " + story.GetBreifDesc()
						};
						TasksInException.Add(exItem);
					}
				}

				//Michael Task or Story size is 0/
			}
		}

		private void AnalyzeHighRiskForPersonByStory()
		{
			Dictionary<string, List<Story>> personToStories = new Dictionary<string, List<Story>>();
			Dictionary<string, List<decimal>> personToWorkToDo = new Dictionary<string, List<decimal>>();
			foreach (var story in CurrentSprint.GetUnCompletedStories())
			{
				string ownerEngName = NameUtil.ConvertToEngName(story.Owner);

				if (!personToWorkToDo.ContainsKey(ownerEngName))
				{
					personToWorkToDo[ownerEngName] = new List<decimal>();
					personToStories[ownerEngName] = new List<Story>();
				}

				personToWorkToDo[ownerEngName].Add(GetTotalStoryWorkToDo(story));
				personToStories[ownerEngName].Add(story);
			}

			int availableDays = DateTimeUtil.GetWorkingDays(DateTime.Now, CurrentSprint.EndTime);
			foreach (var item in personToWorkToDo)
			{
				decimal totalWorkToDoInDays = 0;
				item.Value.ForEach(t => totalWorkToDoInDays += t);

				if (totalWorkToDoInDays > availableDays)
				{
					string errorDesc = $"This member has a total of {item.Value.Count} story/stories.They still need {totalWorkToDoInDays} day(s), but there are only {availableDays} day(s) left in the sprint.";
					var ownerStories = personToStories[item.Key].Select(t => t.Type + " " + t.ID + " " + t.Title + "- Size:" + t.Size);
					var persionRiskItem = new PersonRiskItem() { Owner = item.Key, ExceedDay = totalWorkToDoInDays - availableDays, OwningStories = string.Join("\n", ownerStories), RiskDetail = errorDesc };
					HighRiskPersons.Add(persionRiskItem);
				}
			}
		}

		private void AnalyzeHighRiskForPersonByTask()
		{
			Dictionary<string, List<UTrackTask>> personToTasks = new Dictionary<string, List<UTrackTask>>();
			Dictionary<string, List<decimal>> personToWorkToDo = new Dictionary<string, List<decimal>>();

			foreach (var story in Stories)
			{
				foreach (var task in story.Tasks)
				{
					string ownerEngName = NameUtil.ConvertToEngName(task.Owner);

					if (!personToWorkToDo.ContainsKey(ownerEngName))
					{
						personToWorkToDo[ownerEngName] = new List<decimal>();
						personToTasks[ownerEngName] = new List<UTrackTask>();
					}

					personToWorkToDo[ownerEngName].Add(task.WorkToDo);
					personToTasks[ownerEngName].Add(task);
				}
			}

			int availableDays = DateTimeUtil.GetWorkingDays(DateTime.Now, CurrentSprint.EndTime);
			foreach (var item in personToWorkToDo)
			{
				decimal totalWorkToDoInDays = 0;
				item.Value.ForEach(t => totalWorkToDoInDays += t);
				totalWorkToDoInDays = totalWorkToDoInDays / 8;

				if (totalWorkToDoInDays > availableDays)
				{
					string errorDesc = $"This member has a total of {item.Value.Count} task(s). They still need {totalWorkToDoInDays} day(s), but there are only {availableDays} day(s) left in the sprint";
					var owningTasks = personToTasks[item.Key].Select(t => t.Type + " " + t.ID + " " + t.Description + " - Size:" + t.Estimate);
					var persionRiskItem = new PersonRiskItem() { Owner = item.Key, ExceedDay = totalWorkToDoInDays - availableDays, OwningStories = string.Join("\n", owningTasks), RiskDetail = errorDesc };
					if (!HighRiskPersons.Any(t => t.Owner == persionRiskItem.Owner))
					{
						HighRiskPersons.Add(persionRiskItem);
					}
				}
			}
		}

		private void AnalyzeProgressForStory()
		{
			foreach (var story in CurrentSprint.GetUnCompletedStories())
			{
				StoryProgressItem progressItem = GetStoryProgress(story);
				ProgressOfStories.Add(progressItem);
			}

			ProgressOfStories.Sort(new StoryProgressItemComparer());
		}

		private StoryProgressItem GetStoryProgress(Story story)
		{
			var progressItem = new StoryProgressItem();
			progressItem.StoryType = story.Type.ToString();
			progressItem.StoryDesc = story.GetDesc();
			progressItem.Owner = NameUtil.ConvertToEngName(story.Owner);
			decimal totalWorkDone = GetTotalStoryWorkDoInHours(story);
			decimal totalWorkToDo = GetTotalStoryWorkToDoInHours(story);

			if ((totalWorkToDo + totalWorkDone) == 0)
			{
				progressItem.Progress = 0;
			}
			else
			{
				progressItem.Progress = (int)((totalWorkDone / (totalWorkToDo + totalWorkDone)) * 100);
			}

			return progressItem;
		}

		private bool IsHighRisk(Story story, ref string errorDesc)
		{
			int availableDays = DateTimeUtil.GetWorkingDays(DateTime.Now, CurrentSprint.EndTime);
			decimal totalToDoInDays = GetTotalStoryWorkToDo(story);

			if (totalToDoInDays > availableDays)
			{
				errorDesc = $"This story needs {totalToDoInDays} day(s), but there are only {availableDays} day(s) left in the sprint.";
				return true;
			}

			return false;
		}

		private bool IsSizeMismatch(Story story, ref string errorType, ref string errorDesc, ref string errorImg, ref decimal diffDesc)
		{
			if (story.Status == StoryStatus.Pending)
			{
				decimal storyEstimate = story.Size * 8;
				decimal totalTaskEstimate = GetTotalStoryTaskEstimateInHours(story);

				if (storyEstimate == 0)
				{
					errorType = "Story Size is 0. ";
					errorDesc = $"This Story Size is Zero, it is invalid";
					errorImg = "Zero";
					return true;
				}
				bool mismatch = (totalTaskEstimate / storyEstimate) < 0.8M;
				if (mismatch)
				{
					errorImg = "Up";
					errorType = "Story size greater than total task size";
					errorDesc = $"This story needs {story.Size} days, but total estimate task size only: {(totalTaskEstimate / 8).ToString("#0.00")} days which less than 80% of total size";
					diffDesc = story.Size - (totalTaskEstimate / 8);
				}
				else
				{
					mismatch = (storyEstimate / totalTaskEstimate) < 0.8M;
					if (mismatch)
					{
						diffDesc = story.Size - (totalTaskEstimate / 8);
						errorImg = "Down";
						errorType = "Story size less than total task size";
						errorDesc = $"Story size less than total task size. This story needs {story.Size} days, but total estimate task size needs: {(totalTaskEstimate / 8).ToString("#0.00")} days which greater than 120% of total size";
					}
				}
				return mismatch;
			}
			else if (story.Status == StoryStatus.InProcess)
			{
				decimal storyEstimate = story.Size * 8;
				decimal totalWorkDone = GetTotalStoryWorkDoInHours(story);
				decimal totalWorkToDo = GetTotalStoryWorkToDoInHours(story);
				bool mismatch = storyEstimate == 0 || ((totalWorkDone + totalWorkToDo) / storyEstimate) < 0.8M;
				if (mismatch)
				{
					diffDesc = story.Size - ((totalWorkDone + totalWorkToDo) / 8);
					errorImg = "Up";
					errorType = "Story size greater than total task size";
					errorDesc = $"This story needs {story.Size} days, but total task size only: {((totalWorkDone + totalWorkToDo) / 8).ToString("#0.00")} days which less than 80% of total size";
				}
				else
				{
					mismatch = (storyEstimate / (totalWorkDone + totalWorkToDo)) < 0.8M;
					if (mismatch)
					{
						diffDesc = story.Size - ((totalWorkDone + totalWorkToDo) / 8);
						errorImg = "Down";
						errorType = "Story Size less than TotalTaskSize ";
						errorDesc = $"This story needs {story.Size} days, but total estimate task size needs: {((totalWorkDone + totalWorkToDo) / 8).ToString("#0.00")} days which greater than 120% of total size";
					}
				}
				return mismatch;
			}

			return false;
		}

		private decimal GetTotalStoryWorkToDo(Story story)
		{
			if (story.Status == StoryStatus.Pending)
			{
				return story.Size;
			}
			else if (story.Status == StoryStatus.InProcess)
			{
				decimal totalWorkToDo = 0.0M;
				foreach (var task in story.GetInCompleteTasks())
				{
					totalWorkToDo += task.WorkToDo;
				}

				decimal totalNeedsDays = (totalWorkToDo / 8);

				return totalNeedsDays;
			}

			return 0;
		}

		private decimal GetTotalStoryTaskEstimateInHours(Story story)
		{
			decimal totalEstimate = 0.0M;
			foreach (var task in story.Tasks)
			{
				totalEstimate += task.Estimate;
			}
			return totalEstimate;
		}

		private decimal GetTotalStoryWorkToDoInHours(Story story)
		{
			if (story.Status == StoryStatus.Pending)
			{
				return story.Size * 8;
			}
			//else if (story.Status == StoryStatus.InProcess)
			//{
			decimal totalWorkToDo = 0.0M;
			foreach (var task in story.GetInCompleteTasks())
			{
				totalWorkToDo += task.WorkToDo;
			}
			return totalWorkToDo;
			//}

			//return 0;
		}

		private decimal GetTotalStoryWorkDoInHours(Story story)
		{
			if (story.Status == StoryStatus.Pending)
			{
				return 0;
			}

			decimal totalWorkDone = 0.0M;
			foreach (var task in story.Tasks)
			{
				totalWorkDone += task.WorkDone;
			}

			return totalWorkDone;
		}
	}
}
