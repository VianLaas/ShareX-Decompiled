using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class HotkeySelectionControl : UserControl
{
	private bool selected;

	private bool descriptionHover;

	private IContainer components;

	private ImageLabel lblHotkeyDescription;

	private ColorButton btnHotkey;

	private Button btnEdit;

	public HotkeySettings Setting { get; set; }

	public bool Selected
	{
		get
		{
			return selected;
		}
		set
		{
			selected = value;
			UpdateTheme();
		}
	}

	public bool EditingHotkey { get; private set; }

	public event EventHandler HotkeyChanged;

	public event EventHandler SelectedChanged;

	public event EventHandler EditRequested;

	public HotkeySelectionControl(HotkeySettings setting)
	{
		Setting = setting;
		InitializeComponent();
		UpdateDescription();
		UpdateHotkeyText();
		if (ShareXResources.UseCustomTheme)
		{
			ShareXResources.ApplyCustomThemeToControl(this);
		}
		UpdateHotkeyStatus();
		UpdateTheme();
	}

	public void UpdateTheme()
	{
		if (ShareXResources.UseCustomTheme)
		{
			if (Selected)
			{
				lblHotkeyDescription.ForeColor = SystemColors.ControlText;
				lblHotkeyDescription.BackColor = Color.FromArgb(200, 255, 200);
			}
			else if (descriptionHover)
			{
				lblHotkeyDescription.ForeColor = SystemColors.ControlText;
				lblHotkeyDescription.BackColor = Color.FromArgb(220, 240, 255);
			}
			else
			{
				lblHotkeyDescription.ForeColor = ShareXResources.Theme.TextColor;
				lblHotkeyDescription.BackColor = ShareXResources.Theme.LightBackgroundColor;
			}
			btnHotkey.BorderColor = ShareXResources.Theme.BorderColor;
			if (EditingHotkey)
			{
				btnHotkey.ForeColor = SystemColors.ControlText;
				btnHotkey.BackColor = Color.FromArgb(225, 255, 225);
			}
			else
			{
				btnHotkey.ForeColor = ShareXResources.Theme.TextColor;
				btnHotkey.BackColor = ShareXResources.Theme.LightBackgroundColor;
			}
		}
		else
		{
			lblHotkeyDescription.ForeColor = SystemColors.ControlText;
			if (Selected)
			{
				lblHotkeyDescription.BackColor = Color.FromArgb(200, 255, 200);
			}
			else if (descriptionHover)
			{
				lblHotkeyDescription.BackColor = Color.FromArgb(220, 240, 255);
			}
			else
			{
				lblHotkeyDescription.BackColor = SystemColors.Window;
			}
			btnHotkey.ForeColor = SystemColors.ControlText;
			if (EditingHotkey)
			{
				btnHotkey.BackColor = Color.FromArgb(225, 255, 225);
				return;
			}
			btnHotkey.BackColor = SystemColors.Control;
			btnHotkey.UseVisualStyleBackColor = true;
		}
	}

	public void UpdateDescription()
	{
		if (Setting.TaskSettings.IsUsingDefaultSettings)
		{
			lblHotkeyDescription.ChangeFontStyle(FontStyle.Regular);
		}
		else
		{
			lblHotkeyDescription.ChangeFontStyle(FontStyle.Bold);
		}
		lblHotkeyDescription.Image = TaskHelpers.FindMenuIcon(Setting.TaskSettings.Job);
		lblHotkeyDescription.Text = Setting.TaskSettings.ToString();
	}

	private void UpdateHotkeyText()
	{
		btnHotkey.Text = Setting.HotkeyInfo.ToString();
	}

	public void UpdateHotkeyStatus()
	{
		switch (Setting.HotkeyInfo.Status)
		{
		default:
			btnHotkey.Color = Color.LightGoldenrodYellow;
			break;
		case HotkeyStatus.Failed:
			btnHotkey.Color = Color.IndianRed;
			break;
		case HotkeyStatus.Registered:
			btnHotkey.Color = Color.PaleGreen;
			break;
		}
	}

	private void btnHotkey_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		if (EditingHotkey)
		{
			e.IsInputKey = true;
		}
	}

	private void btnHotkey_KeyDown(object sender, KeyEventArgs e)
	{
		e.SuppressKeyPress = true;
		if (EditingHotkey)
		{
			if (e.KeyData == Keys.Escape)
			{
				Setting.HotkeyInfo.Hotkey = Keys.None;
				StopEditing();
			}
			else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
			{
				Setting.HotkeyInfo.Win = !Setting.HotkeyInfo.Win;
				UpdateHotkeyText();
			}
			else if (new HotkeyInfo(e.KeyData).IsValidHotkey)
			{
				Setting.HotkeyInfo.Hotkey = e.KeyData;
				StopEditing();
			}
			else
			{
				Setting.HotkeyInfo.Hotkey = e.KeyData;
				UpdateHotkeyText();
			}
		}
	}

	private void btnHotkey_KeyUp(object sender, KeyEventArgs e)
	{
		e.SuppressKeyPress = true;
		if (EditingHotkey && e.KeyCode == Keys.Snapshot)
		{
			Setting.HotkeyInfo.Hotkey = e.KeyData;
			StopEditing();
		}
	}

	private void btnHotkey_MouseClick(object sender, MouseEventArgs e)
	{
		if (EditingHotkey)
		{
			StopEditing();
		}
		else
		{
			StartEditing();
		}
	}

	private void btnHotkey_Leave(object sender, EventArgs e)
	{
		if (EditingHotkey)
		{
			StopEditing();
		}
	}

	private void StartEditing()
	{
		EditingHotkey = true;
		Program.HotkeyManager.IgnoreHotkeys = true;
		btnHotkey.Text = Resources.HotkeySelectionControl_StartEditing_Select_a_hotkey___;
		UpdateTheme();
		Setting.HotkeyInfo.Hotkey = Keys.None;
		Setting.HotkeyInfo.Win = false;
		OnHotkeyChanged();
		UpdateHotkeyStatus();
	}

	private void StopEditing()
	{
		EditingHotkey = false;
		Program.HotkeyManager.IgnoreHotkeys = false;
		if (Setting.HotkeyInfo.IsOnlyModifiers)
		{
			Setting.HotkeyInfo.Hotkey = Keys.None;
		}
		UpdateTheme();
		OnHotkeyChanged();
		UpdateHotkeyStatus();
		UpdateHotkeyText();
	}

	protected void OnHotkeyChanged()
	{
		this.HotkeyChanged?.Invoke(this, EventArgs.Empty);
	}

	protected void OnSelectedChanged()
	{
		this.SelectedChanged?.Invoke(this, EventArgs.Empty);
	}

	protected void OnEditRequested()
	{
		this.EditRequested?.Invoke(this, EventArgs.Empty);
	}

	private void btnEdit_Click(object sender, EventArgs e)
	{
		OnEditRequested();
	}

	private void lblHotkeyDescription_MouseEnter(object sender, EventArgs e)
	{
		if (!Selected)
		{
			descriptionHover = true;
			UpdateTheme();
		}
	}

	private void lblHotkeyDescription_MouseLeave(object sender, EventArgs e)
	{
		descriptionHover = false;
		UpdateTheme();
	}

	private void lblHotkeyDescription_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			Selected = true;
			OnSelectedChanged();
			Focus();
		}
	}

	private void lblHotkeyDescription_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			OnEditRequested();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.HotkeySelectionControl));
		this.btnEdit = new System.Windows.Forms.Button();
		this.btnHotkey = new ShareX.HelpersLib.ColorButton();
		this.lblHotkeyDescription = new ShareX.HelpersLib.ImageLabel();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnEdit, "btnEdit");
		this.btnEdit.Image = ShareX.Properties.Resources.gear;
		this.btnEdit.Name = "btnEdit";
		this.btnEdit.UseVisualStyleBackColor = true;
		this.btnEdit.Click += new System.EventHandler(btnEdit_Click);
		componentResourceManager.ApplyResources(this.btnHotkey, "btnHotkey");
		this.btnHotkey.Color = System.Drawing.Color.Empty;
		this.btnHotkey.ColorPickerOptions = null;
		this.btnHotkey.ManualButtonClick = true;
		this.btnHotkey.Name = "btnHotkey";
		this.btnHotkey.Offset = 0;
		this.btnHotkey.UseVisualStyleBackColor = true;
		this.btnHotkey.KeyDown += new System.Windows.Forms.KeyEventHandler(btnHotkey_KeyDown);
		this.btnHotkey.KeyUp += new System.Windows.Forms.KeyEventHandler(btnHotkey_KeyUp);
		this.btnHotkey.Leave += new System.EventHandler(btnHotkey_Leave);
		this.btnHotkey.MouseClick += new System.Windows.Forms.MouseEventHandler(btnHotkey_MouseClick);
		this.btnHotkey.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnHotkey_PreviewKeyDown);
		componentResourceManager.ApplyResources(this.lblHotkeyDescription, "lblHotkeyDescription");
		this.lblHotkeyDescription.BackColor = System.Drawing.SystemColors.Window;
		this.lblHotkeyDescription.Name = "lblHotkeyDescription";
		this.lblHotkeyDescription.UseMnemonic = false;
		this.lblHotkeyDescription.MouseClick += new System.Windows.Forms.MouseEventHandler(lblHotkeyDescription_MouseClick);
		this.lblHotkeyDescription.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(lblHotkeyDescription_MouseDoubleClick);
		this.lblHotkeyDescription.MouseEnter += new System.EventHandler(lblHotkeyDescription_MouseEnter);
		this.lblHotkeyDescription.MouseLeave += new System.EventHandler(lblHotkeyDescription_MouseLeave);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.Controls.Add(this.btnEdit);
		base.Controls.Add(this.btnHotkey);
		base.Controls.Add(this.lblHotkeyDescription);
		base.Name = "HotkeySelectionControl";
		base.ResumeLayout(false);
	}
}
