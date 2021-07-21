using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_DataSource : ContextSpecification<IDataSource>
   {
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IDimension _fakedTimeDimension;
      protected IDimension _fakedConcentrationDimension;
      protected IDimension _fakedErrorDimension;
      protected IImporter _fakedImporter;
      protected IDataSet _fakeDataSet;
      //protected Dictionary<ExtendedColumn, IList<SimulationPoint>> _parsedDataSet;


      protected override void Context()
      {
         _fakedTimeDimension = A.Fake<IDimension>();
         _fakedConcentrationDimension = A.Fake<IDimension>();
         _fakedErrorDimension = A.Fake<IDimension>();
         _fakedImporter = A.Fake<IImporter>();
         _fakeDataSet = new DataSet();

         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time", Name ="Time" },
            new ColumnInfo() { DisplayName = "Concentration", Name = "Concentration"},
            new ColumnInfo() { DisplayName = "Error", Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration"}
         };

         _columnInfos.First(x => x.DisplayName == "Time").SupportedDimensions.Add(_fakedTimeDimension);
         _columnInfos.First(x => x.DisplayName == "Concentration").SupportedDimensions.Add(_fakedConcentrationDimension);
         _columnInfos.First(x => x.DisplayName == "Error").SupportedDimensions.Add(_fakedErrorDimension);
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
                  ColumnInfo = _columnInfos[0]
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
                  ColumnInfo = _columnInfos[1]
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
            },
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Error",
                     Unit = new UnitDescription("")
                  },
                  ColumnInfo = _columnInfos[2]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "",
                     Measurement = double.NaN,
                     Lloq = 1
                  }
               }
            }
         };
         A.CallTo(() => _fakedTimeDimension.HasUnit("min")).Returns(true);
         A.CallTo(() => _fakedConcentrationDimension.HasUnit("pmol/l")).Returns(true);
         A.CallTo(() => _fakedErrorDimension.HasUnit("pmol/l")).Returns(true);

         _fakeDataSet.AddData(new List<ParsedDataSet>() {new ParsedDataSet(new List<(string, IList<string>)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(),
            parsedData)});
               

         sut = new DataSource(_fakedImporter);
         sut.DataSets.Add("sheet1", _fakeDataSet);
      }


   }

   public class When_validating_consistent_manual_input_units : concern_for_DataSource
   {
      protected override void Context()
      {
         base.Context();

      }

      [Observation]
      public void should_be_valid()
      {
         sut.ValidateErrorAgainstMeasurement(_columnInfos).ShouldBeTrue();
      }

   }
}