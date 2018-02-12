using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class When_comparing_reactions_having_the_same_properties : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new ReactionBuilder().WithName("Reaction").WithFormula(f1);
         r1.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r1.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new ReactionBuilder().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r2.AddEduct(new ReactionPartnerBuilder("Metab", 1));
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

   public class When_comparing_reactions_having_the_same_formula_but_different_modifiers : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new ReactionBuilder().WithName("Reaction").WithFormula(f1);
         r1.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r1.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new ReactionBuilder().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r2.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r2.AddModifier("UGT");
         var p12 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_reactions_having_the_same_formula_but_different_products : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new ReactionBuilder().WithName("Reaction").WithFormula(f1);
         r1.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r1.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new ReactionBuilder().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r2.AddEduct(new ReactionPartnerBuilder("Metab2", 1));
         r2.AddModifier("Cyp");
         var p12 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_comparing_reactions_having_the_same_formula_but_different_stoichiometric_coefficients : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new ReactionBuilder().WithName("Reaction").WithFormula(f1);
         r1.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r1.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new ReactionBuilder().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartnerBuilder("Drug", 2));
         r2.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r2.AddModifier("Cyp");
         var p12 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_comparing_reactions_having_same_parameters_with_different_values : concern_for_ObjectComparer
   {
      private Unit _preferredUnit;

      protected override void Context()
      {
         base.Context();
         var displayUnitRetriever = IoC.Resolve<IDisplayUnitRetriever>();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new ReactionBuilder().WithName("Reaction").WithFormula(f1);
         r1.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r1.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r1.AddModifier("Cyp");
         var concentrationDimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         var p1Formula = new ConstantFormula(2).WithDimension(concentrationDimension);
         var p11 = new Parameter().WithName("P1").WithFormula(p1Formula).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new ReactionBuilder().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r2.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r2.AddModifier("Cyp");
         var p12Forumla = new ConstantFormula(3).WithDimension(concentrationDimension);
         var p12 = new Parameter().WithName("P1").WithFormula(p12Forumla).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;

         _preferredUnit = concentrationDimension.Units.ElementAt(0);
         A.CallTo(() => displayUnitRetriever.PreferredUnitFor(p1Formula,null)).Returns(_preferredUnit);
         A.CallTo(() => displayUnitRetriever.PreferredUnitFor(p12Forumla, null)).Returns(_preferredUnit);

      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(1);
         _report[0].DowncastTo<PropertyValueDiffItem>().FormattedValue1.ShouldBeEqualTo(new UnitFormatter().Format(2, _preferredUnit));
      }
   }

   public class When_comparing_reactions_having_different_parameters : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var f1 = new ExplicitFormula("CL*conc").WithName("formula");
         var r1 = new ReactionBuilder().WithName("Reaction").WithFormula(f1);
         r1.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r1.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r1.AddModifier("Cyp");
         var p11 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(2)).WithParentContainer(r1);

         var f2 = new ExplicitFormula("CL*conc").WithName("formula");
         var r2 = new ReactionBuilder().WithName("Reaction").WithFormula(f2);
         r2.AddEduct(new ReactionPartnerBuilder("Drug", 1));
         r2.AddEduct(new ReactionPartnerBuilder("Metab", 1));
         r2.AddModifier("Cyp");
         var p12 = new Parameter().WithName("P2").WithFormula(new ConstantFormula(2)).WithParentContainer(r2);

         _object1 = r1;
         _object2 = r2;
      }

      [Observation]
      public void should_report_the_differences_accordingly()
      {
         _report.Count().ShouldBeEqualTo(2);
      }
   }
}