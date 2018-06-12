using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unisys.Trend.AnalysisService;
using Unisys.Trend.Common;
using Unisys.Trend.DataCollector.Config;

namespace Unisys.Trend.DataCollector
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
