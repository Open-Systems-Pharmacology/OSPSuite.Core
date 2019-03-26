using System;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
  
   public abstract class concern_for_CreateExportModelVisitor : ContextSpecification<SimulationExportCreator>
   {
      protected IContainer _root;
      protected IModel _model;
      protected IObjectPathFactory _objectPathFactory;
      private ITableFormulaToTableFormulaExportMapper _tableFormulaToTableFormulaExportMapper;
      private IConcentrationBasedFormulaUpdater _concentrationBasedFormulaUpdater;
      private IDimensionFactory _dimensionFactory;
      private IObjectBaseFactory _objectBaseFactory;
      private IFormulaTask _formulaTask;
      private IDataRepositoryTask _dataRepositoryTask;
      private IModelFinalizer _modelFinalizer;

      protected override void Context()
      {
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _dimensionFactory = DimensionFactoryForSpecs.Factory;
         _objectBaseFactory = new ObjectBaseFactoryForSpecs(_dimensionFactory);
         _formulaTask= new FormulaTask(_objectPathFactory, _objectBaseFactory, new AliasCreator(),_dimensionFactory);
         _tableFormulaToTableFormulaExportMapper = A.Fake<ITableFormulaToTableFormulaExportMapper>();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _modelFinalizer = new ModelFinalizer(_objectPathFactory, new ReferencesResolver());
         _concentrationBasedFormulaUpdater = new ConcentrationBasedFormulaUpdater(new CloneManagerForModel(new ObjectBaseFactoryForSpecs(_dimensionFactory), _dataRepositoryTask,_modelFinalizer),
            _objectBaseFactory, _dimensionFactory,_formulaTask);
         _model = new Model().WithName("Model");
         _root = new Container().WithName("Root");
         _model.Root = _root;
         sut = new SimulationExportCreator(_objectPathFactory, _tableFormulaToTableFormulaExportMapper, _concentrationBasedFormulaUpdater);
      }
   }

   public class When_visiting_an_event : concern_for_CreateExportModelVisitor
   {
      private ExplicitFormula _altFormula;
      private Parameter _changedEntity;
      private SimulationExport _res;
      private IEvent _event;
      private IEventAssignment _assignment;

      protected override void Context()
      {
         base.Context();
         _event = new Event().WithName("Sigi Event")
            .WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("Time>0"))
            .WithId("1");
         _event.Formula.AddObjectPath(_objectPathFactory.CreateTimePath(A.Fake<IDimension>()));
         _changedEntity = new Parameter().WithName("ChangeMe")
            .WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("1"))
            .WithRHS(null);
         _altFormula = new ExplicitFormula().WithFormulaString("2").WithName("alternative Formula");
         _assignment = new EventAssignment {ObjectPath = _objectPathFactory.CreateAbsoluteObjectPath(_changedEntity), Formula = _altFormula};
         _event.Add(_assignment);
         _assignment.ResolveChangedEntity();
      }

      protected override void Because()
      {
         _res = sut.CreateExportFor(_model);
      }

      [Observation]
      public void should_create_a_correct_event_export()
      {
         var resSwitch = _res.EventList[0];
         resSwitch.EntityId.ShouldBeEqualTo(_event.Id);
         resSwitch.AssignmentList.Count.ShouldBeEqualTo(1);
         var resFormula = (from formulas in _res.FormulaList
            let explicitFormulas = formulas as ExplicitFormulaExport
            where explicitFormulas != null
            where explicitFormulas.Id == resSwitch.ConditionFormulaId
            select explicitFormulas).Single();
         resFormula.Equation.ShouldBeEqualTo(((ExplicitFormula) _event.Formula).FormulaString);
      }

      [Observation]
      public void should_create_the_correct_formulas()
      {
         _res.FormulaList.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_create_the_correct_assignment()
      {
         var resChange = _res.EventList[0].AssignmentList[0];
         resChange.ObjectId.ShouldBeEqualTo(_res.ParameterList[resChange.ObjectId].Id);
         var resFormula = (from formulas in _res.FormulaList
            let explicitFormulas = formulas as ExplicitFormulaExport
            where explicitFormulas != null
            where explicitFormulas.Id == resChange.NewFormulaId
            select explicitFormulas).Single();
         resFormula.Equation.ShouldBeEqualTo(_altFormula.FormulaString);
      }
   }

   public abstract class When_visiting_a_reaction : concern_for_CreateExportModelVisitor
   {
      protected SimulationExport _simulationExport;
      private IMoleculeAmount _spA;
      private IMoleculeAmount _spB;
      protected IReaction _reaction;
      protected IDimension _reactionDimension;
      protected Parameter _volume;
      protected ExplicitFormula _kinetic;

      protected override void Context()
      {
         base.Context();
         _kinetic = new ExplicitFormula().WithFormulaString("a+b");
         _reaction = new Reaction().WithName("Ralf Reaction")
            .WithParentContainer(_root)
            .WithFormula(_kinetic)
            .WithId("1")
            .WithDimension(_reactionDimension);

         _spA = new MoleculeAmount().WithName("SpA").WithParentContainer(_root)
            .WithId("2")
            .WithFormula(new ExplicitFormula().WithFormulaString("1"))
            .WithScaleFactor(1);


         _spB = new MoleculeAmount().WithName("SpB").WithParentContainer(_root)
            .WithId("3")
            .WithFormula(new ExplicitFormula().WithFormulaString("0"))
            .WithScaleFactor(2);

         _reaction.AddEduct(new ReactionPartner(1, _spA));
         _reaction.AddProduct(new ReactionPartner(2, _spB));

         _volume = new Parameter().WithFormula(new ConstantFormula(10)).WithId("4").WithName(Constants.Parameters.VOLUME);
      }
   }

   public class When_visiting_a_reaction_defined_in_amount_per_time : When_visiting_a_reaction
   {
      protected override void Context()
      {
         _reactionDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.AMOUNT_PER_TIME, "unit");
         base.Context();
      }

      protected override void Because()
      {
         _simulationExport = sut.CreateExportFor(_model);
      }

      [Observation]
      public void should_create_the_correct_formulas()
      {
         _simulationExport.FormulaList.Count.ShouldBeEqualTo(4);
      }
   }

   public class When_visiting_a_reaction_defined_in_concentration_per_time_that_is_defined_in_a_container_with_a_volume_parameter_and_uses_already_a_reference_to_the_volume : When_visiting_a_reaction
   {
      protected override void Context()
      {
         _reactionDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME, "unit");
         base.Context();
         _root.Add(_volume);
         _kinetic.AddObjectPath(new FormulaUsablePath(ObjectPath.PARENT_CONTAINER, _volume.Name).WithAlias("VOL"));
         _kinetic.ResolveObjectPathsFor(_reaction);
      }

      protected override void Because()
      {
         _simulationExport = sut.CreateExportFor(_model);
      }

      [Observation]
      public void should_simply_add_the_volume_multiplication_in_the_kinetic_using_the_existing_alias()
      {
         var kinetic = _simulationExport.FormulaList.OfType<ExplicitFormulaExport>().First(x => x.ReferenceList.Any());
         kinetic.Equation.ShouldBeEqualTo("-((a+b)*VOL)");
      }
   }

   public class When_visiting_a_reaction_defined_in_concentration_per_time_that_is_defined_in_a_container_with_a_volume_parameter_and_does_not_use_a_reference_to_the_volume : When_visiting_a_reaction
   {
      protected override void Context()
      {
         _reactionDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME, "unit");
         base.Context();
         _root.Add(_volume);
      }

      protected override void Because()
      {
         _simulationExport = sut.CreateExportFor(_model);
      }

      [Observation]
      public void should_add_the_volume_multiplication_in_the_kinetic_using_the_default_volume_alias()
      {
         var kinetic = _simulationExport.FormulaList.OfType<ExplicitFormulaExport>().First(x => x.ReferenceList.Any());
         kinetic.Equation.ShouldBeEqualTo(string.Format("-((a+b)*{0})", Constants.VOLUME_ALIAS));
      }
   }

   public class When_visiting_a_reaction_defined_in_concentration_per_time_that_is_defined_in_a_container_without_a_volume_parameter : When_visiting_a_reaction
   {
      protected override void Context()
      {
         _reactionDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME, "unit");
         base.Context();
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.CreateExportFor(_model)).ShouldThrowAn<Exception>();
      }
   }
}