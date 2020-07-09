using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Management;

namespace CSharpWin32Common
{
	public static class AccountMgmt
	{
		public static bool IsPassWordValid(string userName, string encryptedPassword)
		{
			bool valid = false;
			if (!string.IsNullOrEmpty(encryptedPassword))
			{
				using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
				{
					valid = context.ValidateCredentials(userName, Encryption.DecryptPlainString(encryptedPassword));
				}
				Log.Debug("Valid pass? " + valid);
			}
			return valid;
		}

# region NotCompleted		
		//http://www.codeproject.com/Articles/31113/Create-a-Remote-Process-using-WMI-in-C
		private static void WmiConnect(string remoteMachine)
		{
			//
			// Any source code blocks look like this
			//
			string sBatFile;
			if (remoteMachine != string.Empty)
				sBatFile = @"\\" + remoteMachine + "\\admin$\\process.bat";
			else
				throw new Exception("Invalid Machine name");

			if (File.Exists(sBatFile))
				File.Delete(sBatFile);
			StreamWriter sw = new StreamWriter(sBatFile);
			string _cmd = "DIR > <a>\\\\</a>" + remoteMachine + "\\admin$\\output.txt";
			Console.Write("Enter the remote Command <eg : Notepad.exe, Dir, Shutdown - r, etc..> : ");
			_cmd = Console.ReadLine();
			if (_cmd.Trim() == string.Empty)
				Console.WriteLine("No command entered using default command for test :" + _cmd);

			sw.WriteLine(_cmd);
			sw.Close();

			//
			//WMI section
			//    
			ConnectionOptions connOptions = new ConnectionOptions();
			connOptions.Impersonation = ImpersonationLevel.Impersonate;
			connOptions.EnablePrivileges = true;
			ManagementScope manScope = new ManagementScope(String.Format(@"\\{0}\ROOT\CIMV2", remoteMachine), connOptions);
			manScope.Connect();
			ObjectGetOptions objectGetOptions = new ObjectGetOptions();
			ManagementPath managementPath = new ManagementPath("Win32_Process");
			ManagementClass processClass = new ManagementClass
				(manScope, managementPath, objectGetOptions);
			ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
			inParams["CommandLine"] = sBatFile;
			ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);
			Console.WriteLine("Creation of the process returned: " + outParams["returnValue"]);
			Console.WriteLine("Process ID: " + outParams["processId"]);

			//The interesting part is the WMI section.

			//Created the ConnectionOptions object and set the Impersonation as Impersonate. This will make sure the current users credentials are used to execute the process in the remote machine.
			//Created the ManagementScope object by passing the root and the ConnectionOptions object. Connect() actually establishes the connection to the remote machine.
			//Created the ProcessClass object of the type ManagementClass by passing the ManagementScope, ManagementPath objects.
			//Called the InvokeMethod on the processClass object. This actually creates the new process on the remote server, by taking the batfile name as parameter to start a new Win32 process.
		}

		//http://www.codeproject.com/Articles/31113/Create-a-Remote-Process-using-WMI-in-C
		public static void ExecuteCommand(string remoteMachine, string command, string workingDir = @"C:\Windows\System32")
		{
			try
			{
				ConnectionOptions connOptions = new ConnectionOptions();
				connOptions.Impersonation = ImpersonationLevel.Impersonate;
				connOptions.EnablePrivileges = true;
				ManagementScope manScope = new ManagementScope(String.Format(@"\\{0}\ROOT\CIMV2", remoteMachine),
					connOptions);
				manScope.Connect();
				ObjectGetOptions objectGetOptions = new ObjectGetOptions();
				ManagementPath managementPath = new ManagementPath("Win32_Process");
				ManagementClass processClass = new ManagementClass
					(manScope, managementPath, objectGetOptions);
				ManagementBaseObject inParams = processClass.GetMethodParameters("Create");
				inParams["CommandLine"] = command;
				inParams["CurrentDirectory"] = workingDir;
				ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);
				Log.Message("Creation of the process returned: " + outParams["returnValue"]);
				Log.Message("Process ID: " + outParams["processId"]);
			}
			catch (Exception e)
			{
				Log.Error("WMI Exception: " + e.Message);
				Log.Error(e.StackTrace);
			}
		}
	}
#endregion
}
