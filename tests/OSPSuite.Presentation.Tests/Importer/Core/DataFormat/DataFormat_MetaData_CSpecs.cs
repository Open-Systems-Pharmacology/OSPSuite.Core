using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Importer;
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   internal class TestUnformattedData : UnformattedData
   {
      public TestUnformattedData(Cache<string, ColumnDescription> headers)
      {
         _headers = headers;
      }
   }
   public abstract class ConcernforDataFormat_Nonmem : ContextSpecification<DataFormat_Nonmem>
   {
      protected IUnformattedData _basicFormat;
      protected IReadOnlyList<ColumnInfo> _columnInfos;

      protected override void Context()
      {
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time", IsMandatory = true },
            new ColumnInfo() { DisplayName = "Concentration", IsMandatory = true },
            new ColumnInfo() { DisplayName = "Error", IsMandatory = false }
         };
         sut = new DataFormat_Nonmem();
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
                  "Time",
                  new ColumnDescription(5)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Concentration",
                  new ColumnDescription(6)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric,
                     ExistingValues = null
                  }
               },
               {
                  "Error",
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
               },
               {
                  "Time_unit",
                  new ColumnDescription(10)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "min" }
                  }
               },
               {
                  "Concentration_unit",
                  new ColumnDescription(11)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "pmol/l" }
                  }
               },
               {
                  "Error_unit",
                  new ColumnDescription(12)
                  {
                     Level = ColumnDescription.MeasurementLevel.Discrete,
                     ExistingValues = new List<string>() { "pmol/l" }
                  }
               },
               {
                  "lloq",
                  new ColumnDescription(13)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric
                  }
               }
            }
         );
      }
   }

   public class When_Nonmem_is_checking_format : ConcernforDataFormat_Nonmem
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
                  "Time",
                  _basicFormat.GetColumnDescription("Time")
               }
            }
         );
         sut.SetParameters(singleColumn, _columnInfos).ShouldBeFalse();
      }
   }

   public class When_Nonmem_is_listing_parameters : ConcernforDataFormat_Nonmem
   {
      protected override void Because()
      {
         sut.SetParameters(_basicFormat, _columnInfos);
      }

      [TestCase]
      public void identify_time_column()
      {
         var timeParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Time");
         (timeParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = timeParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Time");
         mapping.MappedColumn.SelectedUnit.ShouldBeEqualTo("min");
      }

      [TestCase]
      public void identify_error_column()
      {
         var errorParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Error");
         (errorParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = errorParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Error");
         mapping.MappedColumn.SelectedUnit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_measurement_column_without_the_name_on_the_headings()
      {
         var measurementParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Concentration");
         (measurementParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = measurementParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Concentration");
         mapping.MappedColumn.SelectedUnit.ShouldBeEqualTo("pmol/l");
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

   public class When_Nonmem_is_parsing_format : ConcernforDataFormat_Nonmem
   {
      private IUnformattedData _mockedData;
      private string[] _molecules = new string[] { "GLP-1_7-36 total", "Glucose", "Insuline", "GIP_total", "Glucagon" };
      private string[] _groupIds = new string[] { "H", "T2DM" };
      protected override void Context()
      {
         base.Context();
         _mockedData = A.Fake<IUnformattedData>();
         A.CallTo(() => _mockedData.GetHeaders()).Returns(_basicFormat.GetHeaders());
         A.CallTo(() => _mockedData.GetColumnDescription(A<string>.Ignored)).ReturnsLazily(columnName => _basicFormat.GetColumnDescription(columnName.Arguments[0].ToString()));
         foreach (var molecule in _molecules)
            foreach (var groupId in _groupIds)
               for (var time = 0; time < 10; time++)
               {
                  _basicFormat.AddRow(new List<string>()
                  {
                     "PeripheralVenousBlood",
                     "Arterialized",
                     "Human",
                     "75 [g] glucose",
                     molecule,
                     $"time",
                     "0",
                     "0",
                     "po",
                     groupId,
                     "min",
                     "pmol/l",
                     "pmol/l",
                     $"{0.01}"
                  });
               }
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
            _basicFormat,
            _columnInfos
         );
         data.Count.ShouldBeEqualTo(10);
         for (var molecule = 0; molecule < _molecules.Length; molecule++)
            for (var groupId = 0; groupId < _groupIds.Length; groupId++)
            {
               data.Keys.ElementAt(molecule * _groupIds.Length + groupId).ElementAt(5).Value.ShouldBeEqualTo(_molecules[molecule]);
               data.Keys.ElementAt(molecule * _groupIds.Length + groupId).ElementAt(6).Value.ShouldBeEqualTo(_groupIds[groupId]);
            }
      }

      [TestCase]
      public void parse_lloq()
      {
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.Ignored)).ReturnsLazily(
            param => new List<List<string>>()
            {
               new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "0", "0", "po", "<GroupId>", "min", "pmol/l", "pmol/l", $"{0.01}" },
               new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "0", "0", "po", "<GroupId>", "min", "pmol/l", "pmol/l", $" {0.01}" },
               new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "10", "0", "po", "<GroupId>", "min", "pmol/l", "pmol/l", "0" }
            });

         var data = sut.Parse
         (
            _mockedData,
            _columnInfos
         );
         foreach (var dataset in data)
         {
            dataset.Value.ElementAt(1).Value.First().Lloq.ShouldBeEqualTo(0.01);
            dataset.Value.ElementAt(1).Value.First().Value.ShouldBeEqualTo(0);
            dataset.Value.ElementAt(1).Value.ElementAt(1).Lloq.ShouldBeEqualTo(0.01);
            dataset.Value.ElementAt(1).Value.ElementAt(1).Value.ShouldBeEqualTo(0);
            dataset.Value.ElementAt(1).Value.ElementAt(2).Lloq.ShouldBeEqualTo(0);
            dataset.Value.ElementAt(1).Value.ElementAt(2).Value.ShouldBeEqualTo(10);
         }
      }
   }
}
