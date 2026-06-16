using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
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
      public void should_sort_the_units_by_display_name()
      {
         _result.ShouldOnlyContainInOrder(_factory.Dimensions.OrderBy(dimension => dimension.DisplayName).ToArray());
      }
   }

   public class When_getting_all_dimensions_for_editors_for_an_axis_holding_a_merged_dimension : StaticContextSpecification
   {
      private IDimensionFactory _mergingFactory;
      private IDimension _axisDimension;

      protected override void Context()
      {
         _mergingFactory = new DimensionFactory();

         var molarConcentration = new Dimension(new BaseDimensionRepresentation(), "MolarConcentration", "µmol/l");
         var massConcentration = new Dimension(new BaseDimensionRepresentation(), "MassConcentration", "µg/l");
         _mergingFactory.AddDimension(molarConcentration);
         _mergingFactory.AddDimension(massConcentration);
         _mergingFactory.AddMergingInformation(new SimpleDimensionMergingInformation(molarConcentration, massConcentration));

         // Mimics how an axis stores its dimension: a freshly minted merged dimension instance.
         // MergedDimensionFor returns a new instance on every call for mergeable dimensions.
         _axisDimension = _mergingFactory.OptimalDimension(molarConcentration);
      }

      [Observation]
      public void should_return_the_exact_dimension_instance_held_by_the_axis_so_it_is_selectable_by_reference_in_the_editor()
      {
         _mergingFactory.AllDimensionsForEditors(_axisDimension).ShouldContain(_axisDimension);
      }
   }
}
