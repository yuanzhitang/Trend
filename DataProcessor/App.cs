using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.DataVisualization.Charting;
using Trend.Common;
using Trend.DataModel;
using Trend.Persitent;

namespace Trend.AnalysisService
{
	public class App
	{
		#region Private Fileds

		private static readonly string DataFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\CollectedData\\";
		
		private static DateTime lastUpdateTime = DateTime.Now;
		private static DateTime lastRefreshActivityTime = DateTime.Now;
		private static Release release5;
		private static Release release4;
		private static Release release3;
		private static Release release2;
		private static Release release1;
		private static PersonActivity personActivity;

		private static SprintProxy CurrentSprintProxy;
		private static Availability availability = null;

		private static ScrumDataService scrumDataService = null;
		private static ActivityDataService activityDataService = null;

		#endregion

		#region Initialize

		static App()
		{
			InitRepository();
		}

		private static void InitRepository()
		{
			if (scrumDataService == null)
			{
				scrumDataService = RepositoryServiceFactory.CreateScrumDataService(); 
			}

			if (activityDataService == null)
			{
				activityDataService = RepositoryServiceFactory.CreateActitivyDataService(); 
			}
		}

		#endregion

		public static DataResult GetReleaseScrumData()
		{
			if (release5 == null || DateTime.Now.Subtract(lastUpdateTime).TotalMinutes > 1)
			{
				release5 = ReadSprintDataFromRepository();

				if (release5 != null)
				{
					CurrentSprintProxy = new SprintProxy() { CurrentSprint = release5.CurrentSprint };
					CurrentSprintProxy.Analyze();
				}

				lastUpdateTime = DateTime.Now;
			}

			return new DataResult()
			{
				ReleaseData = release5,
				CurrentSprintProxy = CurrentSprintProxy
			};
		}

		public static PersonActivity CurrentSprintActivity
		{
			get
			{
				if (personActivity == null || DateTime.Now.Subtract(lastRefreshActivityTime).TotalMinutes > 1)
				{
					personActivity = ReadPersonActivityFromRepo();

					lastRefreshActivityTime = DateTime.Now;
				}

				return personActivity;
			}
		}

		public static Availability Availability
		{
			get
			{
				if (availability == null)
				{
					string path = $@"{DataFolder}{AppConfig.AvailabilityFile}.json";
					if (!File.Exists(path))
					{
						availability = Availability.Default;
					}
					else
					{
						availability = GetEntityModel<Availability>(AppConfig.AvailabilityFile);
					}
				}
				return availability;
			}
			set
			{
				availability = value;
			}
		}

		public static string TotalFailedStoryCount()
		{
			var appData = App.GetReleaseScrumData().ReleaseData;
			var failedStories = new List<Story>();

			List<SprintProxy> sprintProxyList = new List<SprintProxy>();
			foreach (var sprint in appData.Sprints)
			{
				var sprintProxy = new SprintProxy() { CurrentSprint = sprint };
				//sprintProxy.AnalyzeCompletedStory();
				sprintProxy.AnalyzeFailedStory();

				sprintProxyList.Add(sprintProxy);
			}

			sprintProxyList.ForEach(t =>
			{
				failedStories.AddRange(t.FailedStories);
			});

			return failedStories.Count().ToString();
		}

		#region Data Services

		public static string LastUpdatedTime()
		{
			return GetReleaseScrumData().ReleaseData.LastUpdateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
		}

		public  static Dictionary<string, int> TaskWallOfPersonTasks()
		{
			var personToTaskCount = new Dictionary<string, int>();
			var currentSprint = App.GetReleaseScrumData().CurrentSprintProxy.GetInCompleteTasks();
			foreach (var task in currentSprint)
			{
				string engName = NameUtil.ConvertToEngName(task.Owner);
				if (!personToTaskCount.ContainsKey(engName))
				{
					personToTaskCount[engName] = 1;
				}
				else
				{
					personToTaskCount[engName]++;
				}
			}

			foreach (var task in App.GetReleaseScrumData().CurrentSprintProxy.GetCompletedTasks())
			{
				string engName = NameUtil.ConvertToEngName(task.Owner);
				if (!personToTaskCount.ContainsKey(engName))
				{
					personToTaskCount[engName] = 0;
				}
			}

			return personToTaskCount;
		}

		public static string TotalCompletedStoryCount()
		{
			var appData = App.GetReleaseScrumData().ReleaseData;
			var completedStories = new List<Story>();

			List<SprintProxy> sprintProxyList = new List<SprintProxy>();
			foreach (var sprint in appData.Sprints)
			{
				var sprintProxy = new SprintProxy() { CurrentSprint = sprint };
				sprintProxy.AnalyzeCompletedStory();
				//sprintProxy.AnalyzeFailedStory();

				sprintProxyList.Add(sprintProxy);
			}

			sprintProxyList.ForEach(t =>
			{
				completedStories.AddRange(t.CompletedStories);
			});

			return completedStories.Count().ToString();
		}

