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
   public abstract class concern_for_CsvDataSourceFile : ContextSpecification<CsvDataSourceFile>
   {
      protected string csvFilePath;
      private string csvFile = "sample1.csv";

      protected override void Context()
      {
         sut = new CsvDataSourceFile(csvFilePath, A.Fake<IImportLogger>());
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
         sut.DataSheets.ElementAt(0).Value.RawData.getHeadersList().Count.ShouldBeEqualTo(5);
         for (var i = 1; i <= 5; i++)
         {
            sut.DataSheets.ElementAt(0).Value.RawData.getHeadersList().ElementAt(i - 1).ShouldBeEqualTo("header" + i);
         }
      }

      [TestCase]
      public void boddy_is_read()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.GetRow(0).Count.ShouldBeEqualTo(5);
         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(0).Value.RawData.GetColumn(i).ShouldBeEqualTo(new[] { "str" + (i + 1), "str" + (i + 4), "str" + (i + 7) });
         }
      }

      [TestCase]
      public void measurement_levels_are_read_discrete()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.Headers["header1"].Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.DISCRETE);
         sut.DataSheets.ElementAt(0).Value.RawData.Headers["header2"].Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.DISCRETE);
         sut.DataSheets.ElementAt(0).Value.RawData.Headers["header3"].Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.DISCRETE);
      }

      [TestCase]
      public void measurement_levels_are_read_integer()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.Headers["header4"].Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.NUMERIC);
      }

      [TestCase]
      public void measurement_levels_are_read_double()
      {
         sut.DataSheets.ElementAt(0).Value.RawData.Headers["header5"].Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.NUMERIC);
      }

   }
}
