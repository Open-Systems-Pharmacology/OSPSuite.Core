using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_DisplayUnitsManager : ContextSpecification<DisplayUnitsManager>
   {
      protected override void Context()
      {
         sut = new DisplayUnitsManager();
      }
   }

   public class When_retrieving_the_default_unit_for_a_given_dimension : concern_for_DisplayUnitsManager
   {
      private IDimension _dimension1;
      private IDimension _dimension2;
      private Unit _myDefaultUnit;
      private IDimension _mergedDimensionSource;
      private IDimension _mergedDimensionTarget;

      protected override void Context()
      {
         base.Context();
         _myDefaultUnit = A.Fake<Unit>();
         _dimension1 = new Dimension(new BaseDimensionRepresentation(),"Dim", "BASE_UNIT" );
         _myDefaultUnit = _dimension1.AddUnit("UNIT", 10, 0);

         _dimension2 = A.Fake<IDimension>();
         _mergedDimensionSource = new MergedDimensionFor<Axis>(_dimension1, new[] { _dimension2 },new List<IDimensionConverterFor>());
         var converter = A.Fake<IDimensionConverterFor>();
         A.CallTo(() => converter.CanConvertTo(_dimension1)).Returns(true);
         _mergedDimensionTarget = new MergedDimensionFor<Axis>(_dimension2, new[] { _dimension1 },new [] {converter});

         sut.AddDisplayUnit(new DisplayUnitMap { Dimension = _dimension1, DisplayUnit = _myDefaultUnit });
      }

      [Observation]
      public void should_return_the_managed_default_unit_if_this_was_defined()
      {
         sut.DisplayUnitFor(_dimension1).ShouldBeEqualTo(_myDefaultUnit);
      }

      [Observation]
      public void should_return_the_managed_default_unit_of_the_source_if_this_was_defined_for_a_merged_dimension()
      {
         sut.DisplayUnitFor(_mergedDimensionSource).ShouldBeEqualTo(_myDefaultUnit);
      }

      [Observation]
      public void should_return_the_managed_default_unit_of_the_target_dimension_if_the_source_dimension_in_not_availalbe_for_a_merged_dimension()
      {
         sut.DisplayUnitFor(_mergedDimensionTarget).ShouldBeEqualTo(_myDefaultUnit);
      }

      [Observation]
      public void should_return_null_otherwise()
      {
         sut.DisplayUnitFor(_dimension2).ShouldBeNull();
      }
   }

   public class When_retrieving_the_default_unit_for_a_given_dimension_and_the_stored_unit_does_not_exist_in_the_dimension : concern_for_DisplayUnitsManager
   {
      private IDimension _dimension1;
      private Unit _myDefaultUnit;

      protected override void Context()
      {
         base.Context();
         _dimension1 = A.Fake<IDimension>();
         _myDefaultUnit = A.Fake<Unit>();
         sut.AddDisplayUnit(new DisplayUnitMap { Dimension = _dimension1, DisplayUnit = _myDefaultUnit });
      }

      [Observation]
      public void should_return_null()
      {
         sut.DisplayUnitFor(_dimension1).ShouldBeNull();
      }
   }
}