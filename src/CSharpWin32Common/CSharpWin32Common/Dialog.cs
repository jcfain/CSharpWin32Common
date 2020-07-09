
using System.IO;
using System.Windows.Forms;

namespace CSharpWin32Common
{
	public class LoginPacket
	{
		public string pass { get; set; }
		public bool savePass { get; set; }
	}
	public static class Dialog
	{
		public static LoginPacket ShowLoginPrompt(Form parent, string InputHeader, string FormCaption)
		{
			string pass = null;
			Form dlg = new Form();
			dlg.ControlBox = false;
			dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
			dlg.Width = 200;
			dlg.Height = 150;
			dlg.Text = FormCaption;
			dlg.StartPosition = FormStartPosition.CenterParent;
			Label textHeading = new Label();
			textHeading.AutoSize = true;
			textHeading.Left = 15;
			textHeading.Top = 12;
			textHeading.Text = InputHeader;
			TextBox textC = new TextBox();
			textC.Left=15;
			textC.Top=30;
			textC.Width=155;
			textC.PasswordChar = '*';
			CheckBox savePass = new CheckBox();
			savePass.Text = "Save";
			savePass.Left = 100;
			savePass.Top = 55;
			savePass.Width = 75;
			CheckBox showPass = new CheckBox();
			showPass.Text = "Show";
			showPass.Left = 20;
			showPass.Top = 55;
			showPass.Width = 75;
			showPass.CheckedChanged += (sender, evt) =>
			{
				if (showPass.Checked)
					textC.PasswordChar = '\0';
				else
					textC.PasswordChar = '*';
			};
			
			Button cancelButton = new Button();
			dlg.CancelButton = cancelButton;
			cancelButton.Text = "Cancel";
			cancelButton.Left = 15;
			cancelButton.Top = 80;
			cancelButton.Click += (sender, evt) =>
			{
				pass = "cancelDialog";
				dlg.Close();
			};
			Button grabInput = new Button() ;
			dlg.AcceptButton = grabInput;
			grabInput.Text = "Confirm";
			grabInput.Left = 95;
			grabInput.Top = 80;
			grabInput.Click += (sender, evt) => {
				pass = Encryption.EncryptPlainString(textC.Text);
				dlg.Close(); 
			};
			ToolTip toolTip = new ToolTip();
			toolTip.SetToolTip(savePass, "Save the password to disk. Password will be encrypted\nand will only be usable on this machine.\nIt is NOT stored in your mvnc config file.\nThis is the password you use to log in to all the machines.\nNot the vnc password.");
			toolTip.SetToolTip(showPass, "Show the password text above.");
			//dlg.StartPosition = parent.StartPosition;
			dlg.Controls.Add(showPass);
			dlg.Controls.Add(savePass);
			dlg.Controls.Add(grabInput);
			dlg.Controls.Add(cancelButton);
			dlg.Controls.Add(textHeading);
			dlg.Controls.Add(textC);
			textC.Focus();
			dlg.ShowDialog(parent);
			return new LoginPacket { pass = pass, savePass = savePass.Checked};
		}

		public static void showPleaseWaitForm(Form parent, string InputHeader)
		{
			Form dlg = new Form();
			dlg.Width = 300;
			dlg.Height = 150;
			dlg.Text = "Please Wait";
			dlg.StartPosition = FormStartPosition.CenterParent;
			Label nameHeading = new Label();
			nameHeading.AutoSize = true;
			nameHeading.Left = 20;
			nameHeading.Top = 10;
			nameHeading.Text = InputHeader;
			Button grabInput = new Button();
			dlg.AcceptButton = grabInput;
			grabInput.Text = "OK";
			grabInput.Left = 100;
			grabInput.Top = 75;
			grabInput.Click += (sender, evt) =>
			{
				dlg.Close();
			};
			dlg.StartPosition = parent.StartPosition;
			dlg.Controls.Add(grabInput);
			dlg.Controls.Add(nameHeading);
			dlg.ShowDialog(parent);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="defaultPath"></param>
		/// <param name="filter">Example: "MVNC files (*.mvnc)|*.mvnc"</param>
		/// <returns></returns>
		public static string showOpenFileDialog(Form parent, string defaultPath, string filter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = filter;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.InitialDirectory = defaultPath;
			DialogResult result = openFileDialog.ShowDialog(parent); // Show the dialog.
			if (result == DialogResult.OK) // Test result.
			{
				return openFileDialog.FileName;
			}
			return "Cancel";
		}
		public static string showOpenDirectoryDialog(Form parent, string defaultPath)
		{
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			DialogResult result = folderBrowserDialog.ShowDialog(parent); // Show the dialog.
			if (result == DialogResult.OK) // Test result.
			{
				return folderBrowserDialog.SelectedPath;
			}
			return defaultPath;
		}
		public static string showFileSaveDialog(Form parent, string defaultPath)
		{
			//Stream myStream;
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.Filter = "MVNC files (*.mvnc)|*.mvnc";
			saveFileDialog1.RestoreDirectory = true;
			saveFileDialog1.InitialDirectory = defaultPath;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				if (!File.Exists(saveFileDialog1.FileName))
					return saveFileDialog1.FileName;
				else
				{
					bool overwrite = showYesNoDialog(parent, "File already exists, overwrite?", "File Exists...");
					if (overwrite)
						return saveFileDialog1.FileName;
					else
						return null;
				}
			}
			return null;
		}

