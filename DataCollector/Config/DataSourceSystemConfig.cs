using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Common;

namespace Trend.DataCollector.Config
{
	public class DataSourceSystemConfig
	{
		public static DataSourceSystem DataSource = (DataSourceSystem)Enum.Parse(typeof(DataSourceSystem), ConfigurationManager.AppSettings["DataSourceSystem"]);
	}
}
