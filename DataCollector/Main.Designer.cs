namespace Trend.DataCollector
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.btnStart = new System.Windows.Forms.Button();
			this.btnLogin = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.panelProgess = new System.Windows.Forms.Panel();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.lblPercent = new System.Windows.Forms.Label();
			this.lblProgress = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pbFetchData = new System.Windows.Forms.ProgressBar();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.panelProgess.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.BackColor = System.Drawing.Color.Blue;
			this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStart.ForeColor = System.Drawing.Color.Yellow;
			this.btnStart.Location = new System.Drawing.Point(51, 109);
			this.btnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(316, 77);
			this.btnStart.TabIndex = 3;
			this.btnStart.Text = "Fetch Data From uTrack（DE5.0)";
			this.btnStart.UseVisualStyleBackColor = false;
			this.btnStart.Click += new System.EventHandler(this.Start);
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(51, 23);
			this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(258, 58);
			this.btnLogin.TabIndex = 1;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btn_LoginuTrack);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(0, 954);
			this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(112, 35);
			this.button3.TabIndex = 4;
			this.button3.Text = "button3";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// panelProgess
			// 
			this.panelProgess.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.panelProgess.Controls.Add(this.webBrowser1);
			this.panelProgess.Location = new System.Drawing.Point(404, 5);
			this.panelProgess.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panelProgess.Name = "panelProgess";
			this.panelProgess.Size = new System.Drawing.Size(1018, 678);
			this.panelProgess.TabIndex = 6;
			// 
			// webBrowser1
			// 
			this.webBrowser1.Location = new System.Drawing.Point(8, 5);
			this.webBrowser1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(30, 31);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(1006, 669);
			this.webBrowser1.TabIndex = 10;
			// 
			// lblPercent
			// 
			this.lblPercent.AutoSize = true;
			this.lblPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPercent.Location = new System.Drawing.Point(327, 589);
			this.lblPercent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblPercent.Name = "lblPercent";
			this.lblPercent.Size = new System.Drawing.Size(27, 13);
			this.lblPercent.TabIndex = 9;
			this.lblPercent.Text = "N/A";
			// 
			// lblProgress
			// 
			this.lblProgress.AutoSize = true;
			this.lblProgress.Location = new System.Drawing.Point(142, 625);
			this.lblProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(35, 20);
			this.lblProgress.TabIndex = 8;
			this.lblProgress.Text = "N/A";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(51, 625);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 20);
			this.label1.TabIndex = 7;
			this.label1.Text = "Total sprint: ";
			// 
			// pbFetchData
			// 
			this.pbFetchData.Location = new System.Drawing.Point(51, 582);
			this.pbFetchData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pbFetchData.Name = "pbFetchData";
			this.pbFetchData.Size = new System.Drawing.Size(268, 28);
			this.pbFetchData.TabIndex = 6;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button1);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.lblPercent);
			this.panel1.Controls.Add(this.panelProgess);
			this.panel1.Controls.Add(this.pbFetchData);
			this.panel1.Controls.Add(this.lblProgress);
			this.panel1.Controls.Add(this.button3);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.btnLogin);
			this.panel1.Controls.Add(this.btnStart);
			this.panel1.Location = new System.Drawing.Point(14, 14);
			this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1436, 688);
			this.panel1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(56, 195);
			this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(312, 74);
			this.button1.TabIndex = 11;
			this.button1.Text = "Start fetch Data（DE1.0 to 4.0)";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_2);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(315, 23);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(81, 58);
			this.button2.TabIndex = 10;
			this.button2.Text = "Refresh";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Refresh_Click);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1485, 772);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new System.Drawing.Size(1501, 811);
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Data Collector";
			this.panelProgess.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Panel panelProgess;
		private System.Windows.Forms.Label lblProgress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ProgressBar pbFetchData;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblPercent;
		public System.Windows.Forms.WebBrowser webBrowser1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
	}
}

