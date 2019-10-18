using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MoleculeBuildingBlock : ContextSpecification<IMoleculeBuildingBlock>
   {
      protected override void Context()
      {
         sut = new MoleculeBuildingBlock();
      }
   }

   public class When_accesed_through_index : concern_for_MoleculeBuildingBlock
   {
      private IMoleculeBuilder _result;
      private IMoleculeBuilder _drug;

      protected override void Context()
      {
         base.Context();
         sut.Add(A.Fake<IMoleculeBuilder>().WithName("Protein"));
         _drug = A.Fake<IMoleculeBuilder>().WithName("Drug");
         sut.Add(_drug);
      }

      protected override void Because()
      {
         _result = sut["Drug"];
      }

      [Observation]
      public void should_return_the_right_object()
      {
         _result.ShouldBeEqualTo(_drug);
      }
   }

   public class When_retrieving_the_present_molecules_based_on_the_given_molecule_values : concern_for_MoleculeBuildingBlock
   {
      private IMoleculeStartValuesBuildingBlock _moleculeStartValues;
      private IEnumerable<IMoleculeBuilder> _results;
      private MoleculeBuilder _molecule;
      private MoleculeBuilder _drug;

      protected override void Context()
      {
         base.Context();

         _moleculeStartValues = new MoleculeStartValuesBuildingBlock();
         _moleculeStartValues.Add(new MoleculeStartValue{Name = "drug", IsPresent = true});
         _moleculeStartValues.Add(new MoleculeStartValue{Name = "molecule", IsPresent = true });
         _moleculeStartValues.Add(new MoleculeStartValue{Name = "moleculeThatDoesNotExist", IsPresent = true });
         _moleculeStartValues.Add(new MoleculeStartValue{Name = "moleculeThatDoesExistButNotPresent", IsPresent = false });

         _drug = new MoleculeBuilder().WithName("drug");
         sut.Add(_drug);
         _molecule = new MoleculeBuilder().WithName("molecule");
         sut.Add(_molecule);
         sut.Add(new MoleculeBuilder().WithName("moleculeThatDoesExistButNotPresent"));
      }
      protected override void Because()
      {
         _results=  sut.AllPresentFor(_moleculeStartValues);
      }

      [Observation]
      public void should_only_return_the_available_molecules_that_are_defined_as_present_in_the_molecule_start_value_building_block()
      {
         _results.ShouldOnlyContain(_drug,_molecule);
      }
   }
}