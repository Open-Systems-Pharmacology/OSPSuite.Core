using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.DiffBuilders
{
   public class When_comparing_two_similar_reactions:concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new Reaction().WithName("Reaction").WithFormula(f1);
         
         r1.AddEduct(new ReactionPartner(1,new MoleculeAmount().WithName("Drug")));
         r1.AddEduct(new ReactionPartner(1,new MoleculeAmount().WithName("Metab")));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new Reaction().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartner(1,new MoleculeAmount().WithName("Drug")));
         r2.AddEduct(new ReactionPartner(1,new MoleculeAmount().WithName("Metab")));
         r2.AddModifier("Cyp");
         var p12 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_return_no_differences()
      {
         _report.IsEmpty.ShouldBeTrue();
      }   
   }

   public class When_comparing_two_reactions_with_different_reactions : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL/conc").WithName("formula");
         var r1 = new Reaction().WithName("Reaction").WithFormula(f1);

         r1.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Drug")));
         r1.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Metab")));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new Reaction().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Drug")));
         r2.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Metab")));
         r2.AddModifier("Cyp");
         var p12 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_return_no_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_two_reactions_with_different_reactionParners : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new Reaction().WithName("Reaction").WithFormula(f1);

         r1.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Drug")));
         r1.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Metab")));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new Reaction().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("DrugA")));
         r2.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Metab")));
         r2.AddModifier("Cyp");
         var p12 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_return_no_differences()
      {
         _report.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_two_reactions_with_different_StoichiometricCoeffients : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new Reaction().WithName("Reaction").WithFormula(f1);

         r1.AddEduct(new ReactionPartner(2, new MoleculeAmount().WithName("Drug")));
         r1.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Metab")));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new Reaction().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Drug")));
         r2.AddEduct(new ReactionPartner(1, new MoleculeAmount().WithName("Metab")));
         r2.AddModifier("Cyp");
         var p12 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_return_no_differences()
      {
         _report.Count.ShouldBeEqualTo(1);
      }
   }

}