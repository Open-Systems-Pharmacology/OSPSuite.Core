using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_DataSetToDataRepositoryMapperSpecs : ContextSpecification<DataSetToDataRepositoryMapper>
   {
      protected IDataSource dataSource;
      protected DataRepository result;

      protected override void Context()
      {
         dataSource = A.Fake<IDataSource>();
         var parsedData = new Dictionary<ExtendedColumn, IList<SimulationPoint>>()
         {
            { 
               new ExtendedColumn() 
               { 
                  Column = new Column() 
                  {
                     Name = "Time",
                     Unit = new UnitDescription("s")
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
                     Unit = "s",
                     Measurement = 0,
                     Lloq = double.NaN
                  },
                  new SimulationPoint()
                  {
                     Unit = "s",
                     Measurement = 1,
                     Lloq = double.NaN
                  },
                  new SimulationPoint()
                  {
                     Unit = "s",
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
                     Unit = new UnitDescription("mol")
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
                     Unit = "mol",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "mol",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "mol",
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
         A.CallTo(() => dimensionFactory.DimensionForUnit(A<string>.Ignored)).Returns(Constants.Dimension.NO_DIMENSION);
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
         Assert.AreEqual(new float[] { 10.0f, 0.5f, 0.5f }, result.ObservationColumns().First().Values.ToArray());
      }
   }
}
