using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShareX.HelpersLib;
using ShareX.Properties;
using ShareX.UploadersLib;

namespace ShareX;

public class BeforeUploadControl : UserControl
{
	public delegate void EventHandler(string currentDestination);

	private IContainer components;

	private FlowLayoutPanel flp;

	public event EventHandler InitCompleted;

	public BeforeUploadControl()
	{
		InitializeComponent();
	}

	public void Init(TaskInfo info)
	{
		switch (info.DataType)
		{
		case EDataType.Image:
			InitCapture(info.TaskSettings);
			break;
		case EDataType.Text:
			Helpers.GetEnums<TextDestination>().ForEach(delegate(TextDestination x)
			{
				if (x != TextDestination.FileUploader)
				{
					string overrideText2 = null;
					if (x == TextDestination.CustomTextUploader)
					{
						overrideText2 = GetCustomUploaderName(Program.UploadersConfig.CustomTextUploaderSelected, info.TaskSettings);
					}
					AddDestination<TextDestination>((int)x, EDataType.Text, info.TaskSettings, overrideText2);
				}
			});
			Helpers.GetEnums<FileDestination>().ForEach(delegate(FileDestination x)
			{
				string overrideText = null;
				if (x == FileDestination.CustomFileUploader)
				{
					overrideText = GetCustomUploaderName(Program.UploadersConfig.CustomFileUploaderSelected, info.TaskSettings);
				}
				AddDestination<FileDestination>((int)x, EDataType.Text, info.TaskSettings, overrideText);
			});
			flp.Controls.OfType<RadioButton>().ForEach(delegate(RadioButton x)
			{
				if (info.TaskSettings.TextDestination != TextDestination.FileUploader)
				{
					x.Checked = x.Tag is TextDestination textDestination && textDestination == info.TaskSettings.TextDestination;
				}
				else
				{
					x.Checked = x.Tag is FileDestination fileDestination && fileDestination == info.TaskSettings.TextFileDestination;
				}
			});
			break;
		case EDataType.File:
			Helpers.GetEnums<FileDestination>().ForEach(delegate(FileDestination x)
			{
				string overrideText3 = null;
				if (x == FileDestination.CustomFileUploader)
				{
					overrideText3 = GetCustomUploaderName(Program.UploadersConfig.CustomFileUploaderSelected, info.TaskSettings);
				}
				AddDestination<FileDestination>((int)x, EDataType.File, info.TaskSettings, overrideText3);
			});
			flp.Controls.OfType<RadioButton>().ForEach(delegate(RadioButton x)
			{
				x.Checked = x.Tag is FileDestination fileDestination2 && fileDestination2 == info.TaskSettings.FileDestination;
			});
			break;
		case EDataType.URL:
			Helpers.GetEnums<UrlShortenerType>().ForEach(delegate(UrlShortenerType x)
			{
				string overrideText4 = null;
				if (x == UrlShortenerType.CustomURLShortener)
				{
					overrideText4 = GetCustomUploaderName(Program.UploadersConfig.CustomURLShortenerSelected, info.TaskSettings);
				}
				AddDestination<UrlShortenerType>((int)x, EDataType.URL, info.TaskSettings, overrideText4);
			});
			flp.Controls.OfType<RadioButton>().ForEach(delegate(RadioButton x)
			{
				x.Checked = x.Tag is UrlShortenerType urlShortenerType && urlShortenerType == info.TaskSettings.URLShortenerDestination;
			});
			break;
		}
		OnInitCompleted();
	}

	public void InitCapture(TaskSettings taskSettings)
	{
		Helpers.GetEnums<ImageDestination>().ForEach(delegate(ImageDestination x)
		{
			if (x != ImageDestination.FileUploader)
			{
				string overrideText2 = null;
				if (x == ImageDestination.CustomImageUploader)
				{
					overrideText2 = GetCustomUploaderName(Program.UploadersConfig.CustomImageUploaderSelected, taskSettings);
				}
				AddDestination<ImageDestination>((int)x, EDataType.Image, taskSettings, overrideText2);
			}
		});
		Helpers.GetEnums<FileDestination>().ForEach(delegate(FileDestination x)
		{
			string overrideText = null;
			if (x == FileDestination.CustomFileUploader)
			{
				overrideText = GetCustomUploaderName(Program.UploadersConfig.CustomFileUploaderSelected, taskSettings);
			}
			AddDestination<FileDestination>((int)x, EDataType.File, taskSettings, overrideText);
		});
		flp.Controls.OfType<RadioButton>().ForEach(delegate(RadioButton x)
		{
			if (taskSettings.ImageDestination != ImageDestination.FileUploader)
			{
				x.Checked = x.Tag is ImageDestination imageDestination && imageDestination == taskSettings.ImageDestination;
			}
			else
			{
				x.Checked = x.Tag is FileDestination fileDestination && fileDestination == taskSettings.ImageFileDestination;
			}
		});
	}

