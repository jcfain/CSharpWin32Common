using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CSharpWin32Common
{
	public static class Encryption
	{        
		//static byte[] entropy = Encoding.Unicode.GetBytes("Salt Is Not A Password");
		private static readonly byte[] Entropy = Encoding.Unicode.GetBytes("Mmm mm Mmm I love salt");


		public static string EncryptPlainString(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return String.Empty;
			}
			return EncryptString(ToSecureString(input));
		}

		public static string DecryptPlainString(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return String.Empty;
			}
			return ToInsecureString(DecryptString(input));
		}

		public static string EncryptString(SecureString input)
		{
			byte[] encryptedData = ProtectedData.Protect(
				Encoding.Unicode.GetBytes(ToInsecureString(input)),
				Entropy,
				DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(encryptedData);
		}

		public static SecureString DecryptString(string encryptedData)
		{
			try
			{
				byte[] decryptedData = ProtectedData.Unprotect(
					Convert.FromBase64String(encryptedData),
					Entropy,
					DataProtectionScope.CurrentUser);
				return ToSecureString(Encoding.Unicode.GetString(decryptedData));
			}
			catch (CryptographicException e)
			{
				Log.Error("DecryptString: " + e.Message);
				Log.Message("entropy.Length: " + Entropy.Length);
				return new SecureString();
			}
		}

		public static SecureString ToSecureString(string input)
		{
			SecureString secure = new SecureString();
			foreach (char c in input)
			{
				secure.AppendChar(c);
			}
			secure.MakeReadOnly();
			return secure;
		}

		public static string ToInsecureString(SecureString input)
		{
			string returnValue = string.Empty;
			IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
			try
			{
				returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
			}
			finally
			{
				System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
			}
			return returnValue;
		}
	}
}
