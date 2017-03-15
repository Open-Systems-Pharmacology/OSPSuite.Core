using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core
{
   public abstract class concern_for_MoleculeList : ContextSpecification<MoleculeList>
   {
      protected override void Context()
      {
         sut = new MoleculeList();
      }
   }

   public class When_replacing_a_molecule_name_with_another_molecule_name : concern_for_MoleculeList
   {
      protected override void Context()
      {
         base.Context();
         sut.AddMoleculeName("A_OLD");
         sut.AddMoleculeNameToExclude("B_OLD");

      }
      protected override void Because()
      {
         sut.ReplaceMoleculeName("A_OLD", "A");
         sut.ReplaceMoleculeName("B_OLD","B");
      }

      [Observation]
      public void should_replace_the_name_in_the_include_list()
      {
         sut.MoleculeNames.ShouldContain("A");  
         sut.MoleculeNames.Contains("A_OLD").ShouldBeFalse();  
      }

      [Observation]
      public void should_replace_the_name_in_the_exclude_list()
      {
         sut.MoleculeNamesToExclude.ShouldContain("B");
         sut.MoleculeNamesToExclude.Contains("B_OLD").ShouldBeFalse();  
      }
   }

   public class When_replacing_a_molecule_template_name_with_some_molecule_names : concern_for_MoleculeList
   {
      protected override void Context()
      {
         base.Context();
         sut.AddMoleculeName("A_OLD");
         sut.AddMoleculeNameToExclude("B_OLD");

      }
      protected override void Because()
      {
         sut.ReplaceMoleculeName("A_OLD", new[]{"A1","A2"});
         sut.ReplaceMoleculeName("B_OLD", new[] { "B1", "B2" });
      }

      [Observation]
      public void should_replace_the_name_in_the_include_list()
      {
         sut.MoleculeNames.ShouldContain("A1");
         sut.MoleculeNames.ShouldContain("A2");
         sut.MoleculeNames.Contains("A_OLD").ShouldBeFalse();
      }

      [Observation]
      public void should_replace_the_name_in_the_exclude_list()
      {
         sut.MoleculeNamesToExclude.ShouldContain("B1");
         sut.MoleculeNamesToExclude.ShouldContain("B2");
         sut.MoleculeNamesToExclude.Contains("B_OLD").ShouldBeFalse();
      }
   }
}	