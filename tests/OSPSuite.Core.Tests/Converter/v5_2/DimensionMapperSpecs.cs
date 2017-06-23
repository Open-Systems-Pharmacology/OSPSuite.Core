using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Converter.v5_2;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Converter.v5_2
{
   public abstract class concern_for_DimensionMapper : ContextSpecification<IDimensionMapper>
   {
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _dimensionFactory= A.Fake<IDimensionFactory>();
         A.CallTo(() => _dimensionFactory.Dimension(A<string>._)).Returns(A.Fake<IDimension>());
         sut = new DimensionMapper(_dimensionFactory);
      }
   }

   public class When_retrieving_the_conversion_factor_for_a_dimension_that_did_not_change : concern_for_DimensionMapper
   {
      [Observation]
      public void should_return_1()
      {
         sut.ConversionFactor(Constants.Dimension.TIME).ShouldBeEqualTo(1);
      }
   }

   public class When_retrieving_the_conversion_factor_for_a_dimension_that_was_changed_but_not_renamed : concern_for_DimensionMapper
   {
      [Observation]
      public void should_return_the_expected_factor()
      {
         sut.ConversionFactor("MassAmount").ShouldBeEqualTo(1e-6);
      }
   }

   public class When_checking_if_a_dimension_required_conversion : concern_for_DimensionMapper
   {
      [Observation]
      public void should_return_true_for_a_dimension_with_a_factor_unequal_1()
      {
         sut.NeededConversion("MassAmount").ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_dimension_with_conversion_in_the_dummy_list()
      {
         sut.NeededConversion("Dose per body weight").ShouldBeTrue();
      }


      [Observation]
      public void should_return_true_for_a_dimension_with_a_factor_1()
      {
         sut.NeededConversion("Time").ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_for_a_dimension_without_conversion_in_the_dummy_list()
      {
         sut.NeededConversion("Amount per time").ShouldBeFalse();
      }
   }

   public class When_retrieving_the_conversion_factor_for_a_dimension_that_was_changed_and_renamed : concern_for_DimensionMapper
   {
      [Observation]
      public void should_return_the_expected_factor_for_the_old_name_and_the_new_name()
      {
         sut.ConversionFactor("AbundancePerTissue").ShouldBeEqualTo(1E-3);
         sut.ConversionFactor("Abundance per tissue").ShouldBeEqualTo(1E-3);
      }
   }

   public class When_retrieving_the_conversion_factor_for_a_rhs_dimension_that_was_changed_and_renamed : concern_for_DimensionMapper
   {
      [Observation]
      public void should_return_the_expected_factor_for_the_old_name_and_the_new_name()
      {
         sut.ConversionFactor(string.Format("{0}{1}","AbundancePerTissue",Constants.Dimension.RHS_DIMENSION_SUFFIX)).ShouldBeEqualTo(1E-3);
         sut.ConversionFactor(string.Format("{0}{1}", "Abundance per tissue", Constants.Dimension.RHS_DIMENSION_SUFFIX)).ShouldBeEqualTo(1E-3);
      }
   }
}