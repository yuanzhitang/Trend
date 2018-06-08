using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Common;
using Trend.DataModel;

namespace Trend.AnalysisService.DataAnalyzer
{
	public class AwardAnalyzer
	{
		public static AwardSummary GetAwardSummary()
		{
			var award = new AwardSummary();

			var list = new Dictionary<string, int>();
			foreach (var groupItem in App.CurrentSprintActivity.Items.Where(t => !AppConfig.IgnoredMembers.Contains(t.Name)).GroupBy(t => t.Name))
			{
				list.Add(groupItem.Key, groupItem.Count());
			}

			list = list.OrderByDescending(t => t.Value).ToDictionary(t => t.Key, v => v.Value);

			if (list.Count > 3)
			{
				award.PersonOfIndustrious = list.Take(2).Select(t => t.Key);
				award.PersonOfSilent = list.Skip(list.Count - 2).Select(t => t.Key);
			}
			else if (list.Count > 1)
			{
				award.PersonOfIndustrious = new List<string>() { list.First().Key };
				award.PersonOfSilent = new List<string>() { list.Last().Key };
			}
			else
			{
				award.PersonOfIndustrious = new List<string>();
				award.PersonOfSilent = new List<string>();
			}

			award.PersonOfWarrior = GetPersonOfWarrior();

			return award;
		}

		public static IEnumerable<string> GetPersonOfWarrior()
		{
			var appData = App.GetReleaseScrumData();

			var currentSprint = appData.ReleaseData.CurrentSprint;
			var personToWorkDone = new Dictionary<string, decimal>();
			foreach (var story in currentSprint.Stories)
			{
				foreach (var task in story.Tasks.Where(tsk => tsk.WorkDone != 0))
				{
					var engName = NameUtil.ConvertToEngName(task.Owner);
					var currentPerson = personToWorkDone.FirstOrDefault(t => t.Key == engName);
					if (!personToWorkDone.ContainsKey(engName))
					{
						personToWorkDone.Add(engName, task.WorkDone);
					}
					else
					{
						personToWorkDone[engName] += task.WorkDone;
					}
				}
			}

			personToWorkDone = personToWorkDone.OrderByDescending(t => t.Value).ToDictionary(t => t.Key, v => v.Value);

			if (personToWorkDone.Count > 3)
			{
				return personToWorkDone.Take(2).Select(t => t.Key);
			}
			else if (personToWorkDone.Count > 1)
			{
				return new List<string> { personToWorkDone.First().Key };
			}
			else
			{
				return new List<string>();
			}
		}
	}
}
