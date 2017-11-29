using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_DimensionFactoryExtensions : StaticContextSpecification
   {
      protected IDimensionFactory _factory;
      protected IDimension _dimension;

      protected override void Context()
      {
         _factory = DimensionFactoryForSpecs.Factory;
         _dimension = _factory.Dimensions.First();
      }
   }

   public class When_sorting_merged_dimensions_for_editors : concern_for_DimensionFactoryExtensions
   {
      private IEnumerable<IDimension> _result;

      protected override void Because()
      {
         _result = _factory.AllDimensionsForEditors(_dimension);
      }

      [Observation]
      public void should_sort_the_units_by_name()
      {
         _result.ShouldOnlyContainInOrder(_factory.Dimensions.OrderBy(dimension => dimension.Name).ToArray());
      }
   }
}
