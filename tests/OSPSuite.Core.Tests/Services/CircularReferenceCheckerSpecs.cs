using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_CircularReferenceChecker : ContextSpecification<ICircularReferenceChecker>
   {
      protected IFormula _formula;
      protected List<IFormulaUsablePath> _objectPaths;
      protected IQuantity _testObject;
      protected IObjectPathFactory _objectPathFactory;
      private IObjectTypeResolver _objectTypeResolver;

      protected override void Context()
      {
         _formula = A.Fake<IFormula>();
         _objectPaths = new List<IFormulaUsablePath>();
         A.CallTo(() => _formula.ObjectPaths).Returns(_objectPaths);
         _testObject = A.Fake<IQuantity>();
         _testObject.Formula = _formula;
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         sut = new CircularReferenceChecker(_objectPathFactory, _objectTypeResolver);
      }
   }

   public class When_checking_a_time_reference : concern_for_CircularReferenceChecker
   {
      private bool _result;

      protected override void Because()
      {
         _result = sut.HasCircularReference(new TimePath(), _testObject);
      }

      [Observation]
      public void should_always_return_no_circular_references()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_checking_a_self_reference : concern_for_CircularReferenceChecker
   {
      private bool _result;
      private IFormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         _path = A.Fake<IFormulaUsablePath>();
         A.CallTo(() => _path.Resolve<IUsingFormula>(_testObject)).Returns(_testObject);
         _objectPaths.Add(_path);
      }

      protected override void Because()
      {
         _result = sut.HasCircularReference(_path, _testObject);
      }

      [Observation]
      public void should_always_return_circular_references()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_checking_a_two_step_reference : concern_for_CircularReferenceChecker
   {
      private bool _result;
      private IFormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         var stepOne = A.Fake<IQuantity>().WithName("Step One");

         var stepFormula = A.Fake<IFormula>();

         var otherPath = A.Fake<IFormulaUsablePath>();
         otherPath.Alias = "ToTestObject";
         A.CallTo(() => otherPath.Resolve<IUsingFormula>(stepOne)).Returns(_testObject);


         _path = A.Fake<IFormulaUsablePath>();
         _path.Alias = "ToStepOne";
         A.CallTo(() => _path.Resolve<IUsingFormula>(_testObject)).Returns(stepOne);
         stepOne.Formula = stepFormula;
         A.CallTo(() => stepFormula.ObjectPaths).Returns(new[] {otherPath});
      }

      protected override void Because()
      {
         _result = sut.HasCircularReference(_path, _testObject);
      }

      [Observation]
      public void should_always_return_circular_references()
      {
         _result.ShouldBeTrue();
      }
   }

   public abstract class When_checking_a_simple_tree_structure : concern_for_CircularReferenceChecker
   {
      private Container _container;
      private Model _model;
      protected ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("Container");
         _model = new Model {Root = _container};
      }

      protected override void Because()
      {
         _result = sut.CheckCircularReferencesIn(_model, A.Fake<IBuildConfiguration>());
      }

      protected Parameter CreateParameter(string name, string objectPath)
      {
         var parameter = new Parameter().WithName(name);
         _container.Add(parameter);
         var explicitFormula = new ExplicitFormula("1+2");
         if (!string.IsNullOrEmpty(objectPath))
            explicitFormula.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, objectPath).WithAlias(objectPath));

         parameter.Formula = explicitFormula;

         return parameter;
      }
   }

   public class When_checking_a_simple_tree_structure_without_circular_references : When_checking_a_simple_tree_structure
   {
      protected override void Context()
      {
         base.Context();
         CreateParameter("A", "B");
         CreateParameter("B", "C");
         CreateParameter("C", "D");
         CreateParameter("D", "");
         CreateParameter("E", "B");
         CreateParameter("F", "E");
      }

      [Observation]
      public void should_return_no_circular_references()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   public class When_checking_a_simple_tree_structure_with_circular_references : When_checking_a_simple_tree_structure
   {
      protected override void Context()
      {
         base.Context();
         CreateParameter("A", "B");
         CreateParameter("B", "C");
         CreateParameter("C", "A");
      }

      [Observation]
      public void should_return_no_circular_references()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   public abstract class When_checking_circular_references_in_a_model : concern_for_CircularReferenceChecker
   {
      private Container _container;
      private Model _model;
      protected Parameter _parameter1;
      protected Parameter _parameter2;
      private IBuildConfiguration _buildConfiguration;
      protected ValidationResult _results;

      protected override void Context()
      {
         base.Context();
         _parameter1 = new Parameter().WithName("PARA1");
         _parameter2 = new Parameter().WithName("PARA2");
         _container = new Container().WithName("C");
         _model = new Model {Root = new ARootContainer {_container}.WithName("ROOT")};
         _container.Add(_parameter1);
         _container.Add(_parameter2);
         _buildConfiguration = A.Fake<IBuildConfiguration>();
      }

      protected override void Because()
      {
         _results = sut.CheckCircularReferencesIn(_model, _buildConfiguration);
      }
   }

   public class When_validating_the_references_used_in_a_quantity_resutling_in_formula_with_circular_references : When_checking_circular_references_in_a_model
   {
      protected override void Context()
      {
         base.Context();

         var forumula1 = new ExplicitFormula("PAR2");
         forumula1.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA2").WithAlias("PAR2"));
         _parameter1.Formula = forumula1;

         var forumula2 = new ExplicitFormula("PAR1");
         forumula2.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA1").WithAlias("PAR1"));
         _parameter2.Formula = forumula2;
      }

      [Observation]
      public void should_return_the_expected_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   public class When_validating_the_references_used_in_a_quantity_resutling_in_formula_with_a_rhs_circular_references : When_checking_circular_references_in_a_model
   {
      protected override void Context()
      {
         base.Context();

         var forumula1 = new ExplicitFormula("PAR1");
         forumula1.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA1").WithAlias("PAR1"));
         _parameter1.RHSFormula = forumula1;
      }

      [Observation]
      public void should_return_the_expected_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }
}