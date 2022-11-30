using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class HotkeySettingsForm : Form
{
	private HotkeyManager manager;

	private IContainer components;

	private Button btnDuplicate;

	private Button btnReset;

	private FlowLayoutPanel flpHotkeys;

	private Button btnEdit;

	private Button btnRemove;

	private Button btnAdd;

	private Button btnMoveUp;

	private Button btnMoveDown;

	private Button btnHotkeysDisabled;

	public HotkeySelectionControl Selected { get; private set; }

	public HotkeySettingsForm(HotkeyManager hotkeyManager)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		btnHotkeysDisabled.Visible = Program.Settings.DisableHotkeys;
		PrepareHotkeys(hotkeyManager);
	}

	private void HotkeySettingsForm_FormClosed(object sender, FormClosedEventArgs e)
	{
		if (manager != null)
		{
			manager.IgnoreHotkeys = false;
			HotkeyManager hotkeyManager = manager;
			hotkeyManager.HotkeysToggledTrigger = (HotkeyManager.HotkeysToggledEventHandler)Delegate.Remove(hotkeyManager.HotkeysToggledTrigger, new HotkeyManager.HotkeysToggledEventHandler(HandleHotkeysToggle));
		}
	}

	public void PrepareHotkeys(HotkeyManager hotkeyManager)
	{
		if (manager == null)
		{
			manager = hotkeyManager;
			HotkeyManager hotkeyManager2 = manager;
			hotkeyManager2.HotkeysToggledTrigger = (HotkeyManager.HotkeysToggledEventHandler)Delegate.Combine(hotkeyManager2.HotkeysToggledTrigger, new HotkeyManager.HotkeysToggledEventHandler(HandleHotkeysToggle));
			AddControls();
		}
	}

	private void AddControls()
	{
		flpHotkeys.Controls.Clear();
		foreach (HotkeySettings hotkey in manager.Hotkeys)
		{
			AddHotkeySelectionControl(hotkey);
		}
	}

	private void UpdateButtons()
	{
		Button button = btnEdit;
		Button button2 = btnRemove;
		bool flag2 = (btnDuplicate.Enabled = Selected != null);
		bool enabled = (button2.Enabled = flag2);
		button.Enabled = enabled;
		Button button3 = btnMoveUp;
		enabled = (btnMoveDown.Enabled = Selected != null && flpHotkeys.Controls.Count > 1);
		button3.Enabled = enabled;
	}

	private HotkeySelectionControl FindSelectionControl(HotkeySettings hotkeySetting)
	{
		return flpHotkeys.Controls.Cast<HotkeySelectionControl>().FirstOrDefault((HotkeySelectionControl hsc) => hsc.Setting == hotkeySetting);
	}

	private void control_SelectedChanged(object sender, EventArgs e)
	{
		Selected = (HotkeySelectionControl)sender;
		UpdateButtons();
		UpdateCheckStates();
	}

	private void UpdateCheckStates()
	{
		foreach (Control control in flpHotkeys.Controls)
		{
			((HotkeySelectionControl)control).Selected = Selected == control;
		}
	}

	private void UpdateHotkeyStatus()
	{
		foreach (Control control in flpHotkeys.Controls)
		{
			((HotkeySelectionControl)control).UpdateHotkeyStatus();
		}
	}

	private void RegisterFailedHotkeys()
	{
		foreach (HotkeySettings item in manager.Hotkeys.Where((HotkeySettings x) => x.HotkeyInfo.Status == HotkeyStatus.Failed))
		{
			manager.RegisterHotkey(item);
		}
		UpdateHotkeyStatus();
	}

	private void control_HotkeyChanged(object sender, EventArgs e)
	{
		HotkeySelectionControl hotkeySelectionControl = (HotkeySelectionControl)sender;
		manager.RegisterHotkey(hotkeySelectionControl.Setting);
		RegisterFailedHotkeys();
	}

	private HotkeySelectionControl AddHotkeySelectionControl(HotkeySettings hotkeySetting)
	{
		HotkeySelectionControl hotkeySelectionControl = new HotkeySelectionControl(hotkeySetting);
		hotkeySelectionControl.Margin = new Padding(0, 0, 0, 2);
		hotkeySelectionControl.SelectedChanged += control_SelectedChanged;
		hotkeySelectionControl.HotkeyChanged += control_HotkeyChanged;
		hotkeySelectionControl.EditRequested += control_EditRequested;
		flpHotkeys.Controls.Add(hotkeySelectionControl);
		return hotkeySelectionControl;
	}

	private void Edit(HotkeySelectionControl selectionControl)
	{
		using TaskSettingsForm taskSettingsForm = new TaskSettingsForm(selectionControl.Setting.TaskSettings);
		taskSettingsForm.ShowDialog();
		selectionControl.UpdateDescription();
	}

	private void control_EditRequested(object sender, EventArgs e)
	{
		Edit((HotkeySelectionControl)sender);
	}

	private void EditSelected()
	{
		if (Selected != null)
		{
			Edit(Selected);
		}
	}

	private void HandleHotkeysToggle(bool hotkeysEnabled)
	{
		UpdateHotkeyStatus();
	}

	private void flpHotkeys_Layout(object sender, LayoutEventArgs e)
	{
		foreach (Control control in flpHotkeys.Controls)
		{
			control.ClientSize = new Size(flpHotkeys.ClientSize.Width, control.ClientSize.Height);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		HotkeySettings hotkeySettings = new HotkeySettings();
		hotkeySettings.TaskSettings = TaskSettings.GetDefaultTaskSettings();
		manager.Hotkeys.Add(hotkeySettings);
		HotkeySelectionControl hotkeySelectionControl = AddHotkeySelectionControl(hotkeySettings);
		hotkeySelectionControl.Selected = true;
		Selected = hotkeySelectionControl;
		UpdateButtons();
		UpdateCheckStates();
		hotkeySelectionControl.Focus();
		Update();
		EditSelected();
	}

	private void btnRemove_Click(object sender, EventArgs e)
	{
		if (Selected != null)
		{
			manager.UnregisterHotkey(Selected.Setting);
			HotkeySelectionControl hotkeySelectionControl = FindSelectionControl(Selected.Setting);
			if (hotkeySelectionControl != null)
			{
				flpHotkeys.Controls.Remove(hotkeySelectionControl);
			}
			Selected = null;
			UpdateButtons();
		}
	}

	private void btnEdit_Click(object sender, EventArgs e)
	{
		EditSelected();
	}

	private void btnDuplicate_Click(object sender, EventArgs e)
	{
		if (Selected != null)
		{
			HotkeySettings hotkeySettings = new HotkeySettings();
			hotkeySettings.TaskSettings = Selected.Setting.TaskSettings.Copy();
			hotkeySettings.TaskSettings.WatchFolderEnabled = false;
			hotkeySettings.TaskSettings.WatchFolderList = new List<WatchFolderSettings>();
			manager.Hotkeys.Add(hotkeySettings);
			HotkeySelectionControl hotkeySelectionControl = AddHotkeySelectionControl(hotkeySettings);
			hotkeySelectionControl.Selected = true;
			Selected = hotkeySelectionControl;
			UpdateCheckStates();
			hotkeySelectionControl.Focus();
		}
	}

	private void btnMoveUp_Click(object sender, EventArgs e)
	{
		if (Selected != null && flpHotkeys.Controls.Count > 1)
		{
			int childIndex = flpHotkeys.Controls.GetChildIndex(Selected);
			int newIndex = ((childIndex != 0) ? (childIndex - 1) : (flpHotkeys.Controls.Count - 1));
			flpHotkeys.Controls.SetChildIndex(Selected, newIndex);
			manager.Hotkeys.Move(childIndex, newIndex);
		}
	}

	private void btnMoveDown_Click(object sender, EventArgs e)
	{
		if (Selected != null && flpHotkeys.Controls.Count > 1)
		{
			int childIndex = flpHotkeys.Controls.GetChildIndex(Selected);
			int newIndex = ((childIndex != flpHotkeys.Controls.Count - 1) ? (childIndex + 1) : 0);
			flpHotkeys.Controls.SetChildIndex(Selected, newIndex);
			manager.Hotkeys.Move(childIndex, newIndex);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show(Resources.HotkeySettingsForm_Reset_all_hotkeys_to_defaults_Confirmation, "ShareX", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			manager.ResetHotkeys();
			AddControls();
			Selected = null;
			UpdateButtons();
		}
	}

	private void btnHotkeysDisabled_Click(object sender, EventArgs e)
	{
		TaskHelpers.ToggleHotkeys(disableHotkeys: false);
		btnHotkeysDisabled.Visible = false;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.HotkeySettingsForm));
		this.btnDuplicate = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.flpHotkeys = new System.Windows.Forms.FlowLayoutPanel();
		this.btnEdit = new System.Windows.Forms.Button();
		this.btnRemove = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnMoveUp = new System.Windows.Forms.Button();
		this.btnMoveDown = new System.Windows.Forms.Button();
		this.btnHotkeysDisabled = new System.Windows.Forms.Button();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnDuplicate, "btnDuplicate");
		this.btnDuplicate.Name = "btnDuplicate";
		this.btnDuplicate.UseVisualStyleBackColor = true;
		this.btnDuplicate.Click += new System.EventHandler(btnDuplicate_Click);
		componentResourceManager.ApplyResources(this.btnReset, "btnReset");
		this.btnReset.Name = "btnReset";
		this.btnReset.UseVisualStyleBackColor = true;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		componentResourceManager.ApplyResources(this.flpHotkeys, "flpHotkeys");
		this.flpHotkeys.Name = "flpHotkeys";
		this.flpHotkeys.Layout += new System.Windows.Forms.LayoutEventHandler(flpHotkeys_Layout);
		componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.UseVisualStyleBackColor = true;
		this.btnEdit.Click += new System.EventHandler(btnEdit_Click);
		componentResourceManager.ApplyResources(this.btnRemove, "btnRemove");
		this.btnRemove.Name = "btnRemove";
		this.btnRemove.UseVisualStyleBackColor = true;
		this.btnRemove.Click += new System.EventHandler(btnRemove_Click);
		componentResourceManager.ApplyResources(this.btnAdd, "btnAdd");
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		componentResourceManager.ApplyResources(this.btnMoveUp, "btnMoveUp");
		this.btnMoveUp.Name = "btnMoveUp";
		this.btnMoveUp.UseVisualStyleBackColor = true;
		this.btnMoveUp.Click += new System.EventHandler(btnMoveUp_Click);
		componentResourceManager.ApplyResources(this.btnMoveDown, "btnMoveDown");
		this.btnMoveDown.Name = "btnMoveDown";
		this.btnMoveDown.UseVisualStyleBackColor = true;
		this.btnMoveDown.Click += new System.EventHandler(btnMoveDown_Click);
		componentResourceManager.ApplyResources(this.btnHotkeysDisabled, "btnHotkeysDisabled");
		this.btnHotkeysDisabled.Name = "btnHotkeysDisabled";
		this.btnHotkeysDisabled.UseVisualStyleBackColor = true;
		this.btnHotkeysDisabled.Click += new System.EventHandler(btnHotkeysDisabled_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.btnHotkeysDisabled);
		base.Controls.Add(this.btnMoveDown);
		base.Controls.Add(this.btnMoveUp);
		base.Controls.Add(this.btnDuplicate);
		base.Controls.Add(this.btnReset);
		base.Controls.Add(this.flpHotkeys);
		base.Controls.Add(this.btnEdit);
		base.Controls.Add(this.btnRemove);
		base.Controls.Add(this.btnAdd);
		base.Name = "HotkeySettingsForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(HotkeySettingsForm_FormClosed);
		base.ResumeLayout(false);
	}
}
