using System;
using System.Collections.Generic;
using System.Linq;
using Trend.DataModel;

namespace Trend.AnalysisService
{
	public class WordCloudAnalyzer
	{
		public static WordCloudAnalyzer Instance = new WordCloudAnalyzer();

		public Dictionary<string, int> GetWordCloudData(Release ReleaseData)
		{
			Dictionary<string, int> frequentWords = new Dictionary<string, int>();
			var tempResult = new Dictionary<string, int>();

			foreach (var sprint in ReleaseData.Sprints)
			{
				var contentList = new List<String>();
				sprint.Stories.ForEach(t =>
				{
					if (!string.IsNullOrEmpty(t.Title))
					{
						contentList.Add(t.Title);
					}
					if (!string.IsNullOrEmpty(t.Description))
					{
						contentList.Add(t.Description);
					}
					contentList.AddRange(t.Tasks.Select(task => task.Description));
				});

				foreach (var desc in contentList)
				{
					foreach (var word in desc.Split(' ').Select(t => t.Trim().ToLower()))
					{
						if (string.IsNullOrEmpty(word) || WordCloudConfig.BlackWords.Contains(word))
						{
							continue;
						}

						if (tempResult.ContainsKey(word))
						{
							tempResult[word]++;

						}
						else
						{
							tempResult[word] = 1;
						}
					}
				}
			}

			tempResult = tempResult.OrderByDescending(t => t.Value).ToDictionary(p => p.Key, o => o.Value);
			int firstTen = 10;
			foreach (var item in tempResult.Take(120))
			{
				if (firstTen >= 0)
				{
					frequentWords[item.Key.ToUpper()] = item.Value;
					firstTen--;
				}
				else
				{
					frequentWords[item.Key] = item.Value;
				}
			}

			Random rand = new Random();
			frequentWords = frequentWords.OrderByDescending
				(t => rand.Next(100)).ToDictionary(p => p.Key, o => o.Value
				);

			return frequentWords;
		}
	}
}
