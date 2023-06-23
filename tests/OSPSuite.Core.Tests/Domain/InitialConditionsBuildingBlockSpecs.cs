using System;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_InitialConditionsBuildingBlock : ContextSpecification<InitialConditionsBuildingBlock>
   {
      protected override void Context()
      {
         sut = new InitialConditionsBuildingBlock();
      }
   }

   internal class when_adding_initial_conditions_twice : concern_for_InitialConditionsBuildingBlock
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

   internal class when_adding_initial_conditions_with_matching_id_to_building_block : concern_for_InitialConditionsBuildingBlock
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

   internal class when_adding_initial_conditions_to_building_block : concern_for_InitialConditionsBuildingBlock
   {
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("M/V");
      }

      protected override void Because()
      {
         sut.Add(new InitialCondition { Formula = _formula });
      }

      [Observation]
      public void should_not_add_formula_cache_to_building_block()
      {
         sut.FormulaCache.Any().ShouldBeFalse();
      }
   }
}