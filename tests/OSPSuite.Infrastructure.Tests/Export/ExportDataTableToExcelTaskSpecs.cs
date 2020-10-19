using System;
using System.Data;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;

namespace OSPSuite.Infrastructure.Export
{
   public abstract class concern_for_ExportDataTableToExcelTask : ContextSpecification<IExportDataTableToExcelTask>
   {
      protected string _exportExcelFilePath;
      protected string _exportExcelFile = "export.xlsx";
      protected DataTable _dataTable;

      protected override void Context()
      {
         sut = new ExportDataTableToExcelTask();

         _exportExcelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _exportExcelFile);

         //possibly unnecessary
         if (File.Exists(_exportExcelFilePath))
         {
            File.Delete(_exportExcelFilePath);
         }

         _dataTable = new DataTable(); ;
         _dataTable.Columns.Add("Column1");
         _dataTable.Columns.Add("Column2");

         var row1 = _dataTable.NewRow();
         row1["Column1"] = "str1";
         row1["Column2"] = "str2";

         var row2 = _dataTable.NewRow();
         row2["Column1"] = "str3";
         row2["Column2"] = "str4";

         _dataTable.Rows.Add(row1);
         _dataTable.Rows.Add(row2);

         _dataTable.TableName = "TestSheet";
      }

      public override void Cleanup()
      {
/*
         if (File.Exists(_exportExcelFilePath))
         {
            File.Delete(_exportExcelFilePath);
         }
*/
      }

   }

   public class When_exporting_a_dataTable : concern_for_ExportDataTableToExcelTask
   {

      [Test]
      public void should_create_export_file()
      {
         sut.ExportDataTableToExcel(_dataTable, _exportExcelFilePath, false);

         File.Exists(_exportExcelFilePath).ShouldBeTrue();
      }

      [Test]
      public void should_have_created_sheet()
      {
         sut.ExportDataTableToExcel(_dataTable, _exportExcelFilePath, false);

         var reader = new ExcelReader(_exportExcelFilePath);
         reader.MoveToNextSheet();
         reader.CurrentSheet.SheetName.ShouldBeEqualTo("TestSheet");
      }

      [Test]
      public void should_have_created_data()
      {
         sut.ExportDataTableToExcel(_dataTable, _exportExcelFilePath, true);

         var reader = new ExcelReader(_exportExcelFilePath);
         reader.MoveToNextSheet();
         reader.MoveToNextRow();
         reader.CurrentRow.ElementAt(0).ShouldBeEqualTo("Column1");
         reader.CurrentRow.ElementAt(1).ShouldBeEqualTo("Column2");
      }
   }
}