		public static List<TaskWallOfStory> GetTaskWallOfStorySummary(string personName)
		{
			var result = new List<TaskWallOfStory>();
			foreach (var story in App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint.Stories)
			{
				var storyItem = new TaskWallOfStory();

				var englishName = NameUtil.ConvertToEngName(story.Owner);
				if (string.IsNullOrEmpty(personName) || englishName == personName)
				{
					storyItem.Story = story;
					storyItem.Tasks = new List<TaskWallOfTask>();

					foreach (var task in story.Tasks)
					{
						var taskItem = new TaskWallOfTask();
						taskItem.Task = task;
						//var progress = $"size:{task.WorkDone + task.WorkToDo}";
						int percent = 100;
						if (!task.IsComplete())
						{
							if ((task.WorkDone + task.WorkToDo) != 0)
							{
								percent = (int)((task.WorkDone / (task.WorkDone + task.WorkToDo)) * 100);
							}
						}

						taskItem.Percent = percent;

						storyItem.Tasks.Add(taskItem);


						//var englishName = NameUtil.ConvertToEngName(currentTask.Owner);

						//if (string.IsNullOrEmpty(personName) || personName == englishName)
						//{
						//	taskDetail.Add(string.Format(taskDetailFormat, "#" + currentTask.ID, englishName, currentTask.Description, progress, percent, colorSetting.Item1, colorSetting.Item2));
						//}
					}

					result.Add(storyItem);
				}
			}

			return result;
		}

		public static List<List<TaskWallOfTask>> GetTaskWallOfTaskSummary(string personName)
		{
			var listOfFourItem = new List<List<TaskWallOfTask>>();

			var result = new List<TaskWallOfTask>();

			var appData = App.GetReleaseScrumData();
			var AllInCompletedTask = App.GetReleaseScrumData().CurrentSprintProxy.GetInCompleteTasks();
			var AllCompletedTasks = App.GetReleaseScrumData().CurrentSprintProxy.GetCompletedTasks();

			var taskDetail = new List<string>();
			foreach (var currentTask in AllInCompletedTask.OrderBy(t => t.Owner))
			{
				//var colorSetting = GetColorSetting(currentTask.Owner);
				var englishName = NameUtil.ConvertToEngName(currentTask.Owner);

				if (string.IsNullOrEmpty(personName) || personName == englishName)
				{
					var progress = $"size:{currentTask.WorkDone + currentTask.WorkToDo}";
					int percent = 100;
					if ((currentTask.WorkDone + currentTask.WorkToDo) != 0)
					{
						percent = (int)((currentTask.WorkDone / (currentTask.WorkDone + currentTask.WorkToDo)) * 100);
					}

					result.Add(new TaskWallOfTask()
					{
						Task = currentTask,
						Percent = percent,
					});
				}
			}

			foreach (var currentTask in AllCompletedTasks.OrderBy(t => t.Owner))
			{
				var englishName = NameUtil.ConvertToEngName(currentTask.Owner);

				if (string.IsNullOrEmpty(personName) || personName == englishName)
				{
					result.Add(new TaskWallOfTask()
					{
						Task = currentTask,
						Percent = 100,
					});
				}
			}

			var listItem = new List<TaskWallOfTask>();
			foreach (var task in result)
			{
				listItem.Add(task);
				if (listItem.Count == 4)
				{
					listOfFourItem.Add(listItem);

					listItem = new List<TaskWallOfTask>();
				}
			}

			if (listItem.Any())
			{
				listOfFourItem.Add(listItem);
			}

			return listOfFourItem;
		}

