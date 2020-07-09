using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace CSharpWin32Common
{
	/// <summary>
	/// Interact with an Excel sheet that has a header.
	/// Only works with existing workbooks.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ExcelDataSheet
	{
		private readonly Excel.Workbook _myBook = null;
		private readonly Excel.Worksheet _mySheet = null;
		private readonly Excel.Range _fullRange;
		//private int lastRow = 0;

		public ExcelDataSheet(string path, object sheet)
		{
			var myApp = new Excel.Application {Visible = false};
			_myBook = myApp.Workbooks.Open(path, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
			_mySheet = (Excel.Worksheet)_myBook.Worksheets.Item[sheet];
			//lastRow = _mySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row; 
			_fullRange = _mySheet.UsedRange;
		}

		/// <summary>
		/// Reads an excel sheet with a header as the first row.
		/// </summary>
		/// <returns></returns>
		public DataTable Read()
		{
			DataTable data = new DataTable();
			Log.Debug("Enter excel read");
			Log.Debug("_fullRange.Rows.Count: "+ _fullRange.Rows.Count);
			for(int i=1; i<=_fullRange.Rows.Count; i++)
			{
				if (i == 1)
				{
					Log.Debug("Adding Header");
					for (int j = 1; j <= _fullRange.Columns.Count; j++)
					{
						Log.Debug("Column add: " + _fullRange.Cells[i,j].Value2);
						data.Columns.Add(_fullRange.Cells[i, j].Value2);
					}
				}
				else
				{
					DataRow newRow = data.NewRow();
					var cellIndex = 0;
					Log.Debug("Adding Row: " + i);
					for (int j = 1; j <= _fullRange.Columns.Count; j++)
					{
						Log.Debug("Cell add: " + _fullRange.Cells[i, j].Value2);
						newRow[cellIndex] = _fullRange.Cells[i, j].Value2;
						cellIndex++;
					}
					data.Rows.Add(newRow);
				}
			}
			return data;
		}

		/// <summary>
		/// Writes to an excel sheet with a header as the first row.
		/// </summary>
		/// <param name="data"></param>
		public DataTable write(DataTable data)
		{
			foreach (DataColumn column in data.Columns)
			{
				_mySheet.Cells[1, data.Columns.IndexOf(column)] = column;
			}
			int excelStartRow = 2;
			foreach (DataRow row in data.Rows)
			{
				int cellIndex = 0;
				foreach (var cell in row.ItemArray)
				{
					_mySheet.Cells[excelStartRow, cellIndex] = cell;
					cellIndex++;
				}
				excelStartRow++;
			}
			_myBook.Save();
			return Read();
		}

		/// <summary>
		/// Closes the excel book
		/// </summary>
		public void Close()
		{
			_myBook.Close();
		}
	}
}
