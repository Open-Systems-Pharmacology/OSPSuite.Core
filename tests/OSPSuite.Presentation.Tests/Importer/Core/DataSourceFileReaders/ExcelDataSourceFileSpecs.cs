using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public abstract class ConcernForExcelDataSourceFile : ContextSpecification<ExcelDataSourceFile>
   {
      protected string _excelFilePath;
      protected string _excelFile = "sample1.xlsx";
      protected IHeavyWorkManager workManager;
      protected override void Context()
      {
         workManager = new HeavyWorkManagerForSpecs();

         sut = new ExcelDataSourceFile(A.Fake<IImportLogger>(), workManager)
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
      public void headers_are_adjusted_on_empty_columns()
      {
         sut.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "sample2.xlsx");
         var columns = sut.DataSheets.ElementAt(0).GetHeaders();
         columns.Count().ShouldBeEqualTo(4);
         for (var i = 0; i < 4; i++)
         {
            sut.DataSheets.ElementAt(0).GetColumnDescription(columns.ElementAt(i)).Index.ShouldBeEqualTo(i);
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
         sut.DataSheets.ElementAt(0).GetHeaders().Count().ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheets.ElementAt(0).GetHeaders().ElementAt(i - 1).ShouldBeEqualTo("header" + i);
         }
      }

      [TestCase]
      public void headers_are_read_second_sheet()
      {
         sut.DataSheets.ElementAt(1).GetHeaders().Count().ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheets.GetDataSheetByName("Sheet2").GetHeaders().ElementAt(i - 1).ShouldBeEqualTo("sheet2_header" + i);
         }
      }

      [TestCase]
      public void body_is_read_first_sheet()
      {
         sut.DataSheets.ElementAt(0).GetDataRow(0).Count().ShouldBeEqualTo(3);
         sut.DataSheets.ElementAt(0).GetColumn("header1").Count().ShouldBeEqualTo(3);

         var headers = sut.DataSheets.ElementAt(0).GetHeaders();

         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(0).GetColumn(headers.ElementAt(i)).ShouldBeEqualTo(new[] { "str" + (i + 1), "str" + (i + 4), "str" + (i + 7) });
         }
      }

      [TestCase]
      public void body_is_read_second_sheet()
      {
         sut.DataSheets.ElementAt(1).GetDataRow(0).Count().ShouldBeEqualTo(3);
         sut.DataSheets.ElementAt(1).GetColumn("sheet2_header2").Count().ShouldBeEqualTo(2);

         var headers = sut.DataSheets.ElementAt(1).GetHeaders();
         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(1).GetColumn(headers.ElementAt(i)).ShouldBeEqualTo(new[] { "str" + (i + 7), "str" + (i + 10) });
         }
      }

      [TestCase]
      public void measurement_levels_are_read_third_sheet()
      {
         sut.DataSheets.ElementAt(2).GetColumnDescription("Double").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheets.ElementAt(2).GetColumnDescription("integer").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheets.ElementAt(2).GetColumnDescription("string").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Discrete);
         sut.DataSheets.ElementAt(2).GetColumnDescription("not available").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheets.ElementAt(2).GetColumnDescription("empty row").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
      }

      [TestCase]
      public void existing_values_are_read_third_sheet()
      {
         sut.DataSheets.ElementAt(2).GetColumnDescription("string").ExistingValues.ShouldBeEqualTo(new List<string>() { "str8", "str11" });
      }

      [TestCase]
      public void sheet_names_read_correctly()
      {
         sut.DataSheets.Count().ShouldBeEqualTo(4);
         var keys = sut.DataSheets.GetDataSheetNames();

         for (var i = 0; i < 4; i++)
         {
            keys.ElementAt(i).ShouldBeEqualTo("Sheet" + (i + 1));
         }
      }

      [TestCase]
      public void double_read_with_correct_precision()
      {
         sut.DataSheets.ElementAt(2).GetColumn("Double").ShouldBeEqualTo(new List<string>() { "0.000341012439638598", 34.4399986267089.ToString(CultureInfo.CurrentCulture) });
      }

      [TestCase]
      public void rightmost_column_with_empty_rows_read_correctly()
      {
         sut.DataSheets.ElementAt(2).GetColumn("empty row").ShouldBeEqualTo(new List<string>() { "", "21" });
      }

      [TestCase]
      public void excel_cell_read_correctly()
      {
         sut.DataSheets.ElementAt(2).GetCell("Double", 0).ShouldBeEqualTo("0.000341012439638598");
      }
   }
}