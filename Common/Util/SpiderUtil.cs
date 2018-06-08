using System;
using System.IO;
using System.Net;
using System.Text;

namespace Trend.Common
{
	public class SpiderUtil
	{
		public static string GetHtml(string url, string cookievalue, string charSet)
		{
			try
			{
				var request = (HttpWebRequest)WebRequest.Create(url);
				request.Timeout = 10000;
				request.Accept = "*/*";
				request.UserAgent =
					"Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36";
				request.Headers.Add("Accept-Language", "zh-cn");
				request.Headers.Add("Cookie", cookievalue);

				Stream responseStream = request.GetResponse().GetResponseStream();
				if (responseStream != null)
					responseStream.ReadTimeout = 0x3a98;
				if (string.IsNullOrEmpty(charSet))
					charSet = "gb2312";
				if (responseStream != null)
				{
					var reader = new StreamReader(responseStream, Encoding.GetEncoding(charSet));
					var result = reader.ReadToEnd();
					return result;
				}
			}
			catch (Exception)
			{
				return "";
			}
			return null;
		}

		public static string TrimHtmlTag(string str)
		{
			return str.Replace("\t", "").Replace("\n", "").Replace("\r", string.Empty).Trim();
		}
	}
}
