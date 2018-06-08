using System.Configuration;

namespace Trend.Common
{
	public class RepositoryConfig
	{
		public static string DbServer = ConfigurationManager.AppSettings["MongoDBServer"];
		public static string TrendActivityDB = ConfigurationManager.AppSettings["MongoDBTrendActivity"];
		public static string TrendScrumDataDB = ConfigurationManager.AppSettings["MongoDBTrendScrumData"];
		public static string ProductName = ConfigurationManager.AppSettings["ProductName"];
	}
}
