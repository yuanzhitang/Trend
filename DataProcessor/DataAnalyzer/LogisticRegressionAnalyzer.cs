using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Trend.Common;
using Trend.DataModel;

namespace Trend.AnalysisService
{
	/// <summary>
	/// In statistics, logistic regression (sometimes called the logistic model or Logit model) is used for prediction of the probability of occurrence of an event
	/// by fitting data to a logistic curve.It is a generalized linear model used for binomial regression.
	/// Like many forms of regression analysis, it makes use of several predictor variables that may be either numerical or categorical.
	/// For example, the probability that a person has a heart attack within a specified time period might be predicted from knowledge of the person's age, sex and body mass index.
	/// Logistic regression is used extensively in the medical and social sciences as well as marketing applications such as prediction of a customer's propensity to purchase a product 
	/// or cease a subscription
	/// 
	/// As for Trend here, we'll predicate the probability based on story size and owners' (age, work experience year).
	/// </summary>
	public class LogisticRegressionAnalyzer : IDataAnalyzer
	{
		//format of each row: {story size, owner age, work experience}
		private double[][] learningData;
		private bool[] learningExpectedResult;

		private Dictionary<string, StoryOwnerInfo> MockedOwnerInfo;
		private List<string> excludeOwners = new List<string>() { "Not Assigned", "Jasmine Lin", "Michael Harvey" };
		private readonly string noResultReasonExcludeUser = "null (excluded user)";
		private readonly string noResultReasonStorySize0 = "null (story size is 0)";

		public LogisticRegressionAnalyzer()
		{
			InitializeStoryOwnerInfo();
			InitializeExcludeOwners();
		}

		public void Analyze()
		{
			LoadLearningDataAndResult();
			DoLogisticRegressionAnalyze();
		}

		private void LoadLearningDataAndResult()
		{
			List<double[]> rowData = new List<double[]>();
			List<bool> rowResult = new List<bool>();

			var historyData = AnalyzerUtil.GetHistoryStories();
			foreach (var story in historyData)
			{
				try
				{
					var storySize = story.Size;
					var storyOwner = story.Owner;
					var ownerAge = 0;
					var ownerWorkExperience = 0;
					if (MockedOwnerInfo.ContainsKey(storyOwner))
					{
						ownerAge = MockedOwnerInfo[storyOwner].Age;
						ownerWorkExperience = MockedOwnerInfo[storyOwner].WorkYear;
					}
					
					rowData.Add(new double[] { (double)storySize, ownerAge, ownerWorkExperience });

					var storySucceededFinally = (story.Status == StoryStatus.Accepted || story.Status == StoryStatus.Done);
					rowResult.Add(storySucceededFinally);
				}
				catch
				{
					//do nothihng, continue iteration
				}
			}

			learningData = rowData.ToArray();
			learningExpectedResult = rowResult.ToArray();
		}

		/// <summary>
		/// standard IterativeReweightedLeastSquares algorithm
		/// </summary>
		private void DoLogisticRegressionAnalyze()
		{
			var learner = new IterativeReweightedLeastSquares<LogisticRegression>()
			{
				Tolerance = 1e-4,  // Let's set some convergence parameters
				Iterations = 100,
				MaxIterations = 100, // maximum number of iterations to perform
				Regularization = 0
			};

			// Now, we can use the learner to finally estimate our model:
			LogisticRegression regression = learner.Learn(learningData, learningExpectedResult);

			foreach (var story in App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint.Stories)
			{
				if (excludeOwners.Contains(story.Owner))
				{
					story.LRPreSuccessRate = noResultReasonExcludeUser;
					continue;
				}
				if (story.Size <= 0)
				{
					story.LRPreSuccessRate = noResultReasonStorySize0;
					continue;
				}
				var inputData = GetInputDataForTargetStory(story);
				var scores = regression.Probability(inputData);
				story.LRPreSuccessRate = string.Format("{0}%", string.Format("{0:N2}", scores[0] * 100));
			}
		}

		private double[][] GetInputDataForTargetStory(Story story)
		{
			List<double[]> rowData = new List<double[]>();
			var storySize = story.Size;
			var storyOwner = story.Owner;
			var ownerAge = 0;
			var ownerWorkExperience = 0;
			if (MockedOwnerInfo.ContainsKey(storyOwner))
			{
				ownerAge = MockedOwnerInfo[storyOwner].Age;
				ownerWorkExperience = MockedOwnerInfo[storyOwner].WorkYear;
			}
			rowData.Add(new double[] { (double)storySize, ownerAge, ownerWorkExperience });

			return rowData.ToArray();
		}

