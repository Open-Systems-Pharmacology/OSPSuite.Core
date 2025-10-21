using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Helpers.ConstantsForSpecs;

namespace OSPSuite.Helpers
{
   public class ModuleHelperForSpecs
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ISolverSettingsFactory _solverSettingsFactory;
      private readonly IOutputSchemaFactory _outputSchemaFactory;
      private readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly INeighborhoodBuilderFactory _neighborhoodFactory;
      private readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly ModelHelperForSpecs _modelHelper;

      public ModuleHelperForSpecs(
         IObjectPathFactory objectPathFactory,
         IObjectBaseFactory objectBaseFactory,
         ISolverSettingsFactory solverSettingsFactory,
         IOutputSchemaFactory outputSchemaFactory,
         ISpatialStructureFactory spatialStructureFactory,
         INeighborhoodBuilderFactory neighborhoodFactory,
         IInitialConditionsCreator initialConditionsCreator,
         IDimensionFactory dimensionFactory,
         ModelHelperForSpecs modelHelper)
      {
         _dimensionFactory = dimensionFactory;
         _objectPathFactory = objectPathFactory;
         _objectBaseFactory = objectBaseFactory;
         _solverSettingsFactory = solverSettingsFactory;
         _outputSchemaFactory = outputSchemaFactory;
         _spatialStructureFactory = spatialStructureFactory;
         _neighborhoodFactory = neighborhoodFactory;
         _initialConditionsCreator = initialConditionsCreator;
         _modelHelper = modelHelper;
      }

