using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core.DiffBuilders
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

   public class When_comparing_two_table_formula_with_different_number_of_points : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var parameter1 = new Parameter().WithName("P");
         var formula1 = new TableFormula();
         formula1.AddPoint(10, 1);
         formula1.AddPoint(20, 2); 
         formula1.AddPoint(30, 3); 
         parameter1.Formula = formula1;

         var parameter2 = new Parameter().WithName("P");
         var formula2 = new TableFormula();
         formula2.AddPoint(10, 1); 
         formula2.AddPoint(20, 2); 
         parameter2.Formula = formula2;

         _object1 = parameter1;
         _object2 = parameter2;
      }

      [Observation]
      public void should_report_a_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }


   public class When_comparing_two_table_formula_with_different_x_dimensions : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var formula1 = new TableFormula {XDimension = DomainHelperForSpecs.TimeDimensionForSpecs()};
         formula1.AddPoint(10, 1);

         var formula2 = new TableFormula {XDimension = Constants.Dimension.NO_DIMENSION};
         formula2.AddPoint(10, 1);

         _object1 = formula1;
         _object2 = formula2;
      }

      [Observation]
      public void should_report_the_different_x_dimension()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_table_formula_with_offset_referencing_different_offsets : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var formula1 = new TableFormulaWithOffset();
         formula1.AddTableObjectPath(new FormulaUsablePath("Organism", "Table") {Alias = "Table"});
         formula1.AddOffsetObjectPath(new FormulaUsablePath("Organism", "Offset") {Alias = "Offset"});

         var formula2 = new TableFormulaWithOffset();
         formula2.AddTableObjectPath(new FormulaUsablePath("Organism", "Table") {Alias = "Table"});
         formula2.AddOffsetObjectPath(new FormulaUsablePath("Organism", "OtherOffset") {Alias = "Offset"});

         _object1 = formula1;
         _object2 = formula2;
      }

      [Observation]
      public void should_report_the_different_offset_path()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_table_formula_with_x_argument_using_different_aliases : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var formula1 = new TableFormulaWithXArgument();
         formula1.AddTableObjectPath(new FormulaUsablePath("Organism", "Table") {Alias = "Table"});
         formula1.AddXArgumentObjectPath(new FormulaUsablePath("Organism", "X") {Alias = "X"});

         var formula2 = new TableFormulaWithXArgument();
         formula2.AddTableObjectPath(new FormulaUsablePath("Organism", "Table") {Alias = "Table"});
         formula2.AddXArgumentObjectPath(new FormulaUsablePath("Organism", "X") {Alias = "Y"});

         _object1 = formula1;
         _object2 = formula2;
      }

      [Observation]
      public void should_report_the_different_alias_and_the_path_using_it()
      {
         //x argument alias, path with alias X only in the first formula, path with alias Y only in the second formula
         _report.Count.ShouldBeEqualTo(3);
      }
   }
}
