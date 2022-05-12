using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataSourceFileReaders;

namespace OSPSuite.Presentation.Importer.Infrastructure
{
   public abstract class ConcernForExcelReader : ContextSpecification<ExcelReader>
   {
      protected string[] _excelFilePath = new string[2];
      private readonly string[] _excelFile = {"sample1.xlsx", "sample1.xls"};

      protected override void Context()
      {
         sut = new ExcelReader(_excelFilePath[0]);
      }

      public override void GlobalContext()
      {
         base.GlobalContext();
         for (var i = 0; i < 2; i++)
         {
            _excelFilePath[i] = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", _excelFile[i]);
         }
      }
   }

   public class When_using_excel_reader : ConcernForExcelReader
   {
      [TestCase]
      public void first_sheet_read()
      {
         foreach (var path in _excelFilePath)
         {
            sut.LoadNewWorkbook(path);
            sut.MoveToNextSheet();
            sut.MoveToNextRow();
            sut.CurrentRow.Count.ShouldBeEqualTo(3);
            sut.CurrentRow[0].ShouldBeEqualTo("header1");
         }
      }

      [TestCase]
      public void second_sheet_read()
      {
         foreach (var path in _excelFilePath)
         {
            sut.LoadNewWorkbook(path);
            sut.MoveToNextSheet();
            sut.MoveToNextSheet();
            sut.MoveToNextRow();
            sut.CurrentRow.Count.ShouldBeEqualTo(3);
            sut.CurrentRow[1].ShouldBeEqualTo("sheet2_header2");
         }
      }

      [TestCase]
      public void measurement_levels_read()
      {
         foreach (var path in _excelFilePath)
         {
            sut.LoadNewWorkbook(path);
            for ( var i = 0; i < 3; i++)
               sut.MoveToNextSheet();
            for (var i = 0; i < 2; i++)
               sut.MoveToNextRow();
            sut.GetMeasurementLevels(5).ShouldBeEqualTo(new List<ColumnDescription.MeasurementLevel>() {ColumnDescription.MeasurementLevel.Numeric, ColumnDescription.MeasurementLevel.Discrete, ColumnDescription.MeasurementLevel.Numeric, ColumnDescription.MeasurementLevel.Numeric, ColumnDescription.MeasurementLevel.Numeric });
         }
      }

      [TestCase]
      public void new_excel_formula_read()
      {
         foreach (var path in _excelFilePath)
         {
            sut.LoadNewWorkbook(_excelFilePath[0]);
            for (var i = 0; i < 4; i++)
               sut.MoveToNextSheet();
            for (var i = 0; i < 2; i++)
               sut.MoveToNextRow();
            sut.CurrentRow[0].ShouldBeEqualTo("Concat_test1");
            sut.MoveToNextRow();
            sut.CurrentRow[0].ShouldBeEqualTo("Concat_test2");
         }
      }
   }
}