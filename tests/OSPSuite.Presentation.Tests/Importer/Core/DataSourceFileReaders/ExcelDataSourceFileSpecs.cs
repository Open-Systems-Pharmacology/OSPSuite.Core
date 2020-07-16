using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using System;
using System.IO;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{

   public abstract class concern_for_ExcelDataSourceFile : ContextSpecification<ExcelDataSourceFile>
   {
      protected string excelFilePath;
      private string excelFile = "sample1.xlsx";
      protected override void Context()
      {
         sut = new ExcelDataSourceFile(excelFilePath, A.Fake<IImportLogger>());
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", excelFile);
      }
   }

   public class when_reading_excel : concern_for_ExcelDataSourceFile
   {
      [TestCase]
      public void path_is_set()
      {
         sut.Path.ShouldBeEqualTo(excelFilePath);
      }


      [TestCase]
      public void headers_are_read_first_sheet()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.getHeadersList().Count.ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheets.ElementAt(0).Value.RawData.getHeadersList().ElementAt(i - 1).ShouldBeEqualTo("header" + i);
         }
      }

      [TestCase]
      public void headers_are_read_second_sheet()
      {
         sut.DataSheets.ElementAt(1).Value.RawData.getHeadersList().Count.ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataSheets.ElementAt(1).Value.RawData.getHeadersList().ElementAt(i - 1).ShouldBeEqualTo("sheet2_header" + i);
         }
      }
      [TestCase]
      public void boddy_is_read_first_sheet()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.GetRow(0).Count.ShouldBeEqualTo(3);
         sut.DataSheets.ElementAt(0).Value.RawData.GetColumn("header1").Count.ShouldBeEqualTo(3);
         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(0).Value.RawData.GetColumn(i).ShouldBeEqualTo(new[] { "str" + (i + 1), "str" + (i + 4), "str" + (i + 7) });
         }
      }

      [TestCase]
      public void boddy_is_read_second_sheet()
      {
         sut.DataSheets.ElementAt(1).Value.RawData.GetRow(0).Count.ShouldBeEqualTo(3);
         sut.DataSheets.ElementAt(1).Value.RawData.GetColumn("sheet2_header2").Count.ShouldBeEqualTo(2);
         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(1).Value.RawData.GetColumn(i).ShouldBeEqualTo(new[] { "str" + (i + 7), "str" + (i + 10) });
         }
      }

      [TestCase]
      public void sheet_names_read_correctly()
      {
         sut.DataSheets.Count.ShouldBeEqualTo(2);

         for (var i = 0; i < 2; i++)
         {
            sut.DataSheets.ElementAt(i).Key.ShouldBeEqualTo("Sheet" + (i + 1));
         }
      }
   }
}
