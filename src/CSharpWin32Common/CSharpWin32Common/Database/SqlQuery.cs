using System.Data;
using System.Data.SqlClient;
using Automation.Common.Logging;

namespace Automation.Common.Database
{
	internal class SqlQuery
	{
		internal DataTable Run(string ConnectionString, string query)
		{
			DataTable results = new DataTable();
			using (SqlConnection conn = new SqlConnection(ConnectionString))
			using (SqlCommand command = new SqlCommand(query, conn))
			using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
				dataAdapter.Fill(results);
			return results;
		}

		internal void PrintDataTableToHtmlLog(DataTable dataTable)
		{
			//List<string> sysKeys = GetSysKeys(ConnectionString, topRows);
			string newtable = "";
			newtable += "<table border='1'>";

			DataColumnCollection dataColumns = dataTable.Columns;
			//Log.Message("syskey: " + syskey);
			newtable += "<thead><tr border='1'>";
			foreach (DataColumn dataColumn in dataColumns)
			{
				newtable += "<th cellspacing='10' cellpadding='15' border='1' nowrap>" + dataColumn.ColumnName + "</th>";
			}
			newtable += "</tr></thead><tbody>";

			DataRowCollection dataRows = dataTable.Rows;
			foreach (DataRow dataRow in dataRows)
			{
				//Log.Message("syskey: " + syskey);
				newtable += "<tr border='1'>";
				foreach (object obj in dataRow.ItemArray)
				{
					//if (obj.ToString() != String.Empty)
					//{
					newtable += "<td cellspacing='10' cellpadding='15' border='1' nowrap>" + obj.ToString() + "</td>";
					//}
				}
				newtable += "</tr>";
			}
			newtable += "</tbody></table>";
			Log.Message(newtable);
		}
	}
}
