using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public abstract class concern_for_ExplicitFormula : ContextSpecification<ExplicitFormula>
   {
      protected IFormulaUsable _x;
      protected IFormulaUsable _y;
      protected IUsingFormula _usingObj;
      protected IFormulaUsablePath _pathX;
      protected IFormulaUsablePath _pathY;

      protected override void Context()
      {
         _x = A.Fake<IFormulaUsable>();

         A.CallTo(() => _x.Value).Returns(2);

         _y = A.Fake<IFormulaUsable>();
         A.CallTo(() => _y.Value).Returns(3);

         _pathX = A.Fake<IFormulaUsablePath>();
         A.CallTo(() => _pathX.Alias).Returns("x");
         A.CallTo(() => _pathX.Resolve<IFormulaUsable>(_usingObj)).Returns(_x);


         _pathY = A.Fake<IFormulaUsablePath>();
         A.CallTo(() => _pathY.Alias).Returns("y");
         A.CallTo(() => _pathY.Resolve<IFormulaUsable>(_usingObj)).Returns(_y);


         sut = new ExplicitFormula();

         sut.AddObjectPath(_pathX);
         sut.AddObjectPath(_pathY);
      }
   }

   public class when_calculating_value_with_unresolved_references : concern_for_ExplicitFormula
   {
      protected double _formulaValue;

      protected override void Context()
      {
         base.Context();

         sut.FormulaString = "x*x+y-2";
         _formulaValue = _x.Value * _x.Value + _y.Value - 2;
      }

      [Observation]
      public void should_calculate_value_for_dependent_object()
      {
         sut.Calculate(_usingObj).ShouldBeEqualTo(_formulaValue);
      }
   }

   public class when_calculating_value_with_resolved_references : concern_for_ExplicitFormula
   {
      protected double _formulaValue;

      protected override void Context()
      {
         base.Context();

         sut.FormulaString = "x*x+y-2";
         _formulaValue = _x.Value * _x.Value + _y.Value - 2;

         var root = A.Fake<IContainer>();
         var model = A.Fake<IModel>();
         A.CallTo(() => model.Root).Returns(root);

         A.CallTo(() => _pathX.Resolve<IFormulaUsable>(root)).Returns(_x);
         A.CallTo(() => _pathY.Resolve<IFormulaUsable>(root)).Returns(_y);

         sut.ResolveObjectPathsFor(root);
      }

      [Observation]
      public void should_calculate_value_for_dependent_object()
      {
         sut.Calculate(_usingObj).ShouldBeEqualTo(_formulaValue);
      }
   }
}