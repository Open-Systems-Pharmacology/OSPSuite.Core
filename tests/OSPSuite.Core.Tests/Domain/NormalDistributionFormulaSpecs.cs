using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_NormalDistributionFormula : ContextSpecification<NormalDistributionFormula>
   {
      protected DistributedParameter _distributedParam;

      protected override void Context()
      {
         var dim = A.Fake<IDimension>();
         var meanParam = new Parameter().WithName("Mean_xy").WithDimension(dim).WithValue(2.2);
         var deviationParam = new Parameter().WithName("Std_xy").WithDimension(dim).WithValue(3.5);
         _distributedParam = new DistributedParameter().WithName("P1").WithDimension(dim);
         _distributedParam.Add(meanParam);
         _distributedParam.Add(deviationParam);
         var noDimension = A.Fake<IDimension>();
         var dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => dimensionFactory.NoDimension).Returns(noDimension);

         sut = new DistributionFormulaFactory(new ObjectPathFactory(new AliasCreator()), new ObjectBaseFactory(A.Fake<Utility.Container.IContainer>(), dimensionFactory, new IdGenerator(), A.Fake<ICreationMetaDataFactory>())).CreateNormalDistributionFormulaFor(_distributedParam, meanParam, deviationParam);

      }
   }

   
   public class When_calculating_a_random_deviate_that_should_be_in_a_valid_min_and_max_interval : concern_for_NormalDistributionFormula
   {
      private double _result;

      protected override void Because()
      {
         _result = sut.RandomDeviate(new RandomGenerator(), _distributedParam, 1, 4);
      }
      [Observation]
      public void should_return_a_value_between_min_and_max()
      {
         _result.ShouldBeGreaterThan(1);
         _result.ShouldBeSmallerThan(4);
      }
   }

   
   public class When_calculating_a_random_deviate_that_should_be_in_an_interval_that_cannot_be_reach_by_the_distribution : concern_for_NormalDistributionFormula
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.RandomDeviate(new RandomGenerator(), _distributedParam, 15, 20)).ShouldThrowAn<DistributionException>();
      
      }
   }
}	