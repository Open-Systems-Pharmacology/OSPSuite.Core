using System.Collections.Generic;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core
{
   public abstract class concern_for_UnitToMinorIntervalMapper : ContextSpecification<UnitToMinorIntervalMapper>
   {
      protected IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _dimensionFactory = DimensionFactoryForSpecs.Factory;
         sut = new UnitToMinorIntervalMapper();
      }

      protected Unit GetUnitForName(string unitName)
      {
         return _dimensionFactory.Dimension(Constants.Dimension.TIME).Unit(unitName);
      }
   }

   public class When_checking_availibility_for_unit_with_preferred_intervals : concern_for_UnitToMinorIntervalMapper
   {
      private IEnumerable<string> getUnits()
      {
         yield return Constants.Dimension.Units.Days;
         yield return Constants.Dimension.Units.Hours;
         yield return Constants.Dimension.Units.Minutes;
         yield return Constants.Dimension.Units.Weeks;
         yield return Constants.Dimension.Units.Years;
      }

      [Observation]
      public void mapper_should_indicate_that_it_has_a_preferred_value()
      {
         getUnits().Each(unit => { sut.HasPreferredMinorIntervalsFor(GetUnitForName(unit)).ShouldBeTrue(); });
      }
   }

   public class When_checking_availibility_for_unit_without_preferred_intervals : concern_for_UnitToMinorIntervalMapper
   {
      [Observation]
      public void mapper_should_indicate_that_it_does_not_have_a_preferred_value()
      {
         sut.HasPreferredMinorIntervalsFor(GetUnitForName(Constants.Dimension.Units.Months)).ShouldBeFalse();
      }
   }

   public class When_mapping_preferred_intervals_from_hours : concern_for_UnitToMinorIntervalMapper
   {
      private Unit _unit;

      protected override void Context()
      {
         base.Context();
         _unit = GetUnitForName(Constants.Dimension.Units.Days);
      }

      [TestCase(100, 1000, true, 0, 1.0F)]
      [TestCase(50, 1000, true, 0, 1.0F)]
      [TestCase(20, 1000, false, 1, 1.0F)]
      [TestCase(10, 1000, false, 1, 1.0F)]
      [TestCase(5, 1000, false, 2, 0.5F)]
      [TestCase(2, 1000, false, 2, 0.25F)]
      [TestCase(1, 1000, false, 5, 0.25F)]
      [TestCase(1, 2000, false, 5, 0.25F)]
      [TestCase(0.3F, 1000, true, 5, 0.25F)]
      [TestCase(10, 500, false, 1, 1.0F)]
      public void should_return_the_preferred_number_of_minor_ticks_and_number_of_labels(float axisWidthInUnit, int axisWidthInPixel, bool autoScale, int numberOfMinorTicks, float gridSpacing)
      {
         var res = sut.MapFrom(_unit, axisWidthInUnit, axisWidthInPixel);
         res.AutoScale.ShouldBeEqualTo(autoScale);
         if (!res.AutoScale)
         {
            res.MinorTicks.ShouldBeEqualTo(numberOfMinorTicks);
            res.GridSpacing.ShouldBeEqualTo(gridSpacing);
         }
      }
   }
}