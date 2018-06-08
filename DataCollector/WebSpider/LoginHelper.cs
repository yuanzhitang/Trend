using System;
using System.Configuration;
using System.Windows.Forms;
using Trend.Common;
using Trend.DataCollector.Config;

namespace Trend.DataCollector
{
	public class LoginHelper
	{
		private static WebBrowser webBrowser1;
		public static void InitWebBrowser(WebBrowser wb)
		{
			webBrowser1 = wb;
		}

		public static bool NeedLogin()
		{
			//webBrowser1.Refresh();
			return webBrowser1.Url.ToString().StartsWith(ConfigManager.GetHomePage());
		}

		public static void Login()
		{
			switch (DataSourceSystemConfig.DataSource)
			{
				case DataSourceSystem.uTrack:
					LoginuTrack();
					break;
				case DataSourceSystem.JIRA:
					LoginJIRA();
					break;
				default:
					throw new NotSupportedException(DataSourceSystemConfig.DataSource.ToString());
			}
		}

		private static void LoginuTrack()
		{
			HtmlDocument doc = webBrowser1.Document;
			HtmlElement userName = doc.GetElementById("UserName");
			userName.InnerText = ConfigurationManager.AppSettings["uTrackUserName"];

			HtmlElement password = doc.GetElementById("Password");
			password.InnerText = ConfigurationManager.AppSettings["uTrackPassword"]; ;

			doc.GetElementById("submit").InvokeMember("click");
		}

		public static void LoginJIRA()
		{
			HtmlDocument doc = webBrowser1.Document;
			HtmlElement userName = doc.GetElementById("login-form-username");
			userName.InnerText = ConfigurationManager.AppSettings["JIRAUserName"];

			HtmlElement password = doc.GetElementById("login-form-password");
			password.InnerText = ConfigurationManager.AppSettings["JIRAPassword"]; ;

			doc.GetElementById("login-form-submit").InvokeMember("click");
		}
	}
}
