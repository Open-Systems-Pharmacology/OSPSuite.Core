using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_AliasCreator : ContextSpecification<IAliasCreator>
   {
      protected override void Context()
      {
         sut = new AliasCreator(
            new[] {'+', '-', '*', '\\', '/', '^', '.', ',', '<', '>', '=', '(', ')', '[', ']', '{', '}', '\'', '\"', '|', '&', ';', '¬', ' ', '\t', '\n', '\r'},
            '_');
      }
   }

   internal class When_told_to_create_alias_from_Name_containing_illegal_characters : concern_for_AliasCreator
   {
      private string _name;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _name = "A(abc [1/2] * 3)";
      }

      protected override void Because()
      {
         _alias = sut.CreateAliasFrom(_name);
      }

      [Observation]
      public void should_return_a_valid_alias()
      {
         var formulaParser = new ExplicitFormulaParser(new[] {_alias}, new string[0]) {FormulaString = _alias};
         formulaParser.Parse();
      }
   }

   internal class When_told_to_create_alias_from_Name_containing_illegal_character_Combinations : concern_for_AliasCreator
   {
      private string _name;
      private string _alias;

      protected override void Context()
      {
         base.Context();
         _name = "2e2";
      }

      protected override void Because()
      {
         _alias = sut.CreateAliasFrom(_name);
      }

      [Observation]
      public void should_return_a_valid_alias()
      {
         var formulaParser = new ExplicitFormulaParser(new[] {_alias}, new string[0]);
         formulaParser.FormulaString = _alias;
         formulaParser.Parse();
      }
   }

   public class When_creating_an_alias_that_should_not_be_in_the_list_of_used_alias : concern_for_AliasCreator
   {
      private string _alias;

      protected override void Because()
      {
         _alias = sut.CreateAliasFrom("A", new[] {"A", "A1"});
      }

      [Observation]
      public void should_add_the_counter_at_then_end_of_the_alias()
      {
         _alias.ShouldBeEqualTo("A2");
      }
   }
}