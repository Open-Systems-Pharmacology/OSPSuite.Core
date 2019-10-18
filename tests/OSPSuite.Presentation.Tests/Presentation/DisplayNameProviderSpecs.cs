using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DisplayNameProvider : ContextSpecification<IDisplayNameProvider>
   {
      protected override void Context()
      {
         sut = new DisplayNameProvider();
      }
   }

   public class When_returning_the_display_name_of_an_explicit_formula : concern_for_DisplayNameProvider
   {
      private ExplicitFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("A+B").WithName("Toto");
      }

      [Observation]
      public void should_return_the_name_of_the_formula()
      {
         sut.DisplayNameFor(_formula).ShouldBeEqualTo(_formula.Name);
      }
   }

   public class When_returning_the_display_name_of_an_object_not_null: concern_for_DisplayNameProvider
   {
      private IObjectBase _object;

      protected override void Context()
      {
         base.Context();
         _object = A.Fake<IObjectBase>();
         A.CallTo(() => _object.ToString()).Returns("XX");
      }

      [Observation]
      public void should_return_the_content_of_the_to_string_method()
      {
         sut.DisplayNameFor(_object).ShouldBeEqualTo("XX");
      }
   }
}