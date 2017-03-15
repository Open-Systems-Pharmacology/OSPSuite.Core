using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Importer
{
   [Category("Importer")]
   public abstract class concern_for_Importer : ContextSpecification<Presentation.Services.Importer.Importer>
   {
      protected IImporterTask _importerTask;
      private IDialogCreator _dialogCreator;

      public override void GlobalContext()
      {
         var dataRepositoryMapper = A.Fake<IImportDataTableToDataRepositoryMapper>();
         _dialogCreator = A.Fake<IDialogCreator>();
         var columnInfos = A.Fake<IReadOnlyList<ColumnInfo>>();
         _importerTask = new ImporterTask(new ColumnCaptionHelper(), new LowerLimitOfQuantificationTask());
         sut = new Presentation.Services.Importer.Importer(dataRepositoryMapper, columnInfos, _importerTask, _dialogCreator);
      }
   }

   public class when_excel_file_is_test1 : concern_for_Importer
   {
      protected string _excelFile;
      protected ImportDataTable _importDataTable = new ImportDataTable();
      protected IList<string> _sheetNames;
      protected IList<string> _columnNames;
      protected List<ColumnMapping> _cms;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _excelFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data//Test1.xls");

         _sheetNames = new List<string>(3) {"Sheet1", "Sheet2", "Sheet3"};
         _columnNames = new List<string>(4) {"TextColumn", "NumberColumn", "DateColumn", "BoolColumn"};

         _importDataTable.Columns.Add(new ImportDataColumn {DataType = typeof(string)});
         _importDataTable.Columns.Add(new ImportDataColumn {DataType = typeof(double)});
         _importDataTable.Columns.Add(new ImportDataColumn {DataType = typeof(DateTime)});
         _importDataTable.Columns.Add(new ImportDataColumn {DataType = typeof(bool)});

         _cms = new List<ColumnMapping>(4);
         for (int i = 0; i < _columnNames.Count; i++)
         {
            var cm = new ColumnMapping
            {
               SourceColumn = _columnNames[i],
               Target = _importDataTable.Columns.ItemByIndex(i).ColumnName
            };
            _cms.Add(cm);
         }
      }

      protected override void Context()
      {
         base.Context();
         _importDataTable.Rows.Clear();
      }

      #region test cases

      #region All Sheets

      [Test]
      public void should_retrieve_proper_sheet_names()
      {
         var sheetNames = sut.GetSheetNames(_excelFile);
         sheetNames.Count.ShouldBeEqualTo(_sheetNames.Count);
         sheetNames[0].ShouldBeEqualTo(_sheetNames[0]);
         sheetNames[1].ShouldBeEqualTo(_sheetNames[1]);
         sheetNames[2].ShouldBeEqualTo(_sheetNames[2]);
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_getting_sheet_names()
      {
         The.Action(() => sut.GetSheetNames(_excelFile + "Unknown")).ShouldThrowAn<FileNotFoundException>();
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_getting_column_names()
      {
         The.Action(() => sut.GetColumnNames(_excelFile + "Unknown", _sheetNames[0], 0)).ShouldThrowAn<FileNotFoundException>();
      }

      [Observation]
      public void should_throw_exception_invalid_sheet_for_getting_column_names()
      {
         The.Action(() => sut.GetColumnNames(_excelFile, "UnknownSheet", 0)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_retrieve_proper_column_names()
      {
         var columnNames = sut.GetColumnNames(_excelFile, _sheetNames[0], 0);
         columnNames.Count.ShouldBeEqualTo(_columnNames.Count);
         columnNames[0].ShouldBeEqualTo(_columnNames[0]);
         columnNames[1].ShouldBeEqualTo(_columnNames[1]);
         columnNames[2].ShouldBeEqualTo(_columnNames[2]);
         columnNames[3].ShouldBeEqualTo(_columnNames[3]);
      }

      [Observation]
      public void should_throw_exception_invalid_sheet_for_fill_data_table()
      {
         The.Action(
               () => sut.ImportDataTables(_importDataTable, _excelFile, "Unknown sheet", 0, 0, 1, -1, _cms)).ShouldThrowAn
            <SheetNotFoundException>();
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_fill_data_table()
      {
         The.Action(
               () => sut.ImportDataTables(_importDataTable, _excelFile + "Unknown", _sheetNames[0], 0, 0, 1, -1, _cms)).
            ShouldThrowAn<FileNotFoundException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_caption_row_negative()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], -1, 0, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_caption_row_too_large()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 60, 0, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_unit_row_negative()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, -2, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_unit_row_too_large()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 60, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_data_start_row_negative()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, -1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_data_start_row_too_large()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 60, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_excel_column_not_found_for_fill_data_table_data()
      {
         var cms = new List<ColumnMapping>(_cms.Count - 1);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1]) cm.SourceColumn += "Unknown";
            cms.Add(cm);
         }
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 1, -1, cms)).
            ShouldThrowAn<ExcelColumnNotFoundException>();
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], cms)).ShouldThrowAn
            <ExcelColumnNotFoundException>();
      }

      [Observation]
      public void should_throw_not_all_data_columns_are_mapped_for_fill_data_table_data()
      {
         var cms = new List<ColumnMapping>(_cms.Count - 1);
         ImportDataTable testData = _importDataTable.Clone();
         testData.Columns.ItemByIndex(1).Required = true;

         cms.AddRange(_cms.Select(ocm => new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target}).Where(cm => cm.SourceColumn != _columnNames[1]));

         The.Action(() => sut.ImportDataTables(testData, _excelFile, _sheetNames[0], 0, 0, 1, -1, cms)).
            ShouldThrowAn<NoMappingForDataColumnException>();
         The.Action(() => sut.ImportDataTables(testData, _excelFile, _sheetNames[0], cms)).ShouldThrowAn
            <NoMappingForDataColumnException>();
      }

      [Observation]
      public void should_throw_different_data_types_for_fill_data_table_data()
      {
         ImportDataTable testTable = _importDataTable.Clone();
         testTable.Columns.ItemByIndex(1).DataType = typeof(string);

         The.Action(() => sut.ImportDataTables(testTable, _excelFile, _sheetNames[0], 0, 0, 1, -1, _cms)).ShouldThrowAn
            <DifferentDataTypeException>();
         The.Action(() => sut.ImportDataTables(testTable, _excelFile, _sheetNames[0], _cms)).ShouldThrowAn
            <DifferentDataTypeException>();
      }

      [Observation]
      public void should_guess_first_data_row()
      {
         sut.GetFirstDataRowGuess(_excelFile, _sheetNames[0]).ShouldBeEqualTo(1);
         sut.GetFirstDataRowGuess(_excelFile, _sheetNames[1]).ShouldBeEqualTo(1);
         sut.GetFirstDataRowGuess(_excelFile, _sheetNames[2]).ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_guess_unit_row()
      {
         sut.GetUnitRowGuess(_excelFile, _sheetNames[0]).ShouldBeEqualTo(-1);
         sut.GetUnitRowGuess(_excelFile, _sheetNames[1]).ShouldBeEqualTo(0);
         sut.GetUnitRowGuess(_excelFile, _sheetNames[2]).ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_get_preview()
      {
         var preview = sut.GetPreview(_excelFile, 10, A.Fake<Cache<string, Rectangle>>());
         preview.Tables.Count.ShouldBeEqualTo(_sheetNames.Count);
         preview.Tables[0].Rows.Count.ShouldBeEqualTo(10);
         preview.Tables[0].TableName.ShouldBeEqualTo(_sheetNames[0]);
         preview.Tables[1].TableName.ShouldBeEqualTo(_sheetNames[1]);
         preview.Tables[2].TableName.ShouldBeEqualTo(_sheetNames[2]);
         sut.GetPreview(_excelFile, 20, A.Fake<Cache<string, Rectangle>>()).Tables[1].Rows.Count.ShouldBeEqualTo(20);
         sut.GetPreview(_excelFile, 30, A.Fake<Cache<string, Rectangle>>()).Tables[2].Rows.Count.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_get_preview_should_throw_argument_out_of_range()
      {
         The.Action(() => sut.GetPreview(_excelFile, -1, A.Fake<Cache<string, Rectangle>>())).ShouldThrowAn<ArgumentOutOfRangeException>();
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_getting_preview()
      {
         The.Action(() => sut.GetPreview(_excelFile + "Unknown", 10, A.Fake<Cache<string, Rectangle>>())).ShouldThrowAn<FileNotFoundException>();
      }

      #endregion

      #region Sheet1

      [Observation]
      public void should_fill_data_table_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(49);
      }

      [Observation]
      public void should_fill_data_table_with_less_rows_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 1, 30, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_fill_data_table_with_selected_rows_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 5, 10, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(6);
         var i = 0.0;
         foreach (ImportDataRow row in tables[0].Rows)
         {
            ((double) row[1]).ShouldBeEqualTo(5.0 + i++);
         }
      }

      [Observation]
      public void should_throw_ospsuite_exception_if_data_end_row_less_than_data_start_row()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 5, 4, _cms)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_not_fill_data_table_with_more_rows_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 1, 99, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(49);
      }

      [Observation]
      public void should_fill_data_table_with_skipping_rows_sheet1()
      {
         ImportDataTable testTable = _importDataTable.Clone();
         testTable.Columns.ItemByIndex(1).SkipNullValueRows = true;
         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[0], 0, 0, 1, -1, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(46);
      }

      #endregion

      #region Sheet2

      [Observation]
      public void should_throw_invalid_unit_sheet2()
      {
         var testTable = _importDataTable.Clone();

         var col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "hour", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };
         var cms = new List<ColumnMapping>(_cms.Count);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1])
            {
               cm.SourceColumn += " [h]";
               cm.SelectedUnit = new Unit {Name = "h"};
            }
            cms.Add(cm);
         }

         The.Action(() => sut.ImportDataTables(testTable, _excelFile, _sheetNames[1], 0, 0, 1, -1, cms)).ShouldThrowAn
            <InvalidUnitForExcelColumnException>();
      }

      [Observation]
      public void should_throw_missing_unit_spec_sheet2()
      {
         var cms = new List<ColumnMapping>(_cms.Count);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1]) cm.SourceColumn += " [h]";
            cms.Add(cm);
         }

         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[1], 0, 0, 1, -1, cms)).
            ShouldThrowAn<DataColumnHasNoUnitInformationException>();
      }

      [Observation]
      public void should_fill_data_table_sheet2()
      {
         ImportDataTable testTable = _importDataTable.Clone();

         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };
         var cms = new List<ColumnMapping>(_cms.Count);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1]) cm.SourceColumn += " [h]";
            cms.Add(cm);
         }

         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[1], 0, 0, 1, -1, cms);
         tables[0].Rows.Count.ShouldBeEqualTo(49);
      }

      [Observation]
      public void should_fill_data_table_with_less_rows_sheet2()
      {
         ImportDataTable testTable = _importDataTable.Clone();

         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };
         var cms = new List<ColumnMapping>(_cms.Count);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1]) cm.SourceColumn += " [h]";
            cms.Add(cm);
         }

         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[1], 0, 0, 1, 30, cms);
         tables[0].Rows.Count.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_not_fill_data_table_with_more_rows_sheet2()
      {
         ImportDataTable testTable = _importDataTable.Clone();

         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };
         var cms = new List<ColumnMapping>(_cms.Count);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1]) cm.SourceColumn += " [h]";
            cms.Add(cm);
         }

         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[1], 0, 0, 1, 99, cms);
         tables[0].Rows.Count.ShouldBeEqualTo(49);
      }

      [Observation]
      public void should_fill_data_table_with_skipping_rows_sheet2()
      {
         ImportDataTable testTable = _importDataTable.Clone();
         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.SkipNullValueRows = true;
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };
         var cms = new List<ColumnMapping>(_cms.Count);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1]) cm.SourceColumn += " [h]";
            cms.Add(cm);
         }
         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[1], 0, 0, 1, -1, cms);
         tables[0].Rows.Count.ShouldBeEqualTo(47);
      }

      #endregion

      #region Sheet3

      [Observation]
      public void should_throw_invalid_unit_sheet3()
      {
         ImportDataTable testTable = _importDataTable.Clone();

         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "hour", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };
         var cms = new List<ColumnMapping>(_cms.Count);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1])
               cm.SelectedUnit = new Unit {Name = "h"};
            cms.Add(cm);
         }

         The.Action(() => sut.ImportDataTables(testTable, _excelFile, _sheetNames[2], 0, 1, 2, -1, cms)).ShouldThrowAn
            <InvalidUnitForExcelColumnException>();
      }

      [Observation]
      public void should_throw_missing_unit_spec_sheet3()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[2], 0, 1, 2, -1, _cms)).
            ShouldThrowAn<DataColumnHasNoUnitInformationException>();
      }

      [Observation]
      public void should_fill_data_table_sheet3()
      {
         ImportDataTable testTable = _importDataTable.Clone();

         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };

         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[2], 0, 1, 2, -1, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(49);
      }

      [Observation]
      public void should_fill_data_table_with_less_rows_sheet3()
      {
         ImportDataTable testTable = _importDataTable.Clone();

         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };

         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[2], 0, 1, 2, 30, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(29);
      }

      [Observation]
      public void should_not_fill_data_table_with_more_rows_sheet3()
      {
         ImportDataTable testTable = _importDataTable.Clone();

         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };

         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[2], 0, 1, 2, 99, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(49);
      }

      [Observation]
      public void should_fill_data_table_with_skipping_rows_sheet3()
      {
         ImportDataTable testTable = _importDataTable.Clone();
         ImportDataColumn col = testTable.Columns.ItemByIndex(1);
         col.SkipNullValueRows = true;
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "h", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };

         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[2], 0, 1, 2, -1, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(46);
      }

      #endregion

      #endregion
   }

   public class When_ExcelFile_is_Test5 : concern_for_Importer
   {
      protected string _excelFile;
      protected ImportDataTable _importDataTable = new ImportDataTable();
      protected IList<string> _sheetNames;
      protected IList<string> _columnNames;
      protected List<ColumnMapping> _cms;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _excelFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Test5.xlsx");

         _sheetNames = new List<string>(3) {"Tabelle1", "Tabelle2", "Tabelle3"};
         _columnNames = new List<string>(2) {"Time", "Concentration"};

         _importDataTable.Columns.Add(new ImportDataColumn {DataType = typeof(double)});
         _importDataTable.Columns.Add(new ImportDataColumn {DataType = typeof(double)});

         var col = _importDataTable.Columns.ItemByIndex(0);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Time",
               IsDefault = true,
               Name = "Time",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "hour", DisplayName = "in Stunden"},
                  new Unit {IsDefault = true, Name = "d", DisplayName = "in Tagen"}
               }
            }
         };

         col = _importDataTable.Columns.ItemByIndex(1);
         col.Dimensions = new List<Dimension>
         {
            new Dimension
            {
               DisplayName = "Concentration",
               IsDefault = true,
               Name = "Concentration",
               Units = new List<Unit>
               {
                  new Unit
                     {IsDefault = false, Name = "mg/l", DisplayName = "Milligram per litre"},
                  new Unit {IsDefault = true, Name = "µg/l", DisplayName = "Microgram per litre"}
               }
            }
         };

         _cms = new List<ColumnMapping>(4);
         for (int i = 0; i < _columnNames.Count; i++)
         {
            var cm = new ColumnMapping
            {
               SourceColumn = _columnNames[i],
               Target = _importDataTable.Columns.ItemByIndex(i).ColumnName
            };
            _cms.Add(cm);
         }
      }

      protected override void Context()
      {
         base.Context();
         _importDataTable.Rows.Clear();
      }

      #region test cases

      #region All Sheets

      [Test]
      public void should_retrieve_proper_sheet_names()
      {
         var sheetNames = sut.GetSheetNames(_excelFile);
         sheetNames.Count.ShouldBeEqualTo(_sheetNames.Count);
         sheetNames[0].ShouldBeEqualTo(_sheetNames[0]);
         sheetNames[1].ShouldBeEqualTo(_sheetNames[1]);
         sheetNames[2].ShouldBeEqualTo(_sheetNames[2]);
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_getting_sheet_names()
      {
         The.Action(() => sut.GetSheetNames(_excelFile + "Unknown")).ShouldThrowAn<FileNotFoundException>();
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_getting_column_names()
      {
         The.Action(() => sut.GetColumnNames(_excelFile + "Unknown", _sheetNames[0], 0)).ShouldThrowAn<FileNotFoundException>();
      }

      [Observation]
      public void should_throw_exception_invalid_sheet_for_getting_column_names()
      {
         The.Action(() => sut.GetColumnNames(_excelFile, "UnknownSheet", 0)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_retrieve_proper_column_names()
      {
         var columnNames = sut.GetColumnNames(_excelFile, _sheetNames[0], 0);
         columnNames.Count.ShouldBeEqualTo(_columnNames.Count);
         columnNames[0].ShouldBeEqualTo(_columnNames[0]);
         columnNames[1].ShouldBeEqualTo(_columnNames[1]);
      }

      [Observation]
      public void should_throw_exception_invalid_sheet_for_fill_data_table()
      {
         The.Action(
               () => sut.ImportDataTables(_importDataTable, _excelFile, "Unknown sheet", 0, 0, 1, -1, _cms)).ShouldThrowAn
            <SheetNotFoundException>();
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_fill_data_table()
      {
         The.Action(
               () => sut.ImportDataTables(_importDataTable, _excelFile + "Unknown", _sheetNames[0], 0, 0, 1, -1, _cms)).
            ShouldThrowAn<FileNotFoundException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_caption_row_negative()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], -1, 0, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_caption_row_too_large()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 60, 0, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_unit_row_negative()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, -2, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_unit_row_too_large()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 60, 1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_data_start_row_negative()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, -1, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_ospsuite_exception_for_fill_data_table_data_start_row_too_large()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 60, -1, _cms)).
            ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_throw_excel_column_not_found_for_fill_data_table_data()
      {
         var cms = new List<ColumnMapping>(_cms.Count - 1);
         foreach (ColumnMapping ocm in _cms)
         {
            var cm = new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target};
            if (cm.SourceColumn == _columnNames[1]) cm.SourceColumn += "Unknown";
            cms.Add(cm);
         }
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 0, 1, -1, cms)).
            ShouldThrowAn<ExcelColumnNotFoundException>();
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], cms)).ShouldThrowAn
            <ExcelColumnNotFoundException>();
      }

      [Observation]
      public void should_throw_not_all_data_columns_are_mapped_for_fill_data_table_data()
      {
         var cms = new List<ColumnMapping>(_cms.Count - 1);
         ImportDataTable testData = _importDataTable.Clone();
         testData.Columns.ItemByIndex(1).Required = true;

         cms.AddRange(_cms.Select(ocm => new ColumnMapping {SourceColumn = ocm.SourceColumn, Target = ocm.Target}).Where(cm => cm.SourceColumn != _columnNames[1]));

         The.Action(() => sut.ImportDataTables(testData, _excelFile, _sheetNames[0], 0, 0, 1, -1, cms)).
            ShouldThrowAn<NoMappingForDataColumnException>();
         The.Action(() => sut.ImportDataTables(testData, _excelFile, _sheetNames[0], cms)).ShouldThrowAn
            <NoMappingForDataColumnException>();
      }

      [Observation]
      public void should_guess_first_data_row()
      {
         sut.GetFirstDataRowGuess(_excelFile, _sheetNames[0]).ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_guess_caption_row()
      {
         sut.GetCaptionRowGuess(_excelFile, _sheetNames[0]).ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_guess_unit_row()
      {
         sut.GetUnitRowGuess(_excelFile, _sheetNames[0]).ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_get_preview()
      {
         var preview = sut.GetPreview(_excelFile, 3, A.Fake<Cache<string, Rectangle>>());
         preview.Tables.Count.ShouldBeEqualTo(_sheetNames.Count);
         preview.Tables[0].Rows.Count.ShouldBeEqualTo(3);
         preview.Tables[0].TableName.ShouldBeEqualTo(_sheetNames[0]);
         preview.Tables[1].TableName.ShouldBeEqualTo(_sheetNames[1]);
         preview.Tables[2].TableName.ShouldBeEqualTo(_sheetNames[2]);
      }

      [Observation]
      public void should_get_preview_should_throw_argument_out_of_range()
      {
         The.Action(() => sut.GetPreview(_excelFile, -1, A.Fake<Cache<string, Rectangle>>())).ShouldThrowAn<ArgumentOutOfRangeException>();
      }

      [Observation]
      public void should_throw_exception_file_not_found_for_getting_preview()
      {
         The.Action(() => sut.GetPreview(_excelFile + "Unknown", 10, A.Fake<Cache<string, Rectangle>>())).ShouldThrowAn<FileNotFoundException>();
      }

      #endregion

      #region Sheet1

      [Observation]
      public void should_fill_data_table_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_fill_data_table_with_less_rows_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 1, 2, 3, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_fill_data_table_with_selected_rows_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 1, 3, 4, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(2);
         var i = 0.0;
         foreach (ImportDataRow row in tables[0].Rows)
         {
            ((double) row[1]).ShouldBeEqualTo(2.0 + i++);
         }
      }

      [Observation]
      public void should_throw_ospsuite_exception_if_data_end_row_less_than_data_start_row()
      {
         The.Action(() => sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 1, 4, 3, _cms)).ShouldThrowAn<OSPSuiteException>();
      }

      [Observation]
      public void should_not_fill_data_table_with_more_rows_sheet1()
      {
         var tables = sut.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], 0, 1, 2, 99, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_fill_data_table_with_skipping_rows_sheet1()
      {
         ImportDataTable testTable = _importDataTable.Clone();
         testTable.Columns.ItemByIndex(1).SkipNullValueRows = true;
         var tables = sut.ImportDataTables(testTable, _excelFile, _sheetNames[0], 0, 1, 2, -1, _cms);
         tables[0].Rows.Count.ShouldBeEqualTo(4);
      }

      #endregion

      #endregion
   }
}