using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_parameter_extensions : StaticContextSpecification
   {
      protected IParameter _parameter;

      protected override void Context()
      {
         _parameter = A.Fake<IParameter>();
      }
   }

   
   public class When_setting_the_build_mode_for_a_parameter_builder : concern_for_parameter_extensions
   {
      private IParameter _result;

      protected override void Because()
      {
         _result = _parameter.WithMode(ParameterBuildMode.Local);
      }
      [Observation]
      public void should_set_the_given_mode_into_the_parameter_builder()
      {
         _parameter.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);
      }

      [Observation]
      public void should_return_the_parameter_builder()
      {
         _result.ShouldBeEqualTo(_parameter);
      }
   }
}	