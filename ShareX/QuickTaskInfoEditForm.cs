using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class QuickTaskInfoEditForm : Form
{
	private IContainer components;

	private MenuButton mbAfterCaptureTasks;

	private MenuButton mbAfterUploadTasks;

	private ContextMenuStrip cmsAfterCapture;

	private ContextMenuStrip cmsAfterUpload;

	private Label lblAfterCaptureTasks;

	private Label lblAfterUploadTasks;

	private Label lblName;

	private TextBox txtName;

	private Button btnOK;

	public QuickTaskInfo TaskInfo { get; private set; }

	public QuickTaskInfoEditForm(QuickTaskInfo taskInfo)
	{
		TaskInfo = taskInfo;
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		txtName.Text = TaskInfo.Name;
		AddMultiEnumItemsContextMenu(delegate(AfterCaptureTasks x)
		{
			TaskInfo.AfterCaptureTasks = TaskInfo.AfterCaptureTasks.Swap<AfterCaptureTasks>(x);
		}, cmsAfterCapture);
		AddMultiEnumItemsContextMenu(delegate(AfterUploadTasks x)
		{
			TaskInfo.AfterUploadTasks = TaskInfo.AfterUploadTasks.Swap<AfterUploadTasks>(x);
		}, cmsAfterUpload);
		SetMultiEnumCheckedContextMenu(TaskInfo.AfterCaptureTasks, cmsAfterCapture);
		SetMultiEnumCheckedContextMenu(TaskInfo.AfterUploadTasks, cmsAfterUpload);
		UpdateUploaderMenuNames();
	}

	private void txtName_TextChanged(object sender, EventArgs e)
	{
		TaskInfo.Name = txtName.Text;
	}

	private void AddMultiEnumItemsContextMenu<T>(Action<T> selectedEnum, params ToolStripDropDown[] parents) where T : Enum
	{
		string[] array = (from x in Helpers.GetLocalizedEnumDescriptions<T>().Skip(1)
			select x.Replace("&", "&&")).ToArray();
		ToolStripDropDown[] array2 = parents;
		foreach (ToolStripDropDown toolStripDropDown in array2)
		{
			for (int j = 0; j < array.Length; j++)
			{
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(array[j]);
				toolStripMenuItem.Image = TaskHelpers.FindMenuIcon<T>(j + 1);
				int index = j;
				toolStripMenuItem.Click += delegate
				{
					ToolStripDropDown[] array3 = parents;
					for (int k = 0; k < array3.Length; k++)
					{
						ToolStripMenuItem obj = (ToolStripMenuItem)array3[k].Items[index];
						obj.Checked = !obj.Checked;
					}
					selectedEnum((T)Enum.ToObject(typeof(T), 1 << index));
					UpdateUploaderMenuNames();
				};
				toolStripDropDown.Items.Add(toolStripMenuItem);
			}
		}
	}

	private void SetMultiEnumCheckedContextMenu(Enum value, params ToolStripDropDown[] parents)
	{
		for (int i = 0; i < parents[0].Items.Count; i++)
		{
			for (int j = 0; j < parents.Length; j++)
			{
				((ToolStripMenuItem)parents[j].Items[i]).Checked = value.HasFlag<int>(1 << i);
			}
		}
	}

	private void UpdateUploaderMenuNames()
	{
		txtName.SetWatermark(TaskInfo.ToString(), showCueWhenFocus: true);
		mbAfterCaptureTasks.Text = string.Join(", ", from x in TaskInfo.AfterCaptureTasks.GetFlags()
			select x.GetLocalizedDescription());
		mbAfterUploadTasks.Text = string.Join(", ", from x in TaskInfo.AfterUploadTasks.GetFlags()
			select x.GetLocalizedDescription());
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.QuickTaskInfoEditForm));
		this.mbAfterCaptureTasks = new ShareX.HelpersLib.MenuButton();
		this.cmsAfterCapture = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.mbAfterUploadTasks = new ShareX.HelpersLib.MenuButton();
		this.cmsAfterUpload = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lblAfterCaptureTasks = new System.Windows.Forms.Label();
		this.lblAfterUploadTasks = new System.Windows.Forms.Label();
		this.lblName = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.btnOK = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.mbAfterCaptureTasks, "mbAfterCaptureTasks");
		this.mbAfterCaptureTasks.Menu = this.cmsAfterCapture;
		this.mbAfterCaptureTasks.Name = "mbAfterCaptureTasks";
		this.mbAfterCaptureTasks.UseVisualStyleBackColor = true;
		this.cmsAfterCapture.Name = "cmsAfterCapture";
		componentResourceManager.ApplyResources(this.cmsAfterCapture, "cmsAfterCapture");
		componentResourceManager.ApplyResources(this.mbAfterUploadTasks, "mbAfterUploadTasks");
		this.mbAfterUploadTasks.Menu = this.cmsAfterUpload;
		this.mbAfterUploadTasks.Name = "mbAfterUploadTasks";
		this.mbAfterUploadTasks.UseVisualStyleBackColor = true;
		this.cmsAfterUpload.Name = "cmsAfterUpload";
		componentResourceManager.ApplyResources(this.cmsAfterUpload, "cmsAfterUpload");
		componentResourceManager.ApplyResources(this.lblAfterCaptureTasks, "lblAfterCaptureTasks");
		this.lblAfterCaptureTasks.Name = "lblAfterCaptureTasks";
		componentResourceManager.ApplyResources(this.lblAfterUploadTasks, "lblAfterUploadTasks");
		this.lblAfterUploadTasks.Name = "lblAfterUploadTasks";
		componentResourceManager.ApplyResources(this.lblName, "lblName");
		this.lblName.Name = "lblName";
		componentResourceManager.ApplyResources(this.txtName, "txtName");
		this.txtName.Name = "txtName";
		this.txtName.TextChanged += new System.EventHandler(txtName_TextChanged);
		this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
		componentResourceManager.ApplyResources(this.btnOK, "btnOK");
		this.btnOK.Name = "btnOK";
		this.btnOK.UseVisualStyleBackColor = true;
		base.AcceptButton = this.btnOK;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.btnOK);
		base.Controls.Add(this.txtName);
		base.Controls.Add(this.lblName);
		base.Controls.Add(this.lblAfterUploadTasks);
		base.Controls.Add(this.lblAfterCaptureTasks);
		base.Controls.Add(this.mbAfterUploadTasks);
		base.Controls.Add(this.mbAfterCaptureTasks);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.Name = "QuickTaskInfoEditForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
