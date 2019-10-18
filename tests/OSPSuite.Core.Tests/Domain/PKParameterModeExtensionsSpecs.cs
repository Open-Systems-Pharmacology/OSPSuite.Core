using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Domain
{
   internal class When_testing_equal_pk_parameter_modes : StaticContextSpecification
   {
      [Observation]
      public void should_return_true()
      {
         PKParameterMode.Always.Is(PKParameterMode.Always).ShouldBeTrue();
      }
   }

   internal class When_testing_unequal_pk_parameter_modes : StaticContextSpecification
   {
      [Observation]
      public void should_return_false()
      {
         PKParameterMode.Single.Is(PKParameterMode.Multi).ShouldBeFalse();
      }
   }

   internal class When_testing_type_containing_tested_mode : StaticContextSpecification
   {
      [Observation]
      public void should_return_true()
      {
         (PKParameterMode.Single | PKParameterMode.Multi).Is(PKParameterMode.Always).ShouldBeTrue();
         (PKParameterMode.Always).Is(PKParameterMode.Always | PKParameterMode.Multi).ShouldBeTrue();
         (PKParameterMode.Single).Is(PKParameterMode.Always).ShouldBeTrue();
      }
   }
}	