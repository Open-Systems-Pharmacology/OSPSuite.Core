using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_FormulaUsablePath : ContextSpecification<IFormulaUsablePath>
   {
      protected IContainer _comp;
      protected IContainer _organ;
      protected IDistributedParameter _parameter;
      protected IUsingFormula _reak;
      protected IContainer _root;
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _root = new Container().WithName("root");
         _organ = new Container().WithName("organ").WithParentContainer(_root);
         _comp = new Container().WithName("comp").WithParentContainer(_organ);
         _parameter = new DistributedParameter().WithName("P1").WithParentContainer(_comp);
         new Parameter().WithName("LALA").WithParentContainer(_parameter);
         _reak = A.Fake<IUsingFormula>();
         A.CallTo(() => _reak.ParentContainer).Returns(_root);
         A.CallTo(() => _reak.RootContainer).Returns(_root);
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
      }
   }

   public class When_we_resolve_an_absolute_path_for_a_present_object : concern_for_FormulaUsablePath
   {
      private IParameter _res;

      protected override void Context()
      {
         base.Context();
         sut = _objectPathFactory.CreateAbsoluteFormulaUsablePath(_parameter);
      }

      protected override void Because()
      {
         _res = sut.Resolve<IParameter>(_reak);
      }

      [Observation]
      public void should_return_the_object()
      {
         _res.ShouldBeEqualTo(_parameter);
      }
   }

   public class When_we_resolve_an_absolute_path_for_a_missing_object : concern_for_FormulaUsablePath
   {
      private IParameter _res;

      protected override void Context()
      {
         base.Context();
         sut = _objectPathFactory.CreateAbsoluteFormulaUsablePath(_parameter);
         _parameter.ParentContainer.RemoveChild(_parameter);
      }

      protected override void Because()
      {
         _res = sut.Resolve<IParameter>(_reak);
      }

      [Observation]
      public void should_return_null()
      {
         _res.ShouldBeNull();
      }
   }

   public class When_cloning_a_formula_usable_path : concern_for_FormulaUsablePath
   {
      private IObjectPath _objectPath;
      private IObjectPath _clonePath;

      protected override void Context()
      {
         _objectPath = new FormulaUsablePath(new[] {"A", "B"}).WithAlias("C");
      }

      protected override void Because()
      {
         _clonePath = _objectPath.Clone<IObjectPath>();
      }

      [Observation]
      public void should_return_a_formula_usable_path()
      {
         _clonePath.ShouldBeAnInstanceOf<IFormulaUsablePath>();
      }

      [Observation]
      public void should_have_updated_the_alias()
      {
         _clonePath.DowncastTo<IFormulaUsablePath>().Alias.ShouldBeEqualTo("C");
      }
   }

   public class When_comparing_equal_paths : concern_for_FormulaUsablePath
   {
      private IFormulaUsablePath _equalPath;

      protected override void Context()
      {
         base.Context();
         sut = _objectPathFactory.CreateAbsoluteFormulaUsablePath(_parameter);
         _equalPath = new FormulaUsablePath(sut).WithAlias(sut.Alias).WithDimension(sut.Dimension);
      }

      [Observation]
      public void should_return_true()
      {
         sut.Equals(_equalPath).ShouldBeTrue();
      }
   }

   public class When_comparing_different_paths : concern_for_FormulaUsablePath
   {
      private IFormulaUsablePath _differentPath;

      protected override void Context()
      {
         base.Context();
         sut = _objectPathFactory.CreateAbsoluteFormulaUsablePath(_parameter);
         _differentPath = new FormulaUsablePath(sut).WithAlias("Bla").WithDimension(sut.Dimension);
      }

      [Observation]
      public void should_return_false()
      {
         sut.Equals(_differentPath).ShouldBeFalse();
      }
   }

   public class When_comparing_path_diffeering_only_by_dimension : concern_for_FormulaUsablePath
   {
      private IFormulaUsablePath _differentPath;

      protected override void Context()
      {
         base.Context();
         sut = _objectPathFactory.CreateAbsoluteFormulaUsablePath(_parameter);
         _differentPath = new FormulaUsablePath(sut).WithAlias(sut.Alias).WithDimension(A.Fake<IDimension>());
      }

      [Observation]
      public void should_return_false()
      {
         sut.Equals(_differentPath).ShouldBeFalse();
      }
   }
}