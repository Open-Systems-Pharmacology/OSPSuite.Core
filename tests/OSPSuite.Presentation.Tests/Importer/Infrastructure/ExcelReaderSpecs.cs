using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using System;
using System.IO;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Infrastructure
{
   public abstract class concern_for_ExcelReader : ContextSpecification<ExcelReader>
   {
      protected string[] excelFilePath = new string[2];
      private readonly string[] excelFile = {"sample1.xlsx", "sample1.xls"};

      protected override void Context()
      {
         sut = new ExcelReader();
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         for (int i = 0; i < 2; i++)
         {
            excelFilePath[i] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", excelFile[i]);
         }
      }
   }

   public class when_using_excel_reader : concern_for_ExcelReader
   {
      [TestCase]
      public void first_sheet_read_correctly()
      {
         foreach (var path in excelFilePath)
         {
            sut.LoadNewWorkbook(path);
            sut.MoveToNextSheet();
            sut.MoveToNextRow();
            sut.CurrentRow.Count.ShouldBeEqualTo(3);
            sut.CurrentRow[0].ShouldBeEqualTo("header1");
         }
      }

      [TestCase]
      public void second_sheet_read_correctly()
      {
         foreach (var path in excelFilePath)
         {
            sut.LoadNewWorkbook(path);
            sut.MoveToNextSheet();
            sut.MoveToNextSheet();
            sut.MoveToNextRow();
            sut.CurrentRow.Count.ShouldBeEqualTo(3);
            sut.CurrentRow[1].ShouldBeEqualTo("sheet2_header2");
         }
      }

   }
}