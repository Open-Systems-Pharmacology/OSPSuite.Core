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
   internal abstract class concern_for_CircularReferenceChecker : ContextSpecification<CircularReferenceChecker>
   {
      protected IFormula _formula;
      protected List<FormulaUsablePath> _objectPaths;
      protected IQuantity _testObject;
      protected IObjectPathFactory _objectPathFactory;
      protected IObjectTypeResolver _objectTypeResolver;

      protected override void Context()
      {
         _formula = A.Fake<IFormula>();
         _objectPaths = new List<FormulaUsablePath>();
         A.CallTo(() => _formula.ObjectPaths).Returns(_objectPaths);
         _testObject = A.Fake<IQuantity>();
         _testObject.Formula = _formula;
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         sut = new CircularReferenceChecker(_objectPathFactory, _objectTypeResolver);
      }
   }

   internal class When_checking_a_time_reference : concern_for_CircularReferenceChecker
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

   internal class When_checking_a_self_reference : concern_for_CircularReferenceChecker
   {
      private bool _result;
      private FormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         _path = A.Fake<FormulaUsablePath>();
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

   internal class When_checking_a_two_step_reference : concern_for_CircularReferenceChecker
   {
      private bool _result;
      private FormulaUsablePath _path;

      protected override void Context()
      {
         base.Context();
         var stepOne = A.Fake<IQuantity>().WithName("Step One");

         var stepFormula = A.Fake<IFormula>();

         var otherPath = A.Fake<FormulaUsablePath>();
         otherPath.Alias = "ToTestObject";
         A.CallTo(() => otherPath.Resolve<IUsingFormula>(stepOne)).Returns(_testObject);


         _path = A.Fake<FormulaUsablePath>();
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

   internal abstract class When_checking_a_simple_tree_structure : concern_for_CircularReferenceChecker
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
         var simulationConfiguration = new SimulationConfiguration();
         _result = sut.CheckCircularReferencesIn(new ModelConfiguration(_model, simulationConfiguration, new SimulationBuilder(simulationConfiguration)));
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

   internal class When_checking_a_simple_tree_structure_without_circular_references : When_checking_a_simple_tree_structure
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

   internal class When_checking_a_simple_tree_structure_with_circular_references : When_checking_a_simple_tree_structure
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

   internal abstract class When_checking_circular_references_in_a_model : concern_for_CircularReferenceChecker
   {
      private Container _container;
      private Model _model;
      protected Parameter _parameter1;
      protected Parameter _parameter2;
      private SimulationConfiguration _simulationConfiguration;
      protected ValidationResult _results;
      protected Container _eventContainer;

      protected override void Context()
      {
         base.Context();
         _parameter1 = new Parameter().WithName("PARA1");
         _parameter2 = new Parameter().WithName("PARA2");
         _container = new Container().WithName("C");
         _model = new Model {Root = new ARootContainer {_container}.WithName("ROOT")};
         _eventContainer = new Container().WithName("EventsContainer");
         _model.Root.Add(_eventContainer);
         _container.Add(_parameter1);
         _container.Add(_parameter2);
         _simulationConfiguration = new SimulationConfiguration();
      }

      protected override void Because()
      {
         _results = sut.CheckCircularReferencesIn(new ModelConfiguration(_model, _simulationConfiguration, new SimulationBuilder(_simulationConfiguration)));
      }
   }

   internal abstract class When_validating_the_references_used_in_an_assignment : When_checking_circular_references_in_a_model
   {
      private EventAssignment _assignment;
      private Event _event;

      protected override void Context()
      {
         base.Context();

         _assignment = new EventAssignment { UseAsValue = UseAsValue, ChangedEntity = _parameter1 };
         _event = new Event().WithName("The Event");
         _event.AddAssignment(_assignment);
         _eventContainer.Add(_event);
         _assignment.ObjectPath = _parameter1.EntityPath().ToObjectPath();

         var formula1 = new ExplicitFormula("PAR2");
         formula1.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA2").WithAlias("PAR2"));
         _assignment.Formula = formula1;

         var formula2 = new ExplicitFormula("PAR1");
         formula2.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA1").WithAlias("PAR1"));
         _parameter2.Formula = formula2;

         A.CallTo(() => _objectTypeResolver.TypeFor<IUsingFormula>(_parameter1)).Returns("Parameter");
      }

      public abstract bool UseAsValue { get; }
   }

   internal class When_validating_the_references_used_in_an_assignment_with_circular_references : When_validating_the_references_used_in_an_assignment
   {
      [Observation]
      public void should_return_the_expected_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }

      public override bool UseAsValue => false;
   }

   internal class When_validating_the_references_used_in_an_assignment_with_use_as_value : When_validating_the_references_used_in_an_assignment
   {
      [Observation]
      public void should_return_the_expected_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }

      public override bool UseAsValue => true;
   }

   internal class When_validating_the_references_used_in_a_quantity_resulting_in_formula_with_circular_references : When_checking_circular_references_in_a_model
   {
      protected override void Context()
      {
         base.Context();

         var formula1 = new ExplicitFormula("PAR2");
         formula1.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA2").WithAlias("PAR2"));
         _parameter1.Formula = formula1;

         var formula2 = new ExplicitFormula("PAR1");
         formula2.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA1").WithAlias("PAR1"));
         _parameter2.Formula = formula2;
      }

      [Observation]
      public void should_return_the_expected_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   internal class When_validating_the_references_used_in_a_quantity_resulting_in_formula_with_a_rhs_circular_references : When_checking_circular_references_in_a_model
   {
      protected override void Context()
      {
         base.Context();

         var formula1 = new ExplicitFormula("PAR1");
         formula1.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("ROOT", "C", "PARA1").WithAlias("PAR1"));
         _parameter1.RHSFormula = formula1;
      }

      [Observation]
      public void should_return_the_expected_results()
      {
         _results.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }
}