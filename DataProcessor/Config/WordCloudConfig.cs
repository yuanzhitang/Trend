using System.Collections.Generic;

namespace Trend.AnalysisService
{
	public class WordCloudConfig
	{
		public static HashSet<string> BlackWords = new HashSet<string>()
		{
			"the","to","for","and","-","in","a","of","be","with","on","is","as",
			"when","are","any","an","all","after","by","not","if","can","will","about","into","no",
		};
	}
}
