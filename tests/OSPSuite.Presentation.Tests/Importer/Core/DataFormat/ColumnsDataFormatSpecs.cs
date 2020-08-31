using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   internal class TestUnformattedData : UnformattedData
   {
      public TestUnformattedData(Cache<string, ColumnDescription> headers)
      {
         _headers = headers;
      }
   }
   public abstract class ConcernforColumnsDataFormat : ContextSpecification<DataFormat_TMetaData_C>
   {
      protected IUnformattedData _basicFormat;
      protected IReadOnlyList<ColumnInfo> _columnInfos;

      protected override void Context()
      {
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time" },
            new ColumnInfo() { DisplayName = "Concentration" },
            new ColumnInfo() { DisplayName = "Error" }
         };
         sut = new DataFormat_TMetaData_C();
         _basicFormat = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
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
         sut.SetParameters
         (
            _basicFormat,
            _columnInfos
         ).ShouldBeTrue();
      }

      [TestCase]
      public void reject_single_column_data()
      {
         var singleColumn = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
            {
               {
                  "Organ",
                  _basicFormat.GetColumnDescription("Organ")
               }
            }
         );
         sut.SetParameters
         (
            singleColumn,
            _columnInfos
         ).ShouldBeFalse();
      }

      [TestCase]
      public void reject_multicolumn_with_less_than_two_numeric_columns()
      {
         var singleColumn = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
            {
               {
                  "Organ",
                  _basicFormat.GetColumnDescription("Organ")
               },
               {
                  "Time [min]",
                  _basicFormat.GetColumnDescription("Time [min]")
               }
            }
         );
         sut.SetParameters(singleColumn, _columnInfos).ShouldBeFalse();
      }
   }

   public class When_listing_parameters : ConcernforColumnsDataFormat
   {
      protected override void Because()
      {
         sut.SetParameters(_basicFormat, _columnInfos);
      }

      [TestCase]
      public void identify_time_column()
      {
         var timeParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Time [min]");
         (timeParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = timeParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Time");
         mapping.MappedColumn.Unit.ShouldBeEqualTo("min");
      }

      [TestCase]
      public void identify_error_column()
      {
         var errorParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Error [pmol/l]");
         (errorParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = errorParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Error");
         mapping.MappedColumn.Unit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_measurement_column_without_the_name_on_the_headings()
      {
         var measurementParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Concentration (molar) [pmol/l]");
         (measurementParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = measurementParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Concentration");
         mapping.MappedColumn.Unit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_metadata_parameters()
      {
         var metadataParameters = sut.Parameters.Where(p => p is MetaDataFormatParameter).ToList();
         metadataParameters.Count.ShouldBeEqualTo(5);
         foreach (var name in new[] { "Organ", "Compartment", "Species", "Dose", "Route" })
         {
            metadataParameters.FirstOrDefault(parameter => parameter.ColumnName == name).ShouldNotBeNull();
         }
      }

      [TestCase]
      public void identify_groupBy_parameters()
      {
         var groupByParameters = sut.Parameters.Where(p => p is GroupByDataFormatParameter).ToList();
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
         A.CallTo(() => _mockedData.GetHeaders()).Returns(_basicFormat.GetHeaders());
         A.CallTo(() => _mockedData.GetColumnDescription(A<string>.Ignored)).ReturnsLazily(columnName => _basicFormat.GetColumnDescription(columnName.Arguments[0].ToString()));
      }

      protected override void Because()
      {
         sut.SetParameters(_mockedData, _columnInfos);
      }

      [TestCase]
      public void parse_basic_format()
      {
         var data = sut.Parse
         (
            _mockedData,
            _columnInfos
         );
         data.Count.ShouldBeEqualTo(10);
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.That.Matches(f => f.Invoke(new List<string>() { "", "", "", "", "GLP-1_7-36 total", "", "", "", "", "H" })))).MustHaveHappened();
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

      [TestCase]
      public void parse_lloq()
      {
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.Ignored)).ReturnsLazily(
            param => new List<List<string>>()
            {
               new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", $"<{0.01}", "0", "po", "<GroupId>" },
               new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", $"   <{0.01}", "0", "po", "<GroupId>" },
               new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "10", "0", "po", "<GroupId>" }
            });

         var data = sut.Parse
         (
            _mockedData,
            _columnInfos
         );
         foreach (var dataset in data)
         {
            dataset.ElementAt(1).Value.First().Lloq.ShouldBeEqualTo(0.01);
            dataset.ElementAt(1).Value.First().Value.ShouldBeEqualTo(0);
            dataset.ElementAt(1).Value.ElementAt(1).Lloq.ShouldBeEqualTo(0.01);
            dataset.ElementAt(1).Value.ElementAt(1).Value.ShouldBeEqualTo(0);
            dataset.ElementAt(1).Value.ElementAt(2).Lloq.ShouldBeNull();
            dataset.ElementAt(1).Value.ElementAt(2).Value.ShouldBeEqualTo(10);
         }
      }
   }
}
