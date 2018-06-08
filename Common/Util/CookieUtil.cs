using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Trend.Common
{
	public class CookieUtil
	{
		public static string GetTagClass(int occurs)
		{
			var result = occurs;

			if (result <= 4)
				return "tag1";
			if (result <= 8)
				return "tag2";
			if (result <= 12)
				return "tag3";
			if (result <= 20)
				return "tag4";
			if (result <= 30)
				return "tag5";

			if (result <= 50)
				return "tag6";

			if (result <= 80)
				return "tag7";

			if (result <= 100)
				return "tag8";

			if (result <= 120)
				return "tag9";

			if (result <= 150)
				return "tag10";

			return "tag11";
		}

		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);
		public static string GetCookies(string url)
		{
			uint datasize = 1024;
			StringBuilder cookieData = new StringBuilder((int)datasize);
			if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
			{
				if (datasize < 0)
					return null;

				cookieData = new StringBuilder((int)datasize);
				if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
					return null;
			}
			return cookieData.ToString();
		}
	}
}
