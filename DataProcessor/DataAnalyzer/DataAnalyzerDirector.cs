using System.Collections.Generic;
using Trend.Common;

namespace Trend.AnalysisService
{
	public class DataAnalyzerDirector
	{
		private string dataFile = string.Empty;
		private string lastDataFile = string.Empty;

		private IList<IDataAnalyzer> Analyzers;

		public DataAnalyzerDirector(string dataFile)
		{
			this.dataFile = dataFile;
			this.lastDataFile = dataFile + "_Last";

			InitAnalyzers();
		}

		private void InitAnalyzers()
		{
			Analyzers = new List<IDataAnalyzer>();
			Analyzers.Add(new uTrackActivityAnalyzer(dataFile));
		}

		public void Analyze()
		{
			foreach (var analyzer in Analyzers)
			{
				analyzer.Analyze();
			}
		}
	}
}