		public static HomeTopStatistics GetHomeTopStatistics()
		{
			var statistics = new HomeTopStatistics();

			var appData = App.GetReleaseScrumData();
			if (appData.CurrentSprintProxy.CurrentSprint == Sprint.Null)
			{
				statistics.CurrentSprintName = $"{appData.ReleaseData.ProductName} No Sprint Data.";
			}
			else
			{
				statistics.CurrentSprintName = $"{appData.ReleaseData.ProductName}  <font color='green'>{appData.ReleaseData.CurrentSprint.Name}</font>. Duration: ({appData.ReleaseData.CurrentSprint.StartTime.ToString("MM/dd/yyyy")} ~ {appData.ReleaseData.CurrentSprint.EndTime.ToString("MM/dd/yyyy")}). Total working days: {DateTimeUtil.GetWorkingDays(appData.ReleaseData.CurrentSprint.StartTime, appData.ReleaseData.CurrentSprint.EndTime)}, Left days: <font color='red'>{ DateTimeUtil.GetWorkingDays(DateTime.Now, appData.ReleaseData.CurrentSprint.EndTime)}</font>";
			}

			statistics.TotalStories = appData.ReleaseData.CurrentSprint.Stories.Count();
			statistics.TotalStoryPoints = appData.ReleaseData.CurrentSprint.GetStoryPoints();
			statistics.CompletedStories = appData.CurrentSprintProxy.GetCompletedStories().Count();
			statistics.TotalTaskSizes = appData.CurrentSprintProxy.GetTaskSizes();
			statistics.TotalCompletedTaskSizes = appData.CurrentSprintProxy.GetCompletedTaskSizes();
			statistics.HighRiskStories = appData.CurrentSprintProxy.HighRiskStories.Count();
			statistics.HighRiskPerson = appData.CurrentSprintProxy.HighRiskPersons.Count();

			return statistics;
		}

