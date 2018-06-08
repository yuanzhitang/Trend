using Trend.DataModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Trend.Common;
using Trend.AnalysisService;

namespace AdministratorSite.Controllers
{
	public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }

		public FileResult GetStoryChart()
		{
			return new FileStreamResult(App.GetStoryChart(), "image/png");
		}

		public FileResult GetTaskChart()
		{
			return new FileStreamResult(App.GetTaskChart(), "image/png");
		}

		public FileResult GetAllStorySucessRateChart()
		{
			var samplingStories = new List<Story>();
			samplingStories.AddRange(App.GetRelease1Data().GetAllStories());
			samplingStories.AddRange(App.GetRelease2Data().GetAllStories());
			samplingStories.AddRange(App.GetRelease3Data().GetAllStories());
			samplingStories.AddRange(App.GetRelease4Data().GetAllStories());
			samplingStories.AddRange(App.GetReleaseScrumData().ReleaseData.GetAllStories());

			return BuildStorySuccessRateDiagram(samplingStories);
		}

		public FileResult GetDE5SprintStorySucessRateChart()
		{
			return BuildSprintStorySuccessRateDiagram(App.GetReleaseScrumData().ReleaseData.Sprints);
		}

		private FileResult BuildSprintStorySuccessRateDiagram(List<Sprint> sprints)
		{
			string Total = "Completion Rate(%)";
			List<SprintStoryCompletionRate> allData = GetAllSprintStorySuccessRate(sprints);
			Chart Chart2 = new Chart();
			Chart2.Width = 1500;
			Chart2.Height = 300;
			Chart2.RenderType = RenderType.ImageTag;
			Chart2.Palette = ChartColorPalette.BrightPastel;
			//Title t = new Title($"User Story Success Rate", Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
			//Chart2.Titles.Add(t);
			Chart2.ChartAreas.Add(Total);

			// create a couple of series   
			Chart2.Series.Add(Total);
			//Chart2.Series.Add(Left);
			// add points to series 1   

			foreach (var item in allData)
			{
				DataPoint point = new DataPoint();
				point.AxisLabel = item.SprintName;
				point.SetValueY(item.CompletionRate);
				Chart2.Series[Total].Points.Add(point);
				//Chart2.Series[Left].Points.AddY(item.Left);
			}

			Chart2.Series[Total].IsValueShownAsLabel = true;
			Chart2.Series[Total].Color = Color.Green;
			Chart2.Series[Total].LabelBorderWidth = 14;
			//Chart2.Series[Left].IsValueShownAsLabel = true;
			//Chart2.Series[Left].Color = Color.Green;
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
			return new FileStreamResult(imageStream, "image/png");
		}

		private List<SprintStoryCompletionRate> GetAllSprintStorySuccessRate(List<Sprint> sprints)
		{
			var result = new List<SprintStoryCompletionRate>();
			sprints.Reverse();
			foreach(var sprint in sprints)
			{
				decimal success = 0;
				decimal failed = 0;

				foreach(var story in sprint.Stories)
				{
					if (story.IsComplete())
					{
						success++;
					}
					else if (story.Status == StoryStatus.Incomplete)
					{
						failed++;
					}
				}

				var sprintSuccessItem = new SprintStoryCompletionRate();
				sprintSuccessItem.SprintName= sprint.Name;

				bool isSprintNotFinish = DateTime.Now > sprint.StartTime && DateTime.Now < sprint.EndTime;
				if ((success + failed) == 0 || isSprintNotFinish)
				{
					sprintSuccessItem.CompletionRate = 0;
				}
				else
				{
					sprintSuccessItem.CompletionRate = (int)((success / (success + failed)) * 100);
				}

				result.Add(sprintSuccessItem);
			}

			return result;
		}

		public FileResult GetDE4SprintStorySucessRateChart()
		{
			return BuildSprintStorySuccessRateDiagram(App.GetRelease4Data().Sprints);
		}
		public FileResult GetDE3SprintStorySucessRateChart()
		{
			return BuildSprintStorySuccessRateDiagram(App.GetRelease3Data().Sprints);
		}
		public FileResult GetDE2SprintStorySucessRateChart()
		{
			return BuildSprintStorySuccessRateDiagram(App.GetRelease2Data().Sprints);
		}
		public FileResult GetDE1SprintStorySucessRateChart()
		{
			return BuildSprintStorySuccessRateDiagram(App.GetRelease1Data().Sprints);
		}

		public FileResult GetDE5StorySucessRateChart()
		{
			var samplingStories = App.GetReleaseScrumData().ReleaseData.GetAllStories();
			return BuildStorySuccessRateDiagram(samplingStories);
		}
		public FileResult GetDE4StorySucessRateChart()
		{
			var samplingStories = App.GetRelease4Data().GetAllStories();
			return BuildStorySuccessRateDiagram(samplingStories);
		}
		public FileResult GetDE3StorySucessRateChart()
		{
			var samplingStories = App.GetRelease3Data().GetAllStories();
			return BuildStorySuccessRateDiagram(samplingStories);
		}
		public FileResult GetDE2StorySucessRateChart()
		{
			var samplingStories = App.GetRelease2Data().GetAllStories();
			return BuildStorySuccessRateDiagram(samplingStories);
		}
		public FileResult GetDE1StorySucessRateChart()
		{
			var samplingStories = App.GetRelease1Data().GetAllStories();
			return BuildStorySuccessRateDiagram(samplingStories);
		}

		public FileResult GetAllStorySizeCountChart()
		{
			var samplingStories = new List<Story>();
			samplingStories.AddRange(App.GetRelease1Data().GetAllStories());
			samplingStories.AddRange(App.GetRelease2Data().GetAllStories());
			samplingStories.AddRange(App.GetRelease3Data().GetAllStories());
			samplingStories.AddRange(App.GetRelease4Data().GetAllStories());
			samplingStories.AddRange(App.GetReleaseScrumData().ReleaseData.GetAllStories());

			return BuildStorySizeCountDiagram(samplingStories);
		}

		public FileResult GetDE5StorySizeCountChart()
		{
			var samplingStories = App.GetReleaseScrumData().ReleaseData.GetAllStories();
			return BuildStorySizeCountDiagram(samplingStories);
		}
		public FileResult GetDE4StorySizeCountChart()
		{
			var samplingStories = App.GetRelease4Data().GetAllStories();
			return BuildStorySizeCountDiagram(samplingStories);
		}
		public FileResult GetDE3StorySizeCountChart()
		{
			var samplingStories = App.GetRelease3Data().GetAllStories();
			return BuildStorySizeCountDiagram(samplingStories);
		}
		public FileResult GetDE2StorySizeCountChart()
		{
			var samplingStories = App.GetRelease2Data().GetAllStories();
			return BuildStorySizeCountDiagram(samplingStories);
		}
		public FileResult GetDE1StorySizeCountChart()
		{
			var samplingStories = App.GetRelease1Data().GetAllStories();
			return BuildStorySizeCountDiagram(samplingStories);
		}

		private FileResult BuildStorySizeCountDiagram(List<Story> samplingStories)
		{
			string Total = "Count by size";
			List<StorySizeCount> allData = GetAllStorySizeCount(samplingStories);
			Chart Chart2 = new Chart();
			Chart2.Width = 800;
			Chart2.Height = 300;
			Chart2.RenderType = RenderType.ImageTag;
			Chart2.Palette = ChartColorPalette.BrightPastel;
			//Title t = new Title($"User Story Success Rate", Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
			//Chart2.Titles.Add(t);
			Chart2.ChartAreas.Add(Total);

			// create a couple of series   
			Chart2.Series.Add(Total);
			//Chart2.Series.Add(Left);
			// add points to series 1   

			foreach (var item in allData)
			{
				DataPoint point = new DataPoint();
				point.AxisLabel = $"Size:{item.Size.ToString()}";
				point.SetValueY(item.Count);
				Chart2.Series[Total].Points.Add(point);
				//Chart2.Series[Left].Points.AddY(item.Left);
			}

			Chart2.Series[Total].IsValueShownAsLabel = true;
			Chart2.Series[Total].Color = Color.DarkOrange;
			Chart2.Series[Total].LabelBorderWidth = 14;
			//Chart2.Series[Left].IsValueShownAsLabel = true;
			//Chart2.Series[Left].Color = Color.Green;
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
			return new FileStreamResult(imageStream, "image/png");
		}

		private List<StorySizeCount> GetAllStorySizeCount(List<Story> samplingStories)
		{
			var result = new List<StorySizeCount>();

			Dictionary<decimal, decimal> success = new Dictionary<decimal, decimal>();
			foreach (var story in samplingStories.Where(t => t.Size >= 1 && t.Size < 20))
			{
				if (!success.ContainsKey(story.Size))
				{
					success[story.Size] = 1;
				}
				else
				{
					success[story.Size]++;
				}
			}

			foreach (var item in success.OrderBy(t => t.Key))
			{
				decimal successCount = success[item.Key];

				if (successCount != 0)
				{
					var storySuccessItem = new StorySizeCount();
					storySuccessItem.Size = item.Key;
					storySuccessItem.Count = success[item.Key];
					result.Add(storySuccessItem);
				}
			}

			return result;
		}

		private FileResult BuildStorySuccessRateDiagram(List<Story> samplingStories)
		{
			string Total = "SuccessRate(%)";
			List<StorySuccessRate> allData = GetAllStorySuccessRate(samplingStories);
			Chart Chart2 = new Chart();
			Chart2.Width = 800;
			Chart2.Height = 300;
			Chart2.RenderType = RenderType.ImageTag;
			Chart2.Palette = ChartColorPalette.BrightPastel;
			//Title t = new Title($"User Story Success Rate", Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
			//Chart2.Titles.Add(t);
			Chart2.ChartAreas.Add(Total);

			// create a couple of series   
			Chart2.Series.Add(Total);
			//Chart2.Series.Add(Left);
			// add points to series 1   

			foreach (var item in allData)
			{
				DataPoint point = new DataPoint();
				point.AxisLabel = $"Size:{item.Size.ToString()}";
				point.SetValueY(item.SucessRate);
				Chart2.Series[Total].Points.Add(point);
				//Chart2.Series[Left].Points.AddY(item.Left);
			}

			Chart2.Series[Total].IsValueShownAsLabel = true;
			Chart2.Series[Total].Color = Color.Green;
			Chart2.Series[Total].LabelBorderWidth = 14;
			//Chart2.Series[Left].IsValueShownAsLabel = true;
			//Chart2.Series[Left].Color = Color.Green;
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
			return new FileStreamResult(imageStream, "image/png");
		}

		private List<StorySuccessRate> GetAllStorySuccessRate(List<Story> sampleStories)
		{
			var result = new List<StorySuccessRate>();

			Dictionary<decimal, decimal> success = new Dictionary<decimal, decimal>();
			Dictionary<decimal, decimal> fail = new Dictionary<decimal, decimal>();
			foreach (var story in sampleStories.Where(t=> t.Size>= 1 && t.Size<20))
			{
				if (!success.ContainsKey(story.Size))
				{
					success[story.Size] = 0;
					fail[story.Size] = 0;
				}

				if (story.IsComplete())
				{
					success[story.Size]++;
				}
				else if (story.Status == StoryStatus.Incomplete)
				{
					fail[story.Size]++;
				}
			}

			foreach(var item in success.OrderBy(t=>t.Key))
			{
				decimal successCount = success[item.Key];
				decimal failCount = fail[item.Key];

				if (successCount != 0)
				{
					var storySuccessItem = new StorySuccessRate();
					storySuccessItem.Size = item.Key;


					storySuccessItem.SucessRate = (int)((successCount / (successCount + failCount)) * 100);

					result.Add(storySuccessItem);
				}
			}

			return result;
		}
	}
}