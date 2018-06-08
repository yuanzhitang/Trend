using System;
using System.Collections.Generic;
using System.Linq;

namespace Trend.Common
{
	public class NameUtil
	{
		public static List<string> InValidNames = new List<string>()
		{
			"Yida Chen",
			"Pengbo XU"
		};

		private static List<NameMapping> nameMap = null;
		public static List<NameMapping> NameMap
		{
			get
			{
				if (nameMap == null)
				{
					var type = typeof(List<NameMapping>);
					nameMap = XmlSerializer.LoadFromXml($"{AppDomain.CurrentDomain.BaseDirectory}\\NameMap.xml", type) as List<NameMapping>;
				}

				return nameMap;
			}
		}

        public static string NoOwner = "No Owner";

		public static string ConvertToEngName(string cnName)
		{
			if(cnName==null)
			{
				return string.Empty;
			}
			
			
			var nameMapping = NameMap.FirstOrDefault(t => t.FullName == cnName);
			var engName = nameMapping != null ? nameMapping.Name : cnName;
	
			return engName;
		}
	}

	[Serializable]
	public class NameMapping
	{
		public string FullName;
		public string Name;
	}
}
