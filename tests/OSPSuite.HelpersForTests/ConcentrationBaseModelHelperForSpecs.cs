using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Helpers
{
   public class ConcentrationBaseModelHelperForSpecs
   {
      private readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IParameterValuesCreator _parameterValuesCreator;
      private readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly IOutputSchemaFactory _outputSchemaFactory;
      private readonly IMoleculeBuilderFactory _moleculeBuilderFactory;
      private readonly IModelConstructor _modelConstructor;
      private readonly ISolverSettingsFactory _solverSettingsFactory;
      private readonly ReactionDimensionRetrieverForSpecs _reactionDimensionRetriever;
      private readonly IDimension _concentrationDimension;
      private readonly IDimension _concentrationPerTimeDimension;
      private readonly IDimension _volumeDimension;

      public ConcentrationBaseModelHelperForSpecs(IObjectBaseFactory objectBaseFactory, IParameterValuesCreator parameterValuesCreator, IInitialConditionsCreator initialConditionsCreator,
         IObjectPathFactory objectPathFactory, IDimensionFactory dimensionFactory, ISpatialStructureFactory spatialStructureFactory,
         IOutputSchemaFactory outputSchemaFactory, IMoleculeBuilderFactory moleculeBuilderFactory,
         IReactionDimensionRetriever reactionDimensionRetriever, IModelConstructor modelConstructor, ISolverSettingsFactory solverSettingsFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _outputSchemaFactory = outputSchemaFactory;
         _moleculeBuilderFactory = moleculeBuilderFactory;
         _modelConstructor = modelConstructor;
         _solverSettingsFactory = solverSettingsFactory;
         _reactionDimensionRetriever = reactionDimensionRetriever.DowncastTo<ReactionDimensionRetrieverForSpecs>();
         _spatialStructureFactory = spatialStructureFactory;
         _parameterValuesCreator = parameterValuesCreator;
         _initialConditionsCreator = initialConditionsCreator;
         _objectPathFactory = objectPathFactory;
         _concentrationDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION);
         _concentrationPerTimeDimension = dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION_PER_TIME);
         _volumeDimension = dimensionFactory.Dimension(Constants.Dimension.VOLUME);
      }

      public IModelCoreSimulation CreateSimulation()
      {
         try
         {
            _reactionDimensionRetriever.SelectedDimensionMode = ReactionDimensionMode.ConcentrationBased;

            var simulationConfiguration = createSimulationConfiguration();
            var model = createModel(simulationConfiguration);
            return new ModelCoreSimulation { Configuration = simulationConfiguration, Model = model };
         }
         finally
         {
            _reactionDimensionRetriever.SelectedDimensionMode = ReactionDimensionMode.AmountBased;
         }
      }

      private IModel createModel(SimulationConfiguration simulationConfiguration)
      {
         var result = _modelConstructor.CreateModelFrom(simulationConfiguration, "SpecModel");
         return result.Model;
      }

      private SimulationConfiguration createSimulationConfiguration()
      {
         var simulationConfiguration = new SimulationConfiguration();

         var module = new Module
         {
            getMolecules(),
            getReactions(),
            getSpatialStructure(),
            new PassiveTransportBuildingBlock(),
            new ObserverBuildingBlock(),
            new EventGroupBuildingBlock()
         };
         simulationConfiguration.SimulationSettings = createSimulationSettings();


         var initialConditions = _initialConditionsCreator.CreateFrom(module.SpatialStructure, module.Molecules.ToList());
         var objectPathForContainerThatDoesNotExist = new ObjectPath("TOTO", "TATA");
         initialConditions.Add(_initialConditionsCreator.CreateInitialCondition(objectPathForContainerThatDoesNotExist, "A", _concentrationDimension));

         module.Add(initialConditions);
         var parameterValues = _objectBaseFactory.Create<ParameterValuesBuildingBlock>();
         module.Add(parameterValues);
         setInitialConditions(initialConditions);

         var moduleConfiguration = new ModuleConfiguration(module, initialConditions, parameterValues);
         simulationConfiguration.AddModuleConfiguration(moduleConfiguration);
         return simulationConfiguration;
      }

      private void setInitialConditions(InitialConditionsBuildingBlock initialConditions)
      {
         var organsim_A = initialConditions[new ObjectPath(Constants.ORGANISM, "A")];
         organsim_A.Value = 5;


         var lung_B = initialConditions[new ObjectPath(Constants.ORGANISM, ConstantsForSpecs.Lung, "B")];
         lung_B.Formula = null;
         lung_B.Value = 100;
      }

      private ReactionBuildingBlock getReactions()
      {
         var reactions = _objectBaseFactory.Create<ReactionBuildingBlock>();
         var R1 = _objectBaseFactory.Create<ReactionBuilder>()
            .WithName("R1")
            .WithKinetic(R1Formula(reactions.FormulaCache))
            .WithDimension(_concentrationPerTimeDimension);

         R1.AddEduct(new ReactionPartnerBuilder("A", 1));
         R1.AddProduct(new ReactionPartnerBuilder("B", 1));
         reactions.Add(R1);
         return reactions;
      }

      private IFormula R1Formula(IFormulaCache formulaCache)
      {
         var formula = formulaCache.FirstOrDefault(x => string.Equals(x.Name, "R1"));
         if (formula != null)
            return formula;

         formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("A+B").WithName("R1");
         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER, "A", Constants.Parameters.CONCENTRATION)
            .WithAlias("A")
            .WithDimension(_concentrationDimension));

         formula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER, "B", Constants.Parameters.CONCENTRATION)
            .WithAlias("B")
            .WithDimension(_concentrationDimension));

         formula.WithDimension(_concentrationPerTimeDimension);
         formulaCache.Add(formula);

         return formula;
      }

      private MoleculeBuildingBlock getMolecules()
      {
         var molecules = _objectBaseFactory.Create<MoleculeBuildingBlock>();

         var moleculeA = _moleculeBuilderFactory.Create(molecules.FormulaCache).WithName("A");
         moleculeA.DefaultStartFormula = _objectBaseFactory.Create<ConstantFormula>()
            .WithValue(2)
            .WithDimension(_concentrationDimension);
         moleculeA.Dimension = _concentrationDimension;
         molecules.Add(moleculeA);


         var moleculeB = _moleculeBuilderFactory.Create(molecules.FormulaCache).WithName("B");
         var startValueMoleculeB = _objectBaseFactory.Create<ExplicitFormula>()
            .WithFormulaString("C/2")
            .WithDimension(_concentrationDimension);

         startValueMoleculeB.AddObjectPath(
            _objectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER, "A", Constants.Parameters.CONCENTRATION)
               .WithAlias("C")
               .WithDimension(_concentrationDimension));

         moleculeB.DefaultStartFormula = startValueMoleculeB;
         moleculeB.Dimension = _concentrationDimension;
         molecules.Add(moleculeB);

         return molecules;
      }

      private SpatialStructure getSpatialStructure()
      {
         var spatialStructure = _spatialStructureFactory.Create().WithName("SPATIAL STRUCTURE");

         var organism = _objectBaseFactory.Create<IContainer>()
            .WithName(Constants.ORGANISM)
            .WithMode(ContainerMode.Physical);

         var lung = _objectBaseFactory.Create<IContainer>()
            .WithName(ConstantsForSpecs.Lung)
            .WithMode(ContainerMode.Physical)
            .WithParentContainer(organism);

         lung.Add(createVolumeParameter(20));
         organism.Add(createVolumeParameter(10));

         spatialStructure.AddTopContainer(organism);
         return spatialStructure;
      }

      private IParameter createVolumeParameter(double volumeValue)
      {
         return _objectBaseFactory.Create<IParameter>()
            .WithName(Constants.Parameters.VOLUME)
            .WithFormula(_objectBaseFactory.Create<ConstantFormula>().WithValue(volumeValue).WithDimension(_volumeDimension))
            .WithDimension(_volumeDimension);
      }

      private SimulationSettings createSimulationSettings()
      {
         return new SimulationSettings { Solver = _solverSettingsFactory.CreateCVODE(), OutputSchema = _outputSchemaFactory.Create(0, 1440, 240), OutputSelections = new OutputSelections() };
      }
   }
}