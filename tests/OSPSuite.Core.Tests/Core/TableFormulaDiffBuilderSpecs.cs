using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public class When_comparing_two_table_formula_with_different_properties : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var parameter1 = new Parameter().WithName("P");
         var formula1 = new TableFormula();
         formula1.UseDerivedValues = true; //#1 default is false
         formula1.AddPoint(10, 1);
         formula1.AddPoint(20, 2);//#2 point does not exist in formula2
         formula1.AddPoint(50, 5);
         parameter1.Formula = formula1;

         var parameter2 = new Parameter().WithName("P");
         var formula2 = new TableFormula();
         formula2.UseDerivedValues = false;
         formula2.AddPoint(10, 2);//#3 not same Y value
         formula2.AddPoint(30, 3);//#4 point does not exist in formula1
         formula2.AddPoint(new ValuePoint(50,5) {RestartSolver = true});//#5 default flag is false
         parameter2.Formula = formula2;

         _object1 = parameter1;
         _object2 = parameter2;
      }

      [Observation]
      public void should_report_the_path_entries_and_different_entries_as_difference()
      {
         _report.Count.ShouldBeEqualTo(5);
      }
   }
}