		private void InitializeStoryOwnerInfo()
		{
			MockedOwnerInfo = new Dictionary<string, StoryOwnerInfo>();
			MockedOwnerInfo.Add("Yifan Feng", new StoryOwnerInfo() { Age = 23, Name = "Yifan Feng", WorkYear = 2 });
			MockedOwnerInfo.Add("Ming Liu", new StoryOwnerInfo() { Age = 33, Name = "Ming Liu", WorkYear = 10 });
			MockedOwnerInfo.Add("Meghan Kerr", new StoryOwnerInfo() { Age = 43, Name = "Meghan Kerr", WorkYear = 12 });
			MockedOwnerInfo.Add("Xu Huang", new StoryOwnerInfo() { Age = 35, Name = "Xu Huang", WorkYear = 10 });
			MockedOwnerInfo.Add("Tianning Bu", new StoryOwnerInfo() { Age = 24, Name = "Tianning Bu", WorkYear = 3 });
			MockedOwnerInfo.Add("Risto Keski-Frantti", new StoryOwnerInfo() { Age = 56, Name = "Risto Keski-Frantti", WorkYear = 22 });
			MockedOwnerInfo.Add("Jasmine Lin", new StoryOwnerInfo() { Age = 40, Name = "Jasmine Lin", WorkYear = 15 });
			MockedOwnerInfo.Add("Michael Harvey", new StoryOwnerInfo() { Age = 55, Name = "Michael Harvey", WorkYear = 21 });
			MockedOwnerInfo.Add("Jian Zhang", new StoryOwnerInfo() { Age = 32, Name = "Jian Zhang", WorkYear = 8 });
			MockedOwnerInfo.Add("Chenyu Wang", new StoryOwnerInfo() { Age = 24, Name = "Chenyu Wang", WorkYear = 3 });
			MockedOwnerInfo.Add("Shangjie Xin", new StoryOwnerInfo() { Age = 35, Name = "Shangjie Xin", WorkYear = 6 });
			MockedOwnerInfo.Add("Sha Cheng", new StoryOwnerInfo() { Age = 25, Name = "Sha Cheng", WorkYear = 4 });
			MockedOwnerInfo.Add("Congying Yuan", new StoryOwnerInfo() { Age = 27, Name = "Congying Yuan", WorkYear = 4 });
			MockedOwnerInfo.Add("Yapeng Sun", new StoryOwnerInfo() { Age = 25, Name = "Yapeng Sun", WorkYear = 3 });
			MockedOwnerInfo.Add("Chengjie Xin", new StoryOwnerInfo() { Age = 25, Name = "Chengjie Xin", WorkYear = 3 });
			MockedOwnerInfo.Add("Rinu Sunil", new StoryOwnerInfo() { Age = 27, Name = "Rinu Sunil", WorkYear = 4 });
			MockedOwnerInfo.Add("Chaosong Wang", new StoryOwnerInfo() { Age = 31, Name = "Chaosong Wang", WorkYear = 6 });
			MockedOwnerInfo.Add("Zhonghui Zou", new StoryOwnerInfo() { Age = 34, Name = "Zhonghui Zou", WorkYear = 10 });
			MockedOwnerInfo.Add("Yuanzhi Tang", new StoryOwnerInfo() { Age = 33, Name = "Yuanzhi Tang", WorkYear = 9 });
			MockedOwnerInfo.Add("Liurong Luo", new StoryOwnerInfo() { Age = 34, Name = "Liurong Luo", WorkYear = 10 });
			MockedOwnerInfo.Add("John Ticehurst", new StoryOwnerInfo() { Age = 44, Name = "John Ticehurst", WorkYear = 18 });
			MockedOwnerInfo.Add("He Sun", new StoryOwnerInfo() { Age = 33, Name = "He Sun", WorkYear = 8 });
			MockedOwnerInfo.Add("Not Assigned", new StoryOwnerInfo() { Age = 0, Name = "Not Assigned", WorkYear = 0 });
		}

		private void InitializeExcludeOwners()
		{
			var configedValue = ConfigurationManager.AppSettings["ExcludeOwners"];
			if (!string.IsNullOrWhiteSpace(configedValue))
			{
				var configuredList = configedValue.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();

				if (configuredList.Any())
				{
					excludeOwners.Clear();
					excludeOwners.AddRange(configuredList.Select(c => c.Trim()));
				}
			}
		}
	}


	public struct StoryOwnerInfo
	{
		public string Name { get; set; }
		public int Age { get; set; }
		public int WorkYear { get; set; }
	}

	public class AnalyzerUtil
	{
		public static IEnumerable<Story> GetHistoryStories()
		{
			var release1Data = App.GetRelease1Data();
			foreach (var story in release1Data.GetAllStories())
			{
				yield return story;
			}

			var release2Data = App.GetRelease2Data();
			foreach (var story in release2Data.GetAllStories())
			{
				yield return story;
			}

			var release3Data = App.GetRelease3Data();
			foreach (var story in release3Data.GetAllStories())
			{
				yield return story;
			}

			var release4Data = App.GetRelease4Data();
			foreach (var story in release4Data.GetAllStories())
			{
				yield return story;
			}

			//current release stories except stories of current sprint
			var currentReleaseStories = GetCurrentReleaseStories().ToList();
			var currentSprintStoriesInCurrentRelease = App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint.Stories;
			var allAvailableStories = currentReleaseStories.Except(currentSprintStoriesInCurrentRelease);
			foreach (var story in allAvailableStories)
			{
				yield return story;
			}
		}

		public static IEnumerable<Story> GetCurrentReleaseStories()
		{
			var sprints = App.GetReleaseScrumData().ReleaseData.Sprints;
			foreach (var sprint in sprints)
			{
				foreach (var story in sprint.Stories)
				{
					yield return story;
				}
			}
		}
	}
}
