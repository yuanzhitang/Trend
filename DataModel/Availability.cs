using System;

namespace Trend.DataModel
{
	public class Availability
	{
		public DateTime? CTCHolidayStart { get; set; }
		public DateTime? CTCHolidayEnd { get; set; }
		public DateTime? ACUSHolidayStart { get; set; }
		public DateTime? ACUSHolidayEnd { get; set; }
		public DateTime? CompanyHolidayStart { get; set; }
		public DateTime? CompanyHolidayEnd { get; set; }

		public static Availability Default = new Availability()
		{
			//CTCHolidaystart = 0,
			//ACUSHoliday = 0,
			//CompanyHoliday = 0
		};
	}
}