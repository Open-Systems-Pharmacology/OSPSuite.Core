using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_two_identical_transports : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var t1 = new Transport().WithName("Diff");
         var t2 = new Transport().WithName("Diff");


         t1.Formula = new ConstantFormula(2);
         t2.Formula = new ConstantFormula(2);

         t1.SourceAmount = new MoleculeAmount().WithName("S");
         t2.SourceAmount = new MoleculeAmount().WithName("S");
         t1.TargetAmount = new MoleculeAmount().WithName("T");
         t2.TargetAmount = new MoleculeAmount().WithName("T");

         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_no_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   public class When_comparing_two_transports_with_different_formulas : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var t1 = new Transport().WithName("Diff");
         var t2 = new Transport().WithName("Diff");


         t1.Formula = new ConstantFormula(-2);
         t2.Formula = new ConstantFormula(2);
         t1.SourceAmount = new MoleculeAmount().WithName("S");
         t2.SourceAmount = new MoleculeAmount().WithName("S");
         t1.TargetAmount = new MoleculeAmount().WithName("T");
         t2.TargetAmount = new MoleculeAmount().WithName("T");
         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_one_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_transports_with_swapped_target_and_source : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();

         var t1 = new Transport().WithName("Diff");
         var t2 = new Transport().WithName("Diff");


         t1.Formula = new ConstantFormula(2);
         t2.Formula = new ConstantFormula(2);
         t1.SourceAmount = new MoleculeAmount().WithName("S");
         t2.SourceAmount = new MoleculeAmount().WithName("T");
         t1.TargetAmount = new MoleculeAmount().WithName("T");
         t2.TargetAmount = new MoleculeAmount().WithName("S");
         _object1 = t1;
         _object2 = t2;
      }

      [Observation]
      public void should_report_one_difference()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }
}