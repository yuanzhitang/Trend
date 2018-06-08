using System.Collections.Generic;

namespace Trend.AnalysisService
{
	public class AppConfig
	{
		public const string R1Sprint0URL = "https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/TaskPlanning?iid=766";
		public const string R2Sprint0URL = "https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/TaskPlanning?iid=1175";
		public const string R3Sprint0URL = "https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/TaskPlanning?iid=1698";
		public const string R4Sprint0URL = "https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/TaskPlanning?iid=2136";
		public const string R5Sprint0URL = "https://ustr-vm-0315.na.uis.unisys.com:8443/uTrack/TaskPlanning?iid=2655";

		public const string R1DataFile = "Data1";
		public const string R2DataFile = "Data2";
		public const string R3DataFile = "Data3";
		public const string R4DataFile = "Data4";
		public const string R5DataFile = "Data";
		public const string CurrentSprintActivityFile = "CurrentSprintActivityData";
		public const string AvailabilityFile = "Availability";


		public const string R6Sprint0URL = "https://ustr-jira-1.na.uis.unisys.com:8443/secure/RapidBoard.jspa?rapidView=251&projectKey=DE&view=planning";

		public static List<string> IgnoredMembers = new List<string>
		{
				"Yueling","Jasmine"
		};
	}
}
