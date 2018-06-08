using System;
using System.Windows.Forms;

namespace Trend.DataCollector
{
	public partial class Main : Form
	{
		public Main()
		{
			InitializeComponent();
			this.Load += Form1_Load;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			this.webBrowser1.Navigate(ConfigManager.GetHomePage());
			ZoomInWebbrowser(3);

			LoginHelper.InitWebBrowser(this.webBrowser1);
		}

		private void btn_LoginuTrack(object sender, EventArgs e)
		{
			LoginHelper.Login();
		}

		private WebSpiderWorker worker = null;

		private void Start(object sender, EventArgs e)
		{
			if (LoginHelper.NeedLogin())
			{
				LoginHelper.Login();
			}

			if (worker == null)
			{
				worker = new WebSpiderWorker();
				worker.ProgressChanged += ProgressChanged;
			}
			worker.Start();
		}

		private delegate void UpdateUIHandler(int total, int current);
		private void ProgressChanged(int total, int current)
		{
			if (this.pbFetchData.InvokeRequired)
			{
				UpdateUIHandler handler = new UpdateUIHandler(ProgressChanged);
				this.pbFetchData.Invoke(handler, total, current);
			}
			else
			{
				pbFetchData.Maximum = total;
				pbFetchData.Value = current;
				lblProgress.Text = $"{current}/{total}";
				lblPercent.Text = (int)((current / (double)total) * 100) + "%";
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			webBrowser1.Focus();
			SendKeys.Send("^{-}");
		}

		private void ZoomInWebbrowser(int times)
		{
			for (int i = 0; i < times; i++)
			{
				webBrowser1.Focus();
				SendKeys.Send("^{-}");
			}
		}
	
		private void Refresh_Click(object sender, EventArgs e)
		{
			this.webBrowser1.Refresh();
		}

		private void button1_Click_2(object sender, EventArgs e)
		{
			if (LoginHelper.NeedLogin())
			{
				LoginHelper.Login();
			}

			//var release4 = new Release(Release.R4Sprint0URL, Release.R4DataFile);
			//release4.UpdateEvent += UpdateProgress;

			//var release3 = new Release(Release.R3Sprint0URL, Release.R3DataFile);
			//release3.UpdateEvent += UpdateProgress;

			//var release2 = new Release(Release.R2Sprint0URL, Release.R2DataFile);
			//release2.UpdateEvent += UpdateProgress;

			//var release1 = new Release(Release.R1Sprint0URL, Release.R1DataFile);
			//release1.UpdateEvent += UpdateProgress;

			//release4.FetchAndSaveData().ContinueWith(task =>
			//{
			//	release3.FetchAndSaveData();
			//}).ContinueWith(task =>
			//{
			//	release2.FetchAndSaveData();
			//}).ContinueWith(task =>
			//{
			//	release1.FetchAndSaveData();
			//});
		}
	}
}
