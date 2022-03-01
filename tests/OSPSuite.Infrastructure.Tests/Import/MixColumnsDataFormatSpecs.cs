using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_MixColumnsDataFormat : ContextSpecification<MixColumnsDataFormat>
   {
      protected IUnformattedData _rawData;
      protected ColumnInfoCache _columnInfos;
      protected IReadOnlyList<MetaDataCategory> _metaDataCategories;
      protected virtual IEnumerable<string> _headers { get => new[] { "Time [h]", "Concentration [mol]", "Error [mol]" }; }

      protected override void Context()
      {
         sut = new MixColumnsDataFormat();
         _rawData = A.Fake<IUnformattedData>();
         A.CallTo(() => _rawData.GetHeaders()).ReturnsLazily(() => _headers);
         A.CallTo(() => _rawData.GetColumnDescription(A<string>.Ignored)).Returns(new ColumnDescription(0) { Level = ColumnDescription.MeasurementLevel.Numeric });
         _columnInfos = new ColumnInfoCache
         {
            new ColumnInfo() { DisplayName = "Time", Name ="Time" },
            new ColumnInfo() { DisplayName = "Concentration", Name = "Concentration"},
            new ColumnInfo() { DisplayName = "Error", Name = "Error", IsMandatory = false, RelatedColumnOf = "Concentration"}
         };
         _columnInfos["Time"].SupportedDimensions.Add(DimensionFactoryForSpecs.TimeDimension);
         _columnInfos["Concentration"].SupportedDimensions.Add(DimensionFactoryForSpecs.ConcentrationDimension);
         _columnInfos["Error"].SupportedDimensions.Add(DimensionFactoryForSpecs.ConcentrationDimension);
         _metaDataCategories = new List<MetaDataCategory>()
         {
            new MetaDataCategory() { Name = "Organ" },
            new MetaDataCategory() { Name = "Compartment" },
            new MetaDataCategory() { Name = "Species" },
            new MetaDataCategory() { Name = "Dose" },
            new MetaDataCategory() { Name = "Molecule" },
            new MetaDataCategory() { Name = "Route" }
         };
      }
   }

   public class When_setting_parameters : concern_for_MixColumnsDataFormat
   {
      protected override void Because()
      {
         sut.SetParameters(_rawData, _columnInfos, _metaDataCategories);
      }

      [Observation]
      public void infers_unit_from_header_for_measurement_and_error()
      {
         var concentrationMappingColumn = sut.Parameters.OfType<MappingDataFormatParameter>().First(x => x.ColumnName == "Concentration [mol]").MappedColumn;
         concentrationMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         concentrationMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");

         var errorMappingColumn = sut.Parameters.OfType<MappingDataFormatParameter>().First(x => x.ColumnName == "Error [mol]").MappedColumn;
         errorMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         errorMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");
         errorMappingColumn.ErrorStdDev.ShouldBeEqualTo(Constants.STD_DEV_ARITHMETIC);
      }
   }

   public class When_setting_parameters_without_error_unit_on_headers : concern_for_MixColumnsDataFormat
   {
      protected override IEnumerable<string> _headers { get => new[] { "Time [h]", "Concentration [mol]", "Error" }; }

      protected override void Because()
      {
         sut.SetParameters(_rawData, _columnInfos, _metaDataCategories);
      }

      [Observation]
      public void infers_unit_from_header_for_measurement_and_error()
      {
         var concentrationMappingColumn = sut.Parameters.OfType<MappingDataFormatParameter>().First(x => x.ColumnName == "Concentration [mol]").MappedColumn;
         concentrationMappingColumn.Dimension.Name.ShouldBeEqualTo("Concentration");
         concentrationMappingColumn.Unit.SelectedUnit.ShouldBeEqualTo("mol");

         var errorMappingColumn = sut.Parameters.OfType<MappingDataFormatParameter>().First(x => x.ColumnName == "Error").MappedColumn;
         errorMappingColumn.ErrorStdDev.ShouldBeEqualTo(Constants.STD_DEV_GEOMETRIC);
      }
   }
}
