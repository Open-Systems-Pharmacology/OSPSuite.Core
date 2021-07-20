using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Exceptions;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public abstract class ConcernforDataFormat_DataFormatHeadersWithUnits : ContextSpecification<IDataSource>
   {
      protected IImporter _importer;
      protected override void Context()
      {
         base.Context();
         _importer = A.Fake<IImporter>();
         sut = new DataSource(_importer);
      }
   }

   public class When_validating_data_source : ConcernforDataFormat_DataFormatHeadersWithUnits
   {
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IDimensionFactory _dimensionFactory;
      protected bool _result;

      protected override void Context()
      {
         base.Context();
         _columnInfos = new List<ColumnInfo>()  {
            new ColumnInfo() { Name = "Time", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Concentration", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration", BaseGridName = "Time" }
         };
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
         var dataSet = new DataSet();
         dataSet.AddData(new List<ParsedDataSet>() { { new ParsedDataSet(new List<(string, IList<string>)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(), parsedData) } });
         _dimensionFactory = A.Fake<IDimensionFactory>();
         var fractionDimension = A.Fake<IDimension>();
         A.CallTo(() => fractionDimension.Name).Returns(Constants.Dimension.FRACTION);
         var otherDimension = A.Fake<IDimension>();
         A.CallTo(() => otherDimension.Name).Returns("mol");
         A.CallTo(() => _dimensionFactory.DimensionForUnit("")).Returns(fractionDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("mol")).Returns(otherDimension);
         sut.DataSets.Add("sheet1", dataSet);
      }

      protected override void Because()
      {
         base.Because();
         _result = sut.ValidateDataSource(_columnInfos, _dimensionFactory);
      }

      [Observation]
      public void geometric_error_does_not_check_units()
      {
         Assert.IsTrue(_result);
      }
   }

   public class When_validating_empty_data_source : ConcernforDataFormat_DataFormatHeadersWithUnits
   {
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         base.Context();
         _columnInfos = new List<ColumnInfo>()  {
            new ColumnInfo() { Name = "Time", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Concentration", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration", BaseGridName = "Time" }
         };
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
               new List<SimulationPoint>() { }
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
               new List<SimulationPoint>() { }
            }
         };
         var dataSet = new DataSet();
         dataSet.AddData(new List<ParsedDataSet>() { { new ParsedDataSet(new List<(string, IList<string>)>(), A.Fake<IUnformattedData>(), new List<UnformattedRow>(), parsedData) } });
         _dimensionFactory = A.Fake<IDimensionFactory>();
         var fractionDimension = A.Fake<IDimension>();
         A.CallTo(() => fractionDimension.Name).Returns(Constants.Dimension.FRACTION);
         var otherDimension = A.Fake<IDimension>();
         A.CallTo(() => otherDimension.Name).Returns("mol");
         A.CallTo(() => _dimensionFactory.DimensionForUnit("")).Returns(fractionDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("mol")).Returns(otherDimension);
         sut.DataSets.Add("sheet1", dataSet);
      }

      [Observation]
      public void geometric_error_does_not_check_units()
      {
         The.Action(() => sut.ValidateDataSource(_columnInfos, _dimensionFactory)).ShouldThrowAn<OSPSuiteException>();
      }
   }
}
