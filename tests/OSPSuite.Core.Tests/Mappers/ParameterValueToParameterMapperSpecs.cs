using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_ParameterValueToParameterMapper : ContextSpecification<ParameterValueToParameterMapper>
   {
      protected IParameterFactory _parameterFactory;

      protected override void Context()
      {
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new ParameterValueToParameterMapper(_parameterFactory);
      }
   }

   public class When_mapping_a_distributed_parameter_value_to_parameter_value : concern_for_ParameterValueToParameterMapper
   {
      private IndividualParameter _individualParameter;
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter
         {
            Name = "name",
            DistributionType = DistributionType.Discrete
         };
         _individualParameter.UpdateValueOriginFrom(ValueOrigin.Unknown);
      }

      protected override void Because()
      {
         _parameter = sut.MapFrom(_individualParameter);
      }

      [Observation]
      public void the_value_origin_should_be_updated()
      {
         _parameter.ValueOrigin.ShouldBeEqualTo(ValueOrigin.Unknown);
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

   public class When_mapping_an_parameter_value_with_constant_formula_to_a_parameter_value : concern_for_ParameterValueToParameterMapper
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
