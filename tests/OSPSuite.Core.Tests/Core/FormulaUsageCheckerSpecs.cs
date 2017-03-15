using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_FormulaUsageChecker : ContextSpecification<FormulaUsageChecker>
   {
      protected IFormula _formula;
      protected IMoleculeBuildingBlock _buildingBlock;
      protected IMoleculeBuilder _moleculeBuilder;

      protected override void Context()
      {
         sut = new FormulaUsageChecker();
         _formula = new ExplicitFormula("1").WithName("Test Formula");
         _buildingBlock = new MoleculeBuildingBlock();
         _moleculeBuilder = new MoleculeBuilder().WithName("Container");
         _moleculeBuilder.DefaultStartFormula = A.Fake<IFormula>();
         _buildingBlock.Add(_moleculeBuilder);
      }
   }

   class When_checking_usage_for_a_used_formula : concern_for_FormulaUsageChecker
   {
      private bool _result ;

      protected override void Context()
      {
         base.Context();
         var para = new Parameter().WithName("Para").WithFormula(_formula);
         _moleculeBuilder.AddParameter(para);
      }

      protected override void Because()
      {
         _result = sut.FormulaUsedIn(_buildingBlock,_formula);
      }

      [Observation]
      public void should_return_true()
      {
         _result.ShouldBeTrue();
      }
   }

   
   class When_checking_usage_for_a_unused_formula : concern_for_FormulaUsageChecker
   {
      private bool _result;

      protected override void Context()
      {
         base.Context();
         var para = new Parameter().WithName("Para").WithFormula(A.Fake<IFormula>().WithName("Other Formula"));
         _moleculeBuilder.AddParameter(para);
      }

      protected override void Because()
      {
         _result = sut.FormulaUsedIn(_buildingBlock, _formula);
      }

      [Observation]
      public void should_return_false()
      {
         _result.ShouldBeFalse();
      }
   }
}	