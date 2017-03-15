using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_MoleculeBuildingBlockValidator : ContextSpecification<IMoleculeBuildingBlockValidator>
   {
      protected IMoleculeBuildingBlock _moleculeBuildingBlock;
      protected IMoleculeBuilder _molecule1;
      protected IMoleculeBuilder _molecule2;
      protected IParameter _validParameter;
      protected IParameter _invalidParameter;
      protected IParameter _invalidParameterWithFormula;
      private IMoleculeBuilder _molecule3;

      protected override void Context()
      {
         _moleculeBuildingBlock = new MoleculeBuildingBlock();
         _molecule1 = new MoleculeBuilder {Name = "M1",IsFloating = true};
         _molecule2 = new MoleculeBuilder { Name = "M2", IsFloating = true };
         _molecule3 = new MoleculeBuilder { Name = "M3", IsFloating = false };
         _moleculeBuildingBlock.Add(_molecule1);
         _moleculeBuildingBlock.Add(_molecule2);
         _moleculeBuildingBlock.Add(_molecule3);
         _validParameter = new Parameter().WithFormula(new ConstantFormula(5)).WithName("_validParameter");
         _invalidParameter = new Parameter().WithFormula(new ConstantFormula(double.NaN)).WithName("_invalidParameter");
         _invalidParameterWithFormula = new Parameter().WithFormula(new ExplicitFormula("0/0")).WithName("_invalidParameterWithFormula");
         _molecule3.AddParameter(_invalidParameter);
         sut = new MoleculeBuildingBlockValidator();
      }
   }

   
   public class When_validating_a_molecule_building_block_containing_only_valid_floating_molecules : concern_for_MoleculeBuildingBlockValidator
   {
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _molecule1.AddParameter(_validParameter);
         _molecule1.AddParameter(_invalidParameterWithFormula);
         _molecule2.AddParameter(_validParameter);
      }
      protected override void Because()
      {
         _result = sut.Validate(_moleculeBuildingBlock);
      }

      [Observation]
      public void should_return_an_empty_validation()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   
   public class When_validating_a_molecule_building_block_containing_only_invalid_floating_molecules : concern_for_MoleculeBuildingBlockValidator
   {
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _molecule1.AddParameter(_validParameter);
         _molecule1.AddParameter(_invalidParameterWithFormula);
         _molecule2.AddParameter(_validParameter);
         _molecule2.AddParameter(_invalidParameter);
      }
      protected override void Because()
      {
         _result = sut.Validate(_moleculeBuildingBlock);
      }

      [Observation]
      public void should_return_an_empty_validation()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }
}
