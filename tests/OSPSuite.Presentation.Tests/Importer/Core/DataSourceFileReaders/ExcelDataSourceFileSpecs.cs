using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{

   public abstract class ConcernForExcelDataSourceFile : ContextSpecification<ExcelDataSourceFile>
   {
      protected string _excelFilePath;
      protected string _excelFile = "sample1.xlsx";
      protected override void Context()
      {
         sut = new ExcelDataSourceFile(A.Fake<IImportLogger>())
         {
            Path = _excelFilePath
         };
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         _excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", _excelFile);
      }
   }

   public class When_reading_excel : ConcernForExcelDataSourceFile
   {
      [TestCase]
      public void headers_are_ajusted_on_empty_columns()
      {
         sut.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sample2.xlsx");
         var columns = sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetHeaders();
         columns.Count().ShouldBeEqualTo(4);
         for (var i = 0; i < 4; i++)
         {
            sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetColumnDescription(columns.ElementAt(i)).Index.ShouldBeEqualTo(i);
         }
      }

      [TestCase]
      public void path_is_set()
      {
         sut.Path.ShouldBeEqualTo(_excelFilePath);
      }

      [TestCase]
      public void headers_are_read_first_sheet()
      {
         sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetHeaders().Count().ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetHeaders().ElementAt(i - 1).ShouldBeEqualTo("header" + i);
         }
      }

      [TestCase]
      public void headers_are_read_second_sheet()
      {
         sut.DataSheetsDeprecated.ElementAt(1).RawSheetData.GetHeaders().Count().ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheetsDeprecated["Sheet2"].RawSheetData.GetHeaders().ElementAt(i - 1).ShouldBeEqualTo("sheet2_header" + i);
         }
      }
      [TestCase]
      public void body_is_read_first_sheet()
      {
         sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetDataRow(0).Count().ShouldBeEqualTo(3);
         sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetColumn("header1").Count().ShouldBeEqualTo(3);

         var headers = sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetHeaders();

         for (var i = 0; i < 3; i++)
         {
            sut.DataSheetsDeprecated.ElementAt(0).RawSheetData.GetColumn(headers.ElementAt(i)).ShouldBeEqualTo(new[] { "str" + (i + 1), "str" + (i + 4), "str" + (i + 7) });
         }
      }

      [TestCase]
      public void body_is_read_second_sheet()
      {
         sut.DataSheetsDeprecated.ElementAt(1).RawSheetData.GetDataRow(0).Count().ShouldBeEqualTo(3);
         sut.DataSheetsDeprecated.ElementAt(1).RawSheetData.GetColumn("sheet2_header2").Count().ShouldBeEqualTo(2);

         var headers = sut.DataSheetsDeprecated.ElementAt(1).RawSheetData.GetHeaders();
         for (var i = 0; i < 3; i++)
         {
            sut.DataSheetsDeprecated.ElementAt(1).RawSheetData.GetColumn(headers.ElementAt(i)).ShouldBeEqualTo(new[] { "str" + (i + 7), "str" + (i + 10) });
         }
      }

      [TestCase]
      public void measurement_levels_are_read_third_sheet()
      {
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumnDescription("Double").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumnDescription("integer").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumnDescription("string").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Discrete);
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumnDescription("not available").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumnDescription("empty row").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
      }

      [TestCase]
      public void existing_values_are_read_third_sheet()
      {
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumnDescription("string").ExistingValues.ShouldBeEqualTo(new List<string>(){ "str8", "str11" });
      }

      [TestCase]
      public void sheet_names_read_correctly()
      {
         sut.DataSheetsDeprecated.Count.ShouldBeEqualTo(4);
         var keys = sut.DataSheetsDeprecated.Keys;

         for (var i = 0; i < 4; i++)
         {
            keys.ElementAt(i).ShouldBeEqualTo("Sheet" + (i + 1));
         }
      }

      [TestCase]
      public void double_read_with_correct_precision()
      {
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumn("Double").ShouldBeEqualTo(new List<string>(){ "0.000341012439638598" , 34.4399986267089.ToString(CultureInfo.CurrentCulture) });
      }


      [TestCase]
      public void rightmost_column_with_empty_rows_read_correctly()
      {
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetColumn("empty row").ShouldBeEqualTo(new List<string>() { "", "21"});
      }


      [TestCase]
      public void excel_cell_read_correctly()
      {
         sut.DataSheetsDeprecated.ElementAt(2).RawSheetData.GetCell("Double",0).ShouldBeEqualTo("0.000341012439638598");
      }
   }
}
