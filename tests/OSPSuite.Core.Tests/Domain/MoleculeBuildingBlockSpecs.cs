using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MoleculeBuildingBlock : ContextSpecification<MoleculeBuildingBlock>
   {
      protected override void Context()
      {
         sut = new MoleculeBuildingBlock();
      }
   }

   public class When_accessed_through_index : concern_for_MoleculeBuildingBlock
   {
      private MoleculeBuilder _result;
      private MoleculeBuilder _drug;

      protected override void Context()
      {
         base.Context();
         sut.Add(A.Fake<MoleculeBuilder>().WithName("Protein"));
         _drug = A.Fake<MoleculeBuilder>().WithName("Drug");
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
      private InitialConditionsBuildingBlock _initialConditions;
      private IEnumerable<MoleculeBuilder> _results;
      private MoleculeBuilder _molecule;
      private MoleculeBuilder _drug;

      protected override void Context()
      {
         base.Context();

         _initialConditions = new InitialConditionsBuildingBlock
         {
            new InitialCondition {Name = "drug", IsPresent = true},
            new InitialCondition {Name = "molecule", IsPresent = true},
            new InitialCondition{Name = "moleculeThatDoesNotExist", IsPresent = true },
            new InitialCondition{Name = "moleculeThatDoesExistButNotPresent", IsPresent = false }
         };

         _drug = new MoleculeBuilder().WithName("drug");
         sut.Add(_drug);
         _molecule = new MoleculeBuilder().WithName("molecule");
         sut.Add(_molecule);
         sut.Add(new MoleculeBuilder().WithName("moleculeThatDoesExistButNotPresent"));
      }
      protected override void Because()
      {
         _results=  sut.AllPresentFor(_initialConditions);
      }

      [Observation]
      public void should_only_return_the_available_molecules_that_are_defined_as_present_in_the_initial_condition_building_block()
      {
         _results.ShouldOnlyContain(_drug,_molecule);
      }
   }

   public abstract class concern_for_MoleculeBuildingBlock_filters : concern_for_MoleculeBuildingBlock
   {
      protected MoleculeBuilder _xenobioticFloatingDrug;
      protected MoleculeBuilder _xenobioticStationaryDrug;
      protected MoleculeBuilder _endogenousFloatingEnzyme;
      protected MoleculeBuilder _endogenousStationaryEnzyme;
      protected MoleculeBuilder _endogenousStationaryTransporter;
      protected MoleculeBuilder _endogenousStationaryOtherProtein;

      protected override void Context()
      {
         base.Context();
         _xenobioticFloatingDrug = addMolecule("XenoFloatingDrug", isFloating: true, QuantityType.Drug, isXenobiotic: true);
         _xenobioticStationaryDrug = addMolecule("XenoStationaryDrug", isFloating: false, QuantityType.Drug, isXenobiotic: true);
         _endogenousFloatingEnzyme = addMolecule("EndoFloatingEnzyme", isFloating: true, QuantityType.Enzyme, isXenobiotic: false);
         _endogenousStationaryEnzyme = addMolecule("EndoStationaryEnzyme", isFloating: false, QuantityType.Enzyme, isXenobiotic: false);
         _endogenousStationaryTransporter = addMolecule("EndoStationaryTransporter", isFloating: false, QuantityType.Transporter, isXenobiotic: false);
         _endogenousStationaryOtherProtein = addMolecule("EndoStationaryOtherProtein", isFloating: false, QuantityType.OtherProtein, isXenobiotic: false);
      }

      private MoleculeBuilder addMolecule(string name, bool isFloating, QuantityType quantityType, bool isXenobiotic)
      {
         var molecule = new MoleculeBuilder
         {
            Name = name,
            IsFloating = isFloating,
            QuantityType = quantityType,
            IsXenobiotic = isXenobiotic
         };
         sut.Add(molecule);
         return molecule;
      }
   }

   public class When_retrieving_all_floating_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllFloating();
      }

      [Observation]
      public void should_return_only_floating_molecules()
      {
         _results.ShouldOnlyContain(_xenobioticFloatingDrug, _endogenousFloatingEnzyme);
      }
   }

   public class When_retrieving_all_stationary_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllStationary();
      }

      [Observation]
      public void should_return_only_stationary_molecules()
      {
         _results.ShouldOnlyContain(_xenobioticStationaryDrug, _endogenousStationaryEnzyme, _endogenousStationaryTransporter, _endogenousStationaryOtherProtein);
      }
   }

   public class When_retrieving_all_xenobiotic_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllXenobiotic();
      }

      [Observation]
      public void should_return_only_xenobiotic_molecules()
      {
         _results.ShouldOnlyContain(_xenobioticFloatingDrug, _xenobioticStationaryDrug);
      }
   }

   public class When_retrieving_all_endogenous_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllEndogenous();
      }

      [Observation]
      public void should_return_only_endogenous_molecules()
      {
         _results.ShouldOnlyContain(_endogenousFloatingEnzyme, _endogenousStationaryEnzyme, _endogenousStationaryTransporter, _endogenousStationaryOtherProtein);
      }
   }

   public class When_retrieving_all_molecules_of_a_specific_quantity_type : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllOfType(QuantityType.Drug);
      }

      [Observation]
      public void should_return_only_molecules_with_that_type()
      {
         _results.ShouldOnlyContain(_xenobioticFloatingDrug, _xenobioticStationaryDrug);
      }
   }

   public class When_retrieving_all_molecules_of_a_composite_quantity_type : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllOfType(QuantityType.Protein);
      }

      [Observation]
      public void should_return_molecules_whose_type_is_a_subset_of_the_composite_type()
      {
         _results.ShouldOnlyContain(_endogenousFloatingEnzyme, _endogenousStationaryEnzyme, _endogenousStationaryTransporter, _endogenousStationaryOtherProtein);
      }
   }

   public class When_retrieving_all_xenobiotic_floating_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllXenobioticFloating();
      }

      [Observation]
      public void should_return_only_xenobiotic_floating_molecules()
      {
         _results.ShouldOnlyContain(_xenobioticFloatingDrug);
      }
   }

   public class When_retrieving_all_xenobiotic_stationary_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllXenobioticStationary();
      }

      [Observation]
      public void should_return_only_xenobiotic_stationary_molecules()
      {
         _results.ShouldOnlyContain(_xenobioticStationaryDrug);
      }
   }

   public class When_retrieving_all_endogenous_floating_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllEndogenousFloating();
      }

      [Observation]
      public void should_return_only_endogenous_floating_molecules()
      {
         _results.ShouldOnlyContain(_endogenousFloatingEnzyme);
      }
   }

   public class When_retrieving_all_endogenous_stationary_molecules : concern_for_MoleculeBuildingBlock_filters
   {
      private IEnumerable<MoleculeBuilder> _results;

      protected override void Because()
      {
         _results = sut.AllEndogenousStationary();
      }

      [Observation]
      public void should_return_only_endogenous_stationary_molecules()
      {
         _results.ShouldOnlyContain(_endogenousStationaryEnzyme, _endogenousStationaryTransporter, _endogenousStationaryOtherProtein);
      }
   }
}