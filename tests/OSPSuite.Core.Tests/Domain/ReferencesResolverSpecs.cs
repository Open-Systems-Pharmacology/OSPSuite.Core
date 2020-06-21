using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_model_formula_reference_resovler : ContextSpecification<IReferencesResolver>
   {
      protected override void Context()
      {
         sut = new ReferencesResolver();
      }
   }

   
   public class When_resolving_the_references_used_in_a_give_model : concern_for_model_formula_reference_resovler
   {
      private IModel _model;
      private IContainer _root;
      private IFormula _formula1;
      private IContainer _subContainer;
      private IFormula _formula2;
      private IFormula _formula3;
      private IFormula _rhsformula1;
      private IParameter _para1;
      private IEntity _para2;
      private Parameter _para3;

      protected override void Context()
      {
         base.Context();
         _formula1 = A.Fake<IFormula>();
         _formula2 = A.Fake<IFormula>();
         _formula3 = A.Fake<IFormula>();
         _rhsformula1 = A.Fake<IFormula>();
         _root = new Container().WithName("root");
         _para1 = new Parameter().WithFormula(_formula1).WithName("para");
         _root.Add(_para1);
         _subContainer = new Container().WithName("SubContaiener");
         _root.Add(_subContainer);
         _para2 = new Parameter().WithName("para1").WithFormula(_formula2).WithRHS(null);
         _subContainer.Add(_para2);
         _para3 = new Parameter().WithName("para2").WithFormula(_formula3).WithRHS(_rhsformula1);
         _subContainer.Add(_para3);
         _model = new Model {Neighborhoods = new Container(), Root = _root};
      }

      protected override void Because()
      {
         sut.ResolveReferencesIn(_model);
      }

      [Observation]
      public void should_resolve_the_references_for_all_using_formula_defined_in_the_model()
      {
          A.CallTo(() => _formula1.ResolveObjectPathsFor(_para1)).MustHaveHappened();
          A.CallTo(() => _formula2.ResolveObjectPathsFor(_para2)).MustHaveHappened();
          A.CallTo(() => _formula3.ResolveObjectPathsFor(_para3)).MustHaveHappened();
      }

      [Observation]
      public void should_resolve_the_references_for_all_rhs_formula_for_parameter_defined_in_the_model()
      {
         A.CallTo(() => _rhsformula1.ResolveObjectPathsFor(_para3)).MustHaveHappened();
      }
   }
}