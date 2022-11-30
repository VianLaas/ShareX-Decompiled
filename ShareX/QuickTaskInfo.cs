using System.Collections.Generic;
using System.Linq;
using ShareX.HelpersLib;

namespace ShareX;

public class QuickTaskInfo
{
	public string Name { get; set; }

	public AfterCaptureTasks AfterCaptureTasks { get; set; }

	public AfterUploadTasks AfterUploadTasks { get; set; }

	public bool IsValid => AfterCaptureTasks != AfterCaptureTasks.None;

	public static List<QuickTaskInfo> DefaultPresets => new List<QuickTaskInfo>
	{
		new QuickTaskInfo("Save, Upload, Copy URL", AfterCaptureTasks.SaveImageToFile | AfterCaptureTasks.UploadImageToHost, AfterUploadTasks.CopyURLToClipboard),
		new QuickTaskInfo("Save, Copy image", AfterCaptureTasks.CopyImageToClipboard | AfterCaptureTasks.SaveImageToFile),
		new QuickTaskInfo("Save, Copy image file", AfterCaptureTasks.SaveImageToFile | AfterCaptureTasks.CopyFileToClipboard),
		new QuickTaskInfo("Annotate, Save, Upload, Copy URL", AfterCaptureTasks.AnnotateImage | AfterCaptureTasks.SaveImageToFile | AfterCaptureTasks.UploadImageToHost, AfterUploadTasks.CopyURLToClipboard),
		new QuickTaskInfo(),
		new QuickTaskInfo("Upload, Copy URL", AfterCaptureTasks.UploadImageToHost, AfterUploadTasks.CopyURLToClipboard),
		new QuickTaskInfo("Save", AfterCaptureTasks.SaveImageToFile),
		new QuickTaskInfo("Copy image", AfterCaptureTasks.CopyImageToClipboard),
		new QuickTaskInfo("Annotate", AfterCaptureTasks.AnnotateImage)
	};

	public QuickTaskInfo()
	{
	}

	public QuickTaskInfo(string name, AfterCaptureTasks afterCaptureTasks, AfterUploadTasks afterUploadTasks = AfterUploadTasks.None)
	{
		Name = name;
		AfterCaptureTasks = afterCaptureTasks;
		AfterUploadTasks = afterUploadTasks;
	}

	public QuickTaskInfo(AfterCaptureTasks afterCaptureTasks, AfterUploadTasks afterUploadTasks = AfterUploadTasks.None)
		: this(null, afterCaptureTasks, afterUploadTasks)
	{
	}

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(Name))
		{
			return Name;
		}
		string text = string.Join(", ", from x in AfterCaptureTasks.GetFlags()
			select x.GetLocalizedDescription());
		if (AfterCaptureTasks.HasFlag(AfterCaptureTasks.UploadImageToHost))
		{
			string[] array = (from x in AfterUploadTasks.GetFlags()
				select x.GetLocalizedDescription()).ToArray();
			if (array != null && array.Length != 0)
			{
				text = text + ", " + string.Join(", ", array);
			}
		}
		return text;
	}
}
