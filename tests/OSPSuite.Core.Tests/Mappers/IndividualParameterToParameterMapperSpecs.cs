using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Mappers
{
   public class concern_for_IndividualParameterToParameterMapper : ContextSpecification<IndividualParameterToParameterMapper>
   {
      protected IParameterFactory _parameterFactory;

      protected override void Context()
      {
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new IndividualParameterToParameterMapper(_parameterFactory);
      }
   }

   public class When_mapping_a_distributed_individual_parameter_to_parameter : concern_for_IndividualParameterToParameterMapper
   {
      private IndividualParameter _individualParameter;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter
         {
            Name = "name",
            DistributionType = DistributionType.Discrete,
         };
      }

      protected override void Because()
      {
         sut.MapFrom(_individualParameter);
      }

      [Observation]
      public void should_not_create_a_non_distributed_parameter()
      {
         A.CallTo(() => _parameterFactory.CreateParameter(A<string>._, A<double?>._, A<IDimension>._, A<string>._, A<Formula>._, A<Unit>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_create_distributed_formula()
      {
         A.CallTo(() => _parameterFactory.CreateDistributedParameter(A<string>._, A<DistributionType>._, A<double?>._, A<IDimension>._, A<string>._, A<Unit>._)).MustHaveHappened();
      }
   }

   public class When_mapping_an_individual_parameter_with_constant_formula_to_a_parameter : concern_for_IndividualParameterToParameterMapper
   {
      private IndividualParameter _individualParameter;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter
         {
            Name = "name",
            DistributionType = null,
            Value = 1.0
         };
      }

      protected override void Because()
      {
         sut.MapFrom(_individualParameter);
      }

      [Observation]
      public void should_create_a_non_distributed_parameter()
      {
         A.CallTo(() => _parameterFactory.CreateParameter(A<string>._, A<double?>._, A<IDimension>._, A<string>._, A<Formula>._, A<Unit>._)).MustHaveHappened();
      }

      [Observation]
      public void should_not_create_distributed_formula()
      {
         A.CallTo(() => _parameterFactory.CreateDistributedParameter(A<string>._, A<DistributionType>._, A<double?>._, A<IDimension>._, A<string>._, A<Unit>._)).MustNotHaveHappened();
      }
   }
}
