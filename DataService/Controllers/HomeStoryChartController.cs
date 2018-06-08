using System.Collections.Generic;
using System.Web.Http;
using Trend.DataModel;
using Trend.AnalysisService;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DataService.Controllers
{
	public class HomeStoryChartController : ApiController
	{
		public HttpResponseMessage GetStoryStatisticsChart()
		{
			var response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new ByteArrayContent(App.GetStoryChart().ToArray());  //data为二进制图片数据
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

			return response;
		}
	}
}
