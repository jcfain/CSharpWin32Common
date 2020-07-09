using Microsoft.Win32;

namespace CSharpWin32Common
{
	public class RegistryEditor
	{
		private static void CheckAndTurnFormSuggestOff()
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
		private static void CheckForAndTurnIeUpdateSuggestionPopupOff()
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
	}
}
