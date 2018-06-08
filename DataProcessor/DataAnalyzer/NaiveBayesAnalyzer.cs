using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Accord.MachineLearning.Bayes;
using Accord.Math;
using Accord.Statistics.Filters;
using Trend.Common;
using Trend.DataModel;

namespace Trend.AnalysisService
{
	public class NaiveBayesAnalyzer : IDataAnalyzer
	{
		#region private fields
		private string[] specialValueColumnNames = { "StorySize", "Owner", "Status" };
		private string[][] data;
		private readonly string storyCompleteStatus = "Completed";
		private readonly string storyNotAcceptedStatus = "NoAccepted";
		private List<string> excludeOwners = new List<string>() { "Not Assigned", "Jasmine Lin", "Michael Harvey" };
		private readonly string noResultReasonExcludeUser = "null (excluded user)";
		private readonly string noResultReasonStorySize0 = "null (story size is 0)";
		#endregion

		public List<string> ExcludedOwners
		{
			get { return excludeOwners; }
		}

		public NaiveBayesAnalyzer()
		{
			InitializeExcludeOwners();
		}

		public void Analyze()
		{
			GetHistoryData();
			DoBayesAnalyze();
		}

		private void DoBayesAnalyze()
		{
			Codification codebook = new Codification(specialValueColumnNames, data);
			int[][] symbols = codebook.Transform(data);
			int[][] inputs = symbols.Get(null, 0, -1);
			int[] outputs = symbols.GetColumn(-1);

			// Create a new Naive Bayes learning
			var learner = new NaiveBayesLearning();
			NaiveBayes nb = learner.Learn(inputs, outputs);
			var currentSprint = App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint;
			foreach (var story in currentSprint.Stories)
			{
				if (excludeOwners.Contains(story.Owner))
				{
					story.PredicateSuccessRate = noResultReasonExcludeUser;
					continue;
				}
				if (story.Size <= 0)
				{
					story.PredicateSuccessRate = noResultReasonStorySize0;
					continue;
				}

				int[] storyInstance = codebook.Transform(new string[] { story.Size.ToString(), story.Owner });
				double[] probs = nb.Probabilities(storyInstance);
				var sucessRate = probs[0] * 100;
				var pSuccessRate = string.Format("{0:N2}", sucessRate);
				story.PredicateSuccessRate = string.Format("{0}%", pSuccessRate);
			}
		}

		private void GetHistoryData()
		{
			List<string[]> rows = new List<string[]>();
			var storyStatus = string.Empty;

			var release1Data = App.GetRelease1Data();
			foreach (var story in release1Data.GetAllStories())
			{
				storyStatus = GetStoryFinalStatus(story);
				string[] data = { story.Size.ToString(), story.Owner.ToString(), storyStatus };
				rows.Add(data);
			}

			var release2Data = App.GetRelease2Data();
			foreach (var story in release2Data.GetAllStories())
			{
				storyStatus = GetStoryFinalStatus(story);
				string[] data = { story.Size.ToString(), story.Owner.ToString(), storyStatus };
				rows.Add(data);
			}

			var release3Data = App.GetRelease3Data();
			foreach (var story in release3Data.GetAllStories())
			{
				storyStatus = GetStoryFinalStatus(story);
				string[] data = { story.Size.ToString(), story.Owner.ToString(), storyStatus };
				rows.Add(data);
			}

			var release4Data = App.GetRelease4Data();
			foreach (var story in release4Data.GetAllStories())
			{
				storyStatus = GetStoryFinalStatus(story);
				string[] data = { story.Size.ToString(), story.Owner.ToString(), storyStatus };
				rows.Add(data);
			}

			//current release stories except stories of current sprint
			var currentReleaseStories = GetCurrentReleaseStories().ToList();
			var currentSprintStoriesInCurrentRelease = App.GetReleaseScrumData().CurrentSprintProxy.CurrentSprint.Stories;
			var allAvailableStories = currentReleaseStories.Except(currentSprintStoriesInCurrentRelease);
			foreach (var story in allAvailableStories)
			{
				storyStatus = GetStoryFinalStatus(story);
				string[] data = { story.Size.ToString(), story.Owner.ToString(), storyStatus };
				rows.Add(data);
			}

			this.data = rows.ToArray();
		}

		private string GetStoryFinalStatus(Story story)
		{
			if (story.Status == DataModel.StoryStatus.Accepted || story.Status == DataModel.StoryStatus.Done)
			{
				return storyCompleteStatus;
			}
			else
			{
				return storyNotAcceptedStatus;
			}
		}

		private IEnumerable<Story> GetCurrentReleaseStories()
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
}
