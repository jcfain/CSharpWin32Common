using System.Data;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpWin32Common.UnitTests
{
	[TestFixture]
	internal class ExcelTests
	{
		private ExcelDataSheet _datasheet;
		[TestFixtureSetUp]
		public void FixtureSetup()
		{

		}
		[SetUp]
		public void TestSetup()
		{
		}

		[TearDown]
		public void TearDown()
		{
			_datasheet.Close();
		}
		[Test]
		public void ExcelSheet_Read_VerfyRowCell()
		{
			_datasheet = new ExcelDataSheet(
				Path.GetFullPath(@"UnitTests\TestFiles\TestData.xlsx"), 1);
			DataTable data = _datasheet.Read();
			data.Rows[0].ItemArray[1].Should().Be("Row1Cell1");
		}
		[Test]
		public void ExcelSheet_Read_VerfyRowCellInt()
		{
			_datasheet = new ExcelDataSheet(
				Path.GetFullPath(@"UnitTests\TestFiles\TestData.xlsx"), 1);
			DataTable data = _datasheet.Read();
			data.Rows[1].ItemArray[1].Should().Be("35");
		}
		[Test]
		public void ExcelSheet_Read_VerfySheet2Cell()
		{
			_datasheet = new ExcelDataSheet(
				Path.GetFullPath(@"UnitTests\TestFiles\TestData.xlsx"), "Sheet2");
			DataTable data = _datasheet.Read();
			data.Rows[1].ItemArray[1].Should().Be("65");
		}
	}
}
