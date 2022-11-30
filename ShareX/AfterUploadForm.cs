using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.UploadersLib;

namespace ShareX;

public class AfterUploadForm : Form
{
	private UploadInfoParser parser = new UploadInfoParser();

	private ListViewGroup lvgForums = new ListViewGroup("Forums");

	private ListViewGroup lvgHtml = new ListViewGroup("HTML");

	private ListViewGroup lvgWiki = new ListViewGroup("Wiki");

	private ListViewGroup lvgLocal = new ListViewGroup("Local");

	private ListViewGroup lvgCustom = new ListViewGroup("Custom");

	private IContainer components;

	private MyPictureBox pbPreview;

	private Timer tmrClose;

	private Button btnOpenFolder;

	private Button btnCopyImage;

	private Button btnOpenLink;

	private Button btnCopyLink;

	private Button btnOpenFile;

	private Button btnClose;

	private MyListView lvClipboardFormats;

	private ColumnHeader chDescription;

	private ColumnHeader chFormat;

	public TaskInfo Info { get; private set; }

	public AfterUploadForm(TaskInfo info)
	{
		InitializeComponent();
		ShareXResources.ApplyTheme(this);
		Info = info;
		if (Info.TaskSettings.AdvancedSettings.AutoCloseAfterUploadForm)
		{
			tmrClose.Start();
		}
		bool flag = !string.IsNullOrEmpty(info.FilePath) && File.Exists(info.FilePath);
		if (info.DataType == EDataType.Image)
		{
			if (flag)
			{
				pbPreview.LoadImageFromFileAsync(info.FilePath);
			}
			else
			{
				pbPreview.LoadImageFromURLAsync(info.Result.URL);
			}
		}
		Text = "ShareX - " + (flag ? info.FilePath : info.FileName);
		lvClipboardFormats.Groups.Add(lvgForums);
		lvClipboardFormats.Groups.Add(lvgHtml);
		lvClipboardFormats.Groups.Add(lvgWiki);
		lvClipboardFormats.Groups.Add(lvgLocal);
		lvClipboardFormats.Groups.Add(lvgCustom);
		LinkFormatEnum[] enums = Helpers.GetEnums<LinkFormatEnum>();
		foreach (LinkFormatEnum linkFormatEnum in enums)
		{
			if (FileHelpers.IsImageFile(Info.Result.URL) || (linkFormatEnum != LinkFormatEnum.HTMLImage && linkFormatEnum != LinkFormatEnum.HTMLLinkedImage && linkFormatEnum != LinkFormatEnum.ForumImage && linkFormatEnum != LinkFormatEnum.ForumLinkedImage && linkFormatEnum != LinkFormatEnum.WikiImage && linkFormatEnum != LinkFormatEnum.WikiLinkedImage))
			{
				AddFormat(linkFormatEnum.GetLocalizedDescription(), GetUrlByType(linkFormatEnum));
			}
		}
		if (!FileHelpers.IsImageFile(Info.Result.URL))
		{
			return;
		}
		foreach (ClipboardFormat clipboardContentFormat in Program.Settings.ClipboardContentFormats)
		{
			AddFormat(clipboardContentFormat.Description, parser.Parse(Info, clipboardContentFormat.Format), lvgCustom);
		}
	}

