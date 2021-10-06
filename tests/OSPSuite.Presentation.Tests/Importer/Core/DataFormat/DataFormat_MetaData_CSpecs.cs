using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Import;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public abstract class ConcernforDataFormat_DataFormatHeadersWithUnits : ContextSpecification<DataFormatHeadersWithUnits>
   {
      protected IUnformattedData _basicFormat;
      protected IReadOnlyList<ColumnInfo> _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected IDimension _fakedTimeDimension;
      protected IDimension _fakedConcentrationDimension;
      protected IDimension _fakedErrorDimension;

      protected override void Context()
      {
         _fakedTimeDimension = A.Fake<IDimension>();
         _fakedConcentrationDimension = A.Fake<IDimension>();
         _fakedErrorDimension = A.Fake<IDimension>();

         _columnInfos = new List<ColumnInfo>()
         {
            new ColumnInfo() { DisplayName = "Time", Name = "Time", IsMandatory = true },
            new ColumnInfo() { DisplayName = "Concentration", Name = "Concentration", IsMandatory = true },
            new ColumnInfo() { DisplayName = "Error", Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration" }
         };

         _columnInfos.First(x => x.Name == "Time").SupportedDimensions.Add(_fakedTimeDimension);
         _columnInfos.First(x => x.Name == "Concentration").SupportedDimensions.Add(_fakedConcentrationDimension);
         _columnInfos.First(x => x.Name == "Error").SupportedDimensions.Add(_fakedErrorDimension);
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory() {Name = "Organ"},
            new MetaDataCategory() {Name = "Compartment"},
            new MetaDataCategory() {Name = "Species"},
            new MetaDataCategory() {Name = "Dose"},
            new MetaDataCategory() {Name = "Molecule"},
            new MetaDataCategory() {Name = "Route"}
         };

         A.CallTo(() => _fakedTimeDimension.HasUnit("min")).Returns(true);
         A.CallTo(() => _fakedConcentrationDimension.HasUnit("pmol/l")).Returns(true);
         A.CallTo(() => _fakedErrorDimension.HasUnit("pmol/l")).Returns(true);
         sut = new DataFormatHeadersWithUnits();
         _basicFormat = new TestUnformattedData
         (
            new Cache<string, ColumnDescription>()
            {
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
                  "Time [min]",
                  new ColumnDescription(5, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Concentration (molar) [pmol/l]",
                  new ColumnDescription(6, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Error [pmol/l]",
                  new ColumnDescription(7, null, ColumnDescription.MeasurementLevel.Numeric)
               },
               {
                  "Route",
                  new ColumnDescription(8, new List<string>() { "po" }, ColumnDescription.MeasurementLevel.Discrete)
               },
               {
                  "Groupe Id",
                  new ColumnDescription(9, new List<string>() { "H", "T2DM" }, ColumnDescription.MeasurementLevel.Discrete)
               }
            }
         );
      }
   }

   public class When_checking_format : ConcernforDataFormat_DataFormatHeadersWithUnits
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

      [TestCase]
      public void record_excel_columns()
      {
         sut.SetParameters(_basicFormat, _columnInfos, _metaDataCategories);
         sut.ExcelColumnNames.Count.ShouldBeEqualTo(_basicFormat.GetHeaders().Count());
      }
   }

   public class When_listing_parameters : ConcernforDataFormat_DataFormatHeadersWithUnits
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
         groupByParameters.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_parsing_format : ConcernforDataFormat_DataFormatHeadersWithUnits
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
         data.Count.ShouldBeEqualTo(5);
         for (var molecule = 0; molecule < _molecules.Length; molecule++)
            data[molecule].ValueForColumn(4).ShouldBeEqualTo(_molecules[molecule]);
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
            dataset.Data.ElementAt(1).Value.First().Measurement.ShouldBeEqualTo(0);
            dataset.Data.ElementAt(1).Value.ElementAt(1).Lloq.ShouldBeEqualTo(0.01);
            dataset.Data.ElementAt(1).Value.ElementAt(1).Measurement.ShouldBeEqualTo(0);
            dataset.Data.ElementAt(1).Value.ElementAt(2).Lloq.ShouldBeEqualTo(double.NaN);
            dataset.Data.ElementAt(1).Value.ElementAt(2).Measurement.ShouldBeEqualTo(10);
         }
      }
   }

   public class When_parsing_format_with_measurement_units_but_without_error_units : ConcernforDataFormat_DataFormatHeadersWithUnits
   {
      private IUnformattedData _mockedData;

      protected override void Context()
      {
         base.Context();
         _mockedData = A.Fake<IUnformattedData>();
         var headers = _basicFormat.GetHeaders().ToList();
         headers.Remove("Error [pmol/l]");
         headers.Add("Error");
         A.CallTo(() => _mockedData.GetHeaders()).Returns(headers);
         A.CallTo(() => _mockedData.GetColumnDescription(A<string>.Ignored)).ReturnsLazily(columnName => {
            var param = columnName.Arguments[0].ToString();
            if (param == "Error")
               return _basicFormat.GetColumnDescription("Error [pmol/l]");
            return _basicFormat.GetColumnDescription(columnName.Arguments[0].ToString());
         });
      }

      protected override void Because()
      {
         sut.SetParameters(_mockedData, _columnInfos, _metaDataCategories);
      }

      [TestCase]
      public void initialize_error_unit_from_measurement_unit()
      {
         sut.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => p.MappedColumn.Name == "Error").MappedColumn.Unit.SelectedUnit.ShouldBeEqualTo(
            sut.Parameters.OfType<MappingDataFormatParameter>().FirstOrDefault(p => p.MappedColumn.Name == "Concentration").MappedColumn.Unit.SelectedUnit
         );
      }
   }
}
