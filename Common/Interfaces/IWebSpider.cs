using System;

namespace Trend.Common
{
	public interface IWebSpider
	{
		void Start();

		event Action<int, int> FetchProgressChanged;

		event Action FetchCompleted;

		object Data { get; }
	}
}