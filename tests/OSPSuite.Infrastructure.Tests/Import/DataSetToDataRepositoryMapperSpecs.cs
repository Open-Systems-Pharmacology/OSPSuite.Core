using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_DataSetToDataRepositoryMapperSpecs : ContextSpecification<DataSetToDataRepositoryMapper>
   {
      protected IDataSource _dataSourceLLOQ;
      protected IDataSource _dataSourceUnitFromColumn;
      protected IDimension _concentrationDimensionLLOQ;
      protected IDimension _concentrationDimensionUnitFromColumn;
      protected Dictionary<ExtendedColumn, IList<SimulationPoint>> _parsedDataSetLLOQ;
      protected Dictionary<ExtendedColumn, IList<SimulationPoint>> _parsedDataSetUnitFromColumn;
      protected DataSetToDataRepositoryMappingResult _result;

      protected override void Context()
      {
         _dataSourceLLOQ = A.Fake<IDataSource>();
         _dataSourceUnitFromColumn = A.Fake<IDataSource>();
         var timeDimension = DomainHelperForSpecs.TimeDimensionForSpecs();
         _concentrationDimensionLLOQ = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         _concentrationDimensionUnitFromColumn = new Dimension(new BaseDimensionRepresentation {AmountExponent = 3, LengthExponent = -1}, Constants.Dimension.AMOUNT_PER_TIME, "µmol/min");
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
                     Lloq = 2
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


         A.CallTo(() => _dataSourceLLOQ.DataSetAt(A<int>.Ignored)).Returns(new ImportedDataSet
            (
               "file",
               "sheet1",
               new ParsedDataSet(new List<(string, IList<string>)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(), _parsedDataSetLLOQ),
               "name",
               new List<MetaDataInstance>()
            )
         );

         A.CallTo(() => _dataSourceUnitFromColumn.DataSetAt(A<int>.Ignored)).Returns(new ImportedDataSet
            (
               "file",
               "sheet1",
               new ParsedDataSet(new List<(string, IList<string>)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(), _parsedDataSetUnitFromColumn),
               "name",
               new List<MetaDataInstance>()
            )
         );

         var dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => dimensionFactory.DimensionForUnit("min")).Returns(timeDimension);
         A.CallTo(() => dimensionFactory.DimensionForUnit("µmol/l")).Returns(_concentrationDimensionLLOQ);
         A.CallTo(() => dimensionFactory.DimensionForUnit("µmol/min")).Returns(_concentrationDimensionUnitFromColumn);

         sut = new DataSetToDataRepositoryMapper(dimensionFactory);
      }
   }

   public class When_mapping_a_data_repository : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_dataSourceLLOQ.DataSetAt(0));
      }

      [Observation]
      public void should_use_lloq_halve_two()
      {
         Assert.IsNotNull(_result);
         _result.DataRepository.ObservationColumns().First().Values.ToArray().ShouldBeEqualTo(new float[] { 10.0f, 0.5f, 0.5f });
      }
   }

   public class When_mapping_a_data_repository_with_inconsistent_lloq : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_dataSourceLLOQ.DataSetAt(0));
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
   }

   public class When_mapping_a_data_repository_with_unit_from_a_column : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         _result = sut.ConvertImportDataSet(_dataSourceUnitFromColumn.DataSetAt(0));
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
}
