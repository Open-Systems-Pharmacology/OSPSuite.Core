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
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_DataSetToDataRepositoryMapperSpecs : ContextSpecification<DataSetToDataRepositoryMapper>
   {
      protected IDataSource dataSource;
      protected DataSetToDataRepositoryMappingResult result;

      protected override void Context()
      {
         dataSource = A.Fake<IDataSource>();
         var timeDimension = DomainHelperForSpecs.TimeDimensionForSpecs();
         var concentrationDimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
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
                     Dimension = concentrationDimension
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
         A.CallTo(() => dataSource.DataSetAt(A<int>.Ignored)).Returns(new ImportedDataSet
            (
               "file",
               "sheet1",
               new ParsedDataSet(new List<(string, IList<string>)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(), parsedData),
               "name",
               new List<MetaDataInstance>()
            )
         );

         var dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => dimensionFactory.DimensionForUnit("min")).Returns(timeDimension);
         A.CallTo(() => dimensionFactory.DimensionForUnit("µmol/l")).Returns(concentrationDimension);

         sut = new DataSetToDataRepositoryMapper(dimensionFactory);
      }
   }

   public class When_mapping_a_data_repository : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         result = sut.ConvertImportDataSet(dataSource.DataSetAt(0));
      }

      [Observation]
      public void should_use_lloq_halve_two()
      {
         Assert.IsNotNull(result);
         result.DataRepository.ObservationColumns().First().Values.ToArray().ShouldBeEqualTo(new float[] { 10.0f, 0.5f, 0.5f });
      }
   }

   public class When_mapping_a_data_repository_with_inconsistent_lloq : concern_for_DataSetToDataRepositoryMapperSpecs
   {
      protected override void Because()
      {
         result = sut.ConvertImportDataSet(dataSource.DataSetAt(0));
      }

      [Observation]
      public void should_return_warning_message()
      {
         result.WarningMessage.ShouldNotBeEmpty();
      }

      [Observation]
      public void should_add_maximum_lloq_value_to_data_repository()
      {
         result.DataRepository.ObservationColumns().First().DataInfo.LLOQ.ShouldBeEqualTo(2.0f);
      }
   }
}
