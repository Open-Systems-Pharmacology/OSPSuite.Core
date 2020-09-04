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

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{

   public abstract class ConcernForExcelDataSourceFile : ContextSpecification<ExcelDataSourceFile>
   {
      protected string _excelFilePath;
      private string _excelFile = "sample1.xlsx";
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
      public void path_is_set()
      {
         sut.Path.ShouldBeEqualTo(_excelFilePath);
      }


      [TestCase]
      public void headers_are_read_first_sheet()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.GetHeaders().Count().ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheets.ElementAt(0).Value.RawData.GetHeaders().ElementAt(i - 1).ShouldBeEqualTo("header" + i);
         }
      }

      [TestCase]
      public void headers_are_read_second_sheet()
      {
         sut.DataSheets.ElementAt(1).Value.RawData.GetHeaders().Count().ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheets["Sheet2"].RawData.GetHeaders().ElementAt(i - 1).ShouldBeEqualTo("sheet2_header" + i);
         }
      }
      [TestCase]
      public void body_is_read_first_sheet()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.GetDataRow(0).Count().ShouldBeEqualTo(3);
         sut.DataSheets.ElementAt(0).Value.RawData.GetColumn("header1").Count().ShouldBeEqualTo(3);

         var headers = sut.DataSheets.ElementAt(0).Value.RawData.GetHeaders();

         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(0).Value.RawData.GetColumn(headers.ElementAt(i)).ShouldBeEqualTo(new[] { "str" + (i + 1), "str" + (i + 4), "str" + (i + 7) });
         }
      }

      [TestCase]
      public void body_is_read_second_sheet()
      {
         sut.DataSheets.ElementAt(1).Value.RawData.GetDataRow(0).Count().ShouldBeEqualTo(3);
         sut.DataSheets.ElementAt(1).Value.RawData.GetColumn("sheet2_header2").Count().ShouldBeEqualTo(2);

         var headers = sut.DataSheets.ElementAt(1).Value.RawData.GetHeaders();
         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(1).Value.RawData.GetColumn(headers.ElementAt(i)).ShouldBeEqualTo(new[] { "str" + (i + 7), "str" + (i + 10) });
         }
      }

      [TestCase]
      public void measurement_levels_are_read_third_sheet()
      {
         sut.DataSheets.ElementAt(2).Value.RawData.GetColumnDescription("Double").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheets.ElementAt(2).Value.RawData.GetColumnDescription("integer").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         sut.DataSheets.ElementAt(2).Value.RawData.GetColumnDescription("string").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Discrete);
         sut.DataSheets.ElementAt(2).Value.RawData.GetColumnDescription("not available").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
      }

      [TestCase]
      public void existing_values_are_read_third_sheet()
      {
         sut.DataSheets.ElementAt(2).Value.RawData.GetColumnDescription("string").ExistingValues.ShouldBeEqualTo(new List<string>(){ "str8", "str11" });
      }

      [TestCase]
      public void sheet_names_read_correctly()
      {
         sut.DataSheets.Count.ShouldBeEqualTo(3);

         for (var i = 0; i < 2; i++)
         {
            sut.DataSheets.ElementAt(i).Key.ShouldBeEqualTo("Sheet" + (i + 1));
         }
      }

      [TestCase]
      public void double_read_with_correct_precision()
      {
         sut.DataSheets.ElementAt(2).Value.RawData.GetColumn("Double").ShouldBeEqualTo(new List<string>(){ "0.000341012439638598" , 34.4399986267089.ToString(CultureInfo.CurrentCulture) });
      }


      [TestCase]
      public void excel_cell_read_correctly()
      {
         sut.DataSheets.ElementAt(2).Value.RawData.GetCell("Double",0).ShouldBeEqualTo("0.000341012439638598");
      }

   }
}
