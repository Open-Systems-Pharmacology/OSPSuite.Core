using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimpleKeywordReplacer : ContextSpecification<SimpleKeywordReplacer>
   {
      protected string _replacement;
      protected string _keyword;

      protected override void Context()
      {
         _keyword = "KEYWORD";
         _replacement = "REPLACEMENT";
         sut = new SimpleKeywordReplacer(_keyword,_replacement);
      }
   }
   
   public class When_told_to_replace_keyword_using_simple_key_replacer_in_a_Path_conntaining_keyWord : concern_for_SimpleKeywordReplacer
   {
      private IFormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         _path = new ObjectPathFactory(new AliasCreator()).CreateFormulaUsablePathFrom("Toto", "TaTa", _keyword);
      }
      protected override void Because()
      {
         sut.ReplaceIn(_path);
      }
      [Observation]
      public void should_have_removed_key_word_from_path()
      {
         _path.Contains(_keyword).ShouldBeFalse();
      }
      [Observation]
      public void should_have_added_replacment_from_in_path()
      {
         _path.Contains(_replacement).ShouldBeTrue();
      }
      [Observation]
      public void should_not_change_the_order_of_the_elements()
      {
         _path.ShouldOnlyContainInOrder("Toto", "TaTa", _replacement);
      }
   }

   
   public class When_told_to_replace_keyword_using_simple_key_replacer_in_a_path_not_conntaining_keyword : concern_for_SimpleKeywordReplacer
   {
      private IFormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         _path = new ObjectPathFactory(new AliasCreator()).CreateFormulaUsablePathFrom(  "TuTu", "ToTo", "TaTa" );
      }
      protected override void Because()
      {
         sut.ReplaceIn(_path);
      }
      [Observation]
      public void should_not_have_changed_the_path()
      {
         _path.ShouldOnlyContainInOrder("TuTu", "ToTo", "TaTa");
      }
   }
}	