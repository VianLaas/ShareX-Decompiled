using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShareX.HelpersLib;

namespace ShareX;

public class ShareXCLIManager : CLIManager
{
	public ShareXCLIManager(string[] arguments)
		: base(arguments)
	{
	}

	public async Task UseCommandLineArgs()
	{
		await UseCommandLineArgs(base.Commands);
	}

	public async Task UseCommandLineArgs(List<CLICommand> commands)
	{
		TaskSettings taskSettings = FindCLITask(commands);
		foreach (CLICommand command in commands)
		{
			DebugHelper.WriteLine("CommandLine: " + command);
			if (command.IsCommand)
			{
				bool flag = CheckCustomUploader(command) || CheckImageEffect(command);
				if (!flag)
				{
					flag = await CheckCLIHotkey(command);
				}
				bool flag2 = flag;
				if (!flag2)
				{
					flag2 = await CheckCLIWorkflow(command);
				}
				if (!flag2 && !CheckNativeMessagingInput(command))
				{
				}
			}
			else if (URLHelpers.IsValidURL(command.Command))
			{
				UploadManager.DownloadAndUploadFile(command.Command, taskSettings);
			}
			else
			{
				UploadManager.UploadFile(command.Command, taskSettings);
			}
		}
	}

	private TaskSettings FindCLITask(List<CLICommand> commands)
	{
		if (Program.HotkeysConfig != null)
		{
			CLICommand cLICommand = commands.FirstOrDefault((CLICommand x) => x.CheckCommand("task") && !string.IsNullOrEmpty(x.Parameter));
			if (cLICommand != null)
			{
				foreach (HotkeySettings hotkey in Program.HotkeysConfig.Hotkeys)
				{
					if (cLICommand.Parameter == hotkey.TaskSettings.ToString())
					{
						return hotkey.TaskSettings;
					}
				}
			}
		}
		return null;
	}

	private bool CheckCustomUploader(CLICommand command)
	{
		if (command.Command.Equals("CustomUploader", StringComparison.InvariantCultureIgnoreCase))
		{
			if (!string.IsNullOrEmpty(command.Parameter) && command.Parameter.EndsWith(".sxcu", StringComparison.OrdinalIgnoreCase))
			{
				TaskHelpers.ImportCustomUploader(command.Parameter);
			}
			return true;
		}
		return false;
	}

	private bool CheckImageEffect(CLICommand command)
	{
		if (command.Command.Equals("ImageEffect", StringComparison.InvariantCultureIgnoreCase))
		{
			if (!string.IsNullOrEmpty(command.Parameter) && command.Parameter.EndsWith(".sxie", StringComparison.OrdinalIgnoreCase))
			{
				TaskHelpers.ImportImageEffect(command.Parameter);
			}
			return true;
		}
		return false;
	}

	private async Task<bool> CheckCLIHotkey(CLICommand command)
	{
		HotkeyType[] enums = Helpers.GetEnums<HotkeyType>();
		for (int i = 0; i < enums.Length; i++)
		{
			HotkeyType job = enums[i];
			if (command.CheckCommand(job.ToString()))
			{
				await TaskHelpers.ExecuteJob(job, command);
				return true;
			}
		}
		return false;
	}

	private async Task<bool> CheckCLIWorkflow(CLICommand command)
	{
		if (Program.HotkeysConfig != null && command.CheckCommand("workflow") && !string.IsNullOrEmpty(command.Parameter))
		{
			foreach (HotkeySettings hotkey in Program.HotkeysConfig.Hotkeys)
			{
				if (hotkey.TaskSettings.Job != 0 && command.Parameter == hotkey.TaskSettings.ToString())
				{
					await TaskHelpers.ExecuteJob(hotkey.TaskSettings);
					return true;
				}
			}
		}
		return false;
	}

	private bool CheckNativeMessagingInput(CLICommand command)
	{
		if (command.Command.Equals("NativeMessagingInput", StringComparison.InvariantCultureIgnoreCase))
		{
			if (!string.IsNullOrEmpty(command.Parameter) && command.Parameter.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
			{
				TaskHelpers.HandleNativeMessagingInput(command.Parameter);
			}
			return true;
		}
		return false;
	}
}
