﻿using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Import;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   internal class TestUnformattedData : UnformattedData
   {
      public TestUnformattedData(Cache<string, ColumnDescription> headers)
      {
         _headers = headers;
      }
   }
   public abstract class ConcernforDataFormat_Nonmem : ContextSpecification<DataFormatNonmem>
   {
      protected IUnformattedData _basicFormat;
      protected ColumnInfoCache _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected Cache<string, ColumnDescription> _headersCache;
      protected string[] _molecules = new string[] { "GLP-1_7-36 total", "Glucose", "Insuline", "GIP_total", "Glucagon" };
      protected string[] _groupIds = new string[] { "H", "T2DM" };

      protected override void Context()
      {
         _columnInfos = new ColumnInfoCache
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

         sut = new DataFormatNonmem();
         _headersCache = new Cache<string, ColumnDescription>(){
               {
                  "Organ",
                  new ColumnDescription(0, new List<string>() { "PeripheralVenousBlood" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Compartment",
                  new ColumnDescription(1, new List<string>() { "Arterialized" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Species",
                  new ColumnDescription(2, new List<string>() { "Human" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Dose",
                  new ColumnDescription(3, new List<string>() { "75 [g] glucose" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Molecule",
                  new ColumnDescription(4, new List<string>() { "GLP-1_7-36 total", "Glucose", "Insuline", "GIP_total", "Glucagon" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Time",
                  new ColumnDescription(5, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Concentration",
                  new ColumnDescription(6, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Error",
                  new ColumnDescription(7, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Route",
                  new ColumnDescription(8, new List<string>() { "po" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Groupe Id",
                  new ColumnDescription(9, new List<string>() { "H", "T2DM" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Time_unit",
                  new ColumnDescription(10, new List<string>() { "min" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Concentration_unit",
                  new ColumnDescription(11, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Error_unit",
                  new ColumnDescription(12, new List<string>() { "pmol/l" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Concentration_lloq",
                  new ColumnDescription(13)
                  {
                     Level = ColumnDescription.MeasurementLevel.Numeric
                  }
               }
            };
         _basicFormat = new TestUnformattedData(_headersCache);
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
   }

   public class When_Nonmem_is_checking_format : ConcernforDataFormat_Nonmem
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
            _metaDataCategories
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
                  "Time",
                  _basicFormat.GetColumnDescription("Time")
               }
            }
         );
         sut.SetParameters(singleColumn, _columnInfos, _metaDataCategories).ShouldBeEqualTo(0);
      }
   }

   public class When_Nonmem_is_listing_parameters : ConcernforDataFormat_Nonmem
   {
      protected override void Because()
      {
         sut.SetParameters(_basicFormat, _columnInfos, _metaDataCategories);
      }

      [TestCase]
      public void identify_time_column()
      {
         var timeParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Time");
         (timeParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = timeParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Time");
         mapping.MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("min");
      }

      [TestCase]
      public void identify_error_column()
      {
         var errorParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Error");
         (errorParameter is MappingDataFormatParameter).ShouldBeTrue();
         var mapping = errorParameter as MappingDataFormatParameter;
         mapping.MappedColumn.Name.ShouldBeEqualTo("Error");
         mapping.MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo("pmol/l");
      }

      [TestCase]
      public void identify_measurement_column_without_the_name_on_the_headings()
      {
         var measurementParameter = sut.Parameters.FirstOrDefault(p => p.ColumnName == "Concentration");
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
         groupByParameters.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_Nonmem_is_parsing_format : ConcernforDataFormat_Nonmem
   {
      private IUnformattedData _mockedData;
      private string _testSheetName = "Sheet1";

      protected override void Context()
      {
         base.Context();
         _mockedData = A.Fake<IUnformattedData>();
         A.CallTo(() => _mockedData.GetHeaders()).Returns(_basicFormat.GetHeaders());
         A.CallTo(() => _mockedData.GetColumnDescription(A<string>.Ignored)).ReturnsLazily(columnName => _basicFormat.GetColumnDescription(columnName.Arguments[0].ToString()));
         A.CallTo(() => _mockedData.GetColumn(A<string>.Ignored)).ReturnsLazily(columnName => _basicFormat.GetColumn(columnName.Arguments[0].ToString()));
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
            _testSheetName,
            _basicFormat,
            _columnInfos
         ).ToList();
         data.Count().ShouldBeEqualTo(5);
         for (var molecule = 0; molecule < _molecules.Length; molecule++)
            data[molecule].ValueForColumn(4).ShouldBeEqualTo(_molecules[molecule]);
      }

      [TestCase]
      public void parse_lloq()
      {
         A.CallTo(() => _mockedData.GetRows(A<Func<IEnumerable<string>, bool>>.Ignored)).ReturnsLazily(
            param => new List<UnformattedRow>()
            {
               new UnformattedRow(0, new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "0", "0", "po", "<GroupId>", "min", "pmol/l", "pmol/l", $"{0.01}" }),
               new UnformattedRow(1, new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "0", "0", "po", "<GroupId>", "min", "pmol/l", "pmol/l", $" {0.01}" }),
               new UnformattedRow(2, new List<string>() { "PeripheralVenousBlood", "Arterialized", "Human", "75 [g] glucose", "<Molecule>", "99", "10", "0", "po", "<GroupId>", "min", "pmol/l", "pmol/l", "0" })
            });

         var data = sut.Parse
         (
            _testSheetName,
            _mockedData,
            _columnInfos
         );
         foreach (var dataset in data)
         {
            dataset.Data.ElementAt(1).Value.First().Lloq.ShouldBeEqualTo(0.01);
            dataset.Data.ElementAt(1).Value.First().Measurement.ShouldBeEqualTo(0);
            dataset.Data.ElementAt(1).Value.ElementAt(1).Lloq.ShouldBeEqualTo(0.01);
            dataset.Data.ElementAt(1).Value.ElementAt(1).Measurement.ShouldBeEqualTo(0);
            dataset.Data.ElementAt(1).Value.ElementAt(2).Lloq.ShouldBeEqualTo(0);
            dataset.Data.ElementAt(1).Value.ElementAt(2).Measurement.ShouldBeEqualTo(10);
         }
      }

      public class When_parsing_headers_with_capital : ConcernforDataFormat_Nonmem
      {
         private IUnformattedData _mockedDataCapital;
         protected override void Because()
         {
            _headersCache.Remove("Dose");
            _headersCache.Add("DOSE",
                              new ColumnDescription(3, new List<string>() { "75 [g] glucose" }, ColumnDescription.MeasurementLevel.Discrete)
                           );
            _mockedDataCapital = new TestUnformattedData(_headersCache);
            sut.SetParameters(_mockedDataCapital, _columnInfos, _metaDataCategories);
         }

         [TestCase]
         public void metadataId_should_be_correctly_assigned_and_not_on_capital_letters()
         {
            sut.Parameters.OfType<MetaDataFormatParameter>().FirstOrDefault(p => p.ColumnName == "DOSE").MetaDataId.ShouldBeEqualTo("Dose");
         }

      }
   }
}
