using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DataFactory : ContextSpecification<DataFactory>
   {
      private IDimensionFactory _dimensionFactory;
      private IDisplayUnitRetriever _displayUnitRetriever;
      private IDataRepositoryTask _dataRepositoryTask;

      protected override void Context()
      {
         base.Context();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();

         sut = new DataFactory(_dimensionFactory, _displayUnitRetriever, _dataRepositoryTask);
      }
   }

   public class When_calculating_the_comparison_threshold_for_a_quantity_that_is_not_a_fraction : concern_for_DataFactory
   {
      private IQuantity _quantity;
      private double _defaultThreshold;

      protected override void Context()
      {
         base.Context();
         _quantity = new Parameter().WithDimension(DomainHelperForSpecs.ConcentrationDimensionForSpecs());
         _defaultThreshold = 0.000006;
      }

      [Observation]
      public void should_return_the_default_comparison_threshold()
      {
         sut.CalculateComparisonThreshold(_quantity, _defaultThreshold).ShouldBeEqualTo((float) _defaultThreshold);
      }
   }

   public class When_calculating_the_comparison_threshold_for_a_quantity_that_is_a_fraction : concern_for_DataFactory
   {
      private IQuantity _quantity;
      private double _defaultThreshold;
      private double _defaultThresholdToSmall;

      protected override void Context()
      {
         base.Context();
         _quantity = new Parameter().WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs());
         _defaultThreshold = 0.023;
         _defaultThresholdToSmall = 0.0000005;
      }

      [Observation]
      public void should_return_the_default_comparison_threshold_if_it_is_greater_than_the_min_threshold_for_fraction()
      {
         sut.CalculateComparisonThreshold(_quantity, _defaultThreshold).ShouldBeEqualTo((float) _defaultThreshold);
      }

      [Observation]
      public void should_return_the_min_comparison_threshold_otherwise()
      {
         sut.CalculateComparisonThreshold(_quantity, _defaultThresholdToSmall).ShouldBeEqualTo(Constants.MIN_FRACTION_RELATIVE_COMPARISON_THRESHOLD);
      }
   }
}