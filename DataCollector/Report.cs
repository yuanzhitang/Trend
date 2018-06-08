using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MockClick
{
	public partial class Report : Form
	{
		public Report()
		{
			InitializeComponent();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			foreach (var sprint in Release.Instance.Sprints)
			{
				foreach (var story in sprint.Stories)
				{
					if (!result.ContainsKey(story.Owner))
					{
						result.Add(story.Owner, new StoryItem() { User = story.Owner, StoryNum = 1 });
					}
					else
					{
						result[story.Owner].StoryNum++;
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t=>t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			foreach (var sprint in Release.Instance.Sprints)
			{
				foreach (var story in sprint.Stories.Where(t=>t.Status==  Model.StoryStatus.Done || t.Status == Model.StoryStatus.Accepted))
				{
					if (!result.ContainsKey(story.Owner))
					{
						result.Add(story.Owner, new StoryItem() { User = story.Owner, StoryNum = 1 });
					}
					else
					{
						result[story.Owner].StoryNum++;
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			foreach (var sprint in Release.Instance.Sprints)
			{
				foreach (var story in sprint.Stories.Where(t => t.Status == Model.StoryStatus.Incomplete))
				{
					if (!result.ContainsKey(story.Owner))
					{
						result.Add(story.Owner, new StoryItem() { User = story.Owner, StoryNum = 1 });
					}
					else
					{
						result[story.Owner].StoryNum++;
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button5_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			foreach (var sprint in Release.Instance.Sprints)
			{
				foreach (var story in sprint.Stories.Where(t => t.Status == Model.StoryStatus.Done || t.Status == Model.StoryStatus.Accepted))
				{
					if (!result.ContainsKey(story.Owner))
					{
						result.Add(story.Owner, new StoryItem() { User = story.Owner, StoryNum = story.Size });
					}
					else
					{
						result[story.Owner].StoryNum+=story.Size;
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			foreach (var sprint in Release.Instance.Sprints)
			{
				foreach (var story in sprint.Stories)
				{
					foreach (var task in story.Tasks.Where(t => t.Status == Model.TaskStatus.Done || t.Status == Model.TaskStatus.Accept || t.Status == Model.TaskStatus.Pass))
					{
						if (!result.ContainsKey(task.Owner))
						{
							result.Add(task.Owner, new StoryItem() { User = task.Owner, StoryNum = 1 });
						}
						else
						{
							result[task.Owner].StoryNum ++;
						}
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			foreach (var sprint in Release.Instance.Sprints)
			{
				foreach (var story in sprint.Stories)
				{
					foreach(var task in story.Tasks.Where(t => t.Status == Model.TaskStatus.Done || t.Status == Model.TaskStatus.Accept || t.Status== Model.TaskStatus.Pass))
					{
						if (!result.ContainsKey(task.Owner))
						{
							result.Add(task.Owner, new StoryItem() { User = task.Owner, StoryNum = task.WorkDone });
						}
						else
						{
							result[task.Owner].StoryNum+= task.WorkDone;
						}
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button7_Click(object sender, EventArgs e)
		{
			Dictionary<string, decimal> result = new Dictionary<string, decimal>();
			foreach (var sprint in Release.Instance.Sprints)
			{
				foreach (var story in sprint.Stories)
				{
					foreach (var task in story.Tasks.Where(t => t.Status == Model.TaskStatus.Done || t.Status == Model.TaskStatus.Accept || t.Status == Model.TaskStatus.Pass))
					{
						if (!result.ContainsKey(task.Description))
						{
							result.Add(task.Description, task.WorkDone);
						}
						else
						{
							var currentValue = result[task.Description];
							result[task.Description] = Math.Max(task.WorkDone,currentValue);
						}
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value).Take(10))
			{
				this.textBox1.Text += item.Key + "  " + item.Value + "\r\n";
			}
		}

		private void button8_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			var sprint = Release.Instance.Sprints.First();
			foreach (var story in sprint.Stories)
			{
				if (!result.ContainsKey(story.Owner))
				{
					result.Add(story.Owner, new StoryItem() { User = story.Owner, StoryNum = 1 });
				}
				else
				{
					result[story.Owner].StoryNum++;
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button9_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			var sprint = Release.Instance.Sprints.First();
			foreach (var story in sprint.Stories)
			{
				if (!result.ContainsKey(story.Owner))
				{
					result.Add(story.Owner, new StoryItem() { User = story.Owner, StoryNum = story.Size });
				}
				else
				{
					result[story.Owner].StoryNum+= story.Size;
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button10_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			var sprint = Release.Instance.Sprints.First();
			foreach (var story in sprint.Stories)
			{
				foreach (var task in story.Tasks)
				{
					if (!result.ContainsKey(task.Owner))
					{
						result.Add(task.Owner, new StoryItem() { User = task.Owner, StoryNum = 1 });
					}
					else
					{
						result[task.Owner].StoryNum++;
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "\r\n";
			}
		}

		private void button11_Click(object sender, EventArgs e)
		{
			Dictionary<string, StoryItem> result = new Dictionary<string, StoryItem>();
			Dictionary<string, decimal> completedSide = new Dictionary<string, decimal>();
			var sprint = Release.Instance.Sprints.First();
			foreach (var story in sprint.Stories)
			{
				foreach (var task in story.Tasks)
				{
					if (!result.ContainsKey(task.Owner))
					{
						result.Add(task.Owner, new StoryItem() { User = task.Owner, StoryNum = task.Estimate });
						completedSide.Add(task.Owner, task.WorkDone);
					}
					else
					{
						result[task.Owner].StoryNum += task.Estimate;
						completedSide[task.Owner] += task.WorkDone;
					}
				}
			}

			this.textBox1.Text = string.Empty;
			foreach (var item in result.OrderByDescending(t => t.Value.StoryNum))
			{
				this.textBox1.Text += item.Value.User + "  " + item.Value.StoryNum + "  " + completedSide[item.Value.User] + "\r\n";
			}
		}
	}
}
