using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Trend.Common;
using Trend.DataModel;
using Trend.Persitent;

namespace Trend.AnalysisService
{
	public class uTrackActivityAnalyzer : IDataAnalyzer
	{
		private string dataFile = string.Empty;
		private string lastDataFile = string.Empty;

		private ActivityDataService ActivityDataService;


		public uTrackActivityAnalyzer(string dataFile)
		{
			this.dataFile = dataFile;
			this.lastDataFile = dataFile + "_Last";
			InitRepository();
		}

		private void InitRepository()
		{
			ActivityDataService = RepositoryServiceFactory.CreateActitivyDataService();
		}

		public void Analyze()
		{
			string dataPath = @"..\..\AdministratorSite\CollectedData";
			string newdataFile = $@"{dataPath}\{dataFile}.json";
			string lastDataFile = $@"{dataPath}\{this.lastDataFile}.json";
			string currentSprintActivityDataFile = $@"{dataPath}\{AppConfig.CurrentSprintActivityFile}.json";

			Release currentCaptureRelease = null;
			Release lastCaptureRelease = null;

			if (File.Exists(newdataFile))
			{
				var currntContent = File.ReadAllText(newdataFile);
				currentCaptureRelease = JsonConvert.DeserializeObject<Release>(currntContent);
			}
			if (File.Exists(lastDataFile))
			{
				var lastContent = File.ReadAllText(lastDataFile);
				lastCaptureRelease = JsonConvert.DeserializeObject<Release>(lastContent);
			}

			var personActivity = GetExistingCurrentSprintActivities(currentSprintActivityDataFile, currentCaptureRelease.CurrentSprint.Name);
			if (lastCaptureRelease == null)
			{
				var curentSprintProxy = new SprintProxy() { CurrentSprint = currentCaptureRelease.CurrentSprint };
				foreach (var item in curentSprintProxy.GetRecentActivity())
				{
					personActivity.Items.Add(new PersonActivityItem()
					{
						Name = item.Name,
						Sprint = currentCaptureRelease.CurrentSprint.Name,
						Time = item.ModifiedTime,
						Activity = "Updated" + item.EventObject
					});
				}
			}
			else
			{
				if (File.Exists(currentSprintActivityDataFile))
				{
					if (personActivity.Items.Any(t => t.Sprint != currentCaptureRelease.CurrentSprint.Name))
					{
						string archiveSprintActivityDataFile = $@"{dataPath}\{personActivity.Items.First().Sprint}Activity.json";
						File.Move(currentSprintActivityDataFile, archiveSprintActivityDataFile);

						SaveIntoRepository(personActivity);
					}
				}

				if (currentCaptureRelease.CurrentSprint.Name == lastCaptureRelease.CurrentSprint.Name)
				{
					personActivity.Items.AddRange(AnalyzeSprintDiff(currentCaptureRelease.CurrentSprint, lastCaptureRelease.CurrentSprint));
				}
			}

			personActivity.Items = personActivity.Items.Where(t => t.Sprint == currentCaptureRelease.CurrentSprint.Name).OrderByDescending(t => t.Time).ToList();

			personActivity._id = currentCaptureRelease.CurrentSprint.Name;
			SaveIntoFile(currentSprintActivityDataFile, personActivity);
			SaveIntoRepository(personActivity);
		}

		private void SaveIntoRepository(PersonActivity personActivity)
		{
			ActivityDataService.Save(personActivity);
		}

		private static void SaveIntoFile(string currentSprintActivityDataFile, PersonActivity personActivity)
		{
			var data = JsonConvert.SerializeObject(personActivity);
			File.WriteAllText(currentSprintActivityDataFile, data, Encoding.UTF8);
		}

