using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_KeywordWithPathReplacer : ContextSpecification<KeywordWithPathReplacer>
   {
      protected IList<string> _replacement;
      protected string _keyword;

      protected override void Context()
      {
         _keyword = "KEYWORD";
         _replacement = new List<string> {"re","place","ment"};
         sut = new KeywordWithPathReplacer(_keyword,_replacement);
      }
   }
   
   public class When_told_to_replace_keyword_with_a_path_in_a_path_conntaining_keyword : concern_for_KeywordWithPathReplacer
   {
      private IFormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         _path =new ObjectPathFactory(new AliasCreator()).CreateFormulaUsablePathFrom(_keyword, "ToTo", "TaTa");
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
         _path.Contains(_replacement[0]).ShouldBeTrue();
         _path.Contains(_replacement[1]).ShouldBeTrue();
         _path.Contains(_replacement[2]).ShouldBeTrue();
      }
      [Observation]
      public void should_not_change_the_order_of_the_elements()
      {
         _path.ShouldOnlyContainInOrder(_replacement[0], _replacement[1], _replacement[2], "ToTo", "TaTa");
      }
   }

   
   public class When_told_to_replace_keyword_with_a_path_in_a_path_not_conntaining_keyword : concern_for_KeywordWithPathReplacer 
   {
      private IFormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         _path = new ObjectPathFactory(new AliasCreator()).CreateFormulaUsablePathFrom("TuTu", "ToTo", "TaTa" );
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