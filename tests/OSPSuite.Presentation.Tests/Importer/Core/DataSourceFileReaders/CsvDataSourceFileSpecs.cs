using System;
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

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public abstract class ConcernForCsvDataSourceFile : ContextSpecification<CsvDataSourceFile>
   {
      protected string _csvFileCorrect = "sample1.csv";
      protected string _csvFileCorrupt = "sample2.csv";

      protected override void Context()
      {
         var csvSeparatorSelector = A.Fake<ICsvSeparatorSelector>();
         A.CallTo(() => csvSeparatorSelector.GetCsvSeparator(A<string>._)).Returns(new CSVSeparators { ColumnSeparator = ',', DecimalSeparator = '.' });

         sut = new CsvDataSourceFile(A.Fake<IImportLogger>(), csvSeparatorSelector, new HeavyWorkManagerForSpecs());
      }

      protected string getFileFullName(string fileName) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", fileName);
   }

   public class When_reading_csv : ConcernForCsvDataSourceFile
   {
      protected string _csvFilePath;

      protected override void Context()
      {
         base.Context();
         _csvFilePath = getFileFullName(_csvFileCorrect);
      }

      protected override void Because()
      {
         sut.Path = _csvFilePath;
      }

      [TestCase]
      public void path_is_set()
      {
         sut.Path.ShouldBeEqualTo(_csvFilePath);
      }

      [TestCase]
      public void headers_are_read()
      {
         sut.DataSheets.GetDataSheetByName("").GetHeaders().Count().ShouldBeEqualTo(5);
         var i = 1;
         foreach (var sheet in sut.DataSheets)
         {
            sheet.GetHeaders().ElementAt(i - 1).ShouldBeEqualTo("header" + i);
         }
      }

      [TestCase]
      public void body_is_read()
      {
         sut.DataSheets.ElementAt(0).GetDataRow(0).Count().ShouldBeEqualTo(5);

         //actually the problem here is the way we have written the test. this should be changed
         var headers = sut.DataSheets.ElementAt(0).GetHeaders();

         for (var i = 0; i < 3; i++)
         {
            sut.DataSheets.ElementAt(0).GetColumn(headers.ElementAt(i)).ShouldBeEqualTo(new[] { "str" + (i + 1), "str" + (i + 4), "str" + (i + 7) });
         }
      }

      [TestCase]
      public void measurement_levels_are_read_discrete()
      {
         sut.DataSheets.ElementAt(0).GetColumnDescription("header1").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Discrete);
         sut.DataSheets.ElementAt(0).GetColumnDescription("header2").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Discrete);
         sut.DataSheets.ElementAt(0).GetColumnDescription("header3").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Discrete);
      }

      [TestCase]
      public void measurement_levels_are_read_integer()
      {
         sut.DataSheets.ElementAt(0).GetColumnDescription("header4").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
      }

      [TestCase]
      public void measurement_levels_are_read_double()
      {
         //note: decimal separator info can also be set directly:
         /*
          * var ci = new CultureInfo(currentCulture)
            {
               NumberFormat = { NumberDecimalSeparator = "," },
               DateTimeFormat = { DateSeparator = "/" }
            };
          */
         var culturesList = new string[] { "de - DE", "es-AR", "zh - Hans", "ja-JP", "en - US" };
         foreach (var culture in culturesList)
         {
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            sut.DataSheets.ElementAt(0).GetColumnDescription("header5").Level.ShouldBeEqualTo(ColumnDescription.MeasurementLevel.Numeric);
         }
      }
   }

   public class When_reading_corrupt_csv : ConcernForCsvDataSourceFile
   {
      protected string _csvFilePath;

      protected override void Context()
      {
         base.Context();
         _csvFilePath = getFileFullName(_csvFileCorrupt);
      }

      [Observation]
      public void duplicate_header_file_throws_exception()
      {
         Assert.Throws<InvalidObservedDataFileException>(() => sut.Path = _csvFilePath);
      }
   }
}