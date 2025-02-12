using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core;

namespace OSPSuite.Infrastructure.Import
{
   public class concern_for_ColumnInfo : ContextSpecification<ColumnInfo>
   {
      protected override void Context()
      {
         sut = new ColumnInfo();
      }
   }

   public class When_calculating_the_dimension_for_the_column_when_the_mapping_is_not_set_explicitly : concern_for_ColumnInfo
   {
      private IDimension _dimension;
      private IDimension _massDimension;
      private IDimension _doseDimension;

      protected override void Context()
      {
         base.Context();
         _massDimension = new Dimension(new BaseDimensionRepresentation(), "Mass", "mg");
         _doseDimension = new Dimension(new BaseDimensionRepresentation(), "Dose", "mg");
         sut.SupportedDimensions.AddRange(new List<IDimension> { _doseDimension, _massDimension });
         sut.MappedDimension = _massDimension;
      }

      protected override void Because()
      {
         _dimension = sut.DimensionForUnit("mg");
      }

      [Observation]
      public void should_use_the_mapped_dimension()
      {
         _dimension.ShouldBeEqualTo(_massDimension);
      }
   }

   public class When_calculating_the_dimension_for_the_column_when_the_mapping_is_set_explicitly : concern_for_ColumnInfo
   {
      private IDimension _dimension;
      private IDimension _massDimension;
      private IDimension _doseDimension;

      protected override void Context()
      {
         base.Context();
         _massDimension = new Dimension(new BaseDimensionRepresentation(), "Mass", "mg");
         _doseDimension = new Dimension(new BaseDimensionRepresentation(), "Dose", "mg");
         sut.SupportedDimensions.AddRange(new List<IDimension>{_doseDimension, _massDimension});
      }

      protected override void Because()
      {
         _dimension = sut.DimensionForUnit("mg");
      }

      [Observation]
      public void should_use_the_first_dimension()
      {
         _dimension.ShouldBeEqualTo(_doseDimension);
      }
   }
}