using System;
using System.Data;
using System.IO;
using System.Linq;
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

         _dataTable = CreateDataTable();
      }

      protected DataTable CreateDataTable(string name = "TestSheet")
      {
         var dataTable = new DataTable();

         dataTable.Columns.Add("Column1");
         dataTable.Columns.Add("Column2");

         var row1 = dataTable.NewRow();
         row1["Column1"] = "str1";
         row1["Column2"] = "str2";

         var row2 = dataTable.NewRow();
         row2["Column1"] = "str3";
         row2["Column2"] = "str4";

         dataTable.Rows.Add(row1);
         dataTable.Rows.Add(row2);

         dataTable.TableName = name;
         return dataTable;
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
      [Observation]
      public void should_create_export_file()
      {
         sut.ExportDataTableToExcel(_dataTable, _exportExcelFilePath, false);

         File.Exists(_exportExcelFilePath).ShouldBeTrue();
      }

      [Observation]
      public void should_have_created_sheet()
      {
         sut.ExportDataTableToExcel(_dataTable, _exportExcelFilePath, false);

         var reader = new ExcelReader(_exportExcelFilePath);
         reader.MoveToNextSheet();
         reader.CurrentSheet.SheetName.ShouldBeEqualTo("TestSheet");
      }

      [Observation]
      public void should_have_renamed_the_excel_sheet_having_a_long_name_to_something_unique()
      {
         var dataTable1 = CreateDataTable("A_VERY_LONG_NAME_REQUIRING_THE_CUT_TO_HAPPEN_BECAUSE_EXCEL_IS_STUPID_1");
         var dataTable2 = CreateDataTable("A_VERY_LONG_NAME_REQUIRING_THE_CUT_TO_HAPPEN_BECAUSE_EXCEL_IS_STUPID_2");
         var dataTable3 = CreateDataTable("A_VERY_LONG_NAME_REQUIRING_THE_CUT_TO_HAPPEN_BECAUSE_EXCEL_IS_STUPID_3");
         sut.ExportDataTablesToExcel(new[]{dataTable1, dataTable2, dataTable3 }, _exportExcelFilePath, false);

         var reader = new ExcelReader(_exportExcelFilePath);
         reader.MoveToNextSheet();
         reader.CurrentSheet.SheetName.ShouldBeEqualTo("A_VERY_LONG_NAME_REQUIRING__1");
         reader.MoveToNextSheet();
         reader.CurrentSheet.SheetName.ShouldBeEqualTo("A_VERY_LONG_NAME_REQUIRING__2");
         reader.MoveToNextSheet();
         //This one does not have a number because it's unique in the list now
         reader.CurrentSheet.SheetName.ShouldBeEqualTo("A_VERY_LONG_NAME_REQUIRING_");
      }

      [Observation]
      public void should_have_created_data()
      {
         sut.ExportDataTableToExcel(_dataTable, _exportExcelFilePath, false);

         var reader = new ExcelReader(_exportExcelFilePath);
         reader.MoveToNextSheet();
         reader.MoveToNextRow();
         reader.CurrentRow.ElementAt(0).ShouldBeEqualTo("Column1");
         reader.CurrentRow.ElementAt(1).ShouldBeEqualTo("Column2");
      }
   }
}