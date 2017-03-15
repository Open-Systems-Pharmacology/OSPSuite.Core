using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services.Importer;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.UI
{
   [IntegrationTests]
   public abstract class concern_for_ColumnMappingControl : ContextSpecification<ColumnMappingControl>
   {
      protected string _excelFilePath;
      protected ImportDataTable _importDataTable;
      protected ImportDataTable _importDataTableGroupBy;
      protected ImportDataTable _importDataTablePKSim;
      private IImageListRetriever _imageListRetriever;
      protected IImporterTask _importerTask;
      protected IColumnCaptionHelper _columnCaptionHelper;
      private ILowerLimitOfQuantificationTask _lowerLimitOfQuantificationTask;
      private IDialogCreator _dialogCreator;

      #region Generate Test Case Configuration Settings
      static class Test
      {
         private static MetaDataTable createMetaData()
         {
            var metaData = new MetaDataTable();

            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Gender",
               DisplayName = "Gender",
               Description = "What is the gender? It can be Male or Female.",
               Required = false,
               ListOfValues =
                  new Dictionary<string, string> { { "Male", "Male" }, { "Female", "Female" } },
               IsListOfValuesFixed = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Species",
               Description = "What is the kind of species?",
               Required = true,
               ListOfValues =
                  new Dictionary<string, string>
                                             {
                                                {"Human", "Human"},
                                                {"Dog", "Dog"},
                                                {"Mouse", "Mouse"},
                                                {"Rate", "Rate"},
                                                {"MiniPig", "Mini Pig"}
                                             },
               IsListOfValuesFixed = false
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(DateTime),
               ColumnName = "Date of Measurement",
               Description = "When has the data be measured?",
               Required = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(bool),
               ColumnName = "GMP relevant",
               Description = "Has the measurement be done under GMP?",
               Required = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(double),
               ColumnName = "Reference Value",
               Description = "What is the reference value?",
               Required = true,
               MinValue = 0,
               MinValueAllowed = false,
               MaxValue = 1,
               MaxValueAllowed = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(int),
               ColumnName = "Number of Measurements",
               Description = "How often has been measured?",
               Required = false,
               MinValue = 0,
               MinValueAllowed = true,
               MaxValue = 6
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Remark",
               Description = "Space for a remark!",
               Required = false,
               MaxLength = 255
            });
            return metaData;
         }

         private static IList<InputParameter> createInputParameters()
         {
            return new List<InputParameter>
                      {
                         new InputParameter
                            {
                               DisplayName = "Molecular Weight",
                               Name = "MolWeight",
                               Unit = new Unit
                                         {
                                            DisplayName = "Gram per Mol",
                                            Name = "g/mol"
                                         },
                               MinValue = 0,
                               MaxValueAllowed = false
                            },
                         new InputParameter
                            {
                               DisplayName = "Velocity",
                               Name = "Velocity",
                               Unit =
                                  new Unit
                                     {
                                        DisplayName = "Gram per Litre",
                                        Name = "g/l"
                                     },
                               MinValue = 0,
                               MinValueAllowed = false
                            }
                      };
         }

         private static IList<Dimension> createTimeDimensions()
         {
            return new List<Dimension>
                   {
                      new Dimension
                         {
                            DisplayName = "Time",
                            IsDefault = true,
                            Name = "Time",
                            Units = new List<Unit>
                                       {
                                          new Unit {IsDefault = true, Name = "h", DisplayName = "in Stunden"},
                                          new Unit {IsDefault = false, Name = "d", DisplayName = "in Tagen"},
                                       }
                         },
                      new Dimension
                         {
                            DisplayName = "Time2",
                            IsDefault = false,
                            Name = "SecondTime",
                            Units = new List<Unit>
                                       {
                                          new Unit {IsDefault = true, Name = "w", DisplayName = "in Wochen"},
                                          new Unit {IsDefault = false, Name = "m", DisplayName = "in Monaten"}
                                       },
                            InputParameters = createInputParameters()
                         }
                   };
         }

         private static IList<Dimension> createConcentrationDimensions()
         {
            return new List<Dimension>
                      {
                         new Dimension
                            {
                               DisplayName = "Mass versus volume",
                               IsDefault = true,
                               Name = "Mass versus volume",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "g/l", DisplayName = "Gram per litre"},
                                             new Unit
                                                {IsDefault = false, Name = "mg/l", DisplayName = "Milligram per litre"},
                                          }
                            },
                         new Dimension
                            {
                               DisplayName = "Mole fraction",
                               IsDefault = false,
                               Name = "Mole fraction",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "mol %", DisplayName = "molar percent"}
                                          },
                               InputParameters = createInputParameters()
                            },
                         new Dimension
                            {
                               DisplayName = "",
                               IsDefault = false,
                               Name = "Dimensionless",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "", DisplayName = ""}
                                          },
                            }

                      };
         }

         public static ImportDataTable CreateImportDataTableForGroupByTest()
         {
            var metaData = new MetaDataTable();

            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Category",
               DisplayName = "Category",
               Description = "What is the category? It can be A or B or C.",
               Required = true,
               ListOfValues =
                  new Dictionary<string, string> { { "A", "A" }, { "B", "B" }, { "C", "C" } },
               IsListOfValuesFixed = true
            });

            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(bool),
               ColumnName = "Tested",
               DisplayName = "Tested?",
               Description = "Has it been tested?",
               Required = true
            });

            var idt = new ImportDataTable { MetaData = metaData };

            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Time",
               DisplayName = "Time after Dosis",
               Description = "The time after dosis.",
               DataType = typeof(double),
               MetaData = createMetaData(),
               Required = true,
               SkipNullValueRows = true,
               Dimensions = createTimeDimensions()
            });
            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Concentration",
               DisplayName = "Measured Concentration",
               Description = "The measured concentration.",
               DataType = typeof(double),
               MetaData = createMetaData(),
               Required = true,
               SkipNullValueRows = true,
               Dimensions = createConcentrationDimensions()
            });

            return idt;
         }

         public static ImportDataTable CreateImportDataTable()
         {
            var mdt = createMetaData();
            var idt = new ImportDataTable { MetaData = mdt };

            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Category",
               DisplayName = "Categorie of Experiment",
               Description = "The category of the experiment.",
               DataType = typeof(string),
               Required = true,
               SkipNullValueRows = false
            });
            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Time",
               DisplayName = "Time after Dosis",
               Description = "The time after dosis.",
               DataType = typeof(double),
               MetaData = createMetaData(),
               Required = true,
               SkipNullValueRows = true,
               Dimensions = createTimeDimensions()
            });
            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Concentration",
               DisplayName = "Measured Concentration",
               Description = "The measured concentration.",
               DataType = typeof(double),
               MetaData = createMetaData(),
               Required = true,
               SkipNullValueRows = true,
               Dimensions = createConcentrationDimensions()
            });
            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "DateofMeasurement",
               DisplayName = "Date of Measurement",
               Description = "The date when the measurement has been done.",
               DataType = typeof(DateTime)
            });
            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Released?",
               DisplayName = "Approved?",
               Description = "Has the measurement been approved?",
               DataType = typeof(bool)
            });

            return idt;
         }
      }
      #endregion

      #region Generate PKSim Test Case Configuration Settings
      static class TestSettingsLikePKSim
      {
         private static MetaDataTable createMetaDataForTable()
         {
            var metaData = new MetaDataTable();
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Gender",
               DisplayName = "Gender",
               Description = "What is the gender? It can be Male or Female.",
               Required = false,
               ListOfValues =
                  new Dictionary<string, string>
                                             {
                                                {"Male", "Male"},
                                                {"Female", "Female"},
                                                {"Mixed", "Mixed"},
                                                {"Unspecified", "Unspecified"}
                                             },
               IsListOfValuesFixed = true
            });

            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "MoleculeName",
               DisplayName = "Molecule Name",
               Required = true,
               MaxLength = 40
            });

            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(double),
               ColumnName = "MolWeight",
               DisplayName = "MolWeight",
               Required = true,
            });

            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Species",
               Description = "What is the kind of species?",
               Required = true,
               ListOfValues =
                  new Dictionary<string, string>
                                             {
                                                {"Human", "Human"},
                                                {"Dog", "Dog"},
                                                {"Mouse", "Mouse"},
                                                {"Rate", "Rate"},
                                                {"MiniPig", "MiniPig"}
                                             },
               IsListOfValuesFixed = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Tissue",
               Description = "What kind of tissue has been measured?",
               Required = false,
               MaxLength = 40
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Sample Source",
               Description = "Where has the sample been taken from?",
               Required = false,
               ListOfValues =
                  new Dictionary<string, string>
                                             {
                                                {"VenousBloodPlasma", "Venous Blood Plasma"},
                                                {"ArterialBloodPlasma", "Arterial Blood Plasma"},
                                                {"PeripherialVenousBloodPlasma", "Peripherial Venous Blood Plasma"},
                                                {"Urine", "Urine"}
                                             },
               IsListOfValuesFixed = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Remarks",
               Description = "Space for remarks!",
               Required = false,
               MaxLength = 255
            });
            return metaData;
         }

         private static MetaDataTable createMetaDataForConcentration()
         {
            var metaData = new MetaDataTable();
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Type Of Data Collection",
               DisplayName = "Type Of Data Collection",
               Description = "What is the type of data collection?",
               Required = false,
               ListOfValues =
                  new Dictionary<string, string>
                                             {
                                                {"Individual", "Individual"},
                                                {"Arithmetic Mean", "Arithmetic Mean"},
                                                {"Geometric Mean", "Geometric Mean"}
                                             },
               IsListOfValuesFixed = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Remarks",
               Description = "Space for remarks!",
               Required = false,
               MaxLength = 255
            });
            return metaData;
         }

         private static MetaDataTable createMetaDataForError()
         {
            var metaData = new MetaDataTable();
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Type Of Error",
               DisplayName = "Type Of Error",
               Description = "What is the type of error?",
               Required = true,
               ListOfValues =
                  new Dictionary<string, string>
                                             {
                                                {"Arithmetic Error", "Arithmetic Error"},
                                                {"Geometric Error", "Geometric Error"}
                                             },
               IsListOfValuesFixed = true
            });
            metaData.Columns.Add(new MetaDataColumn
            {
               DataType = typeof(string),
               ColumnName = "Remarks",
               Description = "Space for remarks!",
               Required = false,
               MaxLength = 255
            });
            return metaData;
         }

         private static IList<InputParameter> createInputParameters()
         {
            return new List<InputParameter>
                      {
                         new InputParameter
                            {
                               DisplayName = "Molecular Weight",
                               Name = "MolWeight",
                               Unit = new Unit
                                         {
                                            DisplayName = "Gram per Mol",
                                            Name = "g/mol"
                                         },
                               MinValue = 0,
                               MinValueAllowed = false
                            }
                      };
         }

         private static IList<Dimension> createTimeDimensions()
         {
            return new List<Dimension>
                   {
                      new Dimension
                         {
                            DisplayName = "Short Time",
                            IsDefault = true,
                            Name = "shortTime",
                            Units = new List<Unit>
                                       {
                                          new Unit {IsDefault = false, Name = "sec", DisplayName = "in seconds"},
                                          new Unit {IsDefault = true, Name = "min", DisplayName = "in minutes"},
                                          new Unit {IsDefault = false, Name = "h", DisplayName = "in hours"},
                                          new Unit {IsDefault = false, Name = "d", DisplayName = "in days"},
                                       }
                         },
                      new Dimension
                         {
                            DisplayName = "Long Time",
                            IsDefault = false,
                            Name = "longTime",
                            Units = new List<Unit>
                                       {
                                          new Unit {IsDefault = true, Name = "w", DisplayName = "in weeks"},
                                          new Unit {IsDefault = false, Name = "m", DisplayName = "in months"}
                                       }
                         }
                   };
         }

         private static IList<Dimension> createConcentrationDimensions()
         {
            return new List<Dimension>
                      {
                         new Dimension
                            {
                               DisplayName = "Mass versus volume",
                               IsDefault = true,
                               Name = "Mass versus volume",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "g/l", DisplayName = "Gram per litre"},
                                             new Unit
                                                {IsDefault = false, Name = "mg/l", DisplayName = "Milligram per litre"},
                                             new Unit
                                                {IsDefault = false, Name = "µg/l", DisplayName = "Microgram per litre"},
                                                                                             new Unit
                                                {IsDefault = false, Name = "ng/ml", DisplayName = "Nanogram per millilitre"}
                                          }
                            },
                         new Dimension
                            {
                               DisplayName = "Mol versus volume",
                               IsDefault = false,
                               Name = "Mol versus volume",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "mol/l", DisplayName = "Mol per litre"},
                                             new Unit
                                                {
                                                   IsDefault = false,
                                                   Name = "mmol/l",
                                                   DisplayName = "Millimol per litre"
                                                },
                                             new Unit
                                                {
                                                   IsDefault = false,
                                                   Name = "µmol/l",
                                                   DisplayName = "Micromol per litre"
                                                }
                                          },
                               InputParameters = createInputParameters()
                            }
                      };
         }

         private static IList<Dimension> createErrorDimensions()
         {
            return new List<Dimension>
                      {
                         new Dimension
                            {
                               DisplayName = "Dimensionless",
                               IsDefault = true,
                               Name = "dimensionless",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "", DisplayName = ""},
                                          },
                               MetaDataConditions =
                                  new Dictionary<string, string> {{"Type Of Error", "Geometric Error"}}
                            },
                         new Dimension
                            {
                               DisplayName = "Mass versus volume",
                               IsDefault = false,
                               Name = "Mass versus volume",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "g/l", DisplayName = "Gram per litre"},
                                             new Unit
                                                {IsDefault = false, Name = "mg/l", DisplayName = "Milligram per litre"},
                                             new Unit
                                                {IsDefault = false, Name = "µg/l", DisplayName = "Microgram per litre"},
                                             new Unit
                                                {
                                                   IsDefault = false,
                                                   Name = "ng/ml",
                                                   DisplayName = "Nanogram per millilitre"
                                                }

                                          },
                               MetaDataConditions =
                                  new Dictionary<string, string> {{"Type Of Error", "Arithmetic Error"}}
                            },
                         new Dimension
                            {
                               DisplayName = "Mol versus volume",
                               IsDefault = false,
                               Name = "Mol versus volume",
                               Units = new List<Unit>
                                          {
                                             new Unit {IsDefault = true, Name = "mol/l", DisplayName = "Mol per litre"},
                                             new Unit
                                                {
                                                   IsDefault = false,
                                                   Name = "mmol/l",
                                                   DisplayName = "Millimol per litre"
                                                },
                                             new Unit
                                                {
                                                   IsDefault = false,
                                                   Name = "µmol/l",
                                                   DisplayName = "Micromol per litre"
                                                }
                                          },
                               MetaDataConditions =
                                  new Dictionary<string, string> {{"Type Of Error", "Arithmetic Error"}},
                               InputParameters = createInputParameters()
                            }
                      };
         }

         public static ImportDataTable CreateImportDataTable()
         {
            var mdt = createMetaDataForTable();
            var idt = new ImportDataTable { MetaData = mdt };

            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Time",
               Description = "Time after Dosis.",
               DataType = typeof(double),
               Required = true,
               SkipNullValueRows = true,
               Dimensions = createTimeDimensions()
            });
            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Concentration",
               Description = "Measured concentration.",
               DataType = typeof(double),
               MetaData = createMetaDataForConcentration(),
               Required = true,
               SkipNullValueRows = true,
               Dimensions = createConcentrationDimensions()
            });
            idt.Columns.Add(new ImportDataColumn
            {
               ColumnName = "Error",
               Description = "Value indicating the error.",
               DataType = typeof(double),
               MetaData = createMetaDataForError(),
               Required = false,
               SkipNullValueRows = true,
               Dimensions = createErrorDimensions(),
               ColumnNameOfRelatedColumn = "Concentration"
            });

            return idt;
         }
      }
      #endregion

      public override void GlobalContext()
      {
         _excelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
         _importDataTable = Test.CreateImportDataTable();
         _importDataTableGroupBy = Test.CreateImportDataTableForGroupByTest();
         _importDataTablePKSim = TestSettingsLikePKSim.CreateImportDataTable();
         _imageListRetriever = A.Fake<IImageListRetriever>();
         _dialogCreator= A.Fake<IDialogCreator>();
         _columnCaptionHelper = new ColumnCaptionHelper();
         _lowerLimitOfQuantificationTask =new LowerLimitOfQuantificationTask();
         _importerTask = new ImporterTask(_columnCaptionHelper, _lowerLimitOfQuantificationTask);
      }

      
      public class when_excel_file_is_test2 : concern_for_ColumnMappingControl
      {
         protected string _excelFile;
         protected Presentation.Services.Importer.Importer _importer;
         protected DataSet _data;

         public override void GlobalContext()
         {
            base.GlobalContext();

            var dataRepositoryMapper = A.Fake<IImportDataTableToDataRepositoryMapper>();
            var columnInfos = A.Fake<IReadOnlyList<ColumnInfo>>();
            _importer = new Presentation.Services.Importer.Importer(dataRepositoryMapper, columnInfos, _importerTask, _dialogCreator);
            _excelFile = Path.Combine(_excelFilePath, "Test2.xls");
            _data = _importer.GetPreview(_excelFile, 10, new Cache<string,Rectangle>());
         }

         protected override void Context()
         {
            base.Context();
            sut = null;
         }

         #region test cases

         [Test]
         public void should_map_sheet1()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet1"], _importDataTable,_imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "TextColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "BoolColumn")
               {
                  cm.Target.ShouldBeEqualTo("Released?");
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
            }

         }

         [Observation]
         public void should_map_group_by_sheet1()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet1"], _importDataTableGroupBy,_imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEmpty();
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
            }

         }

         [Observation]
         public void should_map_pksim_sheet1()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet1"], _importDataTablePKSim, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Error");
               }
            }

         }

         [Observation]
         public void should_map_sheet2()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet2"], _importDataTable, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {

               if (cm.SourceColumn == "TextColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "NumberColumn [h]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "BoolColumn")
               {
                  cm.Target.ShouldBeEqualTo("Released?");
               }
               if (cm.SourceColumn == "NumberColumn2 [d]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3 [w]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn4 [m]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("m");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn5 [y]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
               if (cm.SourceColumn == "TextColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
            }

         }

         [Observation]
         public void should_map_group_by_sheet2()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet2"], _importDataTableGroupBy, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn [h]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEmpty();
               }
               if (cm.SourceColumn == "NumberColumn2 [d]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3 [w]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn4 [m]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("m");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn5 [y]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
            }

         }

         [Observation]
         public void should_map_pksim_sheet2()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet2"], _importDataTablePKSim, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn [h]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn2 [d]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3 [w]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn4 [m]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("m");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn5 [y]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
            }

         }

         [Observation]
         public void should_map_sheet3()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet3"], _importDataTable, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {

               if (cm.SourceColumn == "TextColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "BoolColumn")
               {
                  cm.Target.ShouldBeEqualTo("Released?");
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
            }

         }

         [Observation]
         public void should_map_group_by_sheet3()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet3"], _importDataTableGroupBy, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {

               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEmpty();
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
            }

         }

         [Observation]
         public void should_map_pksim_sheet3()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet3"], _importDataTablePKSim, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
            }

         }

         #endregion


      }

      
      public class when_excel_file_is_test4 : concern_for_ColumnMappingControl
      {
         protected string _excelFile;
         protected Presentation.Services.Importer.Importer _importer;
         protected DataSet _data;

         public override void GlobalContext()
         {
            base.GlobalContext();
            var columnInfos = A.Fake<IReadOnlyList<ColumnInfo>>();
            var dataRepositoryMapper = A.Fake<IImportDataTableToDataRepositoryMapper>();
            _importer = new Presentation.Services.Importer.Importer(dataRepositoryMapper, columnInfos, _importerTask, _dialogCreator);
            _excelFile = Path.Combine(_excelFilePath, "Test4.xls");
            _data = _importer.GetPreview(_excelFile, 10, new Cache<string, Rectangle>());
         }

         protected override void Context()
         {
            base.Context();
            sut = null;
         }

         #region test cases

         [Test]
         public void should_map_sheet1()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet1"], _importDataTable, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "TextColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "BoolColumn")
               {
                  cm.Target.ShouldBeEqualTo("Released?");
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
            }

         }

         [Observation]
         public void should_map_groupBy_sheet1()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet1"], _importDataTableGroupBy, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEmpty();
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
            }

         }

         [Observation]
         public void should_map_PKSim_sheet1()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet1"], _importDataTablePKSim, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Error");
               }
            }

         }

         [Observation]
         public void should_map_sheet2()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet2"], _importDataTable, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {

               if (cm.SourceColumn == "TextColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "NumberColumn [h]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "BoolColumn")
               {
                  cm.Target.ShouldBeEqualTo("Released?");
               }
               if (cm.SourceColumn == "NumberColumn2 [d]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }               
               if (cm.SourceColumn == "NumberColumn3 [w]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn4 [m]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("m");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn5 [y]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
               if (cm.SourceColumn == "TextColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "Concentration [g/l]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("g/l");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "Concentration2 [mol %]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("mol %");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
            }

         }

         [Observation]
         public void should_map_groupBy_sheet2()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet2"], _importDataTableGroupBy, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn [h]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEmpty();
               }
               if (cm.SourceColumn == "NumberColumn2 [d]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3 [w]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn4 [m]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("m");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn5 [y]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
               if (cm.SourceColumn == "Concentration [g/l]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("g/l");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "Concentration2 [mol %]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("mol %");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
            }

         }

         [Observation]
         public void should_map_PKSim_sheet2()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet2"], _importDataTablePKSim, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn [h]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn2 [d]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3 [w]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("w");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn4 [m]")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("m");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn5 [y]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
               if (cm.SourceColumn == "Concentration [g/l]")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("g/l");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "Concentration2 [mol %]")
               {
                  cm.Target.ShouldBeEqualTo("Error");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
            }
         }

         [Observation]
         public void should_map_sheet3()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet3"], _importDataTable, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {

               if (cm.SourceColumn == "TextColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEqualTo("Category");
               }
               if (cm.SourceColumn == "BoolColumn")
               {
                  cm.Target.ShouldBeEqualTo("Released?");
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeEmpty();
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
            }

         }

         [Observation]
         public void should_map_groupBy_sheet3()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet3"], _importDataTableGroupBy, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "DateColumn")
               {
                  cm.Target.ShouldBeEmpty();
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeEmpty();
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
            }

         }

         [Observation]
         public void should_map_PKSim_sheet3()
         {
            sut = new ColumnMappingControl(_data.Tables["Sheet3"], _importDataTablePKSim, _imageListRetriever, _importerTask, _columnCaptionHelper);
            var mapping = sut.Mapping;

            foreach (var cm in mapping)
            {
               if (cm.SourceColumn == "NumberColumn")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("h");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn2")
               {
                  cm.Target.ShouldBeEqualTo("Time");
                  cm.SelectedUnit.Name.ShouldBeEqualTo("d");
                  cm.IsUnitExplicitlySet.ShouldBeTrue();
               }
               if (cm.SourceColumn == "NumberColumn3")
               {
                  cm.Target.ShouldBeEqualTo("Concentration");
                  cm.SelectedUnit.Name.ShouldBeNull();
                  cm.IsUnitExplicitlySet.ShouldBeFalse();
               }
            }
         }

         #endregion


      }

   }
}
