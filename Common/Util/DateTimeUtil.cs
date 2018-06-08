using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Common
{
	public class DateTimeUtil
	{
		public static int GetWorkingDays(DateTime startTime, DateTime endTime)
		{
			if (startTime >= endTime)
			{
				return 0;
			}

			TimeSpan ts1 = endTime.Subtract(startTime);//TimeSpan得到dt1和dt2的时间间隔
			int countday = ts1.Days;//获取两个日期间的总天数
			int weekday = 0;//工作日
							//循环用来扣除总天数中的双休日
			for (int i = 0; i <= countday; i++)
			{
				DateTime tempdt = startTime.Date.AddDays(i);
				if (tempdt.DayOfWeek != DayOfWeek.Saturday && tempdt.DayOfWeek != DayOfWeek.Sunday)
				{
					weekday++;
				}
			}

			return weekday;
		}

		private static Dictionary<int, string> CnNameToEngName = new Dictionary<int, string>()
		{
			{1,"Jan" },
			{2,"Feb" },
			{3,"Mar" },
			{4,"Apr" },
			{5,"May" },
			{6,"Jun" },
			{7,"Jul" },
			{8,"Aug" },
			{9,"Sep" },
			{10,"Oct" },
			{11,"Nov" },
			{12,"Dec" }
		};
		public static String ConvertToEngMonth(int month)
		{
			return CnNameToEngName[month];
		}

		public static int GetSubstractDays(DateTime modified)
		{
			var now = DateTime.Now;
			if (modified > now)
			{
				return 0;
			}

			return now.Subtract(modified).Days;
		}

		public static int GetSubstractWorkingDays(DateTime modifiedTime)
		{
			return GetWorkingDays(modifiedTime, DateTime.Now);
		}
	}
}
