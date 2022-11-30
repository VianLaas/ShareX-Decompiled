using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class QuickTaskMenuEditorForm : Form
{
	private IContainer components;

	private MyListView lvPresets;

	private ColumnHeader chName;

	private Button btnAdd;

	private Button btnEdit;

	private Button btnRemove;

	private Button btnReset;

	private Button btnClose;

	private Label lblTip;

	public QuickTaskMenuEditorForm()
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		if (Program.Settings.QuickTaskPresets == null)
		{
			Program.Settings.QuickTaskPresets = new List<QuickTaskInfo>();
		}
		UpdateItems();
	}

	private void UpdateItem(ListViewItem lvi, QuickTaskInfo taskInfo)
	{
		lvi.Tag = taskInfo;
		lvi.Text = taskInfo.ToString();
	}

	private void UpdateItems()
	{
		lvPresets.Items.Clear();
		foreach (QuickTaskInfo quickTaskPreset in Program.Settings.QuickTaskPresets)
		{
			ListViewItem listViewItem = new ListViewItem();
			UpdateItem(listViewItem, quickTaskPreset);
			lvPresets.Items.Add(listViewItem);
		}
	}

	private void Edit(ListViewItem lvi, QuickTaskInfo taskInfo)
	{
		new QuickTaskInfoEditForm(taskInfo).ShowDialog();
		UpdateItem(lvi, taskInfo);
	}

	private void EditSelectedItem()
	{
		if (lvPresets.SelectedItems.Count > 0)
		{
			ListViewItem listViewItem = lvPresets.SelectedItems[0];
			QuickTaskInfo taskInfo = listViewItem.Tag as QuickTaskInfo;
			Edit(listViewItem, taskInfo);
		}
	}

	private void lvPresets_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			EditSelectedItem();
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		QuickTaskInfo quickTaskInfo = new QuickTaskInfo();
		ListViewItem listViewItem = new ListViewItem();
		Program.Settings.QuickTaskPresets.Add(quickTaskInfo);
		lvPresets.Items.Add(listViewItem);
		Edit(listViewItem, quickTaskInfo);
	}

	private void btnEdit_Click(object sender, EventArgs e)
	{
		EditSelectedItem();
	}

	private void btnRemove_Click(object sender, EventArgs e)
	{
		if (lvPresets.SelectedItems.Count > 0)
		{
			ListViewItem listViewItem = lvPresets.SelectedItems[0];
			QuickTaskInfo item = listViewItem.Tag as QuickTaskInfo;
			Program.Settings.QuickTaskPresets.Remove(item);
			lvPresets.Items.Remove(listViewItem);
		}
	}

	private void lvPresets_ItemMoved(object sender, int oldIndex, int newIndex)
	{
		Program.Settings.QuickTaskPresets.Move(oldIndex, newIndex);
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show(Resources.QuickTaskMenuEditorForm_Reset_all_quick_tasks_to_defaults_Confirmation, "ShareX", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			Program.Settings.QuickTaskPresets = QuickTaskInfo.DefaultPresets;
			UpdateItems();
		}
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.QuickTaskMenuEditorForm));
		this.lvPresets = new ShareX.HelpersLib.MyListView();
		this.chName = new System.Windows.Forms.ColumnHeader();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnEdit = new System.Windows.Forms.Button();
		this.btnRemove = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		this.lblTip = new System.Windows.Forms.Label();
		base.SuspendLayout();
		this.lvPresets.AllowDrop = true;
		this.lvPresets.AllowItemDrag = true;
		resources.ApplyResources(this.lvPresets, "lvPresets");
		this.lvPresets.AutoFillColumn = true;
		this.lvPresets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.chName });
		this.lvPresets.FullRowSelect = true;
		this.lvPresets.GridLines = true;
		this.lvPresets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		this.lvPresets.Name = "lvPresets";
		this.lvPresets.UseCompatibleStateImageBehavior = false;
		this.lvPresets.View = System.Windows.Forms.View.Details;
		this.lvPresets.ItemMoved += new ShareX.HelpersLib.MyListView.ListViewItemMovedEventHandler(lvPresets_ItemMoved);
		this.lvPresets.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(lvPresets_MouseDoubleClick);
		resources.ApplyResources(this.chName, "chName");
		resources.ApplyResources(this.btnAdd, "btnAdd");
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		resources.ApplyResources(this.btnEdit, "btnEdit");
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.UseVisualStyleBackColor = true;
		this.btnEdit.Click += new System.EventHandler(btnEdit_Click);
		resources.ApplyResources(this.btnRemove, "btnRemove");
		this.btnRemove.Name = "btnRemove";
		this.btnRemove.UseVisualStyleBackColor = true;
		this.btnRemove.Click += new System.EventHandler(btnRemove_Click);
		resources.ApplyResources(this.btnReset, "btnReset");
		this.btnReset.Name = "btnReset";
		this.btnReset.UseVisualStyleBackColor = true;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		resources.ApplyResources(this.btnClose, "btnClose");
		this.btnClose.Name = "btnClose";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		resources.ApplyResources(this.lblTip, "lblTip");
		this.lblTip.Name = "lblTip";
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnClose;
		base.Controls.Add(this.lblTip);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.btnReset);
		base.Controls.Add(this.btnRemove);
		base.Controls.Add(this.btnEdit);
		base.Controls.Add(this.btnAdd);
		base.Controls.Add(this.lvPresets);
		base.Name = "QuickTaskMenuEditorForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
