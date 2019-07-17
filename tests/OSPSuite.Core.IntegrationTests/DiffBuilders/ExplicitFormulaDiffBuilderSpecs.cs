using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_two_explicit_formula_with_different_path : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuilder1 = new MoleculeBuilder().WithName("Drug");
         var formula1 = new ExplicitFormula("A+B");
         formula1.AddObjectPath(new FormulaUsablePath("A", "B").WithAlias("Same"));
         formula1.AddObjectPath(new FormulaUsablePath("A").WithAlias("DifferentPath"));
         formula1.AddObjectPath(new FormulaUsablePath("A").WithAlias("OnlyIn1"));
         moleculeBuilder1.DefaultStartFormula = formula1;

         var moleculeBuilder2 = new MoleculeBuilder().WithName("Drug");
         var formula2 = new ExplicitFormula("A+B");
         formula2.AddObjectPath(new FormulaUsablePath("A", "B").WithAlias("Same"));
         formula2.AddObjectPath(new FormulaUsablePath("B").WithAlias("DifferentPath"));
         formula2.AddObjectPath(new FormulaUsablePath("A").WithAlias("OnlyIn2"));
         moleculeBuilder2.DefaultStartFormula = formula2;

         _object1 = moleculeBuilder1;
         _object2 = moleculeBuilder2;
      }

      [Observation]
      public void should_report_the_path_entries_and_different_entries_as_difference()
      {
         _report.Count.ShouldBeEqualTo(3);
      }
   }


   public class When_comparing_two_explicit_formula_with_path_only_differing_by_the_first_entry_and_the_references_are_resolved : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var sim1  = new Container().WithName("Sim1");
         var c1 = new Container().WithName("A").WithParentContainer(sim1);
         var p1 = new Parameter().WithName("B").WithParentContainer(c1);
         var param1 = new Parameter().WithName("C").WithParentContainer(c1);

         var sim2 = new Container().WithName("Sim2");
         var c2 = new Container().WithName("A").WithParentContainer(sim2);
         var p2 = new Parameter().WithName("B").WithParentContainer(c2);
         var param2 = new Parameter().WithName("C").WithParentContainer(c2);

         var formula1 = new ExplicitFormula("A+B");
         formula1.AddObjectPath(new FormulaUsablePath("Sim1", "A", "B").WithAlias("Same"));
         param1.Formula = formula1;
         formula1.ResolveObjectPathsFor(param1);

         var formula2 = new ExplicitFormula("A+B");
         formula2.AddObjectPath(new FormulaUsablePath("Sim2","A", "B").WithAlias("Same"));
         param2.Formula = formula2;
         formula2.ResolveObjectPathsFor(param2);

         _object1 = param1;
         _object2 = param2;
      }

      [Observation]
      public void should_not_report_the_path_entries_difference()
      {
         _report.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_comparing_two_explicit_formula_with_path_only_differing_by_the_first_entry_and_the_references_are_not_resolved : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuilder1 = new MoleculeBuilder().WithName("Drug");
         var formula1 = new ExplicitFormula("A+B");
         formula1.AddObjectPath(new FormulaUsablePath("Sim1", "A", "B", "C").WithAlias("Same"));
         moleculeBuilder1.DefaultStartFormula = formula1;

         var moleculeBuilder2 = new MoleculeBuilder().WithName("Drug");
         var formula2 = new ExplicitFormula("A+B");
         formula2.AddObjectPath(new FormulaUsablePath("Sim2", "A", "B", "C").WithAlias("Same"));
         moleculeBuilder2.DefaultStartFormula = formula2;

         _object1 = moleculeBuilder1;
         _object2 = moleculeBuilder2;
      }

      [Observation]
      public void should_report_a_path_entries_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}