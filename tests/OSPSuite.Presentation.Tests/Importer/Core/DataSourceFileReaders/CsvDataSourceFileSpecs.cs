using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
using System.IO;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public abstract class concern_for_CsvDataSourceFile : ContextSpecification<CsvDataSourceFile>
   {
      protected string csvFilePath;
      private string csvFile = "sample1.csv";

      protected override void Context()
      {
         sut = new CsvDataSourceFile(csvFilePath, null);
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", csvFile);
      }
   }

   public class when_reading_csv : concern_for_CsvDataSourceFile
   {
      [TestCase]
      public void path_is_set()
      {
         sut.Path.ShouldBeEqualTo(csvFilePath);
      }

      [TestCase]
      public void headers_are_read()
      {
         sut.DataTables.ElementAt(0).Value.RawData.Keys.Count.ShouldBeEqualTo(3);
         for (var i = 1; i <= 3; i++)
         {
            sut.DataTables.ElementAt(0).Value.RawData.Keys.ElementAt(i - 1).ShouldBeEqualTo("header" + i);
         }
      }

      [TestCase]
      public void boddy_is_read()
      {
         sut.DataTables.ElementAt(0).Value.RawData.Values.Count.ShouldBeEqualTo(3);
         for (var i = 0; i < 3; i++)
         {
            sut.DataTables.ElementAt(0).Value.RawData.Values.ElementAt(i).ShouldBeEqualTo(new[] { "str" + (i + 1), "str" + (i + 4) });
         }
      }
   }
}
