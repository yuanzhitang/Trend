using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Trend.AnalysisService;
using Trend.DataModel;

namespace DataService.Controllers
{
    public class HomeTaskChartController : ApiController
    {
		public HttpResponseMessage GetTaskStatisticsChart()
		{
			var response = Request.CreateResponse(HttpStatusCode.OK);
			response.Content = new ByteArrayContent(App.GetTaskChart().ToArray());  //data为二进制图片数据
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

			return response;
		}
	}
}
