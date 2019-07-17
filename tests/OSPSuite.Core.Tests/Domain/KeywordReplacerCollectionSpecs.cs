using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_KeywordReplacerCollection : ContextSpecification<IKeywordReplacerCollection>
   {
      protected override void Context()
      {
         sut = new KeywordReplacerCollection();
      }
   }

   
   public class When_replacing_the_keywords_in_an_entity : concern_for_KeywordReplacerCollection
   {
      private IParameter _parameter;
      private IKeywordInObjectPathReplacer _replacement;
      private IKeywordInTagsReplacer _replacementTag;
      private IFormulaUsablePath _objectPath1;
      private IFormulaUsablePath _objectPath2;

      protected override void Context()
      {
         base.Context();
         _objectPath1 = new FormulaUsablePath();
         _objectPath2 = new FormulaUsablePath();
         _parameter = new Parameter();
         _parameter.Formula = new ExplicitFormula();
         _parameter.Formula.AddObjectPath(_objectPath1);
         _parameter.RHSFormula = new ExplicitFormula();
         _parameter.RHSFormula.AddObjectPath(_objectPath2);
         _replacement = A.Fake<IKeywordInObjectPathReplacer>();
         _replacementTag = A.Fake<IKeywordInTagsReplacer>();
         sut.AddReplacement(_replacement);
         sut.AddReplacement(_replacementTag);
      }

      protected override void Because()
      {
         sut.ReplaceIn(_parameter);
      }
      [Observation]
      public void should_replace_the_keywords_in_its_underlying_formula()
      {
        A.CallTo(() =>  _replacement.ReplaceIn(_objectPath1)).MustHaveHappened();
      }

      [Observation]
      public void should_replace_the_keywords_in_its_underlying_rhs_for_a_parameter()
      {
         A.CallTo(() => _replacement.ReplaceIn(_objectPath2)).MustHaveHappened();
      }

      [Observation]
      public void should_replace_the_keywords_in_its_tags()
      {
         A.CallTo(() => _replacementTag.ReplaceIn(_parameter.Tags)).MustHaveHappened();
      }
   }
}