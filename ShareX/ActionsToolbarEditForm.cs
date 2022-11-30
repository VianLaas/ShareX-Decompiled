using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class ActionsToolbarEditForm : Form
{
	private IContainer components;

	private Button btnRemove;

	private MenuButton btnAdd;

	private ContextMenuStrip cmsAction;

	private MyListView lvActions;

	private ColumnHeader chAction;

	private ImageList ilMain;

	public List<HotkeyType> Actions { get; private set; }

	public ActionsToolbarEditForm(List<HotkeyType> actions)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		Actions = actions;
		HotkeyType[] enums = Helpers.GetEnums<HotkeyType>();
		for (int i = 0; i < enums.Length; i++)
		{
			HotkeyType hotkeyType = enums[i];
			Image image = ((hotkeyType != 0) ? TaskHelpers.FindMenuIcon(hotkeyType) : Resources.ui_splitter);
			ilMain.Images.Add(hotkeyType.ToString(), image);
		}
		AddEnumItemsContextMenu(AddAction, cmsAction);
		foreach (HotkeyType action in Actions)
		{
			AddActionToList(action);
		}
	}

	private void AddEnumItemsContextMenu(Action<HotkeyType> selectedEnum, params ToolStripDropDown[] parents)
	{
		EnumInfo[] array = (from x in Helpers.GetEnums<HotkeyType>().OfType<Enum>()
			select new EnumInfo(x)).ToArray();
		foreach (ToolStripDropDown toolStripDropDown in parents)
		{
			EnumInfo[] array2 = array;
			foreach (EnumInfo enumInfo in array2)
			{
				HotkeyType hotkeyType = (HotkeyType)Enum.ToObject(typeof(HotkeyType), enumInfo.Value);
				string text;
				Image image;
				if (hotkeyType == HotkeyType.None)
				{
					text = Resources.ActionsToolbarEditForm_Separator;
					image = Resources.ui_splitter;
				}
				else
				{
					text = enumInfo.Description.Replace("&", "&&");
					image = TaskHelpers.FindMenuIcon(hotkeyType);
				}
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text);
				toolStripMenuItem.Image = image;
				toolStripMenuItem.Tag = enumInfo;
				toolStripMenuItem.Click += delegate
				{
					selectedEnum(hotkeyType);
				};
				if (!string.IsNullOrEmpty(enumInfo.Category))
				{
					ToolStripMenuItem toolStripMenuItem2 = toolStripDropDown.Items.OfType<ToolStripMenuItem>().FirstOrDefault((ToolStripMenuItem x) => x.Text == enumInfo.Category);
					if (toolStripMenuItem2 == null)
					{
						toolStripMenuItem2 = new ToolStripMenuItem(enumInfo.Category);
						toolStripDropDown.Items.Add(toolStripMenuItem2);
					}
					toolStripMenuItem2.DropDownItems.Add(toolStripMenuItem);
				}
				else
				{
					toolStripDropDown.Items.Add(toolStripMenuItem);
				}
			}
		}
	}

	private void AddAction(HotkeyType hotkeyType)
	{
		Actions.Add(hotkeyType);
		AddActionToList(hotkeyType);
	}

	private void AddActionToList(HotkeyType hotkeyType)
	{
		string text = ((hotkeyType != 0) ? hotkeyType.GetLocalizedDescription() : Resources.ActionsToolbarEditForm_Separator);
		ListViewItem value = new ListViewItem
		{
			Text = text,
			ImageKey = hotkeyType.ToString()
		};
		lvActions.Items.Add(value);
	}

	private void RemoveAction(int index)
	{
		Actions.RemoveAt(index);
		lvActions.Items.RemoveAt(index);
	}

	private void btnRemove_Click(object sender, EventArgs e)
	{
		if (lvActions.SelectedIndex >= 0)
		{
			RemoveAction(lvActions.SelectedIndex);
		}
	}

	private void lvActions_ItemMoved(object sender, int oldIndex, int newIndex)
	{
		Actions.Move(oldIndex, newIndex);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.ActionsToolbarEditForm));
		this.btnRemove = new System.Windows.Forms.Button();
		this.btnAdd = new ShareX.HelpersLib.MenuButton();
		this.cmsAction = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.lvActions = new ShareX.HelpersLib.MyListView();
		this.chAction = new System.Windows.Forms.ColumnHeader();
		this.ilMain = new System.Windows.Forms.ImageList(this.components);
		base.SuspendLayout();
		resources.ApplyResources(this.btnRemove, "btnRemove");
		this.btnRemove.Name = "btnRemove";
		this.btnRemove.UseVisualStyleBackColor = true;
		this.btnRemove.Click += new System.EventHandler(btnRemove_Click);
		resources.ApplyResources(this.btnAdd, "btnAdd");
		this.btnAdd.Menu = this.cmsAction;
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.cmsAction.Name = "cmsAction";
		resources.ApplyResources(this.cmsAction, "cmsAction");
		this.lvActions.AllowDrop = true;
		this.lvActions.AllowItemDrag = true;
		resources.ApplyResources(this.lvActions, "lvActions");
		this.lvActions.AutoFillColumn = true;
		this.lvActions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1] { this.chAction });
		this.lvActions.FullRowSelect = true;
		this.lvActions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
		this.lvActions.HideSelection = false;
		this.lvActions.MultiSelect = false;
		this.lvActions.Name = "lvActions";
		this.lvActions.SmallImageList = this.ilMain;
		this.lvActions.UseCompatibleStateImageBehavior = false;
		this.lvActions.View = System.Windows.Forms.View.Details;
		this.lvActions.ItemMoved += new ShareX.HelpersLib.MyListView.ListViewItemMovedEventHandler(lvActions_ItemMoved);
		resources.ApplyResources(this.chAction, "chAction");
		this.ilMain.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
		resources.ApplyResources(this.ilMain, "ilMain");
		this.ilMain.TransparentColor = System.Drawing.Color.Transparent;
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.lvActions);
		base.Controls.Add(this.btnAdd);
		base.Controls.Add(this.btnRemove);
		base.Name = "ActionsToolbarEditForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.ResumeLayout(false);
	}
}
