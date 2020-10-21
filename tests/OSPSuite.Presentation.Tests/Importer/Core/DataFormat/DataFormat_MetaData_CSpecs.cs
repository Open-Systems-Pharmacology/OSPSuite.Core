using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public abstract class ConcernforDataFormat_TMetaData_C : ContextSpecification<DataFormatHeadersWithUnits>
   {
      protected IUnformattedData _basicFormat;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;

      protected override void Context()
      {
         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time", IsMandatory = true },
            new ColumnInfo() { DisplayName = "Concentration", IsMandatory = true },
            new ColumnInfo() { DisplayName = "Error", IsMandatory = false }
         };
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory() {Name = "Organ"},
            new MetaDataCategory() {Name = "Compartment"},
            new MetaDataCategory() {Name = "Species"},
            new MetaDataCategory() {Name = "Dose"},
            new MetaDataCategory() {Name = "Molecule"},
            new MetaDataCategory() {Name = "Route"}
         };
         sut = new DataFormatHeadersWithUnits();
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

   public class When_checking_format : ConcernforDataFormat_TMetaData_C
   {
      [TestCase]
      public void identify_basic_format()
      {
         (sut.SetParameters(_basicFormat, _columnInfos, _metaDataCategories) > 0).ShouldBeTrue();
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
            _columnInfos,
            null
         ).ShouldBeEqualTo(0);
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
         sut.SetParameters(singleColumn, _columnInfos, _metaDataCategories).ShouldBeEqualTo(0);
      }
   }

   public class When_listing_parameters : ConcernforDataFormat_TMetaData_C
   {
      protected override void Because()
      {
         sut.SetParameters(_basicFormat, _columnInfos, _metaDataCategories);
      }

      [TestCase]
      public void identify_time_column()
      {
         var timeParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Time [min]");
         (timeParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = timeParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Time");
         mapping.MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("min");
      }

      [TestCase]
      public void identify_error_column()
      {
         var errorParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Error [pmol/l]");
         (errorParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = errorParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Error");
         mapping.MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_measurement_column_without_the_name_on_the_headings()
      {
         var measurementParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Concentration (molar) [pmol/l]");
         (measurementParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = measurementParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Concentration");
         mapping.MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_metadata_parameters()
      {
         var metadataParameters = sut.Parameters.Where(p => p is MetaDataFormatParameter).ToList();
         metadataParameters.Count.ShouldBeEqualTo(6);
         foreach (var name in new[] { "Organ", "Compartment", "Species", "Dose", "Route", "Molecule" })
         {
            metadataParameters.FirstOrDefault(parameter => parameter.ColumnName == name).ShouldNotBeNull();
         }
      }

      [TestCase]
      public void identify_groupBy_parameters()
      {
         var groupByParameters = sut.Parameters.Where(p => p is GroupByDataFormatParameter).ToList();
         groupByParameters.Count.ShouldBeEqualTo(1);
         foreach (var name in new[] { "Groupe Id" })
         {
            groupByParameters.FirstOrDefault(parameter => parameter.ColumnName == name).ShouldNotBeNull();
         }
      }
   }

   public class When_parsing_format : ConcernforDataFormat_TMetaData_C
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
                     groupId
                  });
               }
      }

      protected override void Because()
      {
         sut.SetParameters(_mockedData, _columnInfos, _metaDataCategories);
      }

      [TestCase]
      public void parse_basic_format()
      {
         var data = sut.Parse
         (
            _basicFormat,
            _columnInfos
         ).ToList();
         data.Count.ShouldBeEqualTo(10);
         for (var molecule = 0; molecule < _molecules.Length; molecule++)
            for (var groupId = 0; groupId < _groupIds.Length; groupId++)
            {
               data[molecule * _groupIds.Length + groupId].ValueForColumn(4).ShouldBeEqualTo(_molecules[molecule]);
               data[molecule * _groupIds.Length + groupId].ValueForColumn(9).ShouldBeEqualTo(_groupIds[groupId]);
            }
      }

      [TestCase]
      public void parse_lloq()
      {
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.Ignored)).ReturnsLazily(
            param => new List<UnformattedRow>()
            {
               new UnformattedRow(0, new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", $"<{0.01}", "0", "po", "<GroupId>" }),
               new UnformattedRow(1, new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", $"   <{0.01}", "0", "po", "<GroupId>" }),
               new UnformattedRow(2, new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "10", "0", "po", "<GroupId>" })
            });

         var data = sut.Parse
         (
            _mockedData,
            _columnInfos
         );
         foreach (var dataset in data)
         {
            dataset.Data.ElementAt(1).Value.First().Lloq.ShouldBeEqualTo(0.01);
            dataset.Data.ElementAt(1).Value.First().Value.ShouldBeEqualTo(0);
            dataset.Data.ElementAt(1).Value.ElementAt(1).Lloq.ShouldBeEqualTo(0.01);
            dataset.Data.ElementAt(1).Value.ElementAt(1).Value.ShouldBeEqualTo(0);
            dataset.Data.ElementAt(1).Value.ElementAt(2).Lloq.ShouldBeNull();
            dataset.Data.ElementAt(1).Value.ElementAt(2).Value.ShouldBeEqualTo(10);
         }
      }
   }
}
