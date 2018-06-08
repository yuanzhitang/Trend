using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.AnalysisService;
using Trend.Common;
using Trend.DataCollector.Config;

namespace Trend.DataCollector
{
	public class WebSpiderFactory
	{
		public static IWebSpider Create()
		{
			switch (DataSourceSystemConfig.DataSource)
			{
				case DataSourceSystem.uTrack:
					return new uTrackSpider(ConfigManager.ProductName, AppConfig.R5Sprint0URL);
				case DataSourceSystem.JIRA:
					return new JIRASpider(ConfigManager.ProductName, AppConfig.R6Sprint0URL);
				default:
					throw new NotSupportedException(DataSourceSystemConfig.DataSource.ToString());
			}
		}
	}
}
