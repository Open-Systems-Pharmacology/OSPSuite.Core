using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.Presentation.Views.Importer;
using Dimension = OSPSuite.Core.Importer.Dimension;
using Unit = OSPSuite.Core.Importer.Unit;

namespace OSPSuite.Importer
{
   [Category("Importer")]
   internal abstract class concern_for_DataImporter : ContextSpecification<ImporterPresenter>
   {
      protected string _excelFilePath;
      private ColumnInfosToImportDataTableMapper _importMapper;
      private IDialogCreator _dialogCreator;
      private ImportDataTableToDataRepositoryMapper _dataRepositoryMapper;
      private IImporterTask _importerTask;

      public override void GlobalContext()
      {
         _importMapper = new ColumnInfosToImportDataTableMapper();
         _dialogCreator = A.Fake<IDialogCreator>();
         _dataRepositoryMapper = new ImportDataTableToDataRepositoryMapper(createDimensionFactory(), new ColumnCaptionHelper());
         _importerTask = new ImporterTask(new  ColumnCaptionHelper(), new LowerLimitOfQuantificationTask());
         var namingPatternView = A.Fake<INamingPatternView>();
         var importerView = A.Fake<IImporterView>();
         sut =
            new ImporterPresenter(importerView,
               _dataRepositoryMapper,
               new NamingPatternPresenter(namingPatternView),
               _dialogCreator, A.Fake<IExcelPreviewPresenter>(), new RepositoryNamingTask(new MetaDataCategoryToNamingPatternMapper()), new NamingPatternToRepositoryNameMapper());

         _excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
      }

      private static IDimensionFactory createDimensionFactory()
      {
         var dimensionFactory = new DimensionFactory();

         IDimension dimLess = dimensionFactory.AddDimension(new BaseDimensionRepresentation(), "Fraction", " ");
         dimLess.AddUnit("%", 0.01, 0.0, true);

         IDimension length = dimensionFactory.AddDimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Length", "m");
         length.AddUnit("mm", 0.001, 0.0, true);
         length.AddUnit("cm", 0.01, 0);
         length.AddUnit("µm", 0.000001, 0);
         length.AddUnit("nm", 0.000000001, 0);

         IDimension amount = dimensionFactory.AddDimension(new BaseDimensionRepresentation {AmountExponent = 1}, "Amount", "mol");
         amount.AddUnit("mmol", 0.001, 0.0, true);
         amount.AddUnit("µmol", 0.000001, 0);

         IDimension mass = dimensionFactory.AddDimension(new BaseDimensionRepresentation {MassExponent = 1}, "Mass", "kg");
         mass.AddUnit("mg", 0.000001, 0.0, true);
         mass.AddUnit("g", 0.001, 0);
         mass.AddUnit("µg", 0.000000001, 0);
         mass.AddUnit("ng", 0.000000000001, 0);

         IDimension time = dimensionFactory.AddDimension(new BaseDimensionRepresentation {TimeExponent = 1}, "Time", "s");
         time.AddUnit("min", 60, 0);
         time.AddUnit("h", 3600, 0);
         time.AddUnit("day", 24 * 3600, 0);
         time.AddUnit("ms", 0.001, 0);

         IDimension velocity =
            dimensionFactory.AddDimension(new BaseDimensionRepresentation {LengthExponent = 1, TimeExponent = -1}, "Velocity", "m/s");
         velocity.AddUnit("mm/s", 0.001, 0.0, true);
         velocity.AddUnit("mm/min", 0.001 / 60.0, 0);
         velocity.AddUnit("µm/s", 0.000001, 0);

         IDimension volume = dimensionFactory.AddDimension(new BaseDimensionRepresentation {LengthExponent = 3}, "Volume", "m^3");
         volume.AddUnit("cm^3", 0.000001, 0.0, true);
         volume.AddUnit("mm^3", 0.000000001, 0);
         volume.AddUnit("l", 0.001, 0);

         IDimension specificMass =
            dimensionFactory.AddDimension(new BaseDimensionRepresentation {LengthExponent = -3, MassExponent = 1},
               "SpecificMass", "kg/m^3");
         specificMass.AddUnit("g/cm^3", 1000, 0.0, true);
         specificMass.AddUnit("kg/l", 1000, 0);

         dimensionFactory.AddDimension(Constants.Dimension.NO_DIMENSION);
         return dimensionFactory;
      }

