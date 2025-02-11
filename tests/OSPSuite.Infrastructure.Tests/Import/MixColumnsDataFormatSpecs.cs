using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_MixColumnsDataFormat : ContextSpecification<MixColumnsDataFormat>
   {
      protected DataSheet _rawDataSheet;
      protected ColumnInfoCache _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected IEnumerable<string> _headers { get; set; } = new[] { "Time [h]", "Concentration [mol]", "Error [mol]" };

      protected override void Context()
      {
         sut = new MixColumnsDataFormat();
         _rawDataSheet = A.Fake<DataSheet>();
         A.CallTo(() => _rawDataSheet.GetHeaders()).ReturnsLazily(() => _headers);
         A.CallTo(() => _rawDataSheet.GetColumnDescription(A<string>.Ignored))
            .Returns(new ColumnDescription(0) { Level = ColumnDescription.MeasurementLevel.Numeric });
         _columnInfos = new ColumnInfoCache
         {
            new ColumnInfo { DisplayName = "Time", Name = "Time" },
            new ColumnInfo { DisplayName = "Concentration", Name = "Concentration" },
            new ColumnInfo { DisplayName = "Error", Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration" }
         };
         _columnInfos["Time"].SupportedDimensions.Add(DimensionFactoryForSpecs.TimeDimension);
         _columnInfos["Concentration"].SupportedDimensions.Add(DimensionFactoryForSpecs.ConcentrationDimension);
         _columnInfos["Concentration"].SupportedDimensions.Add(DimensionFactoryForSpecs.LengthDimension);
         _columnInfos["Error"].SupportedDimensions.Add(DimensionFactoryForSpecs.ConcentrationDimension);
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory { Name = "Organ" },
            new MetaDataCategory { Name = "Compartment" },
            new MetaDataCategory { Name = "Species" },
            new MetaDataCategory { Name = "Dose" },
            new MetaDataCategory { Name = "Molecule" },
            new MetaDataCategory { Name = "Route" }
         };
      }
   }

   public class When_setting_parameters : concern_for_MixColumnsDataFormat
   {
      protected override void Because()
      {
         sut.SetParameters(_rawDataSheet, _columnInfos, _metaDataCategories);
      }

      [Observation]
      public void infers_unit_from_header_for_measurement_and_error()
      {
         var concentrationMappingColumn =
            sut.GetColumnByName<MappingDataFormatParameter>("Concentration [mol]").MappedColumn;
         concentrationMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         concentrationMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");

         var errorMappingColumn = sut.GetColumnByName<MappingDataFormatParameter>("Error [mol]").MappedColumn;
         errorMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         errorMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");
         errorMappingColumn.ErrorStdDev.ShouldBeEqualTo(Constants.STD_DEV_ARITHMETIC);
      }
   }

   public class When_setting_parameters_without_error_unit_on_headers : concern_for_MixColumnsDataFormat
   {
      protected override void Context()
      {
         base.Context();
         _headers = new[] { "Time [h]", "Concentration [mol]", "Error" };
      }

      protected override void Because()
      {
         sut.SetParameters(_rawDataSheet, _columnInfos, _metaDataCategories);
      }

      [Observation]
      public void infers_unit_from_header_for_measurement_and_error()
      {
         var concentrationMappingColumn =
            sut.GetColumnByName<MappingDataFormatParameter>("Concentration [mol]").MappedColumn;
         concentrationMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         concentrationMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");

         var errorMappingColumn = sut.GetColumnByName<MappingDataFormatParameter>("Error").MappedColumn;
         errorMappingColumn.ErrorStdDev.ShouldBeEqualTo(Constants.STD_DEV_GEOMETRIC);
      }
   }

   public class When_setting_parameters_without_error_unit_on_headers_even_with_another_casing : concern_for_MixColumnsDataFormat
   {
      protected override void Context()
      {
         base.Context();
         _headers = new[] { "Time [H]", "Concentration [MoL]", "Error" };
      }

      protected override void Because()
      {
         sut.SetParameters(_rawDataSheet, _columnInfos, _metaDataCategories);
      }

      [Observation]
      public void infers_unit_from_header_for_measurement_and_error()
      {
         var concentrationMappingColumn =
            sut.GetColumnByName<MappingDataFormatParameter>("Concentration [MoL]").MappedColumn;
         concentrationMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         concentrationMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");

         var errorMappingColumn = sut.GetColumnByName<MappingDataFormatParameter>("Error").MappedColumn;
         errorMappingColumn.ErrorStdDev.ShouldBeEqualTo(Constants.STD_DEV_GEOMETRIC);
      }
   }

   public class When_setting_parameters_with_wrong_error_unit_on_headers : concern_for_MixColumnsDataFormat
   {
      protected override void Context()
      {
         base.Context();
         _headers = new[] { "Time [h]", "Concentration [mol]", "Error [lol]" };
      }

      protected override void Because()
      {
         sut.SetParameters(_rawDataSheet, _columnInfos, _metaDataCategories);
      }

      [Observation]
      public void infers_unit_from_header_for_measurement_and_error()
      {
         var concentrationMappingColumn =
            sut.GetColumnByName<MappingDataFormatParameter>("Concentration [mol]").MappedColumn;
         concentrationMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         concentrationMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");

         var errorMappingColumn = sut.GetColumnByName<MappingDataFormatParameter>("Error [lol]").MappedColumn;
         errorMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo(UnitDescription.InvalidUnit);
         errorMappingColumn.ErrorStdDev.ShouldBeEqualTo(Constants.STD_DEV_ARITHMETIC);
      }

      public class When_setting_parameters_case_sensitive_unit : concern_for_MixColumnsDataFormat
      {
         protected override void Context()
         {
            base.Context();
            _headers = new[] { "Time [h]", "Measurement [um]" };
         }

         protected override void Because()
         {
            sut.SetParameters(_rawDataSheet, _columnInfos, _metaDataCategories);
         }

         [Observation]
         public void correct_measurement_dimension_should_be_inferred()
         {
            var concentrationMappingColumn =
               sut.GetColumnByName<MappingDataFormatParameter>("Measurement [um]").MappedColumn;
            concentrationMappingColumn.Dimension.Name.ShouldBeEqualTo("Length");
            concentrationMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("um");
         }
      }
   }
}