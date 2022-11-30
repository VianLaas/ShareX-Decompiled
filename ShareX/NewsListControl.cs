using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class NewsListControl : UserControl
{
	private IContainer components;

	private DataGridView dgvNews;

	private DataGridViewTextBoxColumn chIsUnread;

	private DataGridViewTextBoxColumn chDateTime;

	private DataGridViewTextBoxColumn chText;

	public NewsManager NewsManager { get; private set; }

	public event EventHandler NewsLoaded;

	public NewsListControl()
	{
		InitializeComponent();
		dgvNews.DoubleBuffered(value: true);
		UpdateTheme();
	}

	public void UpdateTheme()
	{
		if (ShareXResources.UseCustomTheme)
		{
			dgvNews.BackgroundColor = ShareXResources.Theme.BackgroundColor;
			Color color2 = (dgvNews.DefaultCellStyle.BackColor = (dgvNews.DefaultCellStyle.SelectionBackColor = ShareXResources.Theme.BackgroundColor));
			color2 = (dgvNews.DefaultCellStyle.ForeColor = (dgvNews.DefaultCellStyle.SelectionForeColor = ShareXResources.Theme.TextColor));
			color2 = (dgvNews.AlternatingRowsDefaultCellStyle.BackColor = (dgvNews.AlternatingRowsDefaultCellStyle.SelectionBackColor = ColorHelpers.LighterColor(ShareXResources.Theme.BackgroundColor, 0.02f)));
			dgvNews.GridColor = ShareXResources.Theme.BorderColor;
		}
		else
		{
			dgvNews.BackgroundColor = SystemColors.Window;
			Color color2 = (dgvNews.DefaultCellStyle.BackColor = (dgvNews.DefaultCellStyle.SelectionBackColor = SystemColors.Window));
			color2 = (dgvNews.DefaultCellStyle.ForeColor = (dgvNews.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText));
			color2 = (dgvNews.AlternatingRowsDefaultCellStyle.BackColor = (dgvNews.AlternatingRowsDefaultCellStyle.SelectionBackColor = ColorHelpers.DarkerColor(SystemColors.Window, 0.02f)));
			dgvNews.GridColor = ProfessionalColors.SeparatorDark;
		}
		foreach (DataGridViewRow item in (IEnumerable)dgvNews.Rows)
		{
			Color color2 = (item.Cells[2].Style.ForeColor = (item.Cells[2].Style.SelectionForeColor = (ShareXResources.UseCustomTheme ? ShareXResources.Theme.TextColor : SystemColors.ControlText)));
		}
	}

	public void Start()
	{
		Task.Run(delegate
		{
			NewsManager = new NewsManager();
			NewsManager.UpdateNews();
			NewsManager.UpdateUnread();
		}).ContinueInCurrentContext(delegate
		{
			if (NewsManager != null && NewsManager.NewsItems != null)
			{
				SuspendLayout();
				foreach (NewsItem newsItem in NewsManager.NewsItems)
				{
					if (newsItem != null)
					{
						AddNewsItem(newsItem);
					}
				}
				UpdateUnreadStatus();
				ResumeLayout();
				OnNewsLoaded();
			}
		});
	}

	protected void OnNewsLoaded()
	{
		this.NewsLoaded?.Invoke(this, EventArgs.Empty);
	}

	public void MarkRead()
	{
		if (NewsManager != null && NewsManager.NewsItems != null && NewsManager.NewsItems.Count > 0)
		{
			DateTime dateTime = NewsManager.NewsItems.OrderByDescending((NewsItem x) => x.DateTime).First().DateTime;
			DateTime dateTime2 = DateTime.Now.AddMonths(1);
			if (dateTime < dateTime2)
			{
				NewsManager.UpdateUnread();
			}
		}
		UpdateUnreadStatus();
	}

	public void AddNewsItem(NewsItem item)
	{
		int index = dgvNews.Rows.Add();
		DataGridViewRow dataGridViewRow = dgvNews.Rows[index];
		dataGridViewRow.Tag = item;
		dataGridViewRow.Cells[1].Value = item.DateTime.ToShortDateString();
		double totalDays = (DateTime.Now - item.DateTime).TotalDays;
		string toolTipText = ((totalDays < 1.0) ? "Today." : ((!(totalDays < 2.0)) ? ((int)totalDays + " days ago.") : "Yesterday."));
		dataGridViewRow.Cells[1].ToolTipText = toolTipText;
		dataGridViewRow.Cells[2].Value = item.Text;
		if (URLHelpers.IsValidURL(item.URL))
		{
			dataGridViewRow.Cells[2].ToolTipText = item.URL;
		}
	}

	private void UpdateUnreadStatus()
	{
		foreach (DataGridViewRow item in (IEnumerable)dgvNews.Rows)
		{
			if (item.Tag is NewsItem newsItem && newsItem.IsUnread)
			{
				Color color2 = (item.Cells[0].Style.BackColor = (item.Cells[0].Style.SelectionBackColor = Color.LimeGreen));
			}
			else
			{
				item.Cells[0].Style = null;
			}
		}
	}

	private void dgvNews_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2)
		{
			DataGridViewRow dataGridViewRow = dgvNews.Rows[e.RowIndex];
			if (dataGridViewRow.Tag is NewsItem newsItem && !string.IsNullOrEmpty(newsItem.URL))
			{
				dgvNews.Cursor = Cursors.Hand;
				Color color3 = (dataGridViewRow.Cells[e.ColumnIndex].Style.ForeColor = (dataGridViewRow.Cells[e.ColumnIndex].Style.SelectionForeColor = (ShareXResources.UseCustomTheme ? Color.White : SystemColors.HotTrack)));
			}
		}
	}

	private void dgvNews_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
	{
		if (e.ColumnIndex == 2)
		{
			DataGridViewRow dataGridViewRow = dgvNews.Rows[e.RowIndex];
			if (dataGridViewRow.Tag is NewsItem newsItem && !string.IsNullOrEmpty(newsItem.URL))
			{
				Color color3 = (dataGridViewRow.Cells[e.ColumnIndex].Style.ForeColor = (dataGridViewRow.Cells[e.ColumnIndex].Style.SelectionForeColor = (ShareXResources.UseCustomTheme ? ShareXResources.Theme.TextColor : SystemColors.ControlText)));
			}
		}
		dgvNews.Cursor = Cursors.Default;
	}

	private void dgvNews_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && e.ColumnIndex == 2 && dgvNews.Rows[e.RowIndex].Tag is NewsItem newsItem && URLHelpers.IsValidURL(newsItem.URL))
		{
			URLHelpers.OpenURL(newsItem.URL);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
		this.dgvNews = new System.Windows.Forms.DataGridView();
		this.chIsUnread = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.chDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.chText = new System.Windows.Forms.DataGridViewTextBoxColumn();
		((System.ComponentModel.ISupportInitialize)this.dgvNews).BeginInit();
		base.SuspendLayout();
		this.dgvNews.AllowUserToAddRows = false;
		this.dgvNews.AllowUserToDeleteRows = false;
		this.dgvNews.AllowUserToResizeColumns = false;
		this.dgvNews.AllowUserToResizeRows = false;
		this.dgvNews.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
		this.dgvNews.BackgroundColor = System.Drawing.SystemColors.Window;
		this.dgvNews.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.dgvNews.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
		this.dgvNews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvNews.ColumnHeadersVisible = false;
		this.dgvNews.Columns.AddRange(this.chIsUnread, this.chDateTime, this.chText);
		dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle.BackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle.Font = new System.Drawing.Font("Arial", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		dataGridViewCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle.Padding = new System.Windows.Forms.Padding(5);
		dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Window;
		dataGridViewCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
		dataGridViewCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
		this.dgvNews.DefaultCellStyle = dataGridViewCellStyle;
		this.dgvNews.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvNews.Location = new System.Drawing.Point(0, 0);
		this.dgvNews.Name = "dgvNews";
		this.dgvNews.RowHeadersVisible = false;
		this.dgvNews.Size = new System.Drawing.Size(399, 363);
		this.dgvNews.TabIndex = 0;
		this.dgvNews.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(dgvNews_CellMouseClick);
		this.dgvNews.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(dgvNews_CellMouseEnter);
		this.dgvNews.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(dgvNews_CellMouseLeave);
		this.chIsUnread.HeaderText = "IsUnread";
		this.chIsUnread.Name = "chIsUnread";
		this.chIsUnread.ReadOnly = true;
		this.chIsUnread.Width = 5;
		this.chDateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
		this.chDateTime.DefaultCellStyle = dataGridViewCellStyle2;
		this.chDateTime.HeaderText = "DateTime";
		this.chDateTime.Name = "chDateTime";
		this.chDateTime.ReadOnly = true;
		this.chDateTime.Width = 5;
		this.chText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
		dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
		this.chText.DefaultCellStyle = dataGridViewCellStyle3;
		this.chText.HeaderText = "Text";
		this.chText.Name = "chText";
		this.chText.ReadOnly = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.dgvNews);
		base.Name = "NewsListControl";
		base.Size = new System.Drawing.Size(399, 363);
		((System.ComponentModel.ISupportInitialize)this.dgvNews).EndInit();
		base.ResumeLayout(false);
	}
}
