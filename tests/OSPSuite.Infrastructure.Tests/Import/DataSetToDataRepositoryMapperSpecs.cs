using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_DataSetToDataRepositoryMapperSpecs : ContextSpecification<DataSetToDataRepositoryMapper>
   {
      protected IDataSource _dataSourceLLOQ;
      protected IDataSource _dataSourceInconsistentLLOQ;
      protected IDataSource _dataSourceUnitFromColumn;
      protected IDimension _concentrationDimensionLLOQ;
      protected IDimension _concentrationDimensionUnitFromColumn;
      protected Dictionary<ExtendedColumn, IList<SimulationPoint>> _parsedDataSetLLOQ;
      protected Dictionary<ExtendedColumn, IList<SimulationPoint>> _parsedDataSetInconsistentLLOQ;
      protected Dictionary<ExtendedColumn, IList<SimulationPoint>> _parsedDataSetUnitFromColumn;
      protected DataSetToDataRepositoryMappingResult _result;

      protected override void Context()
      {
         _dataSourceLLOQ = A.Fake<IDataSource>();
         _dataSourceInconsistentLLOQ = A.Fake<IDataSource>();
         _dataSourceUnitFromColumn = A.Fake<IDataSource>();
         var timeDimension = DomainHelperForSpecs.TimeDimensionForSpecs();
         _concentrationDimensionLLOQ = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         _concentrationDimensionUnitFromColumn = new Dimension(new BaseDimensionRepresentation { AmountExponent = 3, LengthExponent = -1 },
            Constants.Dimension.AMOUNT_PER_TIME, "µmol/min");

         _parsedDataSetLLOQ = new Dictionary<ExtendedColumn, IList<SimulationPoint>>()
         {
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Time",
                     Unit = new UnitDescription("min"),
                     Dimension = timeDimension
                  },
                  ColumnInfo = new ColumnInfo()
                  {
                     BaseGridName = null,
                     Name = "Time"
                  }
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "min",
                     Measurement = 0,
                     Lloq = double.NaN
                  },
                  new SimulationPoint()
                  {
                     Unit = "min",
                     Measurement = 1,
                     Lloq = double.NaN
                  },
                  new SimulationPoint()
                  {
                     Unit = "min",
                     Measurement = 2,
                     Lloq = double.NaN
                  }
               }
            },
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Concentration",
                     Unit = new UnitDescription("µmol/l"),
                     Dimension = _concentrationDimensionLLOQ
                  },
                  ColumnInfo = new ColumnInfo()
                  {
                     BaseGridName = "Time",
                     Name = "Concentration"
                  }
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "µmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "µmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "µmol/l",
                     Measurement = double.NaN,
                     Lloq = 1
                  }
               }
            }
         };
         _parsedDataSetUnitFromColumn = new Dictionary<ExtendedColumn, IList<SimulationPoint>>()
         {
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Time",
                     Unit = new UnitDescription("min"),
                     Dimension = timeDimension
                  },
                  ColumnInfo = new ColumnInfo()
                  {
                     BaseGridName = null,
                     Name = "Time"
                  }
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "min",
                     Measurement = 0,
                     Lloq = double.NaN
                  },
                  new SimulationPoint()
                  {
                     Unit = "min",
                     Measurement = 1,
                     Lloq = double.NaN
                  },
                  new SimulationPoint()
                  {
                     Unit = "min",
                     Measurement = 2,
                     Lloq = double.NaN
                  }
               }
            },
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Concentration",
                     Unit = new UnitDescription("ng/ml"),
                     Dimension = null
                  },
                  ColumnInfo = new ColumnInfo()
                  {
                     BaseGridName = "Time",
                     Name = "Concentration"
                  }
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "µmol/min",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "µmol/min",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "µmol/min",
                     Measurement = 17,
                     Lloq = 1
                  }
               }
            }
         };


         //adding supported dimensions
         _parsedDataSetLLOQ.First(x => x.Key.ColumnInfo.Name == "Concentration").Key.ColumnInfo.SupportedDimensions.Add(_concentrationDimensionLLOQ);
         _parsedDataSetLLOQ.First(x => x.Key.ColumnInfo.Name == "Concentration").Key.ColumnInfo.SupportedDimensions
            .Add(_concentrationDimensionUnitFromColumn);
         _parsedDataSetUnitFromColumn.First(x => x.Key.ColumnInfo.Name == "Concentration").Key.ColumnInfo.SupportedDimensions
            .Add(_concentrationDimensionLLOQ);
         _parsedDataSetUnitFromColumn.First(x => x.Key.ColumnInfo.Name == "Concentration").Key.ColumnInfo.SupportedDimensions
            .Add(_concentrationDimensionUnitFromColumn);
         _parsedDataSetInconsistentLLOQ = _parsedDataSetLLOQ;
         _parsedDataSetInconsistentLLOQ.First(x => x.Key.ColumnInfo.Name == "Concentration").Value.ElementAt(0).Lloq = 2;

         A.CallTo(() => _dataSourceLLOQ.ImportedDataSetAt(A<int>.Ignored)).Returns(new ImportedDataSet
            (
               "file",
               "sheet1",
               new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), _parsedDataSetLLOQ),
               "name",
               new List<MetaDataInstance>()
            )
         );

         A.CallTo(() => _dataSourceInconsistentLLOQ.ImportedDataSetAt(A<int>.Ignored)).Returns(new ImportedDataSet
            (
               "file",
               "sheet1",
               new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), _parsedDataSetInconsistentLLOQ),
               "name",
               new List<MetaDataInstance>()
            )
         );

         A.CallTo(() => _dataSourceUnitFromColumn.ImportedDataSetAt(A<int>.Ignored)).Returns(new ImportedDataSet
            (
               "file",
               "sheet1",
               new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), _parsedDataSetUnitFromColumn),
               "name",
               new List<MetaDataInstance>()
            )
         );

         sut = new DataSetToDataRepositoryMapper();
      }
   }

   public class When_mapping_a_data_repository : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_dataSourceInconsistentLLOQ.ImportedDataSetAt(0));
      }

      [Observation]
      public void should_use_lloq_halve_two()
      {
         Assert.IsNotNull(_result);
         _result.DataRepository.ObservationColumns().First().Values.ToArray().ShouldBeEqualTo(new float[] { 10.0f, 1.0f, 1.0f });
      }
   }

   public class When_mapping_a_data_repository_with_inconsistent_lloq : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_dataSourceLLOQ.ImportedDataSetAt(0));
      }

      [Observation]
      public void should_return_warning_message()
      {
         _result.WarningMessage.ShouldNotBeEmpty();
      }

      [Observation]
      public void should_add_maximum_lloq_value_to_data_repository()
      {
         _result.DataRepository.ObservationColumns().First().DataInfo.LLOQ.ShouldBeEqualTo(2.0f);
      }

      [Observation]
      public void values_should_also_be_assigned_the_maximum_lloq_value()
      {
         _result.DataRepository.ObservationColumns().First().Values.ToArray().ShouldBeEqualTo(new float[] { 10.0f, 1.0f, 1.0f });
      }
   }

   public class When_mapping_a_data_repository_with_unit_from_a_column : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_dataSourceUnitFromColumn.ImportedDataSetAt(0));
      }

      [Observation]
      public void should_return__no_warning_message()
      {
         _result.WarningMessage.ShouldBeEmpty();
      }

      [Observation]
      public void should_return_correct_values()
      {
         _result.DataRepository.ObservationColumns().First().InternalValues[0].ShouldBeEqualTo(10);
      }
   }

   public class When_metadata_has_duplicate_names : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Context()
      {
         base.Context();

         var importedDataSet = new ImportedDataSet(
            fileName: "test.xlsx",
            sheetName: "Sheet1",
            parsedDataSet: new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), _parsedDataSetInconsistentLLOQ),
            name: "TestDataSet",
            metaDataDescription: new List<MetaDataInstance>
            {
               new MetaDataInstance("Source", "InstrumentA"),
               new MetaDataInstance("Source", "InstrumentA-Duplicate"),
               new MetaDataInstance("Analyst", "John Doe"),
            }
         );

         _importedDataSet = importedDataSet;
      }

      private ImportedDataSet _importedDataSet;

      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_importedDataSet);
      }

      [Observation]
      public void should_not_add_duplicate_metadata_properties()
      {
         var props = _result.DataRepository.ExtendedProperties.ToList();
         props.Count(p => p.Name == "Source").ShouldBeEqualTo(1);
         props.Count(p => p.Name == "Analyst").ShouldBeEqualTo(1);
      }
   }

   public class When_mapping_a_data_set_with_unresolvable_dimension : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      private ImportedDataSet _importedDataSetWithBadUnit;
      private const string _unsupportedUnit = "INVALID_UNIT";

      protected override void Context()
      {
         base.Context();
         var timeDimension = DomainHelperForSpecs.TimeDimensionForSpecs();

         var parsedData = new Dictionary<ExtendedColumn, IList<SimulationPoint>>()
         {
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Time",
                     Unit = new UnitDescription("min"),
                     Dimension = timeDimension
                  },
                  ColumnInfo = new ColumnInfo()
                  {
                     BaseGridName = null,
                     Name = "Time"
                  }
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint() { Unit = "min", Measurement = 0, Lloq = double.NaN },
                  new SimulationPoint() { Unit = "min", Measurement = 1, Lloq = double.NaN }
               }
            },
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Concentration",
                     Unit = new UnitDescription(_unsupportedUnit),
                     Dimension = null
                  },
                  ColumnInfo = new ColumnInfo()
                  {
                     BaseGridName = "Time",
                     Name = "Concentration"
                  }
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint() { Unit = _unsupportedUnit, Measurement = 10, Lloq = double.NaN },
                  new SimulationPoint() { Unit = _unsupportedUnit, Measurement = 20, Lloq = double.NaN }
               }
            }
         };

         // No supported dimensions added to ColumnInfo, so DimensionForUnit will return null
         _importedDataSetWithBadUnit = new ImportedDataSet(
            "file.xlsx",
            "Sheet1",
            new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData),
            "name",
            new List<MetaDataInstance>()
         );
      }

      [Observation]
      public void should_throw_exception_with_meaningful_message()
      {
         The.Action(() => sut.ConvertImportDataSet(_importedDataSetWithBadUnit))
            .ShouldThrowAn<OSPSuiteException>();
      }
   }


   public abstract class concern_for_DataSetToDataRepositoryMapper_with_error_in_units_of_a_row : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected ImportedDataSet _importedDataSet;

      protected ImportedDataSet importedDataSetWithError(string errorDeviation)
      {
         var timeDimension = DomainHelperForSpecs.TimeDimensionForSpecs();
         var concentrationDimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         var errorColumnInfo = new ColumnInfo
         {
            BaseGridName = "Time",
            Name = "Error",
            IsMandatory = false,
            RelatedColumnOf = "Concentration"
         };
         errorColumnInfo.SupportedDimensions.Add(concentrationDimension);

         var parsedData = new Dictionary<ExtendedColumn, IList<SimulationPoint>>
         {
            {
               new ExtendedColumn
               {
                  Column = new Column { Name = "Time", Unit = new UnitDescription("min"), Dimension = timeDimension },
                  ColumnInfo = new ColumnInfo { BaseGridName = null, Name = "Time" }
               },
               new List<SimulationPoint>
               {
                  new SimulationPoint { Unit = "min", Measurement = 0, Lloq = double.NaN },
                  new SimulationPoint { Unit = "min", Measurement = 1, Lloq = double.NaN }
               }
            },
            {
               new ExtendedColumn
               {
                  Column = new Column { Name = "Concentration", Unit = new UnitDescription("mol/l"), Dimension = concentrationDimension },
                  ColumnInfo = new ColumnInfo { BaseGridName = "Time", Name = "Concentration" }
               },
               new List<SimulationPoint>
               {
                  new SimulationPoint { Unit = "mol/l", Measurement = 10, Lloq = double.NaN },
                  new SimulationPoint { Unit = "mol/l", Measurement = 20, Lloq = double.NaN }
               }
            },
            {
               new ExtendedColumn
               {
                  Column = new Column { Name = "Error", Unit = new UnitDescription("mol/l"), Dimension = concentrationDimension, ErrorStdDev = errorDeviation },
                  ColumnInfo = errorColumnInfo,
                  ErrorDeviation = errorDeviation
               },
               new List<SimulationPoint>
               {
                  new SimulationPoint { Unit = "mol/l", Measurement = 2, Lloq = double.NaN },
                  new SimulationPoint { Unit = "mol/l", Measurement = 3, Lloq = double.NaN }
               }
            }
         };

         return new ImportedDataSet("file", "sheet1",
            new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData), "name", new List<MetaDataInstance>());
      }

      protected DataColumn errorColumn => _result.DataRepository.AllButBaseGrid().Single(x => x.Name == "Error");

      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_importedDataSet);
      }
   }

   public class When_mapping_a_geometric_error_whose_rows_have_a_unit : concern_for_DataSetToDataRepositoryMapper_with_error_in_units_of_a_row
   {
      protected override void Context()
      {
         base.Context();
         _importedDataSet = importedDataSetWithError(Constants.STD_DEV_GEOMETRIC);
      }

      [Observation]
      public void should_create_a_dimensionless_geometric_error_column()
      {
         errorColumn.Dimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.GeometricStdDev);
      }

      [Observation]
      public void should_keep_the_values_as_written()
      {
         errorColumn.Values.ToArray().ShouldBeEqualTo(new[] { 2f, 3f });
      }
   }

   public class When_mapping_an_arithmetic_error_whose_rows_have_a_unit : concern_for_DataSetToDataRepositoryMapper_with_error_in_units_of_a_row
   {
      protected override void Context()
      {
         base.Context();
         _importedDataSet = importedDataSetWithError(Constants.STD_DEV_ARITHMETIC);
      }

      [Observation]
      public void should_create_an_arithmetic_error_column_in_the_dimension_of_the_measurement()
      {
         errorColumn.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
         errorColumn.DataInfo.AuxiliaryType.ShouldBeEqualTo(AuxiliaryType.ArithmeticStdDev);
      }

      [Observation]
      public void should_convert_the_values_to_the_base_unit()
      {
         errorColumn.Values.ToArray().ShouldBeEqualTo(new[] { 2e6f, 3e6f });
      }
   }
}
