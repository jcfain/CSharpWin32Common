using System;
using System.Management;

namespace CSharpWin32Common
{
	public class RemoteMachineMgmt
	{
		/// <summary>
		/// Runs a process on the host machine.
		/// Will not run a process that requires an interactive session. (GUI)
		/// </summary>
		/// <param name="hostname"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="command"></param>
		public static void RunProcess(string hostname, string username, string password, string command)
		{
			try
			{
				//Assign the user name and password of the account to ConnectionOptions object
				//which have administrative privilege on the remote machine.
				ConnectionOptions connectoptions = new ConnectionOptions();
				connectoptions.Username = username;
				connectoptions.Password = password;

				//IP Address of the remote machine
				ManagementScope scope = new ManagementScope(@"\\" + hostname + @"\root\cimv2", connectoptions);
				ObjectGetOptions objectGetOptions = new ObjectGetOptions();
				ManagementPath managementPath = new ManagementPath("Win32_Process");
				ManagementClass mgmtClass = new ManagementClass(scope, managementPath, objectGetOptions);
				//ManagementClass mgmtClass = new ManagementClass(@"\\" + hostname + @"\root\cimv2:Win32_Process");

				object[] methodArgs = {command, null, null, 0};
				mgmtClass.InvokeMethod("Create", methodArgs);
			}
			catch (Exception e)
			{
				Log.Error(e.Message);
				Log.Error(e.StackTrace);
			}
}
		/// <summary>
		/// Logs off of a remote machine
		/// </summary>
		/// <param name="hostname"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public static void LogOff(string hostname, string username, string password)
		{
			try
			{
				ConnectionOptions connectoptions = new ConnectionOptions();
				connectoptions.Username = username;
				connectoptions.Password = password;

				//IP Address of the remote machine
				ManagementScope scope = new ManagementScope(@"\\" + hostname + @"\root\cimv2", connectoptions);
				ManagementBaseObject mboShutdown = null;
				ObjectGetOptions objectGetOptions = new ObjectGetOptions();
				ManagementPath managementPath = new ManagementPath("Win32_OperatingSystem");
				ManagementClass mcWin32 = new ManagementClass(scope, managementPath, objectGetOptions);
				mcWin32.Get();

				// You can't shutdown without security privileges
				mcWin32.Scope.Options.EnablePrivileges = true;
				ManagementBaseObject mboShutdownParams =
						 mcWin32.GetMethodParameters("Win32Shutdown");

				// Flag 1 means we want to shut down the system. Use "2" to reboot.
				mboShutdownParams["Flags"] = "4";
				mboShutdownParams["Reserved"] = "0";
				foreach (ManagementObject manObj in mcWin32.GetInstances())
				{
					mboShutdown = manObj.InvokeMethod("Win32Shutdown",
												   mboShutdownParams, null);
				}
			}
			catch (Exception e)
			{
				Log.Error(e.Message);
				Log.Error(e.StackTrace);
			}
		}
		
		/// <summary>
		/// Could not get this to work fully.
		///  It creates the task on the remote machine but it doesnt start.
		/// </summary>
		/// <param name="hostname"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="command"></param>
		/// <param name="futureStartTime"></param>
		public void CreateScheduledTask(string hostname, string username, string password, string command, DateTime futureStartTime)
		{
			ConnectionOptions conn = new ConnectionOptions();
			conn.Username = username;
			conn.Password = password;

			//connectoptions.Authority = "ntlmdomain:";
			conn.EnablePrivileges = true;
			ManagementScope scope = new ManagementScope(@"\\"+ hostname + @"\root\cimv2", conn);
			scope.Connect();
			Console.WriteLine("Connected");
			ObjectGetOptions objectGetOptions = new ObjectGetOptions();
			ManagementPath managementPath = new ManagementPath("Win32_ScheduledJob");
			ManagementClass classInstance = new ManagementClass(scope, managementPath, objectGetOptions);

			ManagementBaseObject inParams = classInstance.GetMethodParameters("Create");
			//inParams["Command"] = "notepad.exe";
			//the itme must be in UTC format
			string startTime = Utils.DateTimetoUtc(futureStartTime);
			Console.WriteLine("StartTime: " + startTime);

			inParams["StartTime"] = startTime;
			inParams["Command"] = command;
			inParams["InteractWithDesktop"] = true;

			classInstance["Owner"] = username;
			//classInstance["Caption"] = "testinf interactive call wmi";
			//classInstance["Name"] = "Boodiddy";

			ManagementBaseObject outParams = classInstance.InvokeMethod("Create", inParams, null);
			Log.Message("CreateScheduledTask JobId: " + outParams["JobId"]);
			//Console.ReadKey();

			////IP Address of the remote machine
			//string hostname = "FKJRTJ1-PLLOANR";
			//ManagementClass mgmtClass = new ManagementClass(@"\\" + hostname + @"\root\cimv2:Win32_ScheduledJob");

			//object[] methodArgs = { @"C:\Program Files\Internet Explorer\iexplore.exe" };
			//mgmtClass.InvokeMethod("Create", methodArgs);
		}
	}
}
