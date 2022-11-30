using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;

namespace ShareX;

public class FileExistForm : Form
{
	private string fileName;

	private string uniqueFilePath;

	private IContainer components;

	private Label lblTitle;

	private Button btnOverwrite;

	private Button btnCancel;

	private Button btnUniqueName;

	private Button btnNewName;

	private TextBox txtNewName;

	public string FilePath { get; private set; }

	public FileExistForm(string filePath)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		FilePath = filePath;
		fileName = Path.GetFileNameWithoutExtension(FilePath);
		txtNewName.Text = fileName;
		btnOverwrite.Text += Path.GetFileName(FilePath);
		uniqueFilePath = FileHelpers.GetUniqueFilePath(FilePath);
		btnUniqueName.Text += Path.GetFileName(uniqueFilePath);
	}

	private void FileExistForm_Shown(object sender, EventArgs e)
	{
		this.ForceActivate();
	}

	private string GetNewFileName()
	{
		string text = txtNewName.Text;
		if (!string.IsNullOrEmpty(text))
		{
			return text + Path.GetExtension(FilePath);
		}
		return "";
	}

	private void UseNewFileName()
	{
		string newFileName = GetNewFileName();
		if (!string.IsNullOrEmpty(newFileName))
		{
			FilePath = Path.Combine(Path.GetDirectoryName(FilePath), newFileName);
			Close();
		}
	}

	private void UseUniqueFileName()
	{
		FilePath = uniqueFilePath;
		Close();
	}

	private void Cancel()
	{
		FilePath = "";
		Close();
	}

	private void txtNewName_TextChanged(object sender, EventArgs e)
	{
		string text = txtNewName.Text;
		btnNewName.Enabled = !string.IsNullOrEmpty(text) && !text.Equals(fileName, StringComparison.InvariantCultureIgnoreCase);
		btnNewName.Text = Resources.FileExistForm_txtNewName_TextChanged_Use_new_name__ + GetNewFileName();
	}

	private void txtNewName_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return || e.KeyData == Keys.Escape)
		{
			e.SuppressKeyPress = true;
		}
	}

	private void txtNewName_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			string text = txtNewName.Text;
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
				{
					Close();
				}
				else
				{
					UseNewFileName();
				}
			}
		}
		else if (e.KeyData == Keys.Escape)
		{
			Cancel();
		}
	}

	private void btnNewName_Click(object sender, EventArgs e)
	{
		UseNewFileName();
	}

	private void btnOverwrite_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnUniqueName_Click(object sender, EventArgs e)
	{
		UseUniqueFileName();
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		Cancel();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.FileExistForm));
		this.lblTitle = new System.Windows.Forms.Label();
		this.btnOverwrite = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnUniqueName = new System.Windows.Forms.Button();
		this.btnNewName = new System.Windows.Forms.Button();
		this.txtNewName = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.lblTitle, "lblTitle");
		this.lblTitle.Name = "lblTitle";
		componentResourceManager.ApplyResources(this.btnOverwrite, "btnOverwrite");
		this.btnOverwrite.Name = "btnOverwrite";
		this.btnOverwrite.UseVisualStyleBackColor = true;
		this.btnOverwrite.Click += new System.EventHandler(btnOverwrite_Click);
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this.btnUniqueName, "btnUniqueName");
		this.btnUniqueName.Name = "btnUniqueName";
		this.btnUniqueName.UseVisualStyleBackColor = true;
		this.btnUniqueName.Click += new System.EventHandler(btnUniqueName_Click);
		componentResourceManager.ApplyResources(this.btnNewName, "btnNewName");
		this.btnNewName.Name = "btnNewName";
		this.btnNewName.UseVisualStyleBackColor = true;
		this.btnNewName.Click += new System.EventHandler(btnNewName_Click);
		componentResourceManager.ApplyResources(this.txtNewName, "txtNewName");
		this.txtNewName.Name = "txtNewName";
		this.txtNewName.TextChanged += new System.EventHandler(txtNewName_TextChanged);
		this.txtNewName.KeyDown += new System.Windows.Forms.KeyEventHandler(txtNewName_KeyDown);
		this.txtNewName.KeyUp += new System.Windows.Forms.KeyEventHandler(txtNewName_KeyUp);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.Controls.Add(this.txtNewName);
		base.Controls.Add(this.btnNewName);
		base.Controls.Add(this.btnUniqueName);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOverwrite);
		base.Controls.Add(this.lblTitle);
		base.MaximizeBox = false;
		base.Name = "FileExistForm";
		base.TopMost = true;
		base.Shown += new System.EventHandler(FileExistForm_Shown);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
