using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_process_builder_extensions : StaticContextSpecification
   {
      protected IProcessBuilder _processBuilder;

      protected override void Context()
      {
         _processBuilder = A.Fake<IProcessBuilder>();
      }
   }

   
   public class When_setting_the_kinetic_of_a_process_builder_with_the_extensions : concern_for_process_builder_extensions
   {
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = A.Fake<IFormula>();
      }
      protected override void Because()
      {
         _processBuilder.WithKinetic(_formula);
      }
      [Observation]
      public void should_have_set_the_formula_of_the_object()
      {
         _processBuilder.Formula.ShouldBeEqualTo(_formula);
      }
   }
}	