		private IEnumerable<PersonActivityItem> AnalyzeSprintDiff(Sprint current, Sprint last)
		{
			var result = new List<PersonActivityItem>();

			foreach (var story in current.Stories)
			{
				if (last.Stories.Any(t => t.ID == story.ID))
				{
					var oldStory = last.Stories.First(t => t.ID == story.ID);
					string detail = string.Empty;
					if (AnalyzeStoryDiff(story, oldStory, ref detail))
					{
						result.Add(new PersonActivityItem()
						{
							Time = DateTime.Now,
							Name = NameUtil.ConvertToEngName(story.ModifiedOwner),
							Activity = $"Updated story:{story.GetBreifDesc()}{Breakline}{detail}",
							Sprint = current.Name
						});
					}

					foreach (var task in story.Tasks)
					{
						var lastTask = oldStory.Tasks.FirstOrDefault(t => t.ID == task.ID);
						if (lastTask == null)
						{
							result.Add(new PersonActivityItem()
							{
								Time = task.Created,
								Name = string.IsNullOrEmpty(task.CreatedOwner) ? NameUtil.ConvertToEngName(task.CreatedOwner) : NameUtil.ConvertToEngName(task.ModifiedOwner),
								Activity = $"Created new Task:{task.ID}-{task.Description}",
								Sprint = current.Name
							});
						}
						else
						{
							string detailOfTaskDiff = string.Empty;
							if (AnalyzeTaskDiff(task, lastTask, ref detailOfTaskDiff))
							{
								result.Add(new PersonActivityItem()
								{
									Time = DateTime.Now,
									Name = NameUtil.ConvertToEngName(task.ModifiedOwner),
									Activity = $"Updated task:{task.ID}-{task.Description}{Breakline}{detailOfTaskDiff}",
									Sprint = current.Name
								});
							}
						}
					}
				}
				else
				{
					if (DateTime.Now.Subtract(story.Created).Days == 0)
					{
						result.Add(new PersonActivityItem()
						{
							Time = DateTime.Now,
							Name = NameUtil.ConvertToEngName(story.CreatedOwner),
							Activity = $"Created new story:{story.GetBreifDesc()}",
							Sprint = current.Name
						});
					}
				}
			}

			return result;
		}

		private bool AnalyzeTaskDiff(UTrackTask task, UTrackTask lastTask, ref string detail)
		{
			if (task.Description != lastTask.Description)
			{
				detail += $"{twoSpaces}Changed Description from '{lastTask.Description}' to '{task.Description}'{Breakline}";
			}
			if (task.Estimate != lastTask.Estimate)
			{
				detail += $"{twoSpaces}Changed Estimate from '{lastTask.Estimate}' to '{task.Estimate}'{Breakline}";
			}
			if (task.WorkDone != lastTask.WorkDone)
			{
				detail += $"{twoSpaces}Changed WorkDone from '{lastTask.WorkDone}' to '{task.WorkDone}'{Breakline}";
			}
			if (task.WorkToDo != lastTask.WorkToDo)
			{
				detail += $"{twoSpaces}Changed WorkToDo from '{lastTask.WorkToDo}' to '{task.WorkToDo}'{Breakline}";
			}
			if (task.Type != lastTask.Type)
			{
				detail += $"{twoSpaces}Changed Type from '{lastTask.Type.ToString()}' to '{task.Type.ToString()}'{Breakline}";
			}
			if (task.Owner != lastTask.Owner)
			{
				detail += $"{twoSpaces}Changed Owner from '{lastTask.Owner}' to '{task.Owner}'{Breakline}";
			}

			if (task.Status != lastTask.Status)
			{
				detail += $"{twoSpaces}Changed Status from '{lastTask.Status.ToString()}' to '{task.Status.ToString()}'{Breakline}";
			}

			if (string.IsNullOrEmpty(detail))
			{
				return false;
			}
			return true;
		}
		private const string Breakline = "\r\n";
		private const string twoSpaces = " -";

		private bool AnalyzeStoryDiff(Story currentStory, Story oldStory, ref string detail)
		{
			if (currentStory.Title != oldStory.Title)
			{
				detail += $"{twoSpaces}Changed Title from '{oldStory.Title}' to '{currentStory.Title}'{Breakline}";
			}
			if (currentStory.Description != oldStory.Description)
			{
				detail += $"{twoSpaces}Changed Description from '{oldStory.Description}' to '{currentStory.Description}'{Breakline}";
			}
			if (currentStory.Size != oldStory.Size)
			{
				detail += $"{twoSpaces}Changed Size from '{oldStory.Size}' to '{currentStory.Size}'{Breakline}";
			}
			if (currentStory.Owner != oldStory.Owner)
			{
				detail += $"{twoSpaces}Changed Owner from '{oldStory.Owner}' to '{currentStory.Owner}'{Breakline}";
			}

			if (currentStory.Status != oldStory.Status)
			{
				detail += $"{twoSpaces}Changed Status from '{oldStory.Status.ToString()}' to '{currentStory.Status.ToString()}'{Breakline}";
			}

			if (string.IsNullOrEmpty(detail))
			{
				return false;
			}
			return true;
		}

		private PersonActivity GetExistingCurrentSprintActivities(string filePath,string currentSprintName)
		{
			if (File.Exists(filePath))
			{
				var lastContent = File.ReadAllText(filePath);
				return JsonConvert.DeserializeObject<PersonActivity>(lastContent);
			}

			return new PersonActivity();
		}
	}
}
