using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_DimensionExtensions : StaticContextSpecification
   {
      protected IDimension _dimension;

      protected override void Context()
      {
         _dimension = DomainHelperForSpecs.TimeDimensionForSpecs();
      }
   }

   public class When_converting_an_array_of_values_to_base_unit : concern_for_DimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_values()
      {
         _dimension.UnitValuesToBaseUnitValues(_dimension.Unit("h"), new[] {1d, 2d}).ShouldOnlyContainInOrder(60, 120);
      }
   }

   public class When_converting_an_array_of_base_values_to_unit : concern_for_DimensionExtensions
   {
      [Observation]
      public void should_return_the_expected_values()
      {
         _dimension.BaseUnitValuesToUnitValues(_dimension.Unit("h"), new[] {60d, 120d}).ShouldOnlyContainInOrder(1, 2);
      }
   }

   public class When_checking_if_two_simple_dimensions_are_equivalent : concern_for_DimensionExtensions
   {
      [Observation]
      public void should_return_true_if_they_are_the_same()
      {
         _dimension.IsEquivalentTo(_dimension).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_their_base_dimension_representation_are_equal()
      {
         _dimension.IsEquivalentTo(new Dimension(_dimension.BaseRepresentation, _dimension.Name, "XX")).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         _dimension.IsEquivalentTo(new Dimension(new BaseDimensionRepresentation(new double[]{1,2,3,4,5,6,7}), _dimension.Name, "XX")).ShouldBeFalse();
      }
   }

   public class When_checking_if_one_simple_dimension_and_a_merge_dimension_are_equivalent : concern_for_DimensionExtensions
   {
      private Dimension _nonEquivalentDimension;
      private Dimension _equivalentDimension;

      protected override void Context()
      {
         base.Context();
         _nonEquivalentDimension = new Dimension(new BaseDimensionRepresentation(new double[] {1, 2, 3, 4, 5, 6, 7}), _dimension.Name, "XX");
         _equivalentDimension = new Dimension(_dimension.BaseRepresentation, _dimension.Name, "XX");
      }
      [Observation]
      public void should_return_true_if_the_dimension_is_equivalent_to_the_source_dimension()
      {
         var mergedDimension = new MergedDimensionFor<IParameter>(_equivalentDimension, new [] {_nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         _dimension.IsEquivalentTo(mergedDimension).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_dimension_is_equivalent_to_any_of_the_target_dimension()
      {
         var mergedDimension = new MergedDimensionFor<IParameter>(_nonEquivalentDimension, new[] { _equivalentDimension, }, new List<IDimensionConverterFor>());
         _dimension.IsEquivalentTo(mergedDimension).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         var mergedDimension = new MergedDimensionFor<IParameter>(_nonEquivalentDimension, new[] { _nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         _dimension.IsEquivalentTo(mergedDimension).ShouldBeFalse();
      }
   }

   public class When_checking_if_one_merge_dimension_and_a_simple_dimension_are_equivalent : concern_for_DimensionExtensions
   {
      private Dimension _nonEquivalentDimension;
      private Dimension _equivalentDimension;

      protected override void Context()
      {
         base.Context();
         _nonEquivalentDimension = new Dimension(new BaseDimensionRepresentation(new double[] { 1, 2, 3, 4, 5, 6, 7 }), _dimension.Name, "XX");
         _equivalentDimension = new Dimension(_dimension.BaseRepresentation, _dimension.Name, "XX");
      }
      [Observation]
      public void should_return_true_if_the_dimension_is_equivalent_to_the_source_dimension()
      {
         var mergedDimension = new MergedDimensionFor<IParameter>(_equivalentDimension, new[] { _nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         mergedDimension.IsEquivalentTo(_dimension).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_dimension_is_equivalent_to_any_of_the_target_dimension()
      {
         var mergedDimension = new MergedDimensionFor<IParameter>(_nonEquivalentDimension, new[] { _equivalentDimension, }, new List<IDimensionConverterFor>());
         mergedDimension.IsEquivalentTo(_dimension).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         var mergedDimension = new MergedDimensionFor<IParameter>(_nonEquivalentDimension, new[] { _nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         mergedDimension.IsEquivalentTo(_dimension).ShouldBeFalse();
      }
   }


   public class When_checking_if_two_merge_dimensions_are_equivalent : concern_for_DimensionExtensions
   {
      private Dimension _nonEquivalentDimension;
      private Dimension _equivalentDimension;

      protected override void Context()
      {
         base.Context();
         _nonEquivalentDimension = new Dimension(new BaseDimensionRepresentation(new double[] { 1, 2, 3, 4, 5, 6, 7 }), _dimension.Name, "XX");
         _equivalentDimension = new Dimension(_dimension.BaseRepresentation, _dimension.Name, "XX");

      }
      [Observation]
      public void should_return_true_if_the_merge_dimensions_have_equivalent_source_dimension()
      {
         var mergedDimension1 = new MergedDimensionFor<IParameter>(_equivalentDimension, new[] { _nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         var mergedDimension2 = new MergedDimensionFor<IParameter>(_dimension, new[] { _nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         mergedDimension1.IsEquivalentTo(mergedDimension2).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_merge_dimensions_have_equivalent_source_and_target_dimensions()
      {
         var mergedDimension1 = new MergedDimensionFor<IParameter>(_nonEquivalentDimension, new[] { _equivalentDimension, }, new List<IDimensionConverterFor>());
         var mergedDimension2 = new MergedDimensionFor<IParameter>(_dimension, new[] { _nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         mergedDimension1.IsEquivalentTo(mergedDimension2).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         var mergedDimension1 = new MergedDimensionFor<IParameter>(_nonEquivalentDimension, new[] { _nonEquivalentDimension, }, new List<IDimensionConverterFor>());
         var mergedDimension2 = new MergedDimensionFor<IParameter>(_dimension, new[] { _equivalentDimension, }, new List<IDimensionConverterFor>());
         mergedDimension1.IsEquivalentTo(mergedDimension2).ShouldBeFalse();
      }
   }
}