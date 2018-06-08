using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trend.Persitent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;
using Trend.DataModel;

namespace UnitTest
{
	[TestClass]
	public class DataServiceTest
	{
		
		public DataServiceTest()
		{
			
		}

		[TestInitialize]
		public void TestClear()
		{
			
		}

		[TestMethod]
		public async Task ReadActivities()
		{
			try
			{
				var client = new HttpClient();
				client.BaseAddress = new Uri("http://localhost:62928/");
				string response = await client.GetStringAsync("/api/activities");
				Console.WriteLine(response);

				var serializer = new JavaScriptSerializer();
				var activities = serializer.Deserialize<PersonActivityItem[]>(response);

				foreach (var activity in activities)
				{
					//.WriteLine(activity.Title);
				}
				Console.ReadKey();
			}
			catch (Exception ex)
			{

			}
		}
	}
}
