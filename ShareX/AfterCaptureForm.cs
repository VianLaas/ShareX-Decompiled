using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class AfterCaptureForm : Form
{
	private IContainer components;

	private MyPictureBox pbImage;

	private Button btnContinue;

	private Button btnCancel;

	private Button btnCopy;

	private TabControl tcTasks;

	private TabPage tpAfterCapture;

	private TabPage tpBeforeUpload;

	private BeforeUploadControl ucBeforeUpload;

	private Label lblFileName;

	private TextBox txtFileName;

	private TabPage tpAfterUpload;

	private MyListView lvAfterCaptureTasks;

	private ColumnHeader chAfterCapture;

	private MyListView lvAfterUploadTasks;

	private ColumnHeader chAfterUpload;

	public TaskSettings TaskSettings { get; private set; }

	public string FileName { get; private set; }

	private AfterCaptureForm(TaskSettings taskSettings)
	{
		TaskSettings = taskSettings;
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		ImageList imageList = new ImageList
		{
			ColorDepth = ColorDepth.Depth32Bit
		};
		imageList.Images.Add(Resources.checkbox_uncheck);
		imageList.Images.Add(Resources.checkbox_check);
		lvAfterCaptureTasks.SmallImageList = imageList;
		lvAfterUploadTasks.SmallImageList = imageList;
		ucBeforeUpload.InitCapture(TaskSettings);
		AddAfterCaptureItems(TaskSettings.AfterCaptureJob);
		AddAfterUploadItems(TaskSettings.AfterUploadJob);
	}

	public AfterCaptureForm(TaskMetadata metadata, TaskSettings taskSettings)
		: this(taskSettings)
	{
		if (metadata != null && metadata.Image != null)
		{
			pbImage.LoadImage(metadata.Image);
			btnCopy.Enabled = true;
		}
		FileName = TaskHelpers.GetFileName(TaskSettings, null, metadata);
		txtFileName.Text = FileName;
	}

	public AfterCaptureForm(string filePath, TaskSettings taskSettings)
		: this(taskSettings)
	{
		if (FileHelpers.IsImageFile(filePath))
		{
			pbImage.LoadImageFromFileAsync(filePath);
		}
		FileName = Path.GetFileNameWithoutExtension(filePath);
		txtFileName.Text = FileName;
	}

	private void AfterCaptureForm_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private void Continue()
	{
		TaskSettings.AfterCaptureJob = GetAfterCaptureTasks();
		TaskSettings.AfterUploadJob = GetAfterUploadTasks();
		FileName = txtFileName.Text;
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void CheckItem(ListViewItem lvi, bool check)
	{
		lvi.ImageIndex = (check ? 1 : 0);
	}

	private bool IsChecked(ListViewItem lvi)
	{
		return lvi.ImageIndex == 1;
	}

	private void AddAfterCaptureItems(AfterCaptureTasks afterCaptureTasks)
	{
		AfterCaptureTasks[] source = new AfterCaptureTasks[3]
		{
			AfterCaptureTasks.None,
			AfterCaptureTasks.ShowQuickTaskMenu,
			AfterCaptureTasks.ShowAfterCaptureWindow
		};
		int num = 0;
		AfterCaptureTasks[] enums = Helpers.GetEnums<AfterCaptureTasks>();
		foreach (AfterCaptureTasks task in enums)
		{
			if (!source.Any((AfterCaptureTasks x) => x == task))
			{
				ListViewItem listViewItem = new ListViewItem(task.GetLocalizedDescription());
				CheckItem(listViewItem, afterCaptureTasks.HasFlag(task));
				listViewItem.Tag = task;
				lvAfterCaptureTasks.Items.Add(listViewItem);
				if (num == 0)
				{
					num = listViewItem.Bounds.Height;
				}
			}
		}
		int num2 = lvAfterCaptureTasks.Items.Count * num - lvAfterCaptureTasks.Height;
		if (num2 > 0)
		{
			base.Height += num2;
		}
	}

	private AfterCaptureTasks GetAfterCaptureTasks()
	{
		AfterCaptureTasks afterCaptureTasks = AfterCaptureTasks.None;
		for (int i = 0; i < lvAfterCaptureTasks.Items.Count; i++)
		{
			ListViewItem listViewItem = lvAfterCaptureTasks.Items[i];
			if (IsChecked(listViewItem))
			{
				afterCaptureTasks = afterCaptureTasks.Add<AfterCaptureTasks>((AfterCaptureTasks)listViewItem.Tag);
			}
		}
		return afterCaptureTasks;
	}

	private void lvAfterCaptureTasks_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
	{
		e.Item.Selected = false;
	}

	private void lvAfterCaptureTasks_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ListViewItem itemAt = lvAfterCaptureTasks.GetItemAt(e.X, e.Y);
			if (itemAt != null)
			{
				CheckItem(itemAt, !IsChecked(itemAt));
			}
		}
	}

	private void AddAfterUploadItems(AfterUploadTasks afterUploadTasks)
	{
		AfterUploadTasks[] source = new AfterUploadTasks[1];
		AfterUploadTasks[] enums = Helpers.GetEnums<AfterUploadTasks>();
		foreach (AfterUploadTasks task in enums)
		{
			if (!source.Any((AfterUploadTasks x) => x == task))
			{
				ListViewItem listViewItem = new ListViewItem(task.GetLocalizedDescription());
				CheckItem(listViewItem, afterUploadTasks.HasFlag(task));
				listViewItem.Tag = task;
				lvAfterUploadTasks.Items.Add(listViewItem);
			}
		}
	}

	private AfterUploadTasks GetAfterUploadTasks()
	{
		AfterUploadTasks afterUploadTasks = AfterUploadTasks.None;
		for (int i = 0; i < lvAfterUploadTasks.Items.Count; i++)
		{
			ListViewItem listViewItem = lvAfterUploadTasks.Items[i];
			if (IsChecked(listViewItem))
			{
				afterUploadTasks = afterUploadTasks.Add<AfterUploadTasks>((AfterUploadTasks)listViewItem.Tag);
			}
		}
		return afterUploadTasks;
	}

	private void lvAfterUploadTasks_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
	{
		e.Item.Selected = false;
	}

	private void lvAfterUploadTasks_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ListViewItem itemAt = lvAfterUploadTasks.GetItemAt(e.X, e.Y);
			if (itemAt != null)
			{
				CheckItem(itemAt, !IsChecked(itemAt));
			}
		}
	}

	private void txtFileName_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			e.SuppressKeyPress = true;
		}
	}

	private void txtFileName_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			Continue();
		}
	}

	private void btnContinue_Click(object sender, EventArgs e)
	{
		Continue();
	}

	private void btnCopy_Click(object sender, EventArgs e)
	{
		TaskSettings.AfterCaptureJob = AfterCaptureTasks.CopyImageToClipboard;
		FileName = txtFileName.Text;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.AfterCaptureForm));
		this.btnContinue = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnCopy = new System.Windows.Forms.Button();
		this.tcTasks = new System.Windows.Forms.TabControl();
		this.tpAfterCapture = new System.Windows.Forms.TabPage();
		this.lvAfterCaptureTasks = new ShareX.HelpersLib.MyListView();
		this.chAfterCapture = new System.Windows.Forms.ColumnHeader();
		this.tpBeforeUpload = new System.Windows.Forms.TabPage();
		this.ucBeforeUpload = new ShareX.BeforeUploadControl();
		this.tpAfterUpload = new System.Windows.Forms.TabPage();
		this.lvAfterUploadTasks = new ShareX.HelpersLib.MyListView();
		this.chAfterUpload = new System.Windows.Forms.ColumnHeader();
		this.lblFileName = new System.Windows.Forms.Label();
		this.txtFileName = new System.Windows.Forms.TextBox();
		this.pbImage = new ShareX.HelpersLib.MyPictureBox();
		this.tcTasks.SuspendLayout();
		this.tpAfterCapture.SuspendLayout();
		this.tpBeforeUpload.SuspendLayout();
		this.tpAfterUpload.SuspendLayout();
		base.SuspendLayout();
		resources.ApplyResources(this.btnContinue, "btnContinue");
		this.btnContinue.Name = "btnContinue";
		this.btnContinue.UseVisualStyleBackColor = true;
		this.btnContinue.Click += new System.EventHandler(btnContinue_Click);
		resources.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		resources.ApplyResources(this.btnCopy, "btnCopy");
		this.btnCopy.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.btnCopy.Name = "btnCopy";
		this.btnCopy.UseVisualStyleBackColor = true;
		this.btnCopy.Click += new System.EventHandler(btnCopy_Click);
		resources.ApplyResources(this.tcTasks, "tcTasks");
		this.tcTasks.Controls.Add(this.tpAfterCapture);
		this.tcTasks.Controls.Add(this.tpBeforeUpload);
		this.tcTasks.Controls.Add(this.tpAfterUpload);
		this.tcTasks.Name = "tcTasks";
		this.tcTasks.SelectedIndex = 0;
		this.tpAfterCapture.BackColor = System.Drawing.SystemColors.Window;
		this.tpAfterCapture.Controls.Add(this.lvAfterCaptureTasks);
		resources.ApplyResources(this.tpAfterCapture, "tpAfterCapture");
		this.tpAfterCapture.Name = "tpAfterCapture";
		this.lvAfterCaptureTasks.AutoFillColumn = true;
		this.lvAfterCaptureTasks.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lvAfterCaptureTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.chAfterCapture });
		resources.ApplyResources(this.lvAfterCaptureTasks, "lvAfterCaptureTasks");
		this.lvAfterCaptureTasks.FullRowSelect = true;
		this.lvAfterCaptureTasks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		this.lvAfterCaptureTasks.HideSelection = false;
		this.lvAfterCaptureTasks.MultiSelect = false;
		this.lvAfterCaptureTasks.Name = "lvAfterCaptureTasks";
		this.lvAfterCaptureTasks.UseCompatibleStateImageBehavior = false;
		this.lvAfterCaptureTasks.View = System.Windows.Forms.View.Details;
		this.lvAfterCaptureTasks.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(lvAfterCaptureTasks_ItemSelectionChanged);
		this.lvAfterCaptureTasks.MouseDown += new System.Windows.Forms.MouseEventHandler(lvAfterCaptureTasks_MouseDown);
		this.tpBeforeUpload.BackColor = System.Drawing.SystemColors.Window;
		this.tpBeforeUpload.Controls.Add(this.ucBeforeUpload);
		resources.ApplyResources(this.tpBeforeUpload, "tpBeforeUpload");
		this.tpBeforeUpload.Name = "tpBeforeUpload";
		resources.ApplyResources(this.ucBeforeUpload, "ucBeforeUpload");
		this.ucBeforeUpload.Name = "ucBeforeUpload";
		this.tpAfterUpload.BackColor = System.Drawing.SystemColors.Window;
		this.tpAfterUpload.Controls.Add(this.lvAfterUploadTasks);
		resources.ApplyResources(this.tpAfterUpload, "tpAfterUpload");
		this.tpAfterUpload.Name = "tpAfterUpload";
		this.lvAfterUploadTasks.AutoFillColumn = true;
		this.lvAfterUploadTasks.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.lvAfterUploadTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.chAfterUpload });
		resources.ApplyResources(this.lvAfterUploadTasks, "lvAfterUploadTasks");
		this.lvAfterUploadTasks.FullRowSelect = true;
		this.lvAfterUploadTasks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		this.lvAfterUploadTasks.HideSelection = false;
		this.lvAfterUploadTasks.MultiSelect = false;
		this.lvAfterUploadTasks.Name = "lvAfterUploadTasks";
		this.lvAfterUploadTasks.UseCompatibleStateImageBehavior = false;
		this.lvAfterUploadTasks.View = System.Windows.Forms.View.Details;
		this.lvAfterUploadTasks.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(lvAfterUploadTasks_ItemSelectionChanged);
		this.lvAfterUploadTasks.MouseDown += new System.Windows.Forms.MouseEventHandler(lvAfterUploadTasks_MouseDown);
		resources.ApplyResources(this.lblFileName, "lblFileName");
		this.lblFileName.Name = "lblFileName";
		resources.ApplyResources(this.txtFileName, "txtFileName");
		this.txtFileName.Name = "txtFileName";
		this.txtFileName.KeyDown += new System.Windows.Forms.KeyEventHandler(txtFileName_KeyDown);
		this.txtFileName.KeyUp += new System.Windows.Forms.KeyEventHandler(txtFileName_KeyUp);
		resources.ApplyResources(this.pbImage, "pbImage");
		this.pbImage.BackColor = System.Drawing.SystemColors.Window;
		this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pbImage.Cursor = System.Windows.Forms.Cursors.Default;
		this.pbImage.DrawCheckeredBackground = true;
		this.pbImage.EnableRightClickMenu = true;
		this.pbImage.FullscreenOnClick = true;
		this.pbImage.Name = "pbImage";
		this.pbImage.PictureBoxBackColor = System.Drawing.SystemColors.Window;
		this.pbImage.ShowImageSizeLabel = true;
		resources.ApplyResources(this, "$this");
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnCancel;
		base.Controls.Add(this.txtFileName);
		base.Controls.Add(this.lblFileName);
		base.Controls.Add(this.tcTasks);
		base.Controls.Add(this.btnCopy);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnContinue);
		base.Controls.Add(this.pbImage);
		base.Name = "AfterCaptureForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.TopMost = true;
		base.Shown += new System.EventHandler(AfterCaptureForm_Shown);
		this.tcTasks.ResumeLayout(false);
		this.tpAfterCapture.ResumeLayout(false);
		this.tpBeforeUpload.ResumeLayout(false);
		this.tpAfterUpload.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