		public static MemoryStream GetStoryChart()
		{
			var appData = App.GetReleaseScrumData();
			var currentSprintName = appData.ReleaseData.CurrentSprint.Name;
			string Total = "Total Stories";
			string Left = "Completed Stories";
			List<StoryCountProgessItem> allData = GetAllStoryProgressData();
			//List<int> data = mvcChart.Models.StaticModel.createStaticData();
			Chart Chart2 = new Chart();
			Chart2.Width = 1500;
			Chart2.Height = 300;
			Chart2.RenderType = RenderType.ImageTag;
			Chart2.Palette = ChartColorPalette.BrightPastel;
			Title t = new Title($"{appData.ReleaseData.ProductName} {currentSprintName} Story Completion Status", Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
			Chart2.Titles.Add(t);
			Chart2.ChartAreas.Add(Total);

			// create a couple of series   
			Chart2.Series.Add(Total);
			Chart2.Series.Add(Left);
			// add points to series 1   

			foreach (var item in allData)
			{
				DataPoint point = new DataPoint();
				point.AxisLabel = item.Name;
				point.SetValueY(item.Total);
				Chart2.Series[Total].Points.Add(point);
				Chart2.Series[Left].Points.AddY(item.Left);
			}

			Chart2.Series[Total].IsValueShownAsLabel = true;
			Chart2.Series[Left].IsValueShownAsLabel = true;
			Chart2.Series[Left].Color = Color.Green;
			//Chart2.Series[Left].LabelBackColor = Color.Green;

			Chart2.BorderSkin.SkinStyle = BorderSkinStyle.None;
			Chart2.BorderlineWidth = 2;
			Chart2.BorderColor = Color.Transparent;
			Chart2.BorderlineDashStyle = ChartDashStyle.NotSet;
			Chart2.BorderWidth = 2;
			Chart2.Legends.Add("Legend1");

			Chart2.ChartAreas[0].AxisX.Interval = 1;   //设置X轴坐标的间隔为1
			Chart2.ChartAreas[0].AxisX.IntervalOffset = 1;  //设置X轴坐标偏移为1
			Chart2.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;   //设置是否交错显示,比如数据多的时间分成两行来显示 

			MemoryStream imageStream = new MemoryStream();
			Chart2.SaveImage(imageStream, ChartImageFormat.Png);
			imageStream.Position = 0;
			return imageStream;
		}

		public static MemoryStream GetTaskChart()
		{
			var appData = App.GetReleaseScrumData();
			var currentSprintName = appData.CurrentSprintProxy.CurrentSprint.Name;

			string Total = "Total Tasks (hour)";
			string Left = "Remaining Tasks (hour)";
			var allData = App.GetReleaseScrumData().CurrentSprintProxy.PersonTasks.OrderBy(tsk => tsk.Name);
			Chart Chart2 = new Chart();
			Chart2.Width = 1500;
			Chart2.Height = 300;
			Chart2.RenderType = RenderType.ImageTag;
			Chart2.Palette = ChartColorPalette.BrightPastel;
			Title t = new Title($"{appData.ReleaseData.ProductName} {currentSprintName} Task Completion Status", Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
			Chart2.Titles.Add(t);
			Chart2.ChartAreas.Add(Total);

			// create a couple of series   
			Chart2.Series.Add(Total);
			Chart2.Series.Add(Left);
			// add points to series 1   

			foreach (var item in allData)
			{
				DataPoint point = new DataPoint();
				point.AxisLabel = item.Name;
				point.SetValueY(item.Total);
				Chart2.Series[Total].Points.Add(point);
				Chart2.Series[Left].Points.AddY(item.Left);
			}

			Chart2.Series[Total].IsValueShownAsLabel = true;
			Chart2.Series[Left].IsValueShownAsLabel = true;

			Chart2.BorderSkin.SkinStyle = BorderSkinStyle.None;
			Chart2.BorderlineWidth = 2;
			Chart2.BorderColor = Color.Transparent;
			Chart2.BorderlineDashStyle = ChartDashStyle.NotSet;
			Chart2.BorderWidth = 2;
			Chart2.Legends.Add("Legend1");

			Chart2.ChartAreas[0].AxisX.Interval = 1;   //设置X轴坐标的间隔为1
			Chart2.ChartAreas[0].AxisX.IntervalOffset = 1;  //设置X轴坐标偏移为1
															//Chart2.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;   //设置是否交错显示,比如数据多的时间分成两行来显示 

			MemoryStream imageStream = new MemoryStream();
			Chart2.SaveImage(imageStream, ChartImageFormat.Png);
			imageStream.Position = 0;

			return imageStream;
		}

		private static List<StoryCountProgessItem> GetAllStoryProgressData()
		{
			var currentSprint = App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint;
			var result = new List<StoryCountProgessItem>();
			foreach (var story in currentSprint.Stories)
			{
				var engName = NameUtil.ConvertToEngName(story.Owner);
				var currentPerson = result.FirstOrDefault(t => t.Name == engName);
				if (currentPerson == null)
				{
					currentPerson = new StoryCountProgessItem() { Name = engName };
					result.Add(currentPerson);
				}

				currentPerson.Total++;
				if (story.IsComplete())
				{
					currentPerson.Left++;
				}
			}

			return result.OrderBy(t => t.Name).ToList();
		}

		#endregion

		#region Read Scrum Data

		private static bool ReadFromDbFirst = true;
		private static Release ReadSprintDataFromRepository()
		{
			if (ReadFromDbFirst && scrumDataService != null)
			{
				var productDataID = ConfigurationManager.AppSettings["ProductName"];
				var release = scrumDataService.FindBy(productDataID);
				if (release != null)
				{
					return release;
				}
				else
				{
					ReadFromDbFirst = false;
				}
			}
			return DeserializeObject(AppConfig.R5DataFile);
		}

		#endregion

		#region Read Activity

		private static PersonActivity ReadPersonActivityFromRepo()
		{
			if (activityDataService != null)
			{
				var currentRelease = GetReleaseScrumData();
				var currentSprintName = currentRelease.CurrentSprintProxy.CurrentSprint.Name;
				var personActivity = activityDataService.FindBy(currentSprintName);
				if (personActivity != null)
				{
					return personActivity;
				}
			}

			return ReadPersionActivityFromFile();
		}

		private static PersonActivity ReadPersionActivityFromFile()
		{
			string path = $@"{DataFolder}{AppConfig.CurrentSprintActivityFile}.json";
			if (File.Exists(path))
			{
				return GetEntityModel<PersonActivity>(AppConfig.CurrentSprintActivityFile);
			}
			else
			{
				return PersonActivity.Empty;
			}
		}

		#endregion

		public static void SaveAvailability(Availability availability)
		{
			Availability = availability;
			SaveEntityModel<Availability>(AppConfig.AvailabilityFile, availability);
		}

		#region Read History Data

		public static Release GetRelease4Data()
		{
			if (release4 == null)
			{
				release4 = DeserializeObject(AppConfig.R4DataFile);
			}

			return release4;
		}

		public static Release GetRelease3Data()
		{
			if (release3 == null)
			{
				release3 = DeserializeObject(AppConfig.R3DataFile);
			}

			return release3;
		}

		public static Release GetRelease2Data()
		{
			if (release2 == null)
			{
				release2 = DeserializeObject(AppConfig.R2DataFile);
			}

			return release2;
		}

		public static Release GetRelease1Data()
		{
			if (release1 == null)
			{
				release1 = DeserializeObject(AppConfig.R1DataFile);
			}

			return release1;
		}

		#endregion

		#region Util

		private static Release DeserializeObject(string dataFile)
		{
			string path = $@"{DataFolder}{dataFile}.json";
			var content = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Release>(content);
		}

		private static T GetEntityModel<T>(string dataFile)
		{
			string path = $@"{DataFolder}{dataFile}.json";
			var content = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<T>(content);
		}

		private static void SaveEntityModel<T>(string dataFile, T model)
		{
			string path = $@"{DataFolder}{dataFile}.json";
			var data = JsonConvert.SerializeObject(model);
			File.WriteAllText(path, data, Encoding.UTF8);
		}

		#endregion
	}
}