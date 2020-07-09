using System;
using System.IO;

namespace CSharpWin32Common
{

	public static class Log
	{
		public static bool VerboseLogging = false;
		private readonly static bool DebugMode = true;
		private static string VerboseOutputFile = "log.csv";


		/// <summary>
		/// Logs a message at the debug level for developing
		/// </summary>
		public static void Debug(string text)
		{
			if (DebugMode)
			{
				//Console.WriteLine(text);
				System.Diagnostics.Debug.WriteLine(text);
			}
			if (VerboseLogging)
			{
				WriteFile(VerboseOutputFile, DateTime.Now + ",DEBUG," + Utils.RemoveNewLineChars(text.Replace(",", "")), false);
			}
		}

		/// <summary>
		/// Logs a standard info message.
		/// </summary>
		public static void Message(string text)
		{
			//Console.WriteLine(text);
			System.Diagnostics.Debug.WriteLine(text);
			if (VerboseLogging)
			{
				WriteFile(VerboseOutputFile, DateTime.Now + ",MESG," + Utils.RemoveNewLineChars(text.Replace(",", "")), false);
			}
		}

		/// <summary>
		/// Logs a common error for apps. Does not write to the logfile.
		/// </summary>
		public static void Error(string text)
		{
			text = "Error: " + text;
			//Console.WriteLine(text);
			System.Diagnostics.Debug.WriteLine(text);
			if (VerboseLogging)
			{
				WriteFile(VerboseOutputFile, DateTime.Now + ",ERROR," + Utils.RemoveNewLineChars(text.Replace(",", "")), false);
			}
		}

		public static void writeCatchFile(Exception e, string message)
		{
			WriteFile("crashLog.txt", DateTime.Now + ": " + message + " Message:\n" + e.Message, false);
			WriteFile("crashLog.txt", DateTime.Now + ": StackTr:\n" + e.StackTrace, false);
			WriteFile("crashLog.txt", DateTime.Now + ": InnerException:\n" + e.InnerException, false);
		}

		/// <summary>
		/// Writes to a file in the specified path. If the file already exists it appends. If it doesnt exist it creates it.
		/// </summary>
		/// <param name="path">Pathe to the file.</param>
		/// <param name="text">String to write to the file.</param>
		/// <param name="overWrite">Over write the file or append. True will overwrite.</param>
		private static void WriteFile(string path, string text, bool overWrite)
		{
			if (!overWrite && File.Exists(path))
			{
				StreamWriter file = new System.IO.StreamWriter(path, true);
				file.WriteLine(text);
				file.Close();
				return;
			}
			// WriteAllText creates a file, writes the specified string to the file, 
			// and then closes the file.
			File.WriteAllText(path, text + "\n");
		}
	}
}
