using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public class When_comparing_two_dynamic_formulas : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuilder1 = new MoleculeBuilder().WithName("Drug");
         var formula1 = new SumFormula();
         formula1.Criteria = Create.Criteria(x => x.With("A").And.Not("B"));
         formula1.Variable = "X";
         moleculeBuilder1.DefaultStartFormula = formula1;

         var moleculeBuilder2 = new MoleculeBuilder().WithName("Drug");
         var formula2 = new SumFormula();
         formula2.Criteria = Create.Criteria(x => x.With("A").And.With("B"));
         formula2.Variable = "Y";
         moleculeBuilder2.DefaultStartFormula = formula2;

         _object1 = moleculeBuilder1;
         _object2 = moleculeBuilder2;
      }

      [Observation]
      public void should_report_the_criteria_and_variable_difference()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }
}