	private void OnInitCompleted()
	{
		if (this.InitCompleted != null)
		{
			RadioButton radioButton = flp.Controls.OfType<RadioButton>().FirstOrDefault((RadioButton x) => x.Checked);
			string currentDestination = "";
			if (radioButton != null)
			{
				currentDestination = radioButton.Text;
			}
			this.InitCompleted(currentDestination);
		}
	}

	private void AddDestination<T>(int index, EDataType dataType, TaskSettings taskSettings, string overrideText = null)
	{
		Enum @enum = (Enum)Enum.ToObject(typeof(T), index);
		if (UploadersConfigValidator.Validate<T>(index, Program.UploadersConfig))
		{
			RadioButton rb = new RadioButton
			{
				AutoSize = true
			};
			rb.Text = (string.IsNullOrEmpty(overrideText) ? @enum.GetLocalizedDescription() : $"{Resources.BeforeUploadControl_AddDestination_Custom} [{overrideText}]");
			rb.Tag = @enum;
			rb.CheckedChanged += delegate
			{
				SetDestinations(rb.Checked, dataType, rb.Tag, taskSettings);
			};
			flp.Controls.Add(rb);
		}
	}

	private void SetDestinations(bool isActive, EDataType dataType, object destination, TaskSettings taskSettings)
	{
		if (!isActive)
		{
			return;
		}
		switch (dataType)
		{
		case EDataType.Image:
			if (destination is ImageDestination imageDestination)
			{
				taskSettings.ImageDestination = imageDestination;
			}
			else if (destination is FileDestination)
			{
				FileDestination imageFileDestination = (FileDestination)destination;
				taskSettings.ImageDestination = ImageDestination.FileUploader;
				taskSettings.ImageFileDestination = imageFileDestination;
			}
			break;
		case EDataType.Text:
			if (destination is TextDestination textDestination)
			{
				taskSettings.TextDestination = textDestination;
			}
			else if (destination is FileDestination)
			{
				FileDestination textFileDestination = (FileDestination)destination;
				taskSettings.TextDestination = TextDestination.FileUploader;
				taskSettings.TextFileDestination = textFileDestination;
			}
			break;
		case EDataType.File:
			if (destination is FileDestination)
			{
				FileDestination fileDestination = (FileDestination)destination;
				taskSettings.ImageDestination = ImageDestination.FileUploader;
				taskSettings.TextDestination = TextDestination.FileUploader;
				taskSettings.ImageFileDestination = (taskSettings.TextFileDestination = (taskSettings.FileDestination = fileDestination));
			}
			break;
		case EDataType.URL:
			if (destination is UrlShortenerType)
			{
				UrlShortenerType urlShortenerType = (taskSettings.URLShortenerDestination = (UrlShortenerType)destination);
			}
			break;
		}
	}

	private string GetCustomUploaderName(int index, TaskSettings taskSettings)
	{
		if (taskSettings.OverrideCustomUploader)
		{
			index = taskSettings.CustomUploaderIndex.BetweenOrDefault(0, Program.UploadersConfig.CustomUploadersList.Count - 1, 0);
		}
		return Program.UploadersConfig.CustomUploadersList.ReturnIfValidIndex(index)?.ToString();
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
		this.flp = new System.Windows.Forms.FlowLayoutPanel();
		base.SuspendLayout();
		this.flp.AutoScroll = true;
		this.flp.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flp.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
		this.flp.Location = new System.Drawing.Point(0, 0);
		this.flp.Name = "flp";
		this.flp.Padding = new System.Windows.Forms.Padding(8);
		this.flp.Size = new System.Drawing.Size(321, 372);
		this.flp.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.flp);
		base.Name = "BeforeUploadControl";
		base.Size = new System.Drawing.Size(321, 372);
		base.ResumeLayout(false);
	}
}
