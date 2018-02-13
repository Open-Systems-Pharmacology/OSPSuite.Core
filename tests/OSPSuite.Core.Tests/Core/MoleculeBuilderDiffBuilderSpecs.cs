using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public class When_comparing_two_molecule_builder_with_different_start_values : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuilder1 = new MoleculeBuilder().WithName("Drug");
         moleculeBuilder1.DefaultStartFormula = new ConstantFormula(0);

         var moleculeBuilder2 = new MoleculeBuilder().WithName("Drug");
         moleculeBuilder2.DefaultStartFormula = new ConstantFormula(1.1);


         _object1 = moleculeBuilder1;
         _object2 = moleculeBuilder2;
      }

      [Observation]
      public void should_report_a_diiference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_molecule_builder_with_different_properties : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         _comparerSettings.OnlyComputingRelevant = false;
         var moleculeBuilder1 = new MoleculeBuilder().WithName("Drug");
         moleculeBuilder1.DefaultStartFormula = new ConstantFormula(0);
         moleculeBuilder1.IsFloating = false;
         moleculeBuilder1.QuantityType = QuantityType.Drug;

         var moleculeBuilder2 = new MoleculeBuilder().WithName("Drug");
         moleculeBuilder2.DefaultStartFormula = new ConstantFormula(0);
         moleculeBuilder2.IsFloating = true;
         moleculeBuilder2.QuantityType = QuantityType.Metabolite;

         _object1 = moleculeBuilder1;
         _object2 = moleculeBuilder2;
      }

      [Observation]
      public void should_report_a_diiference()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_molecule_builder_with_different_Calculation_methods : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var moleculeBuilder1 = new MoleculeBuilder().WithName("Drug");
         moleculeBuilder1.DefaultStartFormula = new ConstantFormula(0);
         moleculeBuilder1.AddUsedCalculationMethod(new UsedCalculationMethod("Cat1","CM1"));
         moleculeBuilder1.AddUsedCalculationMethod(new UsedCalculationMethod("Cat2", "CM2"));

         var moleculeBuilder2 = new MoleculeBuilder().WithName("Drug");
         moleculeBuilder2.DefaultStartFormula = new ConstantFormula(0);
         moleculeBuilder2.AddUsedCalculationMethod(new UsedCalculationMethod("Cat1","CM1"));
         moleculeBuilder2.AddUsedCalculationMethod(new UsedCalculationMethod("Cat2", "CM3"));


         _object1 = moleculeBuilder1;
         _object2 = moleculeBuilder2;
      }

      [Observation]
      public void should_report_a_diiference()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }
}