      public class when_excel_file_is_test3 : concern_for_DataImporter
      {
         protected string _excelFile;
         protected ImportDataTable _importDataTable = new ImportDataTable();
         protected IList<string> _sheetNames;
         protected IList<string> _columnNames;
         protected List<ColumnMapping> _cms;
         protected IReadOnlyList<ColumnInfo> _columnInfos;
         private const string STR_STDDEV_GEOMETRIC = "Geometric Standard Deviation";
         private const string STR_STDDEV_ARITHMETIC = "Arithmetic Standard Deviation";
         private const string STR_AUXILIARYTYPE = "AuxiliaryType";

         public override void GlobalContext()
         {
            base.GlobalContext();
            _excelFile = Path.Combine(_excelFilePath, "Test3.xls");
            var dimensionFactory = createDimensionFactory();
            _sheetNames = new List<string>(3) {"Sheet1", "Sheet2", "Sheet3"};
            _columnNames = new List<string>(6)
            {
               "TextColumn",
               "NumberColumn",
               "DateColumn",
               "BoolColumn",
               "NumberColumn2",
               "NumberColumn3"
            };

            _importDataTable.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Concentration",
               DataType = typeof(double),
               SkipNullValueRows = true,
               Dimensions =
                  new List<Dimension>
                  {
                     new Dimension
                     {
                        Name = "Fraction",
                        IsDefault = true,
                        Units =
                           new List<Unit>
                           {new Unit {Name = "%", IsDefault = true}}
                     }
                  }
            });
            _importDataTable.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Time",
               DataType = typeof(double),
               SkipNullValueRows = true,
               Dimensions =
                  new List<Dimension>
                  {
                     new Dimension
                     {
                        Name = "Fraction",
                        IsDefault = true,
                        Units =
                           new List<Unit>
                           {new Unit {Name = "%", IsDefault = true}}
                     }
                  }
            });


            var metaData = new MetaDataTable();
            metaData.Columns.Add(new MetaDataColumn
            {
               ColumnName = STR_AUXILIARYTYPE,
               Description = STR_AUXILIARYTYPE,
               DataType = typeof(string),
               ListOfValues =
                  new Dictionary<string, string>
                  {
                     {STR_STDDEV_ARITHMETIC, STR_STDDEV_ARITHMETIC},
                     {STR_STDDEV_GEOMETRIC, STR_STDDEV_GEOMETRIC}
                  },
               IsListOfValuesFixed = true,
               DefaultValue = STR_STDDEV_ARITHMETIC
            });

            _importDataTable.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Error",
               DataType = typeof(double),
               SkipNullValueRows = true,
               Required = false,
               MetaData = metaData,
               Dimensions =
                  new List<Dimension>
                  {
                     new Dimension
                     {
                        Name = "Fraction",
                        IsDefault = true,
                        Units =
                           new List<Unit>
                           {new Unit {Name = "%", IsDefault = true}}
                     }
                  }
            });

            _cms = new List<ColumnMapping>(6);
            for (int i = 0; i < _columnNames.Count; i++)
            {
               var cm = new ColumnMapping
               {
                  SourceColumn = _columnNames[i],
               };
               if (i == 1)
                  cm.Target = _importDataTable.Columns.ItemByIndex(0).ColumnName;
               if (i == 4 || i == 5)
                  cm.Target = _importDataTable.Columns.ItemByIndex(1).ColumnName;
               _cms.Add(cm);
            }

            // create column infos for data repository creation
            var concentration = new ColumnInfo
            {
               Name = "Concentration",
               BaseGridName = "Time",
               Description = "Concentration",
               DisplayName = "Concentration",
               IsMandatory = true
            };
            concentration.DimensionInfos.Add(new DimensionInfo {Dimension = dimensionFactory.NoDimension});

            var time = new ColumnInfo
            {
               Name = "Time",
               BaseGridName = "",
               Description = "Time",
               DisplayName = "Time",
               IsMandatory = true,
            };
            time.DimensionInfos.Add(new DimensionInfo {Dimension = dimensionFactory.NoDimension});

            var error = new ColumnInfo
            {
               Name = "Error",
               BaseGridName = "Time",
               Description = "Error",
               DisplayName = "Error",
               RelatedColumnOf = "Concentration",
               IsMandatory = false,
            };
            time.DimensionInfos.Add(new DimensionInfo {Dimension = dimensionFactory.NoDimension});

            _columnInfos = new List<ColumnInfo> {concentration, time, error};
         }

         protected override void Context()
         {
            base.Context();
            _importDataTable.Rows.Clear();
         }

         #region test cases

         [Test]
         public void should_convert_import_data_table()
         {
            ImportDataTable testTable = _importDataTable.Clone();
            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(new List<ImportDataTable> {testTable}, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            dataRepositories.Count.ShouldBeEqualTo(1);
            foreach (var col in dataRepositories[0])
               testTable.Columns.ContainsName(col.Name).ShouldBeTrue();
         }

         [Observation]
         public void should_convert_data_table_sheet1()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);
            bool errorNaN;
            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], _cms, A.Fake<Cache<string, Rectangle>>());
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            dataRepositories.Count.ShouldBeEqualTo(2);
            tables[0].Rows.Count.ShouldBeEqualTo(46);
            dataRepositories[0].ExtendedProperties.Contains("Source").ShouldBeTrue();
            dataRepositories[0].ExtendedProperties["Source"].ValueAsObject.ShouldBeEqualTo(tables[0].Source);
            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeTrue();
               _columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
               col.Values.Count.ShouldBeEqualTo(46);
            }
            tables[1].Rows.Count.ShouldBeEqualTo(46);
            dataRepositories[1].ExtendedProperties.Contains("Source").ShouldBeTrue();
            dataRepositories[1].ExtendedProperties["Source"].ValueAsObject.ShouldBeEqualTo(tables[1].Source);
            foreach (var col in dataRepositories[1])
            {
               tables[1].Columns.ContainsName(col.Name).ShouldBeTrue();
               _columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
               col.Values.Count.ShouldBeEqualTo(46);
            }
         }

         [Observation]
         public void should_convert_to_import_data_table()
         {
            var metaDataCategories = new List<MetaDataCategory>
            {
               new MetaDataCategory
               {
                  Name = "MolWeight",
                  DisplayName = "Molecular Weight",
                  Description = "Weight of a mol",
                  IsMandatory = true,
                  MinValue = 0,
                  MinValueAllowed = false,
                  MetaDataType = typeof(double)
               }
            };

            var columnInfos = new List<ColumnInfo>
            {
               new ColumnInfo
               {
                  Name = "Column1",
                  IsMandatory = true,
                  NullValuesHandling = NullValuesHandlingType.DeleteRow,
                  Description = "TestColumn",
                  DisplayName = "DisplayColumn1"
               }
            };

            //check column
            var importDataTable = _importMapper.ConvertToImportDataTable(metaDataCategories, columnInfos);
            importDataTable.Columns.Count.ShouldBeEqualTo(1);
            importDataTable.Columns.ItemByIndex(0).ColumnName.ShouldBeEqualTo(columnInfos[0].Name);
            importDataTable.Columns.ItemByIndex(0).DisplayName.ShouldBeEqualTo(columnInfos[0].DisplayName);
            importDataTable.Columns.ItemByIndex(0).Description.ShouldBeEqualTo(columnInfos[0].Description);
            importDataTable.Columns.ItemByIndex(0).Required.ShouldBeEqualTo(columnInfos[0].IsMandatory);
            importDataTable.Columns.ItemByIndex(0).SkipNullValueRows.ShouldBeTrue();
            //check metadata
            importDataTable.MetaData.ShouldNotBeNull();
            importDataTable.MetaData.Columns.Count.ShouldBeEqualTo(1);
            importDataTable.MetaData.Columns.ItemByIndex(0).ColumnName.ShouldBeEqualTo(metaDataCategories[0].Name);
            importDataTable.MetaData.Columns.ItemByIndex(0).DisplayName.ShouldBeEqualTo(metaDataCategories[0].DisplayName);
            importDataTable.MetaData.Columns.ItemByIndex(0).Description.ShouldBeEqualTo(metaDataCategories[0].Description);
            importDataTable.MetaData.Columns.ItemByIndex(0).Required.ShouldBeEqualTo(metaDataCategories[0].IsMandatory);
            importDataTable.MetaData.Columns.ItemByIndex(0).MinValue.ShouldBeEqualTo(metaDataCategories[0].MinValue);
            importDataTable.MetaData.Columns.ItemByIndex(0).MinValueAllowed.ShouldBeEqualTo(metaDataCategories[0].MinValueAllowed);
            importDataTable.MetaData.Columns.ItemByIndex(0).DataType.ShouldBeEqualTo(metaDataCategories[0].MetaDataType);
         }

         [Observation]
         public void should_convert_data_table_sheet1_with_metadata()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);

            var metaData = new MetaDataTable();

            _importDataTable.MetaData = metaData;
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Column1",
               DisplayName = "DisplayColumn1",
               Description = "Text for Column1",
               Required = true,
               ListOfValues = new Dictionary<string, string> {{"A", "A"}, {"B", "B"}},
               IsListOfValuesFixed = true,
               DefaultValue = "A"
            });

            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], _cms);
            tables[0].MetaData.Rows.Add((MetaDataRow) tables[0].MetaData.NewRow());
            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            dataRepositories.Count.ShouldBeEqualTo(2);
            dataRepositories[0].ExtendedProperties.Contains(metaData.Columns.ItemByIndex(0).ColumnName).ShouldBeTrue();
            dataRepositories[0].ExtendedProperties[metaData.Columns.ItemByIndex(0).ColumnName].ValueAsObject.ShouldBeEqualTo(metaData.Columns.ItemByIndex(0).DefaultValue);
         }

         [Observation]
         public void should_convert_data_table_sheet1_with_column_metadata()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);

            var metaData = new MetaDataTable();
            _importDataTable.Columns.ItemByIndex(0).MetaData = metaData;
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Column1",
               DisplayName = "DisplayColumn1",
               Description = "Text for Column1",
               Required = true,
               ListOfValues =
                  new Dictionary<string, string> {{"A", "A"}, {"B", "B"}},
               IsListOfValuesFixed = true,
               DefaultValue = "A"
            });

            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], _cms);
            tables[0].Columns.ItemByIndex(0).MetaData.Rows.Add(
               (MetaDataRow) tables[0].Columns.ItemByIndex(0).MetaData.NewRow());

            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            dataRepositories.Count.ShouldBeEqualTo(2);
            foreach (var col in dataRepositories[0])
            {
               if (col.Name != _importDataTable.Columns.ItemByIndex(0).ColumnName) continue;
               col.DataInfo.ExtendedProperties.Contains(metaData.Columns.ItemByIndex(0).ColumnName).ShouldBeTrue();
               col.DataInfo.ExtendedProperties[metaData.Columns.ItemByIndex(0).ColumnName].ValueAsObject.ShouldBeEqualTo(metaData.Columns.ItemByIndex(0).DefaultValue);
            }
         }

         [Observation]
         public void should_rename_column()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);
            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[0], _cms);

            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            dataRepositories.Count.ShouldBeEqualTo(2);
            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeTrue();
               _columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
            }
            foreach (var col in dataRepositories[1])
            {
               tables[1].Columns.ContainsName(col.Name).ShouldBeTrue();
               _columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
            }

            dataRepositories.Each(repository => repository.RenameColumnsToSource());

            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeFalse();
               _columnNames.Contains(col.Name).ShouldBeTrue();
            }
            foreach (var col in dataRepositories[1])
            {
               tables[1].Columns.ContainsName(col.Name).ShouldBeFalse();
               _columnNames.Contains(col.Name).ShouldBeTrue();
            }
         }

         [Observation]
         public void should_cut_unit_from_column()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);

            var columnNames = new List<string> {"NumberColumn [h]", "Concentration [g/l]", "Concentration2 [mol %]"};
            var cms = new List<ColumnMapping>
            {
               new ColumnMapping {SourceColumn = columnNames[0], Target = "Time"},
               new ColumnMapping {SourceColumn = columnNames[1], Target = "Concentration"},
               new ColumnMapping {SourceColumn = columnNames[2], Target = "Concentration"}
            };

            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[1], cms);

            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            dataRepositories.Count.ShouldBeEqualTo(2);
            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeTrue();
               columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
            }
            foreach (var col in dataRepositories[1])
            {
               tables[1].Columns.ContainsName(col.Name).ShouldBeTrue();
               columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
            }

            dataRepositories.Each(repository =>
            {
               repository.RenameColumnsToSource();
               repository.CutUnitFromColumnNames();
            });

            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeFalse();
               columnNames.Contains(col.Name).ShouldBeFalse();
               var sourceElements = col.DataInfo.Source.Split('.');
               var name = sourceElements[sourceElements.Length - 1];
               columnNames.Contains(name).ShouldBeTrue(); //ends with columnName
               col.Name.ShouldBeEqualTo(name.Substring(0, name.IndexOf('[')) + name.Substring(name.IndexOf(']') + 1).Trim());
            }
            foreach (var col in dataRepositories[1])
            {
               tables[1].Columns.ContainsName(col.Name).ShouldBeFalse();
               columnNames.Contains(col.Name).ShouldBeFalse();
               var sourceElements = col.DataInfo.Source.Split('.');
               var name = sourceElements[sourceElements.Length - 1];
               columnNames.Contains(name).ShouldBeTrue(); //ends with columnName
               col.Name.ShouldBeEqualTo(name.Substring(0, name.IndexOf('[')) + name.Substring(name.IndexOf(']') + 1).Trim());
            }
         }

         [Observation]
         public void should_optional_not_mapped_column_is_not_used()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);

            var columnNames = new List<string> {"NumberColumn [h]", "Concentration [g/l]", "Concentration2 [mol %]"};
            var cms = new List<ColumnMapping>
            {
               new ColumnMapping {SourceColumn = columnNames[0], Target = "Time"},
               new ColumnMapping {SourceColumn = columnNames[1], Target = "Concentration"},
               new ColumnMapping {SourceColumn = columnNames[2], Target = "Concentration"}
            };

            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[1], cms);

            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            dataRepositories.Count.ShouldBeEqualTo(2);
            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeTrue();
               columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
            }
            foreach (var col in dataRepositories[1])
            {
               tables[1].Columns.ContainsName(col.Name).ShouldBeTrue();
               columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
            }
            foreach (var table in tables)
            {
               table.Columns.Count.ShouldBeEqualTo(3);
            }
            foreach (var col in dataRepositories[0])
            {
               col.Name.ShouldNotBeEqualTo("Error");
            }
            foreach (var col in dataRepositories[1])
            {
               col.Name.ShouldNotBeEqualTo("Error");
            }
         }

         [Observation]
         public void should_import_arithmetic_error()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask, _dialogCreator);

            var columnNames = new List<string> {"NumberColumn [h]", "Concentration [g/l]", "Concentration2 [mol %]"};
            //define the mapping (normally done by user within gui
            var cms = new List<ColumnMapping>
            {
               new ColumnMapping {SourceColumn = columnNames[0], Target = "Time"},
               new ColumnMapping {SourceColumn = columnNames[1], Target = "Concentration"},
               new ColumnMapping {SourceColumn = columnNames[2], Target = "Error"}
            };
            //enter meta data setting
            cms[2].MetaData = _importDataTable.Columns.ItemByName("Error").MetaData.Clone();
            var row = (MetaDataRow) cms[2].MetaData.NewRow();
            row[STR_AUXILIARYTYPE] = STR_STDDEV_ARITHMETIC;
            cms[2].MetaData.Rows.Add(row);
            cms[2].MetaData.AcceptChanges();

            //import data and do the conversion
            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[1], cms);

            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeFalse();

            //check correctness
            dataRepositories.Count.ShouldBeEqualTo(1);
            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeTrue();
               columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
               var dataInfo = col.DataInfo;
               switch (col.Name)
               {
                  case "Time":
                     dataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.Undefined);
                     dataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.BaseGrid);
                     break;
                  case "Concentration":
                     dataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.Undefined);
                     dataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.Observation);
                     col.RelatedColumns.ShouldNotBeNull();
                     var hasRelatedColumn = false;
                     foreach (var relatedCol in col.RelatedColumns)
                     {
                        hasRelatedColumn.ShouldBeFalse();
                        relatedCol.Name.ShouldBeEqualTo("Error");
                        relatedCol.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
                        relatedCol.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
                        hasRelatedColumn = true;
                     }
                     hasRelatedColumn.ShouldBeTrue();
                     break;
                  case "Error":
                     dataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
                     dataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
                     break;
               }
            }
         }

         [Observation]
         public void should_import_geometric_error()
         {
            var importer = new Presentation.Services.Importer.Importer(_dataRepositoryMapper, _columnInfos, _importerTask,_dialogCreator);

            var columnNames = new List<string> {"NumberColumn [h]", "Concentration [g/l]", "Concentration2 [mol %]"};
            //define the mapping (normally done by user within gui
            var cms = new List<ColumnMapping>
            {
               new ColumnMapping {SourceColumn = columnNames[0], Target = "Time"},
               new ColumnMapping {SourceColumn = columnNames[1], Target = "Concentration"},
               new ColumnMapping {SourceColumn = columnNames[2], Target = "Error"}
            };
            //enter meta data setting
            cms[2].MetaData = _importDataTable.Columns.ItemByName("Error").MetaData.Clone();
            var row = (MetaDataRow) cms[2].MetaData.NewRow();
            row[STR_AUXILIARYTYPE] = STR_STDDEV_GEOMETRIC;
            cms[2].MetaData.Rows.Add(row);
            cms[2].MetaData.AcceptChanges();

            //import data and do the conversion
            var tables = importer.ImportDataTables(_importDataTable, _excelFile, _sheetNames[1], cms);

            bool errorNaN;
            var dataRepositories = sut.ConvertImportDataTableList(tables, _columnInfos, out errorNaN);
            errorNaN.ShouldBeTrue();

            //check correctness
            dataRepositories.Count.ShouldBeEqualTo(1);
            foreach (var col in dataRepositories[0])
            {
               tables[0].Columns.ContainsName(col.Name).ShouldBeTrue();
               columnNames.Contains(col.DataInfo.Source).ShouldBeTrue();
               var dataInfo = col.DataInfo;
               switch (col.Name)
               {
                  case "Time":
                     dataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.Undefined);
                     dataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.BaseGrid);
                     break;
                  case "Concentration":
                     dataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.Undefined);
                     dataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.Observation);
                     col.RelatedColumns.ShouldNotBeNull();
                     var hasRelatedColumn = false;
                     foreach (var relatedCol in col.RelatedColumns)
                     {
                        hasRelatedColumn.ShouldBeFalse();
                        relatedCol.Name.ShouldBeEqualTo("Error");
                        relatedCol.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.GeometricStdDev);
                        relatedCol.DataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
                        hasRelatedColumn = true;
                     }
                     hasRelatedColumn.ShouldBeTrue();
                     break;
                  case "Error":
                     dataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.GeometricStdDev);
                     dataInfo.Origin.ShouldBeEqualTo(ColumnOrigins.ObservationAuxiliary);
                     break;
               }
            }
         }

         #endregion
      }
   }
}