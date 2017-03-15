using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   internal abstract class concern_for_MoleculeStartValuesBuildingBlock : ContextSpecification<MoleculeStartValuesBuildingBlock>
   {
      protected override void Context()
      {
         sut = new MoleculeStartValuesBuildingBlock();
      }
   }

   internal class when_checking_if_a_building_block_is_referenced_by_the_start_values_building_block_without_a_reference: concern_for_MoleculeStartValuesBuildingBlock
   {
      private IBuildingBlock _buildingBlock;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();

         A.CallTo(() => _buildingBlock.Id).Returns("id");
      }

      protected override void Because()
      {
         _result = sut.Uses(_buildingBlock);
      }

      [Observation]
      public void result_should_show_that_building_block_is_used()
      {
         _result.ShouldBeFalse();
      }
   }

   internal class when_checking_if_a_molecule_building_block_is_referenced_by_the_start_values_building_block : concern_for_MoleculeStartValuesBuildingBlock
   {
      private IBuildingBlock _buildingBlock;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         sut.MoleculeBuildingBlockId = "id";
         _buildingBlock = A.Fake<IBuildingBlock>();

         A.CallTo(() => _buildingBlock.Id).Returns("id");
      }

      protected override void Because()
      {
         _result = sut.Uses(_buildingBlock);
      }

      [Observation]
      public void result_should_show_that_building_block_is_used()
      {
         _result.ShouldBeTrue();
      }
   }

   internal class when_checking_if_a_spatial_structure_is_referenced_by_the_start_values_building_block : concern_for_MoleculeStartValuesBuildingBlock
   {
      private IBuildingBlock _buildingBlock;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         sut.SpatialStructureId = "id";
         _buildingBlock = A.Fake<IBuildingBlock>();

         A.CallTo(() => _buildingBlock.Id).Returns("id");
      }

      protected override void Because()
      {
         _result = sut.Uses(_buildingBlock);
      }

      [Observation]
      public void result_should_show_that_building_block_is_used()
      {
         _result.ShouldBeTrue();
      }
   }
   
   internal class when_adding_molecule_start_values_twice : concern_for_MoleculeStartValuesBuildingBlock
   {
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("");
         sut.AddFormula(_formula);
      }

      protected override void Because()
      {
         sut.AddFormula(_formula);
      }

      [Observation]
      public void the_cache_should_only_contain_one_formula()
      {
         sut.FormulaCache.Count().ShouldBeEqualTo(1);
      }
   }

   internal class when_adding_molecule_start_values_with_matching_id_to_building_block : concern_for_MoleculeStartValuesBuildingBlock
   {
      private IFormula _formula, _addedFormula;
      protected override void Context()
      {
         base.Context();
         _formula = A.Fake<IFormula>();
         _addedFormula = A.Fake<IFormula>();

         _formula = new ExplicitFormula("");
         _addedFormula = new ExplicitFormula("");
         _formula.Id = "id";
         _addedFormula.Id = "id";

         sut.AddFormula(_formula);
      }

      [Observation]
      public void should_throw_key_exception()
      {
         The.Action(() => sut.AddFormula(_addedFormula)).ShouldThrowAn<ArgumentException>();
      }
   }

   internal class when_adding_molecule_start_values_to_building_block : concern_for_MoleculeStartValuesBuildingBlock
   {
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("M/V");
      }

      protected override void Because()
      {
         sut.Add(new MoleculeStartValue {Formula = _formula});
      }

      [Observation]
      public void should_not_add_formula_cache_to_building_block()
      {
         sut.FormulaCache.Any().ShouldBeFalse();
      }

   }
}
