using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   internal class TestUnformattedData : UnformattedData
   {
      public TestUnformattedData(Dictionary<string, ColumnDescription> headers)
      {
         Headers = headers;
      }
   }
   public abstract class ConcernforColumnsDataFormat : ContextSpecification<ColumnsDataFormat>
   {
      protected IUnformattedData _basicFormat;

      protected override void Context()
      {
         sut = new ColumnsDataFormat();
         _basicFormat = new TestUnformattedData
         (
            new Dictionary<string, ColumnDescription>()
            {
               {
                  "Organ",
                  new ColumnDescription(0)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "PeripheralVenousBlood" }
                  }
               },
               {
                  "Compartment",
                  new ColumnDescription(1)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "Arterialized" }
                  }
               },
               {
                  "Species",
                  new ColumnDescription(2)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "Human" }
                  }
               },
               {
                  "Dose",
                  new ColumnDescription(3)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "75 [g] glucose" }
                  }
               },
               {
                  "Molecule",
                  new ColumnDescription(4)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "GLP-1_7-36 total", "Glucose", "Insuline", "GIP_total", "Glucagon" }
                  }
               },
               {
                  "Time [min]",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Concentration (molar) [pmol/l]",
                  new ColumnDescription(6)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Error [pmol/l]",
                  new ColumnDescription(7)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Route",
                  new ColumnDescription(8)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "po" }
                  }
               },
               {
                  "Groupe Id",
                  new ColumnDescription(9)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "H", "T2DM" }
                  }
               }
            }
         );
      }
   }

   public class When_checking_format : ConcernforColumnsDataFormat
   {
      [TestCase]
      public void identify_basic_format()
      {
         sut.CheckFile(_basicFormat).ShouldBeTrue();
      }

      [TestCase]
      public void reject_single_column_data()
      {
         var singleColumn = new TestUnformattedData
         (
            new Dictionary<string, ColumnDescription>()
            {
               {
                  "Organ",
                  _basicFormat.Headers["Organ"]
               }
            }
         );
         sut.CheckFile(singleColumn).ShouldBeFalse();
      }

      [TestCase]
      public void reject_multicolumn_with_less_than_two_numeric_columns()
      {
         var singleColumn = new TestUnformattedData
         (
            new Dictionary<string, ColumnDescription>()
            {
               {
                  "Organ",
                  _basicFormat.Headers["Organ"]
               },
               {
                  "Time [min]",
                  _basicFormat.Headers["Time [min]"]
               }
            }
         );
         sut.CheckFile(singleColumn).ShouldBeFalse();
      }
   }

   public class When_listing_parameters : ConcernforColumnsDataFormat
   {
      protected override void Because()
      {
         sut.CheckFile(_basicFormat);
      }
      
      [TestCase]
      public void identify_time_column()
      {
         var timeParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Time [min]");
         timeParameter.Type.ShouldBeEqualTo(DataFormatParameterType.Mapping);
         (timeParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = timeParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo(Column.ColumnNames.Time);
         mapping.MappedColumn.Unit.ShouldBeEqualTo("min");
      }

      [TestCase]
      public void identify_error_column()
      {
         var errorParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Error [pmol/l]");
         errorParameter.Type.ShouldBeEqualTo(DataFormatParameterType.Mapping);
         (errorParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = errorParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo(Column.ColumnNames.Error);
         mapping.MappedColumn.Unit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_measurement_column_without_the_name_on_the_headings()
      {
         var measurementParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Concentration (molar) [pmol/l]");
         measurementParameter.Type.ShouldBeEqualTo(DataFormatParameterType.Mapping);
         (measurementParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = measurementParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo(Column.ColumnNames.Concentration);
         mapping.MappedColumn.Unit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_metadata_parameters()
      {
         var metadataParameters = sut.Parameters.Where(p => p.Type == DataFormatParameterType.MetaData).ToList();
         metadataParameters.Count.ShouldBeEqualTo(5);
         foreach (var name in new[] { "Organ", "Compartment", "Species", "Dose", "Route" })
         {
            metadataParameters.FirstOrDefault(parameter => parameter.ColumnName == name).ShouldNotBeNull();
         }
      }

      [TestCase]
      public void identify_groupBy_parameters()
      {
         var groupByParameters = sut.Parameters.Where(p => p.Type == DataFormatParameterType.GroupBy).ToList();
         groupByParameters.Count.ShouldBeEqualTo(2);
         foreach (var name in new[] { "Groupe Id", "Molecule" })
         {
            groupByParameters.FirstOrDefault(parameter => parameter.ColumnName == name).ShouldNotBeNull();
         }
      }
   }

   public class When_parsing_format : ConcernforColumnsDataFormat
   {
      private IUnformattedData _mockedData;
      protected override void Context()
      {
         base.Context();
         _mockedData = A.Fake<IUnformattedData>();
         A.CallTo(() => _mockedData.Headers).Returns(_basicFormat.Headers);
      }

      protected override void Because()
      {
         sut.CheckFile(_mockedData);
      }

      [TestCase]
      public void parse_basic_format()
      {
         var data = sut.Parse(_mockedData);
         data.Count.ShouldBeEqualTo(10);
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "GLP-1_7-36 total", "", "", "", "", "H"})))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "Glucose", "", "", "", "", "H" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "Insuline", "", "", "", "", "H" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "GIP_total", "", "", "", "", "H" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "Glucagon", "", "", "", "", "H" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "GLP-1_7-36 total", "", "", "", "", "T2DM" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "Glucose", "", "", "", "", "T2DM" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "Insuline", "", "", "", "", "T2DM" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "GIP_total", "", "", "", "", "T2DM" })))).MustHaveHappened();
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "Glucagon", "", "", "", "", "T2DM" })))).MustHaveHappened();
      }
   }
}
