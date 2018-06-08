using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trend.Common;
using Trend.DataModel;
using Trend.AnalysisService;
using Trend.Persitent;

namespace Trend.DataCollector
{
	public class WebSpiderWorker
	{
		#region Private Fields

		private string dataFile = AppConfig.R5DataFile;
		private string lastDataFile = AppConfig.R5DataFile + "_Last";

		private IWebSpider webSpider = null;
		private DataAnalyzerDirector dataAnalyzer = null;

		private Release Data;

		private ScrumDataService ReleaseDataService;

		public event Action<int, int> ProgressChanged;

		#endregion

		#region Init

		public WebSpiderWorker()
		{
			Initialize();
		}

		private void Initialize()
		{
			webSpider = WebSpiderFactory.Create();
			dataAnalyzer = new DataAnalyzerDirector(AppConfig.R5DataFile);

			webSpider.FetchProgressChanged += OnProgressChanged;

			InitRepository();
		}

		private void InitRepository()
		{
			ReleaseDataService = RepositoryServiceFactory.CreateScrumDataService(); 
		}

		#endregion

		public async void Start()
		{
			while (true)
			{
				await Task.Run(() =>
				{
					webSpider.Start();

					Data = webSpider.Data as Release;

					Save();

				}).ContinueWith(tsk =>
				{
					dataAnalyzer.Analyze();

				}).ContinueWith(tsk =>
				{
					Thread.Sleep(ConfigManager.FetchInterval);
				});
			}
		}

		#region Save

		private void Save()
		{
			SaveDataIntoJsonFile();
			SaveDataIntoRepository();
		}

		private void SaveDataIntoRepository()
		{
			ReleaseDataService.Save(Data);
		}

		private void SaveDataIntoJsonFile()
		{
			string dataPath = @"..\..\AdministratorSite\CollectedData";
			string newdataFile = $@"{dataPath}\{dataFile}.json";
			string lastDataFile = $@"{dataPath}\{this.lastDataFile}.json";

			if (!Directory.Exists(dataPath))
			{
				Directory.CreateDirectory(dataPath);
			}
			else
			{
				if (File.Exists(lastDataFile))
				{
					File.Delete(lastDataFile);
				}
				if (File.Exists(newdataFile))
				{
					File.Move(newdataFile, lastDataFile);
				}
			}

			var data = JsonConvert.SerializeObject(Data);

			File.WriteAllText(newdataFile, data, Encoding.UTF8);
		}

		#endregion

		private void OnProgressChanged(int arg1, int arg2)
		{
			ProgressChanged?.Invoke(arg1, arg2);
		}
	}
}
