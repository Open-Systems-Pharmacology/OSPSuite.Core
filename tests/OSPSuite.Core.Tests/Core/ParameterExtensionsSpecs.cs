using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterExtensions : StaticContextSpecification
   {
      protected override void Context()
      {
         
      }
   }
   
   public class When_setting_the_RHSFormula_of_a_parameter_with_the_withRHS_extension : concern_for_ParameterExtensions
   {
      private IParameter _parameter;
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
         _formula = A.Fake<IFormula>();
      }
      protected override void Because()
      {
         _parameter.WithRHS(_formula);
      }
      [Observation]
      public void should_have_set_the_rhsFormula_of_the_paramerter()
      {
         _parameter.RHSFormula.ShouldBeEqualTo(_formula);
      }
   }
}	