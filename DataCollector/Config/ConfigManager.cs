using System;
using System.Configuration;
using Trend.Common;
using Trend.DataCollector.Config;

namespace Trend.DataCollector
{
	public class ConfigManager
	{
		public static string uTrackIndexUrl = ConfigurationManager.AppSettings["uTrackIndex"];
		public static string uTrackRootUrl = ConfigurationManager.AppSettings["uTrackRootUrl"];

		public static string ProductName = ConfigurationManager.AppSettings["ProductName"];

		public static int FetchInterval = int.Parse(ConfigurationManager.AppSettings["FetchIntervalInMinute"]) * 60 * 1000;


		public static string GetHomePage()
		{
			switch (DataSourceSystemConfig.DataSource)
			{
				case DataSourceSystem.uTrack:
					return uTrackIndexUrl;
				case DataSourceSystem.JIRA:
					return ConfigurationManager.AppSettings["JIRALoginUrl"];
				default:
					throw new NotSupportedException(DataSourceSystemConfig.DataSource.ToString());
			}
		}

		public static string GetRootUrl()
		{
			switch (DataSourceSystemConfig.DataSource)
			{
				case DataSourceSystem.uTrack:
					return uTrackRootUrl;
				case DataSourceSystem.JIRA:
					return ConfigurationManager.AppSettings["JIRARootUrl"];
				default:
					throw new NotSupportedException(DataSourceSystemConfig.DataSource.ToString());
			}
		}
	}
}
