using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ShareX.HelpersLib;

namespace ShareX;

public class ServiceLinksForm : Form
{
	private IContainer components;

	private ComboBox cbServices;

	private Label lblServices;

	private Button btnNew;

	private Button btnRemove;

	private Label lblName;

	private TextBox txtName;

	private Label lblURL;

	private TextBox txtURL;

	private Button btnReset;

	public List<ServiceLink> ServiceLinks { get; private set; }

	public ServiceLinksForm(List<ServiceLink> serviceLinks)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		ServiceLinks = serviceLinks;
		if (ServiceLinks != null && ServiceLinks.Count > 0)
		{
			ComboBox.ObjectCollection items = cbServices.Items;
			object[] items2 = ServiceLinks.ToArray();
			items.AddRange(items2);
			cbServices.SelectedIndex = 0;
		}
		UpdateControls();
	}

	private void UpdateControls()
	{
		Button button = btnRemove;
		ComboBox comboBox = cbServices;
		TextBox textBox = txtName;
		bool flag2 = (txtURL.Enabled = cbServices.SelectedItem != null);
		bool flag4 = (textBox.Enabled = flag2);
		bool enabled = (comboBox.Enabled = flag4);
		button.Enabled = enabled;
	}

	private void cbServices_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (cbServices.SelectedItem is ServiceLink serviceLink)
		{
			txtName.Text = serviceLink.Name;
			txtURL.Text = serviceLink.URL;
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		ServiceLink item = new ServiceLink("Name", "https://example.com/search?q={0}");
		ServiceLinks.Add(item);
		cbServices.Items.Add(item);
		cbServices.SelectedIndex = cbServices.Items.Count - 1;
		UpdateControls();
	}

	private void btnRemove_Click(object sender, EventArgs e)
	{
		if (cbServices.SelectedItem is ServiceLink serviceLink)
		{
			ServiceLinks.Remove(serviceLink);
			cbServices.Items.Remove(serviceLink);
			cbServices.SelectedIndex = cbServices.Items.Count - 1;
			UpdateControls();
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		ServiceLinks.Clear();
		ServiceLinks.AddRange(OCROptions.DefaultServiceLinks);
		cbServices.Items.Clear();
		ComboBox.ObjectCollection items = cbServices.Items;
		object[] items2 = ServiceLinks.ToArray();
		items.AddRange(items2);
		cbServices.SelectedIndex = 0;
		UpdateControls();
	}

	private void txtName_TextChanged(object sender, EventArgs e)
	{
		if (cbServices.SelectedItem is ServiceLink serviceLink)
		{
			serviceLink.Name = txtName.Text;
			int selectedIndex = cbServices.SelectedIndex;
			cbServices.Items[selectedIndex] = cbServices.Items[selectedIndex];
		}
	}

	private void txtURL_TextChanged(object sender, EventArgs e)
	{
		if (cbServices.SelectedItem is ServiceLink serviceLink)
		{
			serviceLink.URL = txtURL.Text;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.ServiceLinksForm));
		this.cbServices = new System.Windows.Forms.ComboBox();
		this.lblServices = new System.Windows.Forms.Label();
		this.btnNew = new System.Windows.Forms.Button();
		this.btnRemove = new System.Windows.Forms.Button();
		this.lblName = new System.Windows.Forms.Label();
		this.txtName = new System.Windows.Forms.TextBox();
		this.lblURL = new System.Windows.Forms.Label();
		this.txtURL = new System.Windows.Forms.TextBox();
		this.btnReset = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.cbServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbServices.FormattingEnabled = true;
		componentResourceManager.ApplyResources(this.cbServices, "cbServices");
		this.cbServices.Name = "cbServices";
		this.cbServices.SelectedIndexChanged += new System.EventHandler(cbServices_SelectedIndexChanged);
		componentResourceManager.ApplyResources(this.lblServices, "lblServices");
		this.lblServices.Name = "lblServices";
		componentResourceManager.ApplyResources(this.btnNew, "btnNew");
		this.btnNew.Name = "btnNew";
		this.btnNew.UseVisualStyleBackColor = true;
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		componentResourceManager.ApplyResources(this.btnRemove, "btnRemove");
		this.btnRemove.Name = "btnRemove";
		this.btnRemove.UseVisualStyleBackColor = true;
		this.btnRemove.Click += new System.EventHandler(btnRemove_Click);
		componentResourceManager.ApplyResources(this.lblName, "lblName");
		this.lblName.Name = "lblName";
		componentResourceManager.ApplyResources(this.txtName, "txtName");
		this.txtName.Name = "txtName";
		this.txtName.TextChanged += new System.EventHandler(txtName_TextChanged);
		componentResourceManager.ApplyResources(this.lblURL, "lblURL");
		this.lblURL.Name = "lblURL";
		componentResourceManager.ApplyResources(this.txtURL, "txtURL");
		this.txtURL.Name = "txtURL";
		this.txtURL.TextChanged += new System.EventHandler(txtURL_TextChanged);
		componentResourceManager.ApplyResources(this.btnReset, "btnReset");
		this.btnReset.Name = "btnReset";
		this.btnReset.UseVisualStyleBackColor = true;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.btnReset);
		base.Controls.Add(this.txtURL);
		base.Controls.Add(this.lblURL);
		base.Controls.Add(this.txtName);
		base.Controls.Add(this.lblName);
		base.Controls.Add(this.btnRemove);
		base.Controls.Add(this.btnNew);
		base.Controls.Add(this.lblServices);
		base.Controls.Add(this.cbServices);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "ServiceLinksForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
