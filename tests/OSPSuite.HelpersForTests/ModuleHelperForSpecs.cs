using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using System.Linq;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Helpers.ConstantsForSpecs;

namespace OSPSuite.Helpers
{
   public class ModuleHelperForSpecs
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ISolverSettingsFactory _solverSettingsFactory;
      private readonly IOutputSchemaFactory _outputSchemaFactory;
      private readonly ISpatialStructureFactory _spatialStructureFactory;
      private readonly INeighborhoodBuilderFactory _neighborhoodFactory;
      private readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly ModelHelperForSpecs _modelHelper;

      public ModuleHelperForSpecs(
         IObjectBaseFactory objectBaseFactory,
         ISolverSettingsFactory solverSettingsFactory,
         IOutputSchemaFactory outputSchemaFactory,
         ISpatialStructureFactory spatialStructureFactory,
         INeighborhoodBuilderFactory neighborhoodFactory,
         IInitialConditionsCreator initialConditionsCreator,
         ModelHelperForSpecs modelHelper)
      {
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
            SimulationSettings = createSimulationConfiguration(),
         };

         var module1 = createModule1();
         var module2 = createModule2();
         var module3 = createModule3();

         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2));
         simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module3));
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
         var globalParameter = newConstantParameter("A_Global", 5, ParameterBuildMode.Global);
         var formula = _objectBaseFactory.Create<ExplicitFormula>().WithFormulaString("A_Global_Formula").WithFormulaString("2+2");
         globalParameter.Formula = formula;
         formulaCache.Add(formula);
         moleculeC.Add(globalParameter);
         moleculeC.IsFloating = true;
         return moleculeC;
      }

      private SpatialStructure getSpatialStructureModule1()
      {
         var spatialStructure = _spatialStructureFactory.Create().WithName("SPATIAL STRUCTURE MODULE 1");

         var organism = _objectBaseFactory.Create<IContainer>()
            .WithName(ORGANISM)
            .WithMode(ContainerMode.Logical);

         //Create a parameter with formula in Organism with absolute path
         var bw = newConstantParameter(BW, 20);
         organism.Add(bw);

         var globalMoleculeProperties = _objectBaseFactory.Create<IContainer>().WithName(MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);

         //add two parameters. One will be replaced when merging the second module
         globalMoleculeProperties.Add(newConstantParameter("P1", 10));
         globalMoleculeProperties.Add(newConstantParameter("P2", 20));

         spatialStructure.GlobalMoleculeDependentProperties = globalMoleculeProperties;

         //ART
         var art = createContainerWithName(ArterialBlood);

         var artPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         artPlasma.Add(newConstantParameter(Volume, 2));
         art.Add(artPlasma);
         art.Add(newConstantParameter(Q, 2));
         art.Add(newConstantParameter(P, 10));
         organism.Add(art);


         //LUNG
         var lung = createContainerWithName(Lung);

         var lngPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         lngPlasma.Add(newConstantParameter(Volume, 2));
         lngPlasma.Add(newConstantParameter(pH, 7.5));
         lung.Add(lngPlasma);

         var lngCell = createContainerWithName(Cell, ContainerMode.Physical);
         lngCell.Add(newConstantParameter(Volume, 1));
         lngCell.Add(newConstantParameter(pH, 7));
         lung.Add(lngCell);

         lung.Add(newConstantParameter(Q, 3));
         lung.Add(newConstantParameter(P, 2));
         organism.Add(lung);

         //BONE
         var bone = createContainerWithName(Bone);
         organism.Add(bone);

         var bonePlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         bonePlasma.Add(newConstantParameter(Volume, 2));
         bonePlasma.Add(newConstantParameter(pH, 7.5));
         bone.Add(bonePlasma);


         var boneCell = createContainerWithName(Cell, ContainerMode.Physical);
         boneCell.Add(newConstantParameter(Volume, 1));
         boneCell.Add(newConstantParameter(pH, 7));
         bone.Add(boneCell);


         //VEN
         var ven = createContainerWithName(VenousBlood);
         var venPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         venPlasma.Add(newConstantParameter(Volume, 2));
         ven.Add(venPlasma);
         ven.Add(newConstantParameter(Q, 2));
         organism.Add(ven);

         organism.Add(newConstantParameter(fu, 1));
         spatialStructure.AddTopContainer(organism);

         var neighborhood1 = _neighborhoodFactory.CreateBetween(artPlasma, bonePlasma).WithName("art_pls_to_bon_pls");
         //this is a constant parameter that will be referenced from arterial plasma compartment
         neighborhood1.AddParameter(newConstantParameter("K", 10));
         spatialStructure.AddNeighborhood(neighborhood1);
         var neighborhood2 = _neighborhoodFactory.CreateBetween(lngPlasma, artPlasma).WithName("lng_pls_to_art_pls");
         spatialStructure.AddNeighborhood(neighborhood2);
         var neighborhood3 = _neighborhoodFactory.CreateBetween(bonePlasma, venPlasma).WithName("bon_pls_to_ven_pls");
         spatialStructure.AddNeighborhood(neighborhood3);
         var neighborhood4 = _neighborhoodFactory.CreateBetween(venPlasma, lngPlasma).WithName("ven_pls_to_lng_pls");
         spatialStructure.AddNeighborhood(neighborhood4);

         var neighborhood5 = _neighborhoodFactory.CreateBetween(lngPlasma, lngCell).WithName("lng_pls_to_lng_cell");
         neighborhood5.AddTag("Cell2Plasma");
         neighborhood5.AddParameter(newConstantParameter("SA", 22));
         spatialStructure.AddNeighborhood(neighborhood5);

         var neighborhood6 = _neighborhoodFactory.CreateBetween(bonePlasma, boneCell).WithName("bon_pls_to_bon_cell");
         neighborhood6.AddTag("Cell2Plasma");
         neighborhood6.AddParameter(newConstantParameter("SA", 22));

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

      private IParameter newConstantParameter(string name, double value, ParameterBuildMode parameterBuildMode = ParameterBuildMode.Local)
         => _modelHelper.NewConstantParameter(name, value, parameterBuildMode);

      //Module 2 will introduce
      //1. A replacement of the lung container
      //2. a new organ structure  for heart
      //3. a new neighborhood between lung pls and heart pls
      private Module createModule2()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module2");
         var spatialStructure = _spatialStructureFactory.Create().WithName("SPATIAL STRUCTURE MODULE 2");

         var globalMoleculeProperties = _objectBaseFactory.Create<IContainer>().WithName(MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);

         //add two parameters. One will be replaced when merging the second module
         globalMoleculeProperties.Add(newConstantParameter("P1", 100, ParameterBuildMode.Global));
         globalMoleculeProperties.Add(newConstantParameter("P3", 30, ParameterBuildMode.Global));
         spatialStructure.GlobalMoleculeDependentProperties = globalMoleculeProperties;

         //LUNG with other parameters and interstitial compartment
         var lung = createContainerWithName(Lung);

         var lngPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         lngPlasma.Add(newConstantParameter(Volume, 20));
         lngPlasma.Add(newConstantParameter(pH, 2));
         lung.Add(lngPlasma);

         var lngCell = createContainerWithName(Cell, ContainerMode.Physical);
         lngCell.Add(newConstantParameter(Volume, 10));
         lngCell.Add(newConstantParameter(pH, 2));
         lung.Add(lngCell);

         var lngInt = createContainerWithName(Interstitial, ContainerMode.Physical);
         lngInt.Add(newConstantParameter(Volume, 10));
         lngInt.Add(newConstantParameter(pH, 2));
         lung.Add(lngInt);

         //it will be added to the organism
         lung.ParentPath = new ObjectPath(ORGANISM);

         spatialStructure.AddTopContainer(lung);

         //new neighborhood between pls and int
         var neighborhood = _neighborhoodFactory.CreateBetween(lngPlasma, lngInt, lung.ParentPath).WithName("lng_pls_to_lng_int");
         neighborhood.AddParameter(newConstantParameter("SA", 10));
         spatialStructure.AddNeighborhood(neighborhood);


         //LUNG with other parameters and interstitial compartment
         var heart = createContainerWithName(Heart);
         var heartPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         heart.Add(newConstantParameter(Volume, 20));
         heart.Add(newConstantParameter(pH, 2));
         heart.Add(heartPlasma);
         heart.ParentPath = new ObjectPath(ORGANISM);

         spatialStructure.AddTopContainer(heart);
         //new neighborhood between pls and int
         var neighborhood2 = _neighborhoodFactory.CreateBetween(lngPlasma, heartPlasma, lung.ParentPath).WithName("lng_pls_to_hrt_pls");
         neighborhood2.AddParameter(newConstantParameter("SA", 10));
         spatialStructure.AddNeighborhood(neighborhood2);

         module.Add(spatialStructure);
         return module;
      }

      //Module 3 will add a tumor organ in the heart organ created above
      private Module createModule3()
      {
         var module = _objectBaseFactory.Create<Module>().WithName("Module3");
         var spatialStructure = _spatialStructureFactory.Create().WithName("SPATIAL STRUCTURE MODULE 3");

         var tumor = createContainerWithName(Tumor);
         var tumorPlasma = createContainerWithName(Plasma, ContainerMode.Physical);
         tumorPlasma.Add(newConstantParameter(Volume, 20));
         tumorPlasma.Add(newConstantParameter(pH, 2));
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

      private SimulationSettings createSimulationConfiguration()
      {
         return new SimulationSettings {Solver = _solverSettingsFactory.CreateCVODE(), OutputSchema = _outputSchemaFactory.CreateDefault(), OutputSelections = new OutputSelections()};
      }
   }
}