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
      private readonly IMoleculeStartValuesCreator _moleculeStartValuesCreator;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IParameterStartValuesCreator _parameterStartValuesCreator;
      private readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly IOutputSchemaFactory _outputSchemaFactory;
      private readonly IMoleculeBuilderFactory _moleculeBuilderFactory;
      private readonly IModelConstructor _modelConstructor;
      private readonly ISolverSettingsFactory _solverSettingsFactory;
      private readonly ReactionDimensionRetrieverForSpecs _reactionDimensionRetriever;
      private readonly IDimension _concentrationDimension;
      private readonly IDimension _concentrationPerTimeDimension;
      private readonly IDimension _volumeDimension;

      public ConcentrationBaseModelHelperForSpecs(IObjectBaseFactory objectBaseFactory, IParameterStartValuesCreator parameterStartValuesCreator, IMoleculeStartValuesCreator moleculeStartValuesCreator,
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
         _parameterStartValuesCreator = parameterStartValuesCreator;
         _moleculeStartValuesCreator = moleculeStartValuesCreator;
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

            var buildConfig = createBuildConfiguration();
            var model = createModel(buildConfig);
            return new ModelCoreSimulation { BuildConfiguration = buildConfig, Model = model };
         }
         finally
         {
            _reactionDimensionRetriever.SelectedDimensionMode = ReactionDimensionMode.AmountBased;
         }
      }

      private IModel createModel(IBuildConfiguration buildConfiguration)
      {
         var result = _modelConstructor.CreateModelFrom(buildConfiguration, "SpecModel");
         return result.Model;
      }

      private IBuildConfiguration createBuildConfiguration()
      {
         var buildConfiguration = new BuildConfigurationForSpecs();
         buildConfiguration.Molecules = getMolecules();
         buildConfiguration.Reactions = getReactions();
         buildConfiguration.SpatialStructure = getSpatialStructure();
         buildConfiguration.PassiveTransports = new PassiveTransportBuildingBlock();
         buildConfiguration.Observers = new ObserverBuildingBlock();
         buildConfiguration.EventGroups = new EventGroupBuildingBlock();
         buildConfiguration.SimulationSettings = createSimulationConfiguration();


         buildConfiguration.MoleculeStartValues = _moleculeStartValuesCreator.CreateFrom(buildConfiguration.SpatialStructure, buildConfiguration.Molecules);
         var objectPathForContainerThatDoesNotExist = _objectPathFactory.CreateObjectPathFrom("TOTO", "TATA");
         buildConfiguration.MoleculeStartValues.Add(_moleculeStartValuesCreator.CreateMoleculeStartValue(objectPathForContainerThatDoesNotExist, "A", _concentrationDimension));
         buildConfiguration.ParameterStartValues = _parameterStartValuesCreator.CreateFrom(buildConfiguration.SpatialStructure, buildConfiguration.Molecules);

         setMoleculeStartValues(buildConfiguration.MoleculeStartValues);
         return buildConfiguration;
      }

      private void setMoleculeStartValues(IMoleculeStartValuesBuildingBlock moleculeStartValues)
      {
         var organsim_A = moleculeStartValues[_objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, "A")];
         organsim_A.StartValue = 5;


         var lung_B = moleculeStartValues[_objectPathFactory.CreateObjectPathFrom(ConstantsForSpecs.Organism, ConstantsForSpecs.Lung, "B")];
         lung_B.Formula = null;
         lung_B.StartValue = 100;
      }

      private IReactionBuildingBlock getReactions()
      {
         var reactions = _objectBaseFactory.Create<IReactionBuildingBlock>();
         var R1 = _objectBaseFactory.Create<IReactionBuilder>()
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

      private IMoleculeBuildingBlock getMolecules()
      {
         var molecules = _objectBaseFactory.Create<IMoleculeBuildingBlock>();

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

      private ISpatialStructure getSpatialStructure()
      {
         var spatialStructure = _spatialStructureFactory.Create().WithName("SPATIAL STRUCTURE");

         var organism = _objectBaseFactory.Create<IContainer>()
            .WithName(ConstantsForSpecs.Organism)
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

      private SimulationSettings createSimulationConfiguration()
      {
         return new SimulationSettings {Solver = _solverSettingsFactory.CreateCVODE(), OutputSchema = _outputSchemaFactory.Create(0, 1440, 240), OutputSelections = new OutputSelections()};
      }
   }
}