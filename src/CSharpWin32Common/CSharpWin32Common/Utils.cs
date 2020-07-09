using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace CSharpWin32Common
{
	/// <summary>
	/// Functions that are useful but do not have a home yet.
	/// </summary>
	public static class Utils
	{
		

		

		/// <summary>
		/// Sets the registry key that disables AutoCompletes modals.
		/// see https://social.technet.microsoft.com/Forums/en-US/8ae0dfd5-4fd5-47fe-b762-54d0ac7027f9/disable-do-you-want-to-turn-autocomplete-on-prompt?forum=w7itproinstall
		/// </summary>
		internal static void CheckForAndTurnIEAutoCompleteOff()
		{
			//For more registry keys https://msdn.microsoft.com/en-us/library/dn479807.aspx
			//Check Autocomplete modal AskUser registry key.
			RegistryKey myKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\IntelliForms", true);
			if (myKey != null)
			{
				var keyValue = myKey.GetValue("AskUser");
				Log.Debug("Autocompete AskUser reg key: " + keyValue);
				if (keyValue != null && keyValue.ToString() != "0")
				{
					//This modal will popup and make the test fail if it is not disabled.
					myKey.SetValue("AskUser", "0", RegistryValueKind.DWord);
				}
				myKey.Close();
			}
		}
		//Dont think this did anything.
		///// <summary>
		///// Sets or creates the registry key that disables Password caching modals.
		///// See http://blogs.msdn.com/b/asiatech/archive/2014/03/03/case-study-how-to-enable-or-disable-remember-my-credentials-in-ie-in-credential-window.aspx
		///// </summary>
		//internal static void CheckForAndTurnIEPasswordCachingOff()
		//{
		//	//Check for DisablePasswordCaching registry key.
		//	RegistryKey myKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
		//	if (myKey != null)
		//	{
		//		var keyValue = myKey.GetValue("DisablePasswordCaching");
		//		Log.Message("DisablePasswordCaching reg key: " + keyValue);
		//		if (keyValue != null && keyValue.ToString() != "1")
		//		{
		//			//This modal will popup and make the test fail if it is not disabled.
		//			myKey.SetValue("DisablePasswordCaching", "1", RegistryValueKind.DWord);
		//		}
		//		else if (keyValue == null)
		//		{
		//			myKey.CreateSubKey("DisablePasswordCaching");
		//			myKey.SetValue("DisablePasswordCaching", "1", RegistryValueKind.DWord);
		//		}
		//		myKey.Close();
		//	}
		//}

		internal static void CheckAndTurnFormSuggestOff()
		{
			//Check for DisablePasswordCaching registry key.
			RegistryKey myKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main", true);
			if (myKey != null)
			{
				CheckForKeyStringByNameCreateAndSetValue(myKey, "FormSuggest PW Ask", "no");

				CheckForKeyStringByNameCreateAndSetValue(myKey, "FormSuggest Passwords", "no");

				CheckForKeyStringByNameCreateAndSetValue(myKey, "Use FormSuggest", "no");

				myKey.Close();
			}
		}

		/// <summary>
		/// Turns off update popup when launching a new window on Ie10 or below.
		/// </summary>
		public static void CheckForAndTurnIeUpdateSuggestionPopupOff()
		{
			RegistryKey myKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_DISABLE_IE11_SECURITY_EOL_NOTIFICATION", true);
			Log.Debug("Mykey: " + myKey);
			if (myKey != null)
			{
				CheckForKeyStringByNameCreateAndSetValue(myKey, "iexplore.exe", 1, RegistryValueKind.DWord);
				myKey.Close();
			}
		}
		/// <summary>
		/// Creates a subkey under a RegistryKey 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyName"></param>
		/// <param name="value"></param>
		/// <param name="regValKind"></param>
		private static void CheckForKeyStringByNameCreateAndSetValue(RegistryKey key, string keyName, object value, RegistryValueKind regValKind = RegistryValueKind.String)
		{
			var keyValue = key.GetValue(keyName);
			Log.Debug("Use FormSuggest reg key: " + keyValue);
			if (keyValue != null && keyValue != value)
			{
				Log.Debug("keyValue != null && keyValue != value");
				key.SetValue(keyName, value, regValKind);
			}
			else if (keyValue == null)
			{
				Log.Debug("keyValue == null");
				key.CreateSubKey(keyName);
				key.SetValue(keyName, value, regValKind);
			}
		}


		/// <summary>
		/// Puts the thread to sleep for a specified time in seconds
		/// </summary>
		/// <param name="seconds">Number of seconds to sleep.</param>
		public static void Sleep(int seconds)
		{
			//Log.Action("Sleep", seconds.ToString());
			System.Threading.Thread.Sleep(seconds*1000);
		}

		/// <summary>
		/// Closes all process by name passed in.
		/// Example: "iexplore"
		/// </summary>
		[Obsolete("Use Desktop.KillAllProcesses")]
		public static void KillAllProcesses(string processName)
		{
			foreach (var process in Process.GetProcessesByName(processName))
			{
				Log.Debug($"Killing process: {processName}.");
				process.Kill();
			}
		}

		/// <summary>
		/// Capitalizes the first letter of a word.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}

		/// <summary>
		/// Removes all new line characters and replaces them with an empty space.
		/// </summary>
		/// <param name="inString"></param>
		/// <returns></returns>
		public static string RemoveNewLineChars(string inString)
		{
			if (!string.IsNullOrEmpty(inString))
			{
				return Regex.Replace(inString, @"\r\n|\t|\n|\r|<br/>|<br>", " ");
			}
			return inString;
		}
		/// <summary>
		/// Removes all new line characters and replaces them with an empty space.
		/// </summary>
		/// <param name="inString"></param>
		/// <returns></returns>
		public static string EscapeSpecialChars(string inString)
		{
			if(!inString.Equals(String.Empty))
				return Regex.Escape(inString);
			return inString;
		}

		/// <summary>
		/// Gets the time seconds into the future.
		/// </summary>
		/// <param name="seconds"></param>
		/// <returns></returns>
		public static DateTime AddSecondsToNow(int seconds)
		{
			return DateTime.Now.AddSeconds(seconds);
		}

		/// <summary>
		/// Gets CST(MedAssets Texas) time stamp from the time zone you are currently in.
		/// </summary>
		/// <returns></returns>
		public static DateTime GetCstFromCurrentZone()
		{
			//try
			//{
				TimeZoneInfo localZone = TimeZoneInfo.Local;
				TimeZoneInfo centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
				return TimeZoneInfo.ConvertTime(DateTime.Now, localZone, centralZone);
			//}
			//catch (TimeZoneNotFoundException)
			//{
			//	Log.Fail("Unable to find the 'Central Standard Time' zone in the registry.");
			//}
			//catch (InvalidTimeZoneException)
			//{
			//	Log.Fail("Registry data on the 'Central Standard Time' zone has been corrupted.");
			//}
			//return DateTime.MinValue;
		}

		public static string DateTimetoUtc(DateTime dateParam)
		{
			string buffer = dateParam.ToString("********HHmmss.ffffff");
			TimeSpan tickOffset = TimeZone.CurrentTimeZone.GetUtcOffset(dateParam);
			buffer += (tickOffset.Ticks >= 0) ? '+' : '-';
			buffer += (Math.Abs(tickOffset.Ticks) / System.TimeSpan.TicksPerMinute).ToString("d3");
			return buffer;
		}

		internal static string GetIeVersion()
		{
			return (string)Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer").GetValue("Version"); //For Windows 8 the key name is 'svcVersion'
		}

		internal static string GetFirefoxVersion()
		{
			string wowNode = string.Empty;
			if (Environment.Is64BitOperatingSystem)
			{
				wowNode = @"Wow6432Node\";
			}
			RegistryKey regKey = Registry.LocalMachine;
			return (string)regKey.OpenSubKey(@"Software\" + wowNode + @"Mozilla\Mozilla Firefox").GetValue("CurrentVersion");
		}

		internal static string GetChromeVersion()
		{
			string wowNode = string.Empty;
			if (Environment.Is64BitOperatingSystem) wowNode = @"Wow6432Node\";

			RegistryKey regKey = Registry.LocalMachine;
			RegistryKey keyPath = regKey.OpenSubKey(@"Software\" + wowNode + @"Google\Update\Clients");

			if (keyPath == null)
			{
				regKey = Registry.CurrentUser;
				keyPath = regKey.OpenSubKey(@"Software\" + wowNode + @"Google\Update\Clients");
			}

			if (keyPath == null)
			{
				regKey = Registry.LocalMachine;
				keyPath = regKey.OpenSubKey(@"Software\Google\Update\Clients");
			}

			if (keyPath == null)
			{
				regKey = Registry.CurrentUser;
				keyPath = regKey.OpenSubKey(@"Software\Google\Update\Clients");
			}

			if (keyPath != null)
			{
				string[] subKeys = keyPath.GetSubKeyNames();
				foreach (string subKey in subKeys)
				{
					object value = keyPath.OpenSubKey(subKey).GetValue("name");
					bool found = false;
					if (value != null)
						found =
							value.ToString()
								 .Equals("Google Chrome", StringComparison.InvariantCultureIgnoreCase);
					if (found)
					{
						return (string)keyPath.OpenSubKey(subKey).GetValue("pv");
					}
				}
			}
			Log.Debug("Registry key not found for chrome");
			return null;
		}

		/// <summary>
		/// Gets a type name from a full name by searching for '.'.
		/// </summary>
		/// <param name="fullName"></param>
		/// <returns></returns>
		public static string GetTestFixtureNameFromFullName(string fullName)
		{
			string temp = fullName.Substring(0, fullName.LastIndexOf('.'));
			return temp.Substring(temp.LastIndexOf('.') + 1);
		}

		/// <summary>
		/// Formats a double into US currency format
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string USCurrencyFormat(double number)
		{
			return $"{number:C}";
		}

		/// <summary>
		/// Formats a string into US currency format
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string USCurrencyFormat(string number)
		{
			if (number.Contains("$"))
			{
				return number;
			}
			return USCurrencyFormat(Convert.ToDouble(number));
		}

		/// <summary>
		/// Formats a int into US currency format
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string USCurrencyFormat(int number)
		{
			return USCurrencyFormat(Convert.ToDouble(number));
		}

		/// <summary>
		/// Determines if the string passed in is a valid uri or not.
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		public static bool IsValidUri(String uri)
		{
			try
			{
				new Uri(uri);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
