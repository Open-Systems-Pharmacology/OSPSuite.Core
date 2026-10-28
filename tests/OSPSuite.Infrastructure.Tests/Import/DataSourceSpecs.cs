using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_DataSource : ContextSpecification<IDataSource>
   {
      protected ColumnInfoCache _columnInfos;
      protected IDimension _fakedTimeDimension;
      protected IDimension _fakedConcentrationDimensionMolar;
      protected IDimension _fakedConcentrationDimensionMass;
      protected IDimension _fakedErrorDimension;
      protected IImporter _fakedImporter;
      protected IDataSet _fakeDataSet;
      protected IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _fakedTimeDimension = A.Fake<IDimension>();
         _fakedConcentrationDimensionMolar = A.Fake<IDimension>();
         _fakedConcentrationDimensionMass = A.Fake<IDimension>();
         _fakedErrorDimension = A.Fake<IDimension>();
         _fakeDataSet = new DataSet();
         _fakedImporter = A.Fake<IImporter>();
         _dimensionFactory = A.Fake<IDimensionFactory>();

         _columnInfos = new ColumnInfoCache
         {
            new ColumnInfo() { DisplayName = "Time", Name = "Time" },
            new ColumnInfo() { DisplayName = "Concentration", Name = "Concentration" },
            new ColumnInfo() { DisplayName = "Error", Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration" }
         };

         _columnInfos["Time"].SupportedDimensions.Add(_fakedTimeDimension);
         _columnInfos["Concentration"].SupportedDimensions.Add(_fakedConcentrationDimensionMolar);
         _columnInfos["Concentration"].SupportedDimensions.Add(_fakedConcentrationDimensionMass);
         _columnInfos["Error"].SupportedDimensions.Add(_fakedErrorDimension);
         _columnInfos["Error"].SupportedDimensions.Add(_fakedConcentrationDimensionMass);
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
                  ColumnInfo = _columnInfos["Time"]
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
                     Unit = new UnitDescription("pmol/l")
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
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
                  ColumnInfo = _columnInfos["Error"]
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
         A.CallTo(() => _fakedTimeDimension.HasUnit("s")).Returns(true);
         A.CallTo(() => _fakedConcentrationDimensionMolar.HasUnit("pmol/l")).Returns(true);
         A.CallTo(() => _fakedConcentrationDimensionMass.HasUnit("ng/ml")).Returns(true);
         A.CallTo(() => _fakedErrorDimension.HasUnit("pmol/l")).Returns(true);

         A.CallTo(() => _fakedConcentrationDimensionMolar.FindUnit("no unit", true)).Returns(null);
         A.CallTo(() => _fakedConcentrationDimensionMass.FindUnit("no unit", true)).Returns(null);
         A.CallTo(() => _fakedErrorDimension.FindUnit("no unit", true)).Returns(null);
         A.CallTo(() => _fakedConcentrationDimensionMolar.FindUnit("ng/ml", true)).Returns(null);
         A.CallTo(() => _fakedConcentrationDimensionMass.FindUnit("pmol/l", true)).Returns(null);

         _fakeDataSet.AddData(new List<ParsedDataSet>()
         {
            new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(),
               parsedData)
         });


         sut = new DataSource(_fakedImporter);
         sut.DataSets.Add("sheet1", _fakeDataSet);
      }

      protected string getFirstMeasurementUnit()
      {
         return sut.DataSets.KeyValues.First().Value.Data.First().Data.ToList()[1].Value[1].Unit;
      }
   }

   public class When_validating_empty_data_source : concern_for_DataSource
   {
      protected override void Context()
      {
         base.Context();
         _columnInfos = new ColumnInfoCache
         {
            new ColumnInfo() { DisplayName = "Time", Name = "Time", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration", Name = "Concentration", IsMandatory = true, BaseGridName = "Time" },
            new ColumnInfo() { DisplayName = "Error", Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration", BaseGridName = "Time" }
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
                  ColumnInfo = _columnInfos["Time"]
               },
               new List<SimulationPoint>() { }
            },
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Concentration",
                     Unit = new UnitDescription("mol")
                  },
                  ColumnInfo = _columnInfos["Concentration"]
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
                  ColumnInfo = _columnInfos["Error"]
               },
               new List<SimulationPoint>() { }
            }
         };
         var dataSet = new DataSet();
         dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         _dimensionFactory = A.Fake<IDimensionFactory>();
         var fractionDimension = A.Fake<IDimension>();
         A.CallTo(() => fractionDimension.Name).Returns(Constants.Dimension.FRACTION);
         var otherDimension = A.Fake<IDimension>();
         A.CallTo(() => otherDimension.Name).Returns("mol");
         A.CallTo(() => _dimensionFactory.DimensionForUnit("")).Returns(fractionDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("mol")).Returns(otherDimension);
         sut.DataSets.Clear();
         sut.DataSets.Add("sheet1", dataSet);
      }

      [Observation]
      public void throw_on_empty_dataset()
      {
         var sheets = new DataSheetCollection();
         var sheet = new DataSheet() { SheetName = "sheet1" };
         sheets.AddSheet(sheet);
         sut.AddSheets(sheets, _columnInfos, "").Any().ShouldBeTrue();
      }
   }

   public class When_validating_geometric_error : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();

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
                  ColumnInfo = _columnInfos["Time"]
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
                     Unit = new UnitDescription("pmol/l")
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
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
                     Unit = new UnitDescription("pmol/l"),
                     Dimension = Constants.Dimension.NO_DIMENSION
                  },
                  ColumnInfo = _columnInfos["Error"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = double.NaN,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void geometric_error_does_not_check_units()
      {
         sut.ValidateDataSourceUnits(_columnInfos);
      }
   }

   public class When_validating_units : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();

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
                  ColumnInfo = _columnInfos["Time"]
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
                     Unit = new UnitDescription("pmol/l")
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/L",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
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
                     Unit = new UnitDescription("pmol/l"),
                     Dimension = Constants.Dimension.NO_DIMENSION
                  },
                  ColumnInfo = _columnInfos["Error"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = double.NaN,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void should_ignore_casing()
      {
         sut.ValidateDataSourceUnits(_columnInfos);
      }
   }

   public class When_validating_missing_unit_column : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();

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
                  ColumnInfo = _columnInfos["Time"]
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
                     Unit = new UnitDescription("")
                  },
                  ColumnInfo = _columnInfos["Concentration"]
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
            },
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Error",
                     Unit = new UnitDescription("pmol/l"),
                     Dimension = Constants.Dimension.NO_DIMENSION
                  },
                  ColumnInfo = _columnInfos["Error"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = double.NaN,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void should_have_set_the_error_to_dimensionless()
      {
         getFirstMeasurementUnit().ShouldBeEmpty();
      }
   }

   public class When_validating_consistent_column_input_units : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();
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
                  ColumnInfo = _columnInfos["Time"]
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
                     Unit = new UnitDescription("pmol/l")
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
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
                  ColumnInfo = _columnInfos["Error"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = double.NaN,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void should_be_valid()
      {
         sut.ValidateDataSourceUnits(_columnInfos);
      }
   }

   public class When_validating_inconsistent_column_input_units : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();
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
                  ColumnInfo = _columnInfos["Time"]
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
                     Unit = new UnitDescription("pmol/l")
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "no unit",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "no unit",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "no unit",
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
                     Unit = new UnitDescription("ng/ml")
                  },
                  ColumnInfo = _columnInfos["Error"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "ng/ml",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "ng/ml",
                     Measurement = 0.1,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "ng/ml",
                     Measurement = double.NaN,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void should_not_be_valid()
      {
         sut.ValidateDataSourceUnits(_columnInfos).Any().ShouldBeTrue();
      }
   }

   //although we should never be able to reach such a scenario, still it should be caught
   public class When_validating_consistent_manual_input_units : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();
         var parsedData = new Dictionary<ExtendedColumn, IList<SimulationPoint>>()
         {
            {
               new ExtendedColumn()
               {
                  Column = new Column()
                  {
                     Name = "Time",
                     Unit = new UnitDescription("s"),
                     Dimension = _fakedTimeDimension
                  },
                  ColumnInfo = _columnInfos["Time"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "s",
                     Measurement = 0,
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
                     Unit = new UnitDescription("pmol/l"),
                     Dimension = _fakedConcentrationDimensionMass
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
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
                     Unit = new UnitDescription(""),
                     Dimension = _fakedConcentrationDimensionMolar
                  },
                  ColumnInfo = _columnInfos["Error"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void should_be_valid()
      {
         sut.ValidateDataSourceUnits(_columnInfos).Any().ShouldBeTrue();
      }
   }

   public class When_validating_inconsistent_error_units : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();
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
                  ColumnInfo = _columnInfos["Time"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "s",
                     Measurement = 0,
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
                     Unit = new UnitDescription("pmol/l"),
                     Dimension = null
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "pmol/l",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "ng/ml",
                     Measurement = 10,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void should_be_valid()
      {
         sut.ValidateDataSourceUnits(_columnInfos).Any().ShouldBeTrue();
      }
   }

   public class When_validating_not_supported_error_units : concern_for_DataSource
   {
      private DataSet _dataSet;

      protected override void Context()
      {
         base.Context();
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
                  ColumnInfo = _columnInfos["Time"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "s",
                     Measurement = 0,
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
                     Unit = new UnitDescription("pmol/l"),
                     Dimension = null
                  },
                  ColumnInfo = _columnInfos["Concentration"]
               },
               new List<SimulationPoint>()
               {
                  new SimulationPoint()
                  {
                     Unit = "s",
                     Measurement = 10,
                     Lloq = 1
                  },
                  new SimulationPoint()
                  {
                     Unit = "no unit",
                     Measurement = 10,
                     Lloq = 1
                  }
               }
            }
         };
         _dataSet = new DataSet();
         _dataSet.AddData(new List<ParsedDataSet>()
            { { new ParsedDataSet(new List<string>(), A.Fake<DataSheet>(), new List<UnformattedRow>(), parsedData) } });
         sut.DataSets.Clear();
      }

      protected override void Because()
      {
         sut.DataSets.Add("sheet1", _dataSet);
      }

      [Observation]
      public void should_be_valid()
      {
         sut.ValidateDataSourceUnits(_columnInfos).Any().ShouldBeTrue();
      }
   }

   public abstract class concern_for_DataSource_with_error_type_from_column : concern_for_DataSource
   {
      protected ParseErrors _errors;

      protected void setErrorStdDevValues(params string[] errorStdDevValues)
      {
         var errorColumn = _fakeDataSet.Data.First().Data.Keys.First(x => x.ColumnInfo.Name == "Error");
         errorColumn.ErrorStdDevValues = errorStdDevValues.ToList();
      }

      protected override void Because()
      {
         var sheets = new DataSheetCollection();
         sheets.AddSheet(new DataSheet { SheetName = "sheet1" });
         _errors = sut.AddSheets(sheets, _columnInfos, "");
      }
   }

   public class When_adding_sheets_with_an_unknown_error_type : concern_for_DataSource_with_error_type_from_column
   {
      protected override void Context()
      {
         base.Context();
         setErrorStdDevValues("SD");
      }

      [Observation]
      public void should_report_the_unknown_error_type()
      {
         var error = _errors.ErrorsFor(_fakeDataSet).OfType<UnknownErrorTypeParseErrorDescription>().Single();
         error.Message.Contains("'SD'").ShouldBeTrue();
         error.Message.Contains("sheet1").ShouldBeTrue();
         error.Message.Contains(ErrorStdDevValues.AcceptedValues.First()).ShouldBeTrue();
      }

      [Observation]
      public void should_not_report_a_conflict()
      {
         _errors.ErrorsFor(_fakeDataSet).OfType<ConflictingErrorTypesParseErrorDescription>().ShouldBeEmpty();
      }
   }

   public class When_adding_sheets_with_conflicting_error_types_in_one_data_set : concern_for_DataSource_with_error_type_from_column
   {
      protected override void Context()
      {
         base.Context();
         setErrorStdDevValues("ASD", "GSD");
      }

      [Observation]
      public void should_report_the_conflict()
      {
         var error = _errors.ErrorsFor(_fakeDataSet).OfType<ConflictingErrorTypesParseErrorDescription>().Single();
         error.Message.Contains("ASD").ShouldBeTrue();
         error.Message.Contains("GSD").ShouldBeTrue();
      }
   }

   public class When_adding_sheets_with_equivalent_error_types_in_one_data_set : concern_for_DataSource_with_error_type_from_column
   {
      protected override void Context()
      {
         base.Context();
         setErrorStdDevValues("gsd", Constants.STD_DEV_GEOMETRIC);
      }

      [Observation]
      public void should_not_report_any_error()
      {
         _errors.Any().ShouldBeFalse();
      }
   }

   public abstract class concern_for_DataSource_with_an_arithmetic_error_type_from_column : concern_for_DataSource_with_error_type_from_column
   {
      protected override void Context()
      {
         base.Context();
         setErrorStdDevValues("ASD");
         var errorColumn = _fakeDataSet.Data.First().Data.Keys.First(x => x.ColumnInfo.Name == "Error");
         errorColumn.ErrorDeviation = Constants.STD_DEV_ARITHMETIC;
         errorColumn.Column.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
         errorColumn.Column.ErrorStdDevColumn = "Error type";
         errorColumn.Column.Dimension = null;
         A.CallTo(() => _fakedConcentrationDimensionMolar.SupportsUnit("pmol/l", A<bool>._)).Returns(true);
      }
   }

   public class When_adding_sheets_with_an_arithmetic_error_type_from_column_without_error_unit : concern_for_DataSource_with_an_arithmetic_error_type_from_column
   {
      [Observation]
      public void should_report_that_the_error_unit_does_not_match_the_measurement_unit()
      {
         _errors.ErrorsFor(_fakeDataSet).OfType<ErrorUnitParseErrorDescription>().Any().ShouldBeTrue();
      }
   }

   public class When_adding_sheets_with_an_arithmetic_error_type_from_column_in_the_measurement_unit : concern_for_DataSource_with_an_arithmetic_error_type_from_column
   {
      protected override void Context()
      {
         base.Context();
         foreach (var errorValue in _fakeDataSet.Data.First().Data.First(x => x.Key.ColumnInfo.Name == "Error").Value)
         {
            errorValue.Unit = "pmol/l";
         }
      }

      [Observation]
      public void should_not_report_any_error()
      {
         _errors.Any().ShouldBeFalse();
      }
   }
}