      public SimulationConfiguration CreateSimulationConfiguration()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = CreateSimulationSettings(),
         };

         var module1 = createModule1();
         var module2 = createModule2();
         var module3 = createModule3();

         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module3));
         return simulationConfiguration;
      }

      public SimulationConfiguration CreateSimulationConfigurationForExtendMergeBehavior()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = CreateSimulationSettings(),
         };

         var module1 = createModule1();
         var module2 = createModule2();

         module2.MergeBehavior = MergeBehavior.Extend;
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         // Using this constructor to validate that the merge behavior is set correctly
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2, null, null));
         return simulationConfiguration;
      }

      public SimulationConfiguration CreateSimulationConfigurationForExtendMergeBehaviorOverridingModuleBehavior()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = CreateSimulationSettings(),
         };

         var module1 = createModule1();
         var module2 = createModule2();
         module2.MergeBehavior = MergeBehavior.Extend;

         module1.Add(createEventGroupBuildingBlock("EventForModule1", "eventGroup1", "eventBuilder1", "eventAssignment1", "parameter1"));
         module2.Add(createEventGroupBuildingBlock("EventForModule2", "eventGroup2", "eventBuilder1", "eventAssignment1", "parameter1"));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2, null, null));

         return simulationConfiguration;
      }

      public SimulationConfiguration CreateSimulationConfigurationForOverrideMergeBehavior()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = CreateSimulationSettings(),
         };

         var module1 = createModule1();
         var module2 = createModule2();
         module2.MergeBehavior = MergeBehavior.Overwrite;

         module1.Add(createEventGroupBuildingBlock("EventForModule1", "eventGroup1", "eventBuilder1", "eventAssignment1", "parameter1"));
         module2.Add(createEventGroupBuildingBlock("EventForModule1", "eventGroup1", "eventBuilder2", "eventAssignment1", "parameter1"));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2, null, null));

         return simulationConfiguration;
      }

      public SimulationConfiguration CreateSimulationConfigurationForExtendMergeBehaviorSameEventGroupName()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = CreateSimulationSettings(),
         };

         var module1 = createModule1();
         var module2 = createModule2();
         module2.MergeBehavior = MergeBehavior.Extend;

         module1.Add(createEventGroupBuildingBlock("EventForModule1", "eventGroup1", "eventBuilder1", "eventAssignment1", "parameter1", true));
         module2.Add(createEventGroupBuildingBlock("EventForModule1", "eventGroup1", "eventBuilder2", "eventAssignment1", "parameter1", true));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2, null, null));

         return simulationConfiguration;
      }

      public SimulationConfiguration CreateSimulationConfigurationForOverrideMergeBehaviorSameEventGroupName()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = CreateSimulationSettings(),
         };

         var module1 = createModule1();
         var module2 = createModule2();
         module2.MergeBehavior = MergeBehavior.Overwrite;

         module1.Add(createEventGroupBuildingBlock("EventForModule1", "eventGroup1", "eventBuilder1", "eventAssignment1", "parameter1", true));
         module2.Add(createEventGroupBuildingBlock("EventForModule1", "eventGroup1", "eventBuilder2", "eventAssignment1", "parameter1", true));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2, null, null));

         return simulationConfiguration;
      }

      public SimulationConfiguration CreateSimulationConfigurationForExtendMergeBehaviorWithRecursiveContainers()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = CreateSimulationSettings(),
         };

         var module4 = createModule4();
         var module5 = createModule5();

         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module4));
         module5.MergeBehavior = MergeBehavior.Extend;
         // Using this constructor to validate that the merge behavior is set correctly
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module5));
         return simulationConfiguration;
      }

      private Module createModule1()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module1");
         module.Add(getSpatialStructureModule1());
         module.Add(getMoleculesModule1());

         var initialConditions = _initialConditionsCreator.CreateFrom(module.SpatialStructure, module.Molecules.ToList());
         module.Add(initialConditions);

         return module;
      }

      private MoleculeBuildingBlock getMoleculesModule1()
      {
         var molecules = _objectBaseFactory.Create<MoleculeBuildingBlock>();
         molecules.Add(createMoleculeA(molecules.FormulaCache));
         return molecules;
      }

      private MoleculeBuilder createMoleculeA(IFormulaCache formulaCache)
      {
         var moleculeC = _modelHelper.DefaultMolecule("A", 3, 3, QuantityType.Drug, formulaCache);
         var globalParameter = NewConstantParameter("A_Global", 5, ParameterBuildMode.Global);
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("A_Global_Formula").WithFormulaString("2+2");
         globalParameter.Formula = formula;
         formulaCache.Add(formula);
         moleculeC.Add(globalParameter);
         moleculeC.IsFloating = true;
         return moleculeC;
      }

      public IContainer CreateOrganism()
      {
         return _objectBaseFactory.Create<IContainer>()
            .WithName(ORGANISM)
            .WithMode(ContainerMode.Logical);
      }

      private EventGroupBuildingBlock createEventGroupBuildingBlock(string collectionName, string eventGroupName, string eventBuilderName, string eventAssignmentName, string parameterName, bool createTwoEvents = false)
      {
         var eventGroupBuilderCollection = _objectBaseFactory.Create<EventGroupBuildingBlock>().WithName(collectionName);
         var eventGroupBuilder = createBolusApplication(eventGroupBuilderCollection.FormulaCache, eventGroupName, eventBuilderName, eventAssignmentName, parameterName, eventCount: createTwoEvents ? 2 : 1);
         eventGroupBuilderCollection.Add(eventGroupBuilder);

         return eventGroupBuilderCollection;
      }

      private EventGroupBuilder createBolusApplication(IFormulaCache cache, string eventGroupName, string eventBuilderName, string eventAssignmentName, string parameterName, int eventCount = 1)
      {
         var eventGroup = _objectBaseFactory.Create<EventGroupBuilder>().WithName(eventGroupName);
         eventGroup.SourceCriteria = Create.Criteria(x => x.With(ArterialBlood).And.With(Plasma));

         for (var i = 0; i < eventCount; i++)
         {
            var eventBuilder = _objectBaseFactory.Create<EventBuilder>()
               .WithName(i == 0 ? eventBuilderName : eventBuilderName + (i + 1))
               .WithFormula(conditionForBolusApp(cache));
            eventBuilder.OneTime = true;

            var eventAssignment = _objectBaseFactory.Create<EventAssignmentBuilder>()
               .WithName(i == 0 ? eventAssignmentName : eventAssignmentName + (i + 1))
               .WithFormula(createBolusDosisFormula(cache));
            eventAssignment.UseAsValue = true;
            eventAssignment.ObjectPath = new ObjectPath(ORGANISM, ArterialBlood, Plasma, "A");
            eventBuilder.AddAssignment(eventAssignment);
            eventBuilder.AddParameter(NewConstantParameter(parameterName, 10));
            eventGroup.Add(eventBuilder);
         }

         return eventGroup;
      }

      private IFormula createBolusDosisFormula(IFormulaCache cache)
      {
         var dosisFormula = _objectBaseFactory.Create<ExplicitFormula>()
            .WithFormulaString("A+10")
            .WithName("BolusDosis");
         dosisFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom(
            ORGANISM, ArterialBlood,
            Plasma, "A").WithAlias("A"));
         cache.Add(dosisFormula);
         return dosisFormula;
      }

      private IFormula conditionForBolusApp(IFormulaCache cache)
      {
         var conditionFormula = _objectBaseFactory.Create<ExplicitFormula>()
            .WithFormulaString("Time = StartTime")
            .WithName("StartCondition");
         conditionFormula.AddObjectPath(_objectPathFactory.CreateTimePath(_dimensionFactory.Dimension(Constants.Dimension.TIME)));
         conditionFormula.AddObjectPath(_objectPathFactory.CreateFormulaUsablePathFrom("StartTime").WithAlias("StartTime"));
         cache.Add(conditionFormula);
         return conditionFormula;
      }

      private SpatialStructure getSpatialStructureModule1()
      {
         var spatialStructure = CreateSpatialStructure("SPATIAL STRUCTURE MODULE 1");
         var organism = CreateOrganism();

         //Create a parameter with formula in Organism with absolute path
         var bw = NewConstantParameter(BW, 20);
         organism.Add(bw);

         var globalMoleculeProperties = _objectBaseFactory.Create<IContainer>().WithName(MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);

         //add two parameters. One will be replaced when merging the second module
         globalMoleculeProperties.Add(NewConstantParameter("P1", 10));
         globalMoleculeProperties.Add(NewConstantParameter("P2", 20));

         spatialStructure.GlobalMoleculeDependentProperties = globalMoleculeProperties;

         //ART
         var art = createContainerWithName(ArterialBlood);

         var artPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         artPlasma.AddTag(new Tag(ArterialBlood));
         artPlasma.AddTag(new Tag(Plasma));
         artPlasma.Add(NewConstantParameter(Volume, 2));
         art.Add(artPlasma);
         art.Add(NewConstantParameter(Q, 2));
         art.Add(NewConstantParameter(P, 10));
         organism.Add(art);


         //LUNG
         // Lung
         //   - Plasma
         //      - Volume
         //      - pH
         //   - Cell
         //      - Volume
         //      - pH
         //   - Q
         //   - P

         //set the container to being physical. It will become logical after update
         var lung = createContainerWithName(Lung, ContainerMode.Logical);
         lung.AddTag("Tag1");
         var lngPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         lngPlasma.Add(NewConstantParameter(Volume, 2));
         lngPlasma.Add(NewConstantParameter(pH, 7.5));
         lung.Add(lngPlasma);

         var lngCell = createContainerWithName(Cell, ContainerMode.Physical);
         lngCell.Add(NewConstantParameter(Volume, 1));
         lngCell.Add(NewConstantParameter(pH, 7));
         lung.Add(lngCell);

         var lungQ = NewConstantParameter(Q, 3);
         lungQ.AddTag("ParamTag1");
         lung.Add(lungQ);

         lung.Add(NewConstantParameter(P, 2));
         organism.Add(lung);

         //BONE
         var bone = createContainerWithName(Bone);
         organism.Add(bone);

         var bonePlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         bonePlasma.Add(NewConstantParameter(Volume, 2));
         bonePlasma.Add(NewConstantParameter(pH, 7.5));
         bone.Add(bonePlasma);


         var boneCell = createContainerWithName(Cell, ContainerMode.Physical);
         boneCell.Add(NewConstantParameter(Volume, 1));
         boneCell.Add(NewConstantParameter(pH, 7));
         bone.Add(boneCell);


         //VEN
         var ven = createContainerWithName(VenousBlood);
         var venPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         venPlasma.Add(NewConstantParameter(Volume, 2));
         ven.Add(venPlasma);
         ven.Add(NewConstantParameter(Q, 2));
         organism.Add(ven);

         organism.Add(NewConstantParameter(fu, 1));
         spatialStructure.AddTopContainer(organism);

         var neighborhood1 = _neighborhoodFactory.CreateBetween(artPlasma, bonePlasma).WithName("art_pls_to_bon_pls");
         //this is a constant parameter that will be referenced from arterial plasma compartment
         neighborhood1.AddParameter(NewConstantParameter("K", 10));
         spatialStructure.AddNeighborhood(neighborhood1);
         var neighborhood2 = _neighborhoodFactory.CreateBetween(lngPlasma, artPlasma).WithName("lng_pls_to_art_pls");
         spatialStructure.AddNeighborhood(neighborhood2);
         var neighborhood3 = _neighborhoodFactory.CreateBetween(bonePlasma, venPlasma).WithName("bon_pls_to_ven_pls");
         spatialStructure.AddNeighborhood(neighborhood3);
         var neighborhood4 = _neighborhoodFactory.CreateBetween(venPlasma, lngPlasma).WithName("ven_pls_to_lng_pls");
         spatialStructure.AddNeighborhood(neighborhood4);

         var neighborhood5 = _neighborhoodFactory.CreateBetween(lngPlasma, lngCell).WithName("lng_pls_to_lng_cell");
         neighborhood5.AddTag("Cell2Plasma");
         neighborhood5.AddTag("NeighborhoodTag1");
         neighborhood5.AddParameter(NewConstantParameter("SA", 22));
         spatialStructure.AddNeighborhood(neighborhood5);

         var neighborhood6 = _neighborhoodFactory.CreateBetween(bonePlasma, boneCell).WithName("bon_pls_to_bon_cell");
         neighborhood6.AddTag("Cell2Plasma");
         neighborhood6.AddParameter(NewConstantParameter("SA", 22));

         var neighborhood7 = _neighborhoodFactory.CreateBetween(bonePlasma, boneCell).WithName("does_not_match_existing");
         neighborhood7.FirstNeighborPath = new ObjectPath("Organism", "NOPE");
         spatialStructure.AddNeighborhood(neighborhood7);

         spatialStructure.ResolveReferencesInNeighborhoods();
         return spatialStructure;
      }

      private IContainer createContainerWithName(string containerName, ContainerMode containerMode = ContainerMode.Logical)
      {
         return _objectBaseFactory.Create<IContainer>()
            .WithName(containerName)
            .WithMode(containerMode);
      }

      public IParameter NewConstantParameter(string name, double value, ParameterBuildMode parameterBuildMode = ParameterBuildMode.Local)
         => _modelHelper.NewConstantParameter(name, value, parameterBuildMode);

      //Module 2 will introduce
      //1. A replacement of the lung container
      //2. a new organ structure  for heart
      //3. a new neighborhood between lung pls and heart pls
      private Module createModule2()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module2");
         var spatialStructure = CreateSpatialStructure("SPATIAL STRUCTURE MODULE 2");

         var globalMoleculeProperties = _objectBaseFactory.Create<IContainer>().WithName(MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);

         //add two parameters. One will be replaced when merging the second module
         globalMoleculeProperties.Add(NewConstantParameter("P1", 100, ParameterBuildMode.Global));
         globalMoleculeProperties.Add(NewConstantParameter("P3", 30, ParameterBuildMode.Global));
         spatialStructure.GlobalMoleculeDependentProperties = globalMoleculeProperties;

         //LUNG with other parameters and interstitial compartment
         // Lung
         //   - Plasma
         //      - Volume
         //      - pH
         //   - Cell
         //      - Volume
         //      - pH
         //   - Interstitial
         //      - Volume
         //      - pH

         var lung = createContainerWithName(Lung, ContainerMode.Physical);

         var lngPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         lngPlasma.Add(NewConstantParameter(Volume, 20));
         lngPlasma.Add(NewConstantParameter(pH, 2));
         lung.Add(lngPlasma);
         lung.AddTag("Tag2");

         var lngCell = createContainerWithName(Cell, ContainerMode.Physical);
         lngCell.Add(NewConstantParameter(Volume, 10));
         lngCell.Add(NewConstantParameter(pH, 2));
         lung.Add(lngCell);

         var lngInt = createContainerWithName(Interstitial, ContainerMode.Physical);
         lngInt.Add(NewConstantParameter(Volume, 10));
         lngInt.Add(NewConstantParameter(pH, 2));

         var lungQ = NewConstantParameter(Q, 5);
         lungQ.AddTag("ParamTag2");
         lung.Add(lungQ);

         lung.Add(NewConstantParameter(P2, 10));
         lung.Add(lngInt);

         //it will be added to the organism
         lung.ParentPath = new ObjectPath(ORGANISM);

         spatialStructure.AddTopContainer(lung);

         //new neighborhood between pls and int
         var neighborhood = _neighborhoodFactory.CreateBetween(lngPlasma, lngInt).WithName("lng_pls_to_lng_int");


         neighborhood.AddParameter(NewConstantParameter("SA", 10));
         spatialStructure.AddNeighborhood(neighborhood);


         //HEART with other parameters and interstitial compartment
         var heart = createContainerWithName(Heart);
         var heartPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         heart.Add(NewConstantParameter(Volume, 20));
         heart.Add(NewConstantParameter(pH, 2));
         heart.Add(heartPlasma);
         heart.ParentPath = new ObjectPath(ORGANISM);

         spatialStructure.AddTopContainer(heart);
         //new neighborhood between pls and int
         var neighborhood2 = _neighborhoodFactory.CreateBetween(lngPlasma, heartPlasma).WithName("lng_pls_to_hrt_pls");
         neighborhood2.AddParameter(NewConstantParameter("SA", 10));
         spatialStructure.AddNeighborhood(neighborhood2);

         //existing neighborhood between lngPlasma and lngCell
         var neighborhood3 = _neighborhoodFactory.CreateBetween(lngPlasma, lngCell).WithName("lng_pls_to_lng_cell");
         neighborhood3.AddTag("NeighborhoodTag2");
         spatialStructure.AddNeighborhood(neighborhood3);

         module.Add(spatialStructure);
         return module;
      }

      //Module 3 will add a tumor organ in the heart organ created above
      private Module createModule3()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module3");
         var spatialStructure = CreateSpatialStructure("SPATIAL STRUCTURE MODULE 3");

         var tumor = createContainerWithName(Tumor);
         var tumorPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         tumorPlasma.Add(NewConstantParameter(Volume, 20));
         tumorPlasma.Add(NewConstantParameter(pH, 2));
         tumor.Add(tumorPlasma);

         //tumor located between heart and lung
         tumor.ParentPath = new ObjectPath(ORGANISM, Heart);
         spatialStructure.AddTopContainer(tumor);

         //new neighborhood between pls and int
         var neighborhood = _neighborhoodFactory.CreateBetween(new ObjectPath(Tumor, Plasma), new ObjectPath(Plasma), new ObjectPath(ORGANISM, Heart)).WithName("kid_tmr_pls_to_hrt_pls");
         spatialStructure.AddNeighborhood(neighborhood);

         module.Add(spatialStructure);
         return module;
      }

      public SimulationSettings CreateSimulationSettings()
      {
         return new SimulationSettings { Solver = _solverSettingsFactory.CreateCVODE(), OutputSchema = _outputSchemaFactory.CreateDefault(), OutputSelections = new OutputSelections() };
      }

      public SpatialStructure CreateSpatialStructure(string name = "SPATIAL STRUCTURE")
      {
         return _spatialStructureFactory.Create().WithName(name);
      }

      private Module createModule4()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module4");
         var spatialStructure = CreateSpatialStructure("SPATIAL STRUCTURE MODULE 4");

         var organism = _objectBaseFactory.Create<IContainer>()
            .WithName(ORGANISM)
            .WithMode(ContainerMode.Logical);

         //Create a parameter with formula in Organism with absolute path
         var bw = NewConstantParameter(BW, 20);
         organism.Add(bw);

         //Organism
         // ArterialBlood
         //   - Plasma
         //      - Volume
         //   - Q
         //   - P
         // Lung
         //   - Plasma
         //      - Volume
         //      - pH
         //   - Cell
         //      - Volume
         //      - pH
         //   - Q
         //   - P
         // BW


         var art = createContainerWithName(ArterialBlood);

         var artPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         artPlasma.Add(NewConstantParameter(Volume, 2));
         art.Add(artPlasma);
         art.Add(NewConstantParameter(Q, 2));
         art.Add(NewConstantParameter(P, 10));
         organism.Add(art);


         var lung = createContainerWithName(Lung);
         var lngPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         lngPlasma.Add(NewConstantParameter(Volume, 2));
         lngPlasma.Add(NewConstantParameter(pH, 7.5));
         lung.Add(lngPlasma);

         var lngCell = createContainerWithName(Cell, ContainerMode.Physical);
         lngCell.Add(NewConstantParameter(Volume, 1));
         lngCell.Add(NewConstantParameter(pH, 7));
         lung.Add(lngCell);

         lung.Add(NewConstantParameter(Q, 3));
         lung.Add(NewConstantParameter(P, 2));
         organism.Add(lung);

         organism.Add(NewConstantParameter(fu, 1));
         spatialStructure.AddTopContainer(organism);

         var neighborhood2 = _neighborhoodFactory.CreateBetween(lngPlasma, artPlasma).WithName("lng_pls_to_art_pls");
         spatialStructure.AddNeighborhood(neighborhood2);

         var neighborhood5 = _neighborhoodFactory.CreateBetween(lngPlasma, lngCell).WithName("lng_pls_to_lng_cell");
         neighborhood5.AddTag("Cell2Plasma");
         neighborhood5.AddParameter(NewConstantParameter("SA", 22));
         spatialStructure.AddNeighborhood(neighborhood5);

         spatialStructure.ResolveReferencesInNeighborhoods();

         module.Add(spatialStructure);
         return module;
      }

      private Module createModule5()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module5");
         var spatialStructure = CreateSpatialStructure("SPATIAL STRUCTURE MODULE 5");

         var organism = _objectBaseFactory.Create<IContainer>()
            .WithName(ORGANISM)
            .WithMode(ContainerMode.Logical);

         //Create a parameter with formula in Organism with absolute path
         var bw = NewConstantParameter(BW, 200);
         organism.Add(bw);

         //Organism
         // ArterialBlood
         //   - Plasma
         //      - Volume <== THIS WILL OVERWRITE EXISTING VOLUME
         //      - Q      <== new PARAMETER
         //   - Interstitial <== NEW CONTAINER
         //      - P
         // Lung
         //   - Plasma
         //      - Volume <== THIS WILL OVERWRITE EXISTING VOLUME
         //      - Q      <== new PARAMETER
         //   - Interstitial <== NEW CONTAINER
         //      - P
         // BW <== THIS WILL OVERWRITE EXISTING BW


         var art = createContainerWithName(ArterialBlood);

         var artPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         artPlasma.Add(NewConstantParameter(Volume, 10));
         artPlasma.Add(NewConstantParameter(Q, 11));
         var artInterstitial = createContainerWithName(Interstitial, ContainerMode.Physical);
         artInterstitial.Add(NewConstantParameter(P, 12));
         art.AddChildren(artPlasma, artInterstitial);
         organism.Add(art);


         var lng = createContainerWithName(Lung);
         var lngPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         lngPlasma.Add(NewConstantParameter(Volume, 20));
         lngPlasma.Add(NewConstantParameter(Q, 21));
         var lngInterstitial = createContainerWithName(Interstitial, ContainerMode.Physical);
         lng.AddChildren(lngPlasma, lngInterstitial);
         organism.Add(lng);

         spatialStructure.AddTopContainer(organism);

         module.Add(spatialStructure);
         return module;
      }
   }
}