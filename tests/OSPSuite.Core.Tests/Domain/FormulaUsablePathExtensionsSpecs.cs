using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_object_path_extensions : StaticContextSpecification
   {
      protected FormulaUsablePath _formulaUsablePath;

      protected override void Context()
      {
         _formulaUsablePath = A.Fake<FormulaUsablePath>();
      }
   }

   
   public class When_setting_the_alias_of_an_object_path_with_the_extension : concern_for_object_path_extensions
   {
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _alias = "tralal";
      }
      protected override void Because()
      {
         _formulaUsablePath.WithAlias(_alias);
      }
      [Observation]
      public void should_have_set_the_alias_of_the_formula_useable_path()
      {
         _formulaUsablePath.Alias.ShouldBeEqualTo(_alias);
      }
   }
}	