		public static string showYesNoLockDialog(Form parent, string InputHeader, string FormCaption)
		{
			string returnText = null;
			Form dlg = new Form();
			//dlg.ControlBox = false;
			dlg.MinimizeBox = false;
			dlg.MaximizeBox = false;
			dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
			dlg.Width = 275;
			dlg.Height = 95;
			dlg.Text = FormCaption;
			dlg.StartPosition = FormStartPosition.CenterParent;
			Label textHeading = new Label();
			textHeading.AutoSize = true;
			textHeading.Left = 15;
			textHeading.Top = 7;
			textHeading.Text = InputHeader;
			Button lockButton = new Button();
			lockButton.Text = "Lock";
			lockButton.Left = 15;
			lockButton.Top = 25;
			lockButton.Click += (sender, evt) =>
			{
				returnText = "lock";
				dlg.Close();
			};
			Button noButton = new Button();
			dlg.CancelButton = noButton;
			noButton.Text = "No";
			noButton.Left = 95;
			noButton.Top = 25;
			noButton.Click += (sender, evt) =>
			{
				returnText = "no";
				dlg.Close();
			};
			Button grabInput = new Button();
			dlg.AcceptButton = grabInput;
			grabInput.Text = "Yes";
			grabInput.Left = 170;
			grabInput.Top = 25;
			grabInput.Click += (sender, evt) =>
			{
				returnText = "logOut";
				dlg.Close();
			};
			dlg.Controls.Add(grabInput);
			dlg.Controls.Add(noButton);
			dlg.Controls.Add(textHeading);
			dlg.Controls.Add(lockButton);
			dlg.ShowDialog(parent);
			return returnText;
		}
		public static bool showYesNoDialog(Form parent, string message, string dialogTitle)
		{
			return (MessageBox.Show(parent, message, dialogTitle,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes);
		}
		public static void showInfoDialog(Form parent, string message, string dialogTitle)
		{
			MessageBox.Show(parent, message, dialogTitle,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		public static void showErrorDialog(Form parent, string message, string dialogTitle)
		{
			MessageBox.Show(parent, message, dialogTitle,
					MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		public static string ShowGetTextPrompt(Form parent, string InputHeader, string FormCaption)
		{
			string text = null;
			Form dlg = new Form();
			dlg.ControlBox = false;
			dlg.FormBorderStyle = FormBorderStyle.SizableToolWindow;
			dlg.Width = 200;
			dlg.Height = 150;
			dlg.Text = FormCaption;
			dlg.StartPosition = FormStartPosition.CenterParent;
			Label textHeading = new Label();
			textHeading.AutoSize = true;
			textHeading.Left = 15;
			textHeading.Top = 12;
			textHeading.Text = InputHeader;
			TextBox textC = new TextBox();
			textC.Left = 15;
			textC.Top = 30;
			textC.Width = 155;

			Button cancelButton = new Button();
			dlg.CancelButton = cancelButton;
			cancelButton.Text = "Cancel";
			cancelButton.Left = 15;
			cancelButton.Top = 80;
			cancelButton.Click += (sender, evt) =>
			{
				text = "cancelDialog";
				dlg.Close();
			};
			Button grabInput = new Button();
			dlg.AcceptButton = grabInput;
			grabInput.Text = "Confirm";
			grabInput.Left = 95;
			grabInput.Top = 80;
			grabInput.Click += (sender, evt) => {
				text = textC.Text;
				dlg.Close();
			};
			ToolTip toolTip = new ToolTip();
			toolTip.SetToolTip(textC, "Enter up to 9 values seperated by spaces as needed by the bat file you have chosen.");
			dlg.Controls.Add(grabInput);
			dlg.Controls.Add(cancelButton);
			dlg.Controls.Add(textHeading);
			dlg.Controls.Add(textC);
			textC.Focus();
			dlg.ShowDialog(parent);
			return text;
		}
	}
}