	private void AddFormat(string description, string text, ListViewGroup group = null)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		ListViewItem listViewItem = new ListViewItem(description);
		if (group == null)
		{
			if (description.Contains("HTML"))
			{
				listViewItem.Group = lvgHtml;
			}
			else if (description.Contains("Forums"))
			{
				listViewItem.Group = lvgForums;
			}
			else if (description.Contains("Local"))
			{
				listViewItem.Group = lvgLocal;
			}
			else if (description.Contains("Wiki"))
			{
				listViewItem.Group = lvgWiki;
			}
		}
		else
		{
			listViewItem.Group = group;
		}
		listViewItem.SubItems.Add(text);
		lvClipboardFormats.Items.Add(listViewItem);
		lvClipboardFormats.FillLastColumn();
	}

	private void tmrClose_Tick(object sender, EventArgs e)
	{
		Close();
	}

	private void btnCopyImage_Click(object sender, EventArgs e)
	{
		if (!string.IsNullOrEmpty(Info.FilePath) && FileHelpers.IsImageFile(Info.FilePath) && File.Exists(Info.FilePath))
		{
			ClipboardHelpers.CopyImageFromFile(Info.FilePath);
		}
	}

	private void btnCopyLink_Click(object sender, EventArgs e)
	{
		if (lvClipboardFormats.Items.Count > 0)
		{
			string value = ((lvClipboardFormats.SelectedItems.Count != 0) ? lvClipboardFormats.SelectedItems[0].SubItems[1].Text : lvClipboardFormats.Items[0].SubItems[1].Text);
			if (!string.IsNullOrEmpty(value))
			{
				ClipboardHelpers.CopyText(value);
			}
		}
	}

	private void btnOpenLink_Click(object sender, EventArgs e)
	{
		string uRL = Info.Result.URL;
		if (!string.IsNullOrEmpty(uRL))
		{
			URLHelpers.OpenURL(uRL);
		}
	}

	private void btnOpenFile_Click(object sender, EventArgs e)
	{
		FileHelpers.OpenFile(Info.FilePath);
	}

	private void btnFolderOpen_Click(object sender, EventArgs e)
	{
		FileHelpers.OpenFolderWithFile(Info.FilePath);
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	public string GetUrlByType(LinkFormatEnum type)
	{
		return type switch
		{
			LinkFormatEnum.URL => Info.Result.URL, 
			LinkFormatEnum.ShortenedURL => Info.Result.ShortenedURL, 
			LinkFormatEnum.ForumImage => parser.Parse(Info, "[img]$url[/img]"), 
			LinkFormatEnum.HTMLImage => parser.Parse(Info, "<img src=\"$url\">"), 
			LinkFormatEnum.WikiImage => parser.Parse(Info, "[$url]"), 
			LinkFormatEnum.ForumLinkedImage => parser.Parse(Info, "[url=$url][img]$thumbnailurl[/img][/url]"), 
			LinkFormatEnum.HTMLLinkedImage => parser.Parse(Info, "<a href=\"$url\"><img src=\"$thumbnailurl\"></a>"), 
			LinkFormatEnum.WikiLinkedImage => parser.Parse(Info, "[$url $thumbnailurl]"), 
			LinkFormatEnum.ThumbnailURL => Info.Result.ThumbnailURL, 
			LinkFormatEnum.LocalFilePath => Info.FilePath, 
			LinkFormatEnum.LocalFilePathUri => GetLocalFilePathAsUri(Info.FilePath), 
			_ => Info.Result.URL, 
		};
	}

	public string GetLocalFilePathAsUri(string fp)
	{
		if (!string.IsNullOrEmpty(fp) && File.Exists(fp))
		{
			try
			{
				return new Uri(fp).AbsoluteUri;
			}
			catch (Exception exception)
			{
				DebugHelper.WriteException(exception);
			}
		}
		return "";
	}

	private void lvClipboardFormats_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left && lvClipboardFormats.SelectedItems.Count > 0)
		{
			string value = lvClipboardFormats.SelectedItems[0].SubItems[1].Text;
			if (!string.IsNullOrEmpty(value))
			{
				ClipboardHelpers.CopyText(value);
			}
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShareX.AfterUploadForm));
		this.pbPreview = new ShareX.HelpersLib.MyPictureBox();
		this.btnCopyImage = new System.Windows.Forms.Button();
		this.btnCopyLink = new System.Windows.Forms.Button();
		this.btnOpenLink = new System.Windows.Forms.Button();
		this.btnOpenFile = new System.Windows.Forms.Button();
		this.btnOpenFolder = new System.Windows.Forms.Button();
		this.tmrClose = new System.Windows.Forms.Timer(this.components);
		this.btnClose = new System.Windows.Forms.Button();
		this.lvClipboardFormats = new ShareX.HelpersLib.MyListView();
		this.chDescription = new System.Windows.Forms.ColumnHeader();
		this.chFormat = new System.Windows.Forms.ColumnHeader();
		base.SuspendLayout();
		resources.ApplyResources(this.pbPreview, "pbPreview");
		this.pbPreview.BackColor = System.Drawing.SystemColors.Window;
		this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pbPreview.DrawCheckeredBackground = true;
		this.pbPreview.EnableRightClickMenu = true;
		this.pbPreview.FullscreenOnClick = true;
		this.pbPreview.Name = "pbPreview";
		this.pbPreview.PictureBoxBackColor = System.Drawing.SystemColors.Window;
		this.pbPreview.ShowImageSizeLabel = true;
		resources.ApplyResources(this.btnCopyImage, "btnCopyImage");
		this.btnCopyImage.Name = "btnCopyImage";
		this.btnCopyImage.UseVisualStyleBackColor = true;
		this.btnCopyImage.Click += new System.EventHandler(btnCopyImage_Click);
		resources.ApplyResources(this.btnCopyLink, "btnCopyLink");
		this.btnCopyLink.Name = "btnCopyLink";
		this.btnCopyLink.UseVisualStyleBackColor = true;
		this.btnCopyLink.Click += new System.EventHandler(btnCopyLink_Click);
		resources.ApplyResources(this.btnOpenLink, "btnOpenLink");
		this.btnOpenLink.Name = "btnOpenLink";
		this.btnOpenLink.UseVisualStyleBackColor = true;
		this.btnOpenLink.Click += new System.EventHandler(btnOpenLink_Click);
		resources.ApplyResources(this.btnOpenFile, "btnOpenFile");
		this.btnOpenFile.Name = "btnOpenFile";
		this.btnOpenFile.UseVisualStyleBackColor = true;
		this.btnOpenFile.Click += new System.EventHandler(btnOpenFile_Click);
		resources.ApplyResources(this.btnOpenFolder, "btnOpenFolder");
		this.btnOpenFolder.Name = "btnOpenFolder";
		this.btnOpenFolder.UseVisualStyleBackColor = true;
		this.btnOpenFolder.Click += new System.EventHandler(btnFolderOpen_Click);
		this.tmrClose.Interval = 60000;
		this.tmrClose.Tick += new System.EventHandler(tmrClose_Tick);
		resources.ApplyResources(this.btnClose, "btnClose");
		this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnClose.Name = "btnClose";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		resources.ApplyResources(this.lvClipboardFormats, "lvClipboardFormats");
		this.lvClipboardFormats.AutoFillColumn = true;
		this.lvClipboardFormats.AutoFillColumnIndex = 1;
		this.lvClipboardFormats.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.chDescription, this.chFormat });
		this.lvClipboardFormats.FullRowSelect = true;
		this.lvClipboardFormats.GridLines = true;
		this.lvClipboardFormats.HideSelection = false;
		this.lvClipboardFormats.Name = "lvClipboardFormats";
		this.lvClipboardFormats.UseCompatibleStateImageBehavior = false;
		this.lvClipboardFormats.View = System.Windows.Forms.View.Details;
		this.lvClipboardFormats.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(lvClipboardFormats_MouseDoubleClick);
		resources.ApplyResources(this.chDescription, "chDescription");
		resources.ApplyResources(this.chFormat, "chFormat");
		resources.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.Window;
		base.CancelButton = this.btnClose;
		base.Controls.Add(this.lvClipboardFormats);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.btnOpenLink);
		base.Controls.Add(this.btnOpenFile);
		base.Controls.Add(this.btnOpenFolder);
		base.Controls.Add(this.btnCopyLink);
		base.Controls.Add(this.btnCopyImage);
		base.Controls.Add(this.pbPreview);
		base.Name = "AfterUploadForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
