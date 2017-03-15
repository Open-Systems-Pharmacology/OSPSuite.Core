using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core
{
   public abstract class concern_for_ObjectReferencingRetriever : ContextSpecification<IObjectReferencingRetriever>
   {
      protected override void Context()
      {
         sut = new ObjectReferencingRetriever();
      }
   }

   public class When_retrieving_the_using_formula_defined_in_a_simulation_referencing_a_given_object : concern_for_ObjectReferencingRetriever
   {
      private Model _model;
      private IParameter _parameter1;
      private IParameter _parameter2;
      private IParameter _parameter3;

      protected override void Because()
      {
         _model = new Model();
         var root = new Container().WithName("ROOT");
         _model.Root = root;
         var subContainer = new Container().WithName("SUB").WithParentContainer(root);
         var p1Formula = new ExplicitFormula("P3").WithName("p1Formula");
         p1Formula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, "P3").WithAlias("P3"));

         var p2Formula = new ExplicitFormula("P3").WithName("p2Formula");
         p2Formula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER, "P3").WithAlias("P3"));
         var p3RHSFormula = new ExplicitFormula("P2").WithName("p3RHSFormula");
         p3RHSFormula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER,"SUB", "P2").WithAlias("P2"));

         _parameter1 = new Parameter().WithName("P1").WithFormula(p1Formula);
         _parameter2 = new Parameter().WithName("P2").WithFormula(p2Formula);
         _parameter3 = new Parameter().WithName("P3").WithFormula(new ExplicitFormula("1+2"));
         _parameter3.WithRHS(p3RHSFormula);

         root.Add(_parameter3);
         subContainer.Add(_parameter1);
         subContainer.Add(_parameter2);

         p1Formula.ResolveObjectPathsFor(_parameter1);
         p2Formula.ResolveObjectPathsFor(_parameter2);
         p3RHSFormula.ResolveObjectPathsFor(_parameter3);
      }

      [Observation]
      public void should_return_an_empty_set_if_the_object_is_not_referenced()
      {
         sut.AllUsingFormulaReferencing(_parameter1, _model).ShouldBeEmpty();
      }

      [Observation]
      public void should_return_the_expected_list_of_using_formula_referencing_the_given_reference_otherwise()
      {
         sut.AllUsingFormulaReferencing(_parameter3, _model).ShouldOnlyContain(_parameter1,_parameter2);
         sut.AllUsingFormulaReferencing(_parameter2, _model).ShouldOnlyContain(_parameter3);
      